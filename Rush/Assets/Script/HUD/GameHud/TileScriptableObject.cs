using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    public List<TileButton.TileType> tileList;

    public List<TileButton.Side> sideList;

    public List<int> nTileList;

    

}
