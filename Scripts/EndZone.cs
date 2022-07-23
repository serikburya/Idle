using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [HideInInspector]
    public int indexZone = 0;

    [SerializeField]
    private GameController gameController;


    public void set_color(Color colorZone_temp) //add  function
    {
        gameObject.GetComponent<SpriteRenderer>().color = colorZone_temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject coll = collision.gameObject;

        if (coll.tag == "Ball" ) // change if condition 
        {
            BallElement ballElement = coll.GetComponent<BallElement>();

            if (indexZone == ballElement.indexElement)
            {
                gameController.Add_coins(ballElement.costBall);
            }
            // пока что убрал дополнительные условия
            /*else if((indexZone + 2)%3 == ballElement.indexElement )
            {
                gameController.add_coins(2*(ballElement.costBall));
            }*/

            Destroy(coll);
        }
    }
}
