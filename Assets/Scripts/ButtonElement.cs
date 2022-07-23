using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonElement : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private int indexZone;
    [SerializeField]
    private EndZone endZone;
    [SerializeField]
    private Color colorZone;



    public void OnPointerDown(PointerEventData eventData)
    {
        
        endZone.indexZone = indexZone;
        endZone.set_color(colorZone);
        Debug.Log(indexZone + " index zone");
    }

}
