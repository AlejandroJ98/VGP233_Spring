using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;

    private float startFOV, targetFOV;//set the variable for zoom in for guns

    public float zoomSpeed = 1.0f;

    public Camera theCam;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startFOV = theCam.fieldOfView;
        targetFOV = startFOV;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);//smooth movement of changing camera
    }

    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom;
    }

    public void ZoomOut()
    {
        targetFOV = startFOV;
    }
}
