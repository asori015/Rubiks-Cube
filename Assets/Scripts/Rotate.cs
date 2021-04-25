using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform boxes;
    public static bool isRotating;
    private bool imRotating;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;
    public float rotationSpeed;
    private float rotationCount;
    private bool direction;

    void Start(){
        // rotateX = false;
        // rotateY = false;
        // rotateZ = false;
    }

    void Update(){
        if(imRotating){
            rotationCount += rotationSpeed;
            if(rotationCount > 90){
                rotationCount = 90f;
                imRotating = false;
            }

            float dirRotationCount = rotationCount;
            if(direction){
                dirRotationCount *= -1;
            }

            if(rotateX){
                transform.localRotation = Quaternion.Euler(dirRotationCount, 0f, 0f);
            }
            if(rotateY){
                transform.localRotation = Quaternion.Euler(0f, dirRotationCount, 0f);
                }
            if(rotateZ){
                transform.localRotation = Quaternion.Euler(0f, 0f, dirRotationCount);
            }

            if(!imRotating){
                for(int i = 0; i < transform.childCount; i++){
                    transform.GetChild(i).transform.SetParent(boxes);
                    i--;
                }
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                rotationCount = 0f;
                isRotating = false;
            }
        }
    }

    //'direction' in this context means clockwise or counter-clockwise
    public void RotateCubeOrbit(bool direction){
        //do not enter when in mid-rotation
        if(isRotating){
            return;
        }
        else{
            isRotating = true;
            imRotating = true;
            rotationCount = 0;
            this.direction = direction;
        }

        //set up rotation
        if(rotateX){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.x - transform.localPosition.x;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--; //as it turns out childCount becomes smaller after this operation
                }
            }
        }
        else if(rotateY){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.y - transform.localPosition.y;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--;
                }
            }
        }
        else if(rotateZ){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.z - transform.localPosition.z;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--;
                }
            }
        }
    }

    public void RotateCubeOrbitFast(bool direction){
        //set up rotation
        if(rotateX){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.x - transform.localPosition.x;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--;
                }
            }
            transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else if(rotateY){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.y - transform.localPosition.y;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--;
                }
            }
            transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if(rotateZ){
            for(int i = 0; i < boxes.childCount; i++){
                var box = boxes.GetChild(i).transform;
                float diff = box.localPosition.z - transform.localPosition.z;
                if(diff > -0.1f && diff < 0.1f){
                    box.SetParent(transform);
                    i--;
                }
            }
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }

        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).transform.SetParent(boxes);
            i--;
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
