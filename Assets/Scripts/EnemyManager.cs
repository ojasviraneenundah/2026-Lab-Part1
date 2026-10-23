using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Awake()
    {
        // other instructions
        // GameManager.instance.gameRestart.AddListener(GameRestart); // lab 5 replace
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        Debug.Log("EnemyManager.GameRestart() called");
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }
}
