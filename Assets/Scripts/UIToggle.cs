using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIToggle : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private GameObject[] hiddenUIArray;
    [SerializeField]
    private GameObject[] activatorButtonArray;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private bool uiToggle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ///if activation button
        //turn off all hidden uis
        //turn on all activation buttons
        //turn on specific hidden ui
        //turn off this activation button
        ///if exit button
        //turn off all hidden uis
        //turn on all activation buttons
        foreach(var hiddenUI in hiddenUIArray){
            if(hiddenUI.transform.parent == transform.parent){
                hiddenUI.SetActive(true);
            }
            else{
                hiddenUI.SetActive(false);
            }
        }

        foreach(var activatorButton in activatorButtonArray){
            activatorButton.SetActive(true);
        }


        if(uiToggle){
            gameObject.SetActive(false);
            exitButton.SetActive(true);
        }
        else{
            exitButton.SetActive(false);
        }
    }
}
