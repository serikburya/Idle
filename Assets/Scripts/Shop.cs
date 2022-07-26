using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    
    public int numPlaceSpawn;

    [SerializeField]
    private Text numPlaceSpawnText, numPlaceSpawnButtonText, spawnTimeButtonText, spawnTimeText;

    public  Button numPlaceSpawnButton, spawnTimeButton;

    private GameController gameController;

    public void Awake()
    {
        gameController = GetComponent<GameController>();
        numPlaceSpawnButton.onClick.AddListener(NumPlaceSpawn);
        spawnTimeButton.onClick.AddListener(BuySpawnTime);

    }

    void NumPlaceSpawn()
    {
        if(numPlaceSpawn>=5)
        {
            numPlaceSpawnButton.gameObject.SetActive(false);
            numPlaceSpawn = 5;
        }
        else
        {
            BuyNumPlaceSpawn();
        }
    }

    void BuyNumPlaceSpawn()
    {
        int temp = CostNumPlaceSpawn();
        if (gameController.coins >= temp)
        {
            gameController.coins = gameController.coins - temp;
            numPlaceSpawn++;
            SetText();
        }
            
    }

    int CostNumPlaceSpawn()
    {
        return 100 * numPlaceSpawn * numPlaceSpawn;
    }

    public void SetText()
    {
        numPlaceSpawnText.text = "Цена " + $"{CostNumPlaceSpawn()}"; 
        numPlaceSpawnButtonText.text = "Мест " + $"{numPlaceSpawn}";
        spawnTimeText.text = "Цена " + $"{CostSpawnTime()}";
        spawnTimeButtonText.text =  "Время " + $"{ReduceSpawnTime(gameController.timeSpawn)}";
    }

    void BuySpawnTime()
    {
        int temp = CostSpawnTime();
        if (gameController.coins >= temp)
        {
            gameController.coins = gameController.coins - temp;
            gameController.timeSpawn = ReduceSpawnTime(gameController.timeSpawn);
            SetText();
        }

    }

    int CostSpawnTime()
    {
        float temp = gameController.timeSpawn;
        return (int)Mathf.Floor(2000 / (temp*temp*temp));
    }

    float ReduceSpawnTime(float temp)
    {
        return 0.9f * temp;
    }


}
