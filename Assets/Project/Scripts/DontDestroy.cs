using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameMode gameMode;
    public int gameModeValue;
    

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
