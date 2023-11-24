using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlPreview : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Vector3 _PreviousPos;
    private Vector3 _Dir;

    [SerializeField] private float _mouseSpeed = 500;

    private void Start()
    {
        _rectTransform = GetComponentInParent<RectTransform>();

        _PreviousPos = transform.position;
        SetChildrenLayer(5, transform);
    }

    private void SetChildrenLayer(int pLayer, Transform pTransform) 
    {
        foreach (Transform child in pTransform) 
        {
            child.gameObject.layer = pLayer;
            if (child.childCount > 0) SetChildrenLayer(pLayer, child.transform);
        }
    }

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            _Dir = (Input.mousePosition - _PreviousPos).normalized;
            transform.rotation = Quaternion.AngleAxis(_mouseSpeed * Time.deltaTime,Vector3.Cross(_Dir , Camera.allCameras[1].transform.forward)) * transform.rotation;
        }
        else 
        {
            _rectTransform.rotation = Quaternion.AngleAxis(Time.deltaTime * 10, Vector3.up) * _rectTransform.rotation;
        }

        _PreviousPos = Input.mousePosition;
    }
}
