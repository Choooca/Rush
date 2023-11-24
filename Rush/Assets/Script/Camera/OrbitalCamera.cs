using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{

    [SerializeField] private Transform _PointToLook;
    [SerializeField] private float verticalAngle;
    [SerializeField] public float horizontalAngle { get; private set; }
    [SerializeField] private float Distance;

    private float moveSpeed = 5f;

    void Update()
    {
        float x = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Cos(Mathf.Deg2Rad * horizontalAngle);
        float y = Distance * Mathf.Sin(Mathf.Deg2Rad * verticalAngle);
        float z = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Sin(Mathf.Deg2Rad * horizontalAngle);
        transform.position = new Vector3(x,y,z) ;
        transform.LookAt( _PointToLook );

        horizontalAngle += moveSpeed * Input.GetAxis("Horizontal");
        verticalAngle += moveSpeed * Input.GetAxis("Vertical");
        verticalAngle = Mathf.Clamp(verticalAngle, -45, 45);
    }
}
