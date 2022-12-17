using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private SpawnTargets spawnTargets;

    void Start()
    {
        spawnTargets = GameObject.Find("GameManager").GetComponent<SpawnTargets>();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("bang!");
            RaycastHit hit;
            if (Physics.Raycast(
                    Camera.main.transform.position,
                    Camera.main.transform.forward,
                    out hit,
                    1000f
                ))
            {
                hit.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
                if (hit.transform.CompareTag("Target1line") || hit.transform.CompareTag("Target2line") || hit.transform.CompareTag("Target3line")) 
                {
                    if (hit.transform.CompareTag("Target1line"))
                    {
                        for (int i=0; i < spawnTargets.GetFirstLinePositions().Count; i++)
                        {
                            spawnTargets.GetFirstLinePositions()[i] = System.Math.Round(spawnTargets.GetFirstLinePositions()[i], 1);
                        }
                        double value = System.Math.Round(hit.transform.parent.transform.position.x, 1);
                        int posId = spawnTargets.GetFirstLinePositions().IndexOf(value);
                        spawnTargets.GetFirstLinePositions().RemoveAt(posId);
                        Debug.Log("Value= " + value);
                    }
                    else if (hit.transform.CompareTag("Target2line"))
                    {
                        for (int i = 0; i < spawnTargets.GetSecondLinePositions().Count; i++)
                        {
                            spawnTargets.GetSecondLinePositions()[i] = System.Math.Round(spawnTargets.GetSecondLinePositions()[i], 1);
                        }
                        double value = System.Math.Round(hit.transform.parent.transform.position.x, 1);
                        int posId = spawnTargets.GetSecondLinePositions().IndexOf(value);
                        spawnTargets.GetSecondLinePositions().RemoveAt(posId);
                    }
                    else if(hit.transform.CompareTag("Target3line"))
                    {
                        for (int i = 0; i < spawnTargets.GetThirdLinePositions().Count; i++)
                        {
                            spawnTargets.GetThirdLinePositions()[i] = System.Math.Round(spawnTargets.GetThirdLinePositions()[i], 1);
                        }
                        double value = System.Math.Round(hit.transform.parent.transform.position.x, 1);
                        int posId = spawnTargets.GetThirdLinePositions().IndexOf(value);
                        spawnTargets.GetThirdLinePositions().RemoveAt(posId);
                    }
                    Destroy(hit.transform.parent.gameObject);
                    spawnTargets.DecCountTargets();
                } 
            }            
        }
    }
}