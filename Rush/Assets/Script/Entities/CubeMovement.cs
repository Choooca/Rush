using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(0.1f, 5f)]
    [SerializeField]
    private float _TickDuration;

    private float _ElapseTime = 0;
    private float _Ratio;

    private Action DoAction;
    private Action TickAction;

    private Quaternion _BaseRotation;
    private Vector3 _BasePos;
    private Vector3 _Pivot;
    private Vector3 _Direction;

    void Start()
    {
        _Direction = Vector3.forward;
        SetModeMove();
    }

    void Update()
    {
        if (DoAction != null) DoAction();

        if (_ElapseTime > _TickDuration)
        {
            if (TickAction != null) TickAction();
            _ElapseTime -= _TickDuration;
        }    
        _ElapseTime += Time.deltaTime;
        _Ratio = _ElapseTime / _TickDuration;

    }

    private void CheckWall() 
    {
        Ray lRay = new Ray(transform.position, _Direction);

        if (Physics.Raycast(lRay, 1f)) _Direction = Quaternion.AngleAxis(90, Vector3.up)* _Direction;
    }

    private void MovementTick() 
    {
        CheckWall();
        _Pivot = transform.position + new Vector3(_Direction.x, -1f, _Direction.z) / 2f;
        _BaseRotation = transform.rotation;
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (!Physics.Raycast(lRay, 1f)) SetModeFall();
            ;
    }

    private void FallTick() 
    {
        _BasePos = transform.position;
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(lRay, 1f)) SetModeMove();
    }

    private void SetModeVoid() 
    {
        DoAction = DoActionVoid;
    }

    private void DoActionVoid() 
    {
        
    }

    private void SetModeMove() 
    {
        MovementTick();
        DoAction = DoActionMove;
        TickAction = MovementTick;
    }

    private void DoActionMove() 
    {;
        transform.position = _Pivot + Vector3.Slerp(new Vector3(-_Direction.x, 1f, -_Direction.z) / 2f, new Vector3(_Direction.x, 1f, _Direction.z) / 2f, _Ratio);
        transform.rotation = Quaternion.Lerp(_BaseRotation, Quaternion.AngleAxis(-90, Vector3.Cross(_Direction, Vector3.up)) * _BaseRotation, _Ratio);
    }

    //bon flemme de faire tout de suite mais pour la fleche qui te fais avancer que de 1 case dans son sens et après t'es re dans ton ancien sens faut te créer une fonction Move(Vector3 direction)
    //et jpense faire un set mode je vois pas de meilleur manière de faire sah au pire je verrai demain. Bon code

    private void SetModeFall() 
    {
        FallTick();
        DoAction = DoActionFall;
        TickAction = FallTick;
    }

    private void DoActionFall() 
    {
        transform.position = _BasePos + Vector3.Lerp(Vector3.zero,Vector3.down, _Ratio);
    }
}
