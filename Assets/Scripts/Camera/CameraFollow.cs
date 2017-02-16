using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;    //this refers to the position of the player, we will be dragging and dropping thr player game object onto this
    public float smoothing = 5f;

    Vector3 offset;

    void Start(){
        offset = transform.position - target.position;   //distance between camera and player at start
    }

    void FixedUpdate(){     //we're using fixedUpdate since we're following a physics object with the camera, and the player is using fixedupdate
        Vector3 targetCamPos = target.position + offset; //where we want the camera to be as it updates. recall it starts off at a specified point
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime); //(current position, final position, speed)
                                                    // Lerp moves  between those two positions smoothly.
    }
}
