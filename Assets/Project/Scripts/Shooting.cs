using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    void Start()
    {
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
                    Destroy(hit.transform.gameObject);
                } 
            }
        }
    }
}