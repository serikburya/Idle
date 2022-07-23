using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameController gameController;
    public int numPlaceSpawn;

    public void Start()
    {
        numPlaceSpawn = 5;
        gameController = GetComponent<GameController>();
    }
}
