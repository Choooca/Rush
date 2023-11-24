using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    private static TilePlacer _Instance;  
    
    private GameObject objectToPlace;
    [SerializeField] private LayerMask groundMask = 6;
    [SerializeField] private LayerMask tileMask = 6;

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
        if(GameHud.isPause ||GameManager.isPlaying) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit plateHitInfo, 1000f, tileMask))
        {
            string lPlateTag = plateHitInfo.collider.gameObject.tag;
            if (lPlateTag == "Turn" || lPlateTag == "Stop" || lPlateTag == "Arrow" || lPlateTag == "Convoyeur")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    plateHitInfo.collider.gameObject.GetComponent<PlacableTiles>().Delete();
                }
            }
        }

        if (currentObject == null) 
        {
            currentButton = FindValidButton();
            if (currentButton == null) return;
            currentObject = currentButton.CreateTile();
        }

        if (Physics.Raycast(ray,out RaycastHit hitInfo, 1000f)) 
        {

            if (hitInfo.collider.gameObject.layer == 6)
            {
                if(!Physics.Raycast(hitInfo.collider.transform.position, Vector3.up)) 
                {
                    currentObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                    currentObject.transform.position = hitInfo.transform.position + Vector3.up / 2f;

                    if (Input.GetMouseButtonDown(0))
                    {
                        currentObject.GetComponent<BoxCollider>().enabled = true;
                        currentButton.RemoveTile();
                        if (currentButton._Num > 0) currentObject = currentButton.CreateTile();
                        else currentObject = null;
                    }

                    return;
                }
            }
        }
        currentObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private TileButton FindValidButton() 
    {
        foreach (TileButton pButton in TileButton.buttonList) if (pButton._Num > 0) return pButton;
        return null;
    }

    public void ChangeCurrentButton(TileButton pCurrentButton, GameObject pNextObject) 
    {
        if(currentObject != null) Destroy(currentObject);

        currentButton = pCurrentButton;
        currentObject = pNextObject;
    }

    public void ResetPlacer() 
    {
        currentButton = null;
        currentObject = null;
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }


}
