using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTargets : MonoBehaviour
{

    private int countTargets = 0;
    private List<double> firstLinePositions;
    private List<double> secondLinePositions;
    private List<double> thirdLinePositions;
    private int startCountTargets;

    private int startFirstLinePositions;
    private int startSecondLinePositions;
    private int startThirdLinePositions;

    private int numOfClonesFirstLine;
    private int numOfClonesSecondLine;
    private int numOfClonesThirdLine;

    private List<double> targetPositionsFirstLine;
    private List<double> targetPositionsSecondLine;
    private List<double> targetPositionsThirdLine;

    public GameObject target;

    public void DecCountTargets()
    {
        countTargets--;
    }
    public List<double> GetFirstLinePositions()
    {
        return firstLinePositions;
    }

    public List<double> GetSecondLinePositions()
    {
        return secondLinePositions;
    }

    public List<double> GetThirdLinePositions()
    {
        return thirdLinePositions;
    }


    private List<double> CreateTargetsPositions(double start, double end)
    {
        List<double> targetPositions = new();
        
        for (double i = start; i <= end; i += 0.1)
        {
            targetPositions.Add(i);
        }

        return targetPositions;
    }

    private double GenerateStartRandomPosition(List<double> positions)
    {
        System.Random rnd = new();
        double pos = (rnd.NextDouble() * (positions.Last() - positions[0])) + positions[0];
        return pos;
    }

    void GenerateTempPosition(double startPos, List<double> positions, ref List<double> res)
    {
        double tempPos;
        System.Random rnd = new();
        System.Random rndBin = new();
        if (rndBin.Next(0, 2) == 0)
        {
            tempPos = startPos + (rnd.NextDouble() * (15 - 4)) + 4;
        }
        else
        {
            tempPos = startPos - (rnd.NextDouble() * (15 - 4)) + 4;
        }
        if (tempPos >= positions[0] && tempPos <= positions.Last())
        {
            double differenceBtwPositions;
            bool flag = true;
            for (int k = 0; k < res.Count; k++)
            {
                differenceBtwPositions = System.Math.Abs(tempPos - res[k]);
                if (differenceBtwPositions < 3.2)
                {
                    flag = false;
                    break;
                }
            }
            if (flag == true)
            {
                res.Add(tempPos);
            }
        }
    }

    private List<double> GeneratePositions(List<double> positions, double startPos)
    {
        List<double> res = new()
        {
            startPos
        };
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 1000; j++)      // 1000 - может не попасть в условие в функции GenerateTempPosition
            {
                GenerateTempPosition(startPos, positions, ref res);
                if (res.Count == 3)
                {
                    return res;
                }
            }
        }
        return res;
    }

    private void FormListPositionsOnTheScreen(out List<double> firstLinePositions, out List<double> secondLinePositions, out List<double> thirdLinePositions)
    {
        targetPositionsFirstLine = CreateTargetsPositions(17.5, 33.5);
        targetPositionsSecondLine = CreateTargetsPositions(17.5, 35);
        targetPositionsThirdLine = CreateTargetsPositions(17.5, 33);


        firstLinePositions = GeneratePositions(targetPositionsFirstLine, GenerateStartRandomPosition(targetPositionsFirstLine));
        secondLinePositions = GeneratePositions(targetPositionsSecondLine, GenerateStartRandomPosition(targetPositionsSecondLine));
        thirdLinePositions = GeneratePositions(targetPositionsThirdLine, GenerateStartRandomPosition(targetPositionsThirdLine));
    }

    private void SpawnObject(GameObject gameObj, string tagName, double position, float yCoord, float zCoord)
    {
        GameObject temp = Instantiate(gameObj);
        temp.tag = tagName;
        temp.transform.GetChild(0).tag = tagName;
        temp.transform.position = new Vector3((float)position, yCoord, zCoord);
        countTargets++;
    }

    private void Spawn(GameObject gameObj)
    {
        if (gameObj == null)
        {
            return; 
        }
        
        FormListPositionsOnTheScreen(out firstLinePositions, out secondLinePositions, out thirdLinePositions);
        
        if (firstLinePositions.Count != 0)
        {
            for (int i = 0; i < firstLinePositions.Count; i++)
            {
                SpawnObject(gameObj, "Target1line", firstLinePositions[i], 7.5f, 7.12f);
            }
        }
        if (secondLinePositions.Count != 0)
        {
            for (int i = 0; i < secondLinePositions.Count; i++)
            {
                SpawnObject(gameObj, "Target2line", secondLinePositions[i], 10f, 44.72f);
            }
        }
        if (thirdLinePositions.Count != 0)
        {
            for (int i = 0; i < thirdLinePositions.Count; i++)
            {
                SpawnObject(gameObj, "Target3line", thirdLinePositions[i], 17.12f, 80.16f);
            }
        }
    }

    private void Start()
    {
        Spawn(target);
        startCountTargets = firstLinePositions.Count + secondLinePositions.Count + thirdLinePositions.Count;

        startFirstLinePositions = firstLinePositions.Count;
        startSecondLinePositions = secondLinePositions.Count;
        startThirdLinePositions = thirdLinePositions.Count;
    }


    private void FixedUpdate()
    {
        numOfClonesFirstLine = GameObject.FindGameObjectsWithTag("Target1line").Count();
        numOfClonesSecondLine = GameObject.FindGameObjectsWithTag("Target2line").Count();
        numOfClonesThirdLine = GameObject.FindGameObjectsWithTag("Target3line").Count();

        if (countTargets < startCountTargets)
        {
            if (numOfClonesFirstLine != startFirstLinePositions)
            {
                int check = firstLinePositions.Count;
                for (int i = 0; i < 100; i++)
                {
                    GenerateTempPosition(firstLinePositions[0], targetPositionsFirstLine, ref firstLinePositions);
                    if (firstLinePositions.Count == 3)
                    {
                        break;
                    }
                }
                if (firstLinePositions.Count != check)
                {
                    SpawnObject(target, "Target1line", firstLinePositions.Last(), 7.5f, 7.12f);
                }
            }
            if (numOfClonesSecondLine != startSecondLinePositions)
            {
                int check = secondLinePositions.Count;
                for (int i = 0; i < 100; i++)
                {
                    GenerateTempPosition(secondLinePositions[0], targetPositionsSecondLine, ref secondLinePositions);
                    if (secondLinePositions.Count == 3)
                    {
                        break;
                    }
                }
                if (secondLinePositions.Count != check)
                {
                    SpawnObject(target, "Target2line", secondLinePositions.Last(), 10f, 44.72f);
                }
            }
            if (numOfClonesThirdLine != startThirdLinePositions)
            {
                int check = thirdLinePositions.Count;
                for (int i = 0; i < 100; i++)
                {
                    GenerateTempPosition(thirdLinePositions[0], targetPositionsThirdLine, ref thirdLinePositions);
                    if (thirdLinePositions.Count == 3)
                    {
                        break;
                    }
                }
                if (thirdLinePositions.Count != check)
                {
                    SpawnObject(target, "Target3line", thirdLinePositions.Last(), 17.12f, 80.16f);
                }
            }
        }
    }
}
