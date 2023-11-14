using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    private static TilePlacer _Instance;  
    
    private GameObject objectToPlace;
    [SerializeField] private LayerMask groundMask = 6;

    private GameObject currentObject;

    private TileButton currentButton;

    public TilePlacer()
    {
        if (_Instance != null) Destroy(this);
        else _Instance = this;
    }

    void Start()
    {

        if(objectToPlace != null) currentObject = Instantiate(objectToPlace);
    }

    public static TilePlacer GetInstance()
    {
        if (_Instance == null)
        {
            GameObject myObject = new GameObject();
            _Instance = myObject.AddComponent<TilePlacer>();
            myObject.name = "Tile Placer";

        }
        return _Instance;
    }

    void Update()
    {
        if (currentObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray,out RaycastHit hitInfo, 1000f,groundMask)) 
        {
            currentObject.transform.position = hitInfo.transform.position + Vector3.up / 2f;

            Physics.Raycast(ray, out RaycastHit isAnotherTile);

            if (Input.GetMouseButtonDown(0) && isAnotherTile.collider.gameObject.layer != 3)  
            {
                currentObject.GetComponent<BoxCollider>().enabled = true ;
                currentObject = null;
                currentButton.RemoveTile();
            }
        }
        else currentObject.transform.position = new Vector3(Input.mousePosition.x, currentObject.transform.position.y, Input.mousePosition.z);
    }

    public void ChangeCurrentObject(TileButton pCurrentButton, GameObject pNextObject, float pAngle) 
    {
        if(currentObject != null) Destroy(currentObject);

        currentButton = pCurrentButton;
        currentObject = pNextObject;
        currentObject.transform.rotation = Quaternion.AngleAxis(pAngle, Vector3.up);
        currentObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }


}
