using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private LayerMask groundMask;

    private GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        currentObject = Instantiate(objectToPlace);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hitInfo)) 
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.green) ;
                Debug.Log(hitInfo.collider.gameObject.layer == 6);
            if (hitInfo.collider.gameObject.layer == 6)
            {
                currentObject.transform.position = hitInfo.transform.position + Vector3.up / 2f;
            }
        }
    }
}
