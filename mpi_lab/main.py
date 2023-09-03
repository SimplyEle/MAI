
import torch
import torchtext
import sklearn
import numpy as np
import mpi4py
import gc

from mpi4py import MPI

from torch import nn
from torch.optim import Adam
from torch.nn import functional as F
from torch.utils.data import DataLoader

from torchtext.data import get_tokenizer
from torchtext.vocab import build_vocab_from_iterator
from torchtext.data.functional import to_map_style_dataset

from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics import accuracy_score, classification_report, confusion_matrix

comm = MPI.COMM_WORLD
size = comm.Get_size()
rank = comm.Get_rank()

def build_vocab(datasets):
    for dataset in datasets:
        for _, text in dataset:
            yield tokenizer(text)

def vectorize_batch(batch):
    Y, X = list(zip(*batch))
    X = vectorizer.transform(X).todense()
    return torch.tensor(X, dtype=torch.float32), torch.tensor(Y) - 1

class TextClassifier(nn.Module):
    def __init__(self):
        super(TextClassifier, self).__init__()
        self.seq = nn.Sequential(
            nn.Linear(len(vocab), 128),
            nn.ReLU(),

            nn.Linear(128, 64),
            nn.ReLU(),

            nn.Linear(64, 4)
        )

    def forward(self, X_batch):
        return self.seq(X_batch)

def CalcValLossAndAccuracy(model, loss_fn, val_loader):
    with torch.no_grad():
        Y_shuffled, Y_preds, losses = [],[],[]
        for X, Y in val_loader:
            preds = model(X)
            loss = loss_fn(preds, Y)
            losses.append(loss.item())

            Y_shuffled.append(Y)
            Y_preds.append(preds.argmax(dim=-1))

        Y_shuffled = torch.cat(Y_shuffled)
        Y_preds = torch.cat(Y_preds)

        print("Valid Loss : {:.3f}".format(torch.tensor(losses).mean()))
        print("Valid Acc  : {:.3f}".format(accuracy_score(Y_shuffled.detach().numpy(), Y_preds.detach().numpy())))


def TrainModel(model, loss_fn, optimizer, train_loader, epochs):
    for i in range(1, epochs+1):
        losses = []
        for X, Y in train_loader:
            Y_preds = model(X)

            loss = loss_fn(Y_preds, Y)
            losses.append(loss.item())

            optimizer.zero_grad()
            loss.backward()
            optimizer.step()

        print("Train Loss : {:.3f}".format(torch.tensor(losses).mean()))

def MakePredictions(model, loader):
    Y_shuffled, Y_preds = [], []
    for X, Y in loader:
        preds = model(X)
        Y_preds.append(preds)
        Y_shuffled.append(Y)
    gc.collect()
    Y_preds, Y_shuffled = torch.cat(Y_preds), torch.cat(Y_shuffled)

    return Y_shuffled.detach().numpy(), F.softmax(Y_preds, dim=-1).argmax(dim=-1).detach().numpy()

if rank == 0:
    train_dataset, test_dataset  = torchtext.datasets.AG_NEWS()
    
    tokenizer = get_tokenizer("basic_english")
    
    vocab = build_vocab_from_iterator(build_vocab([train_dataset, test_dataset]), specials=["<UNK>"])
    vocab.set_default_index(vocab["<UNK>"])
    
    vectorizer = CountVectorizer(vocabulary=vocab.get_itos(), tokenizer=tokenizer)

    train_dataset, test_dataset = to_map_style_dataset(train_dataset), to_map_style_dataset(test_dataset)
    train_dataset_1 = train_dataset[0:len(train_dataset)//4]
    train_dataset_2 = train_dataset[len(train_dataset)//4:len(train_dataset)//2]
    train_dataset_3 = train_dataset[len(train_dataset)//2:3*len(train_dataset)//4]
    train_dataset_4 = train_dataset[3*len(train_dataset)//4:len(train_dataset)]
    
    train_loader_1 = DataLoader(train_dataset_1, batch_size=256, collate_fn=vectorize_batch)
    train_loader_2 = DataLoader(train_dataset_2, batch_size=256, collate_fn=vectorize_batch)
    train_loader_3 = DataLoader(train_dataset_3, batch_size=256, collate_fn=vectorize_batch)
    train_loader_4 = DataLoader(train_dataset_4, batch_size=256, collate_fn=vectorize_batch)
    train_loader = [train_loader_1, train_loader_2, train_loader_3, train_loader_4]

    test_loader = DataLoader(test_dataset[0:len(test_dataset)//4], batch_size=256, collate_fn=vectorize_batch)
    
    Y_actual_lst = []
    Y_preds_lst = []
    for id in range(1,size):
        comm.send(vocab, dest = id)
        comm.send(vectorizer, dest = id)
        comm.send(train_loader[id], dest = id)
        comm.send(test_loader, dest = id)
        
        
        Y_actual_lst.append(comm.recv(source = id))
        Y_preds_lst.append(comm.recv(source = id))
    
    comparsion = []
    for i in range(len(Y_preds_lst[0])):
        temp = np.array([Y_preds_lst[0][i], Y_preds_lst[1][i], Y_preds_lst[2][i]])
        comparsion.append(np.argmax(np.bincount(temp)))
        
    voting_classifier = np.sum(Y_actual_lst[0] == comparsion) / len(Y_actual_lst[0])
    print("Ensemble Voting Result: ", voting_classifier)
else:
    vocab = comm.recv(source = 0)
    vectorizer = comm.recv(source = 0)
    train_loader = comm.recv(source = 0)
    test_loader = comm.recv(source = 0)
    
    target_classes = ["World", "Sports", "Business", "Sci/Tec"]
    
    epochs = 8
    learning_rate = 1e-4

    loss_fn = nn.CrossEntropyLoss()
    text_classifier = TextClassifier()
    optimizer = Adam(text_classifier.parameters(), lr=learning_rate)

    TrainModel(text_classifier, loss_fn, optimizer, train_loader, epochs)
    CalcValLossAndAccuracy(text_classifier, loss_fn, test_loader)
    Y_actual, Y_preds = MakePredictions(text_classifier, test_loader)
    
    print("Test Accuracy on {0} process : {1}".format(rank, accuracy_score(Y_actual, Y_preds)))
    print("\nClassification Report on {} process : ".format(rank))
    print(classification_report(Y_actual, Y_preds, target_names=target_classes))
    
    comm.send(Y_actual, dest = 0)
    comm.send(Y_preds, dest = 0)

MPI.Finalize()
