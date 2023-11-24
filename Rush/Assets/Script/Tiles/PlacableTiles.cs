using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableTiles : MonoBehaviour
{
    public TileButton myButton;

    public static List<PlacableTiles> list = new List<PlacableTiles>();

    private void Start()
    {
        list.Add(this);
    }

    public void Delete() 
    {
        myButton.AddTile();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        list.Remove(this);
    }
}
