using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Rotate[] rotationObjects;
    [SerializeField]
    private GameObject boxes;
    private Dictionary<string, Vector3> savedNames;
    [SerializeField]
    private TMPro.TextMeshProUGUI fpsDisplay;
    private int frameCounter;
    private float timeCounter;

    void Start(){
        Application.targetFrameRate = 60;
        savedNames = new Dictionary<string, Vector3>();
        foreach(Transform child in boxes.transform){
            savedNames.Add(child.gameObject.name, child.gameObject.transform.localPosition);
        }
    }

    void Update()
    {
        //FPS Counter
        // frameCounter += 1;
        // timeCounter += Time.deltaTime;
        // if(timeCounter >= 1.0f){
        //     fpsDisplay.text = "FPS: " + frameCounter;
        //     frameCounter = 0;
        //     timeCounter -= 1f;
        // }
        
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void Scramble(int moves){
        if(Rotate.isRotating){
            return;
        }

        for(int i = 0; i < moves; i++){
            int randInt = Random.Range(0,17);
            if(randInt % 2 == 1){
                rotationObjects[randInt / 2].RotateCubeOrbitFast(true);
            }
            else{
                rotationObjects[randInt / 2].RotateCubeOrbitFast(false);
            }
        }
    }

    public void Reset(){
        if(Rotate.isRotating){
            return;
        }

        foreach(Transform child in boxes.transform){
            child.localPosition = savedNames[child.gameObject.name];
            child.localRotation = Quaternion.Euler(0f,0f,0f);
        }
    }
}
