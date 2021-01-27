using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private int rubikSize;
    [SerializeField]
    private GameObject rubikObject;
    [SerializeField]
    private GameObject rubikBox;
    private float boxSpacing;
    [SerializeField]
    private Material[] rubikColors;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private GameObject buttonCanvas;
    [SerializeField]
    private GameObject buttonContainer;

    void Start(){
        Application.targetFrameRate = 60;
        boxSpacing = 1.05f;
        CreateCube(rubikSize);
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

    //paint appropriate cube faces
    void PaintCube(int n, int i, int j, int k, Transform x){
        if(i == 0){
            var y = x.GetChild(4);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[2];
        }
        else if(i == n - 1){
            var y = x.GetChild(2);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[3];
        }
        if(j == 0){
            var y = x.GetChild(5);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[4];
        }
        else if(j == n - 1){
            var y = x.GetChild(0);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[5];
        }
        if(k == 0){
            var y = x.GetChild(1);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[1];
        }
        else if(k == n - 1){
            var y = x.GetChild(3);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[0];
        }
    }

    //Instantiate all the boxes that the Rubik's Cube uses
    //Also paint appropriate faces
    GameObject InstantiateCubes(int n){
        var boxes = new GameObject("Boxes");
        boxes.transform.parent = rubikObject.transform;

        float curr1 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2));
        for(int i = 0; i < n; i++){
            float curr2 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2));
            for(int j = 0; j < n; j++){
                float curr3 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2));
                for(int k = 0; k < n; k++){
                    var x = Instantiate(rubikBox, new Vector3(curr1, curr2, curr3), Quaternion.identity, boxes.transform);
                    //paint appropriate cube faces
                    PaintCube(n, i, j, k, x.transform);
                    curr3 -= boxSpacing;
                }
                curr2 -= boxSpacing;
            }
            curr1 -= boxSpacing;
        }

        return boxes;
    }

    //Create a rubik's cube size N x N
    public void CreateCube(int n){
        var boxes = InstantiateCubes(n);
        
        //Create Rotator Objects
        var rotations = new GameObject("Rotations");
        rotations.transform.parent = rubikObject.transform;

        var curr1 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2)); //position for each cube
        for(int i = 0; i < n; i++){
            var x = new GameObject("RotatorX");
            x.transform.parent = rotations.transform;
            x.transform.localPosition = new Vector3(curr1, 0, 0);
            var y = x.AddComponent(typeof(Rotate)) as Rotate;
            y.boxes = boxes.transform;
            y.rotationSpeed = rotationSpeed;
            y.rotateX = true;
            curr1 -= boxSpacing;
        }

        curr1 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2));
        for(int i = 0; i < n; i++){
            var x = new GameObject("RotatorY");
            x.transform.parent = rotations.transform;
            x.transform.localPosition = new Vector3(0, curr1, 0);
            var y = x.AddComponent(typeof(Rotate)) as Rotate;
            y.boxes = boxes.transform;
            y.rotationSpeed = rotationSpeed;
            y.rotateY = true;
            curr1 -= boxSpacing;
        }

        curr1 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2));
        for(int i = 0; i < n; i++){
            var x = new GameObject("RotatorZ");
            x.transform.parent = rotations.transform;
            x.transform.localPosition = new Vector3(0, 0, curr1);
            var y = x.AddComponent(typeof(Rotate)) as Rotate;
            y.boxes = boxes.transform;
            y.rotationSpeed = rotationSpeed;
            y.rotateZ = true;
            curr1 -= boxSpacing;
        }

        //
        //Create Canvas and Button Objects
        //
        var canvases = new GameObject("Canvases");
        canvases.transform.SetParent(rubikObject.transform, false);

        //Instantiate canvas prefab
        curr1 = (boxSpacing * (n / 2)) - ((boxSpacing / 2) * ((n + 1) % 2)) + 0.5f;
        float converter = 32.258064516129032258064516129032f;
        var comp = Instantiate(buttonCanvas, new Vector3(0, 0, -curr1 - 0.005f), Quaternion.identity);
        comp.transform.SetParent(canvases.transform, false);

        //resize activator button (child of canvas)
        var a1 = comp.transform.GetChild(0);
        a1.GetComponent<RectTransform>().sizeDelta = new Vector2(curr1 * converter * 2, curr1 * converter * 2);

        comp.transform.GetChild(1).gameObject.SetActive(true);

        //resize maintain ui button (child of hidden ui)
        var a2 = comp.transform.GetChild(1).GetChild(0);
        a2.GetComponent<RectTransform>().sizeDelta = new Vector2((curr1 + boxSpacing) * converter * 2, (curr1 + boxSpacing) * converter * 2);

        //resize and place button row object
        var a3 = comp.transform.GetChild(1).GetChild(1);
        a3.GetComponent<RectTransform>().sizeDelta = new Vector2(converter, curr1 * converter * 2);
        a3.GetComponent<RectTransform>().localPosition = new Vector3(-(curr1 - 0.5f + boxSpacing) * converter, 0, 0);
        
        //Instantiate and place button prefabs for *one* row
        float curr2 = curr1 - 0.5f;
        for(int i = 0; i < n; i++){
            var a4 = Instantiate(buttonContainer);
            a4.transform.SetParent(a3.transform, false);
            a4.transform.localPosition = new Vector3(0, curr2 * converter, 0);
            curr2 -= boxSpacing;
            // var z = rotations.transform.GetChild(i);
            // a4.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => z.GetComponent<Rotate>().RotateCubeOrbit(false));
        }

        //Clone row objects
        var a5 = new GameObject("Temp");
        a5.transform.SetParent(a2.parent, false);
        a3.SetParent(a5.transform, false);
        float w = 90f;
        for(int i = 0; i < 3; i++){
            var a6 = Instantiate(a5);
            a6.transform.SetParent(a2.parent, false);
            a6.transform.RotateAround(a6.transform.position, transform.forward, w);
            w += 90f;
            a6.transform.GetChild(0).transform.SetParent(a2.parent,true);
            Destroy(a6);
        }
        a5.transform.GetChild(0).transform.SetParent(a2.parent, false);
        Destroy(a5);

        //Clone canvass objects
        var a7 = Instantiate(comp, new Vector3(0,0,0), Quaternion.identity, comp.transform.parent);
        a7.transform.RotateAround(rubikObject.transform.position, transform.up, 90f);
        
        
        //--
        //set up function calls in buttons
        //--
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
