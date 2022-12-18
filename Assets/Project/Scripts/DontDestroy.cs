using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string gameMode;
    public int gameModeValue;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
