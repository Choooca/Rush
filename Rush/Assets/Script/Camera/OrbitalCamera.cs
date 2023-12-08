using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{

    [SerializeField] private Transform _PointToLook;
    [SerializeField] private float verticalAngle;
    [SerializeField] public float horizontalAngle { get; private set; }
    [SerializeField] private float Distance;

    [SerializeField] private float minZoom = 50f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomSpeed = 50f;
    [SerializeField] private float moveSpeed = 50f;

    private float baseDistance;
    private float baseHorizontalAngle;
    private float baseVerticalAngle;

    private bool canMove = false;

    private void Start()
    {
        baseVerticalAngle = verticalAngle;
        baseHorizontalAngle = horizontalAngle;
        baseDistance = Distance;
    }

    void Update()
    {

        float x = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Cos(Mathf.Deg2Rad * horizontalAngle);
        float y = Distance * Mathf.Sin(Mathf.Deg2Rad * verticalAngle);
        float z = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Sin(Mathf.Deg2Rad * horizontalAngle);
        transform.position = new Vector3(x,y,z) ;
        transform.LookAt( _PointToLook );

        if (!canMove) return;
        horizontalAngle += moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        verticalAngle += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, -80, 80);

        Distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        Distance = Mathf.Clamp(Distance , maxZoom, minZoom);

    }

    public void ResetCam() 
    {
        Distance = baseDistance;
        horizontalAngle = baseHorizontalAngle;
        verticalAngle = baseVerticalAngle;
    }

    public void FreeCam() => canMove = true;

    public void StopCam() => canMove = false;
}
