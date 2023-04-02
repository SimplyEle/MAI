DELETE FROM 'Category';
DELETE FROM 'ReportConference';
DELETE FROM 'Conference';
DELETE FROM `Report`;
DELETE FROM `User`;


INSERT INTO `User`
(login, password, first_name, last_name, email)
VALUES('Helen', 'helen', 'Helen', 'KarKarycheva', 'smeshar@yandex.ru');

INSERT INTO `User`
(login, password, first_name, last_name, email)
VALUES('basta', 'moyaigra', 'Vasiliy', 'Vak', 'basta@yandex.ru');

INSERT INTO `User`
(login, password, first_name, last_name, email)
VALUES('vesna', 'music', 'Music', 'Vesna', 'music_vesna@yandex.ru');

INSERT INTO 'Category'
(name_of_category)
VALUES('Международный');
	  
INSERT INTO 'Category'
(name_of_category)
VALUES('Всероссийский');
	  
INSERT INTO 'Category'
(name_of_category)
VALUES('Внутривузовский');
	   
INSERT INTO `Conference`
(name_conf, organizer_id, category_id, description, date_of_conf)
VALUES('Гагаринские чтения', 0, 0, "МАИ", "11-14 апреля 2023");
      
INSERT INTO `Conference`
(name_conf, organizer_id, category_id, description, date_of_conf)
VALUES('Колачевские чтения', 0, 1, "СФ МАИ", "12 апреля 2023");

INSERT INTO 'Report'
(name_report, author_id, annotation, text_report, date_creation)
VALUES('Ещё до старта далеко', 0, 'Храброе сердце', 'Только вперёд!', '01.04.2023');
      
INSERT INTO 'Report'
(name_report, author_id, annotation, text_report, date_creation)
VALUES('Смысл', 1, 'Кузинатра', 'Смотреть там нечего, ничего интересного!', '02.04.2023');
            
INSERT INTO 'Report'
(name_report, author_id, annotation, text_report, date_creation)
VALUES('Nutracker!', 2, 'Violin', 'Winter', '31.03.2023');      

INSERT INTO 'ReportConference'
(report_id, conf_id)
VALUES(0,0);

INSERT INTO 'ReportConference'
(report_id, conf_id)
VALUES(1,0);

INSERT INTO 'ReportConference'
(report_id, conf_id)
VALUES(2,1);