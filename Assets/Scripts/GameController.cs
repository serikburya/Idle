using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;
using InstantGamesBridge.Modules.Leaderboard;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text coins_text;
    [SerializeField]
    private Text numBowlOFBalls_cost, numBowlOFBalls_text;

    public float timeSpawn;

    [SerializeField]
    private int numBowlOFBalls = 0;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Sprite[] ballElementSprite = new Sprite[3];
    [SerializeField]
    private float[] xSpawnBall = new float[5];

    private GameObject ball;
    private int  reward_ads_mult;
    private bool reward_is_obtain, yandex_is_on;

    public UnityEvent isInitialized_event;
    private Shop shop;
    public int coins = 0;

    private void Start()
    {
        reward_ads_mult = 1;
        reward_is_obtain = false;
        yandex_is_on = false;

        shop = GetComponent<Shop>();

        

        timeSpawn = 2f;
        Set_text();

        StartCoroutineCreateBall(shop.numPlaceSpawn);

        Bridge.Initialize(OnBridgeInitializationCompleted);
    }

   public IEnumerator CreateBall(int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSpawn);
            int indexElement = Random.Range(0, ballElementSprite.Length);
            ball = Instantiate(ballPrefab, new Vector3(xSpawnBall[index], 5f, 0f), Quaternion.identity);
            ball.GetComponent<SpriteRenderer>().sprite = ballElementSprite[indexElement];
            ball.GetComponent<BallElement>().costBall = numBowlOFBalls + 1;
            ball.GetComponent<BallElement>().indexElement = indexElement;
        }
    }

    private void OnBridgeInitializationCompleted(bool isInitialized)
    {
       if (isInitialized)
       {
           Bridge.advertisement.rewardedStateChanged += Reward_ads;
           Bridge.game.GetData("data", OnGetCompleted);
           StartCoroutine(save_enum());
           isInitialized_event.Invoke();
           yandex_is_on = true;
        }
    }

    public void StartCoroutineCreateBall(int numPlaceSpawn)
    {
        for (int i = 0; i < numPlaceSpawn-1; i++)
        {
            StopCoroutine(CreateBall(i));
        }

        for (int i = 0; i < numPlaceSpawn; i++)
        {
            StartCoroutine(CreateBall(i));
        }
    }

     private void OnGetCompleted(bool success, string data)
     {
        if (success)
        {
            if (data != null)
            {
                DataClass dataObject = JsonUtility.FromJson<DataClass>(data);
                coins  = dataObject.coins;
                numBowlOFBalls = dataObject.numBowlOFBalls;
                shop.numPlaceSpawn = dataObject.numPlaceSpawn;

                timeSpawn = dataObject.timeSpawn;

                if (shop.numPlaceSpawn>=5)
                {
                    shop.numPlaceSpawnButton.gameObject.SetActive(false);
                }
            }
            else 
            {
                coins = 0;
                numBowlOFBalls = 0;
                shop.numPlaceSpawn = 0;
                timeSpawn = 2f;
            }  
        }
        shop.SetText();
        Set_text();
     }


    void Set_text()
    {
        coins_text.text = $"{coins}";
        numBowlOFBalls_cost.text = "Цена: " + Bowls_cost();
        numBowlOFBalls_text.text = "Кубков: " + numBowlOFBalls;
    }







    public void Add_coins(int reward)
    {
        coins += reward * reward_ads_mult;
        Set_text();
    }


    void save()
    {
        if (yandex_is_on)
        {
            DataClass dataObject = new DataClass();
            dataObject.coins = coins;
            dataObject.numBowlOFBalls = numBowlOFBalls;
            dataObject.numPlaceSpawn = shop.numPlaceSpawn;
            dataObject.timeSpawn = timeSpawn;
            string data = JsonUtility.ToJson(dataObject);
            Bridge.game.SetData("data", data, success =>
            {

            });
        }
    }


    private IEnumerator save_enum()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            save();
        }
    }

    int Bowls_cost()
    {
        return 100 * (int)Mathf.Pow(2, numBowlOFBalls);
    }

    public void Buy_bowls()
    {
        if (coins - Bowls_cost() >= 0)
        {
            coins -= Bowls_cost();
            numBowlOFBalls += 1;
            Set_text();
        }
    }

    void Reward_ads(RewardedState state)
    {
        if (state == RewardedState.Rewarded)
        {
            reward_is_obtain = true;
        }
        if (state == RewardedState.Closed)
        {
            if (reward_is_obtain)
            {
                StartCoroutine(Reward_enum());
            }
            reward_is_obtain = false;
        }
        Debug.Log(state);

    }

    private IEnumerator Reward_enum()
    {
        reward_ads_mult = 2;
        yield return new WaitForSeconds(30f);
        reward_ads_mult = 1;
        StopCoroutine(Reward_enum());
    }

    public void Watch_ads()
    {
        if (yandex_is_on)
        {
            Bridge.advertisement.ShowRewarded(success =>
            {
                if (success)
                {
                    reward_is_obtain = false;
                }
            });
        }
    }
}

[System.Serializable]
public class DataClass
{
    public int coins;
    public int numBowlOFBalls;
    public int numPlaceSpawn;
    public float timeSpawn;
}
