using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float speed;

    public List<Color> colors = new List<Color>() {Color.red, Color.blue };
    private float count;
    private float lifeTime = 20;

    private void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", colors[Random.Range(0, colors.Count)]) ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Vector3.up * Time.deltaTime;

        count += Time.deltaTime;
        if(count > lifeTime) Destroy(gameObject);
    }
}
