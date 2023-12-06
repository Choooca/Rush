using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractionneurPlate : MonoBehaviour
{
    public static List<FractionneurPlate> list = new List<FractionneurPlate>();

    public int TurnSide = 1;

    private void Start()
    {
        list.Add(this);
    }

    private void OnDestroy()
    {
        list.Remove(this);
    }
}
