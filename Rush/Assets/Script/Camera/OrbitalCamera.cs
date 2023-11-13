using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{

    [SerializeField] private Transform _PointToLook;
    [SerializeField] private float verticalAngle;
    [SerializeField] private float horizontalAngle;
    [SerializeField] private float Distance;

    private float moveSpeed = .3f;

    // Start is called before the first frame update
    void Start()
    {
    }       

    // Update is called once per frame
    void Update()
    {
        float x = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Cos(Mathf.Deg2Rad * horizontalAngle);
        float y = Distance * Mathf.Sin(Mathf.Deg2Rad * verticalAngle);
        float z = Distance * Mathf.Cos(Mathf.Deg2Rad * verticalAngle) * Mathf.Sin(Mathf.Deg2Rad * horizontalAngle);
        transform.position = new Vector3(x,y,z) ;
        transform.LookAt( _PointToLook );

        horizontalAngle += moveSpeed * Input.GetAxis("Horizontal");
        verticalAngle += moveSpeed * Input.GetAxis("Vertical");
    }
}
