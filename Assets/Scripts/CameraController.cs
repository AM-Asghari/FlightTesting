using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Array of transforms representing camera positions")]
    [SerializeField] Transform[] povs;
    [Tooltip("The speed at which the camera follows the plane")]
    [SerializeField] float speed;

    private int index = 0;
    private Vector3 target;

    private void Update()
    {
        //Numbers 0-3 represent different povs
        if (Input.GetKeyDown(KeyCode.Mouse0)) index ++;
        if (Input.GetKeyDown(KeyCode.Mouse1)) index --;
        //set target to relative pov
        target = povs[index].position;
    }

    private void FixedUpdate()
    {
        //Move camera to desired position/orientation. Must be in FixedUpdate to avoid camera jitters
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        transform.forward = povs[index].forward;
    }
}
