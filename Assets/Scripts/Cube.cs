using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private GameObject boxes;
    [SerializeField]
    private Rotate[] rotationObjects;
    private Dictionary<int, Vector3> originalBoxPositions;
    [SerializeField]
    private int rubikSize; //N x N size of cube
    [SerializeField]
    private GameObject rubikBox; //Prefab for individual Rubik's cube box
    private float boxSpacing; //Spacing between each individual box; 1.0 = no spacing
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Material[] rubikColors;
    [SerializeField]
    private GameObject buttonCanvas;
    [SerializeField]
    private GameObject buttonContainer;

    //paint appropriate cube faces
    void PaintCube(int i, int j, int k, Transform x){
        if(i == 0){
            var y = x.GetChild(4);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[0];
        }
        if(i == rubikSize - 1){
            var y = x.GetChild(2);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[1];
        }
        if(j == 0){
            var y = x.GetChild(5);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[2];
        }
        if(j == rubikSize - 1){
            var y = x.GetChild(0);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[3];
        }
        if(k == 0){
            var y = x.GetChild(1);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[4];
        }
        if(k == rubikSize - 1){
            var y = x.GetChild(3);
            y.gameObject.GetComponent<Renderer>().material = rubikColors[5];
        }
    }

    //Instantiate all the boxes that the Rubik's Cube uses
    //Also paint appropriate faces
    GameObject InstantiateCubes(){
        var boxes = new GameObject("Boxes");
        boxes.transform.parent = gameObject.transform;

        float boxPosX = (boxSpacing * (rubikSize / 2)) - ((boxSpacing / 2) * ((rubikSize + 1) % 2));
        for(int i = 0; i < rubikSize; i++){
            float boxPosY = (boxSpacing * (rubikSize / 2)) - ((boxSpacing / 2) * ((rubikSize + 1) % 2));
            for(int j = 0; j < rubikSize; j++){
                float boxPosZ = (boxSpacing * (rubikSize / 2)) - ((boxSpacing / 2) * ((rubikSize + 1) % 2));
                for(int k = 0; k < rubikSize; k++){
                    var x = Instantiate(rubikBox, new Vector3(boxPosX, boxPosY, boxPosZ), Quaternion.identity, boxes.transform);
                    //paint appropriate cube faces
                    PaintCube(i, j, k, x.transform);
                    boxPosZ -= boxSpacing;
                }
                boxPosY -= boxSpacing;
            }
            boxPosX -= boxSpacing;
        }

        return boxes;
    }

    //Instantiate rotations on specified axis of rubiks cube
    //Helper function for InstantiateRotations()
    void CreateRotationsOnAxis(string name, GameObject rotationsObject, Vector3 axis, Rotate[] rotationsArray, int index){
        var boxPosition = (boxSpacing * (rubikSize / 2)) - ((boxSpacing / 2) * ((rubikSize + 1) % 2)); //position for each cube
        for(int i = 0; i < rubikSize; i++){
            var newRotation = new GameObject(name);
            newRotation.transform.parent = rotationsObject.transform;
            newRotation.transform.localPosition = axis * boxPosition;

            var rotationComponent = newRotation.AddComponent(typeof(Rotate)) as Rotate;
            rotationComponent.boxes = boxes.transform;
            rotationComponent.rotationSpeed = rotationSpeed;
            if(axis.x != 0){
                rotationComponent.rotateX = true;
            }
            else if(axis.y != 0){
                rotationComponent.rotateY = true;
            }
            else{
                rotationComponent.rotateZ = true;
            }
            rotationsArray[index + i] = rotationComponent;
            
            boxPosition -= boxSpacing;
        }
    }

    //Instantiate all rotations for rubiks cube
    Rotate[] InstantiateRotations(){
        Rotate [] rotateArray = new Rotate[rubikSize * 3];
        var rotations = new GameObject("Rotations");
        rotations.transform.parent = gameObject.transform;

        CreateRotationsOnAxis("RotatorX", rotations, new Vector3(1,0,0), rotateArray, 0);
        CreateRotationsOnAxis("RotatorY", rotations, new Vector3(0,1,0), rotateArray, rubikSize);
        CreateRotationsOnAxis("RotatorZ", rotations, new Vector3(0,0,1), rotateArray, rubikSize * 2);

        return rotateArray;
    }

    void CreateButton(Transform a3, float curr2, float converter, int i){
        var button = Instantiate(buttonContainer, a3, false);
        button.transform.localPosition = new Vector3(0, curr2 * converter, 0);
        curr2 -= boxSpacing;
        // a4.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => rotationObjects[i].RotateCubeOrbit(false));
        button.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {rotationObjects[i].RotateCubeOrbit(false);});
    }

    void CreateCanvasHierarchy(GameObject canvasObject){
        //Instantiate canvas prefab
        float canvasPos = (boxSpacing * (rubikSize / 2)) - ((boxSpacing / 2) * ((rubikSize + 1) % 2)) + 0.5f;
        float converter = 1.0f / 0.031f;
        var canvas = Instantiate(buttonCanvas, new Vector3(0, 0, -canvasPos - 0.001f), Quaternion.identity, canvasObject.transform);

        //resize activator button (child of canvas)
        var a1 = canvas.transform.Find("Activator Button");
        a1.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasPos * converter * 2, canvasPos * converter * 2);

        //resize maintain ui button (child of hidden ui)
        // var a2 = canvas.transform.GetChild(1).GetChild(0);
        canvas.transform.Find("Hidden UI").gameObject.SetActive(true);
        var a2 = canvas.transform.Find("Hidden UI").Find("Maintain UI Button");
        a2.GetComponent<RectTransform>().sizeDelta = new Vector2((canvasPos + boxSpacing) * converter * 2, (canvasPos + boxSpacing) * converter * 2);

        //resize and place button row object
        // var a3 = canvas.transform.GetChild(1).GetChild(1);
        var a3 = canvas.transform.Find("Hidden UI").Find("Row");
        a3.GetComponent<RectTransform>().sizeDelta = new Vector2(converter, canvasPos * converter * 2);
        a3.GetComponent<RectTransform>().localPosition = new Vector3(-(canvasPos - 0.5f + boxSpacing) * converter, 0, 0);
        
        //Instantiate and place button prefabs for *one* row
        float curr2 = canvasPos - 0.5f;
        for(int i = 0; i < rubikSize; i++){
            CreateButton(a3, curr2, converter, i);
            // var a4 = Instantiate(buttonContainer, a3.transform, false);
            // a4.transform.localPosition = new Vector3(0, curr2 * converter, 0);
            curr2 -= boxSpacing;
            // // a4.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => rotationObjects[i].RotateCubeOrbit(false));
            // a4.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {rotationObjects[i].RotateCubeOrbit(false);});
            Debug.Log(i);
        }

        //Clone row objects
        // var a5 = new GameObject("Temp");
        // a5.transform.SetParent(a2.parent, false);
        // a3.SetParent(a5.transform, false);
        // float w = 90f;
        // for(int i = 0; i < 3; i++){
        //     var a6 = Instantiate(a5);
        //     a6.transform.SetParent(a2.parent, false);
        //     a6.transform.RotateAround(a6.transform.position, transform.forward, w);
        //     w += 90f;
        //     a6.transform.GetChild(0).transform.SetParent(a2.parent,true);
        //     Destroy(a6);
        // }
        // a5.transform.GetChild(0).transform.SetParent(a2.parent, false);
        // Destroy(a5);
    }

    void InstantiateCanvases(){
        var canvases = new GameObject("Canvases");
        canvases.transform.parent = gameObject.transform;

        CreateCanvasHierarchy(canvases);
        
        //Clone canvass objects

        // var a7 = Instantiate(comp, new Vector3(0,0,0), Quaternion.identity, comp.transform.parent);
        // a7.transform.RotateAround(gameObject.transform.position, transform.up, 90f);
    }

    //Create a rubik's cube size N x N
    //This is the main function that makes the cube
    public void CreateCube(int n){
        boxSpacing = 1.05f;
        rotationSpeed = 2;
        rubikSize = n;
        boxes = InstantiateCubes(); //Create Cubes
        rotationObjects = InstantiateRotations(); //Create Rotator Objects
        InstantiateCanvases();

        originalBoxPositions = new Dictionary<int, Vector3>();
        foreach(Transform child in boxes.transform){
            originalBoxPositions.Add(child.gameObject.GetInstanceID(), child.gameObject.transform.localPosition);
        }
    }

    public void Scramble(int moves){
        if(Rotate.isRotating || rubikSize <= 0){
            return;
        }

        for(int i = 0; i < moves; i++){
            int randInt = Random.Range(0, rubikSize * 6);
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
            child.localPosition = originalBoxPositions[child.gameObject.GetInstanceID()];
            child.localRotation = Quaternion.Euler(0f,0f,0f);
        }
    }
}
