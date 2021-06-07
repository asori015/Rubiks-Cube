using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int rubikSize;
    [SerializeField]
    GameObject rubiksCubePrefab;
    public GameObject exitButton;
    GameObject rubiksCube;
    //Fields for FPS debugging purposes
    // [SerializeField]
    // private TMPro.TextMeshProUGUI fpsDisplay;
    // private int frameCounter;
    // private float timeCounter;

    void Start(){
        Application.targetFrameRate = 60;
        rubiksCube = Instantiate(rubiksCubePrefab);
        rubiksCube.GetComponent<Cube>().CreateCube(rubikSize);
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
        rubiksCube.GetComponent<Cube>().Scramble(moves);
    }

    public void Reset(){
        rubiksCube.GetComponent<Cube>().Reset();
    }
}
