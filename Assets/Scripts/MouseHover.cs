using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject turnOnUI;
    [SerializeField]
    private GameObject turnOffUI;
    public static int layer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if(layer == 0){
        //     turnOnUI.SetActive(true);
        //     layer += 1;
        //     turnOffUI.SetActive(false);
        // }
        // Debug.Log("Enter");
        // Debug.Log(layer);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if(layer == 1){
        //     turnOnUI.SetActive(true);
        //     layer -= 1;
        //     turnOffUI.SetActive(false);
        // }
        // Debug.Log("Exiting");
        // Debug.Log(layer);
    }
}
