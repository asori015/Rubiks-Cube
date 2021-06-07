using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    public float cameraSpeed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){
            transform.RotateAround(transform.position, transform.right, cameraSpeed);
        }
        if(Input.GetKey("s")){
            transform.RotateAround(transform.position, transform.right, -cameraSpeed);
        }
        if(Input.GetKey("a")){
            transform.RotateAround(transform.position, transform.up, cameraSpeed);
        }
        if(Input.GetKey("d")){
            transform.RotateAround(transform.position, transform.up, -cameraSpeed);
        }
        if(Input.GetKey("q")){
            transform.RotateAround(transform.position, transform.forward, cameraSpeed);
        }
        if(Input.GetKey("e")){
            transform.RotateAround(transform.position, transform.forward, -cameraSpeed);
        }
    }
}
