using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float currentXRotation;
    private float currentYRotation;
    private float currentZRotation;
    [SerializeField]
    public float cameraSpeed;

    void Start(){
        currentXRotation = 13f;
        currentYRotation = 20f;
        currentZRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){
            transform.localRotation = Quaternion.Euler(currentXRotation + cameraSpeed, currentYRotation, currentZRotation);
            currentXRotation += cameraSpeed;
        }
        if(Input.GetKey("s")){
            transform.localRotation = Quaternion.Euler(currentXRotation - cameraSpeed, currentYRotation, currentZRotation);
            currentXRotation -= cameraSpeed;
        }
        if(Input.GetKey("a")){
            transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation + cameraSpeed, currentZRotation);
            currentYRotation += cameraSpeed;
        }
        if(Input.GetKey("d")){
            transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation - cameraSpeed, currentZRotation);
            currentYRotation -= cameraSpeed;
        }
        if(Input.GetKey("q")){
            transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, currentZRotation + cameraSpeed);
            currentZRotation += cameraSpeed;
        }
        if(Input.GetKey("e")){
            transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, currentZRotation - cameraSpeed);
            currentZRotation -= cameraSpeed;
        }
    }
}
