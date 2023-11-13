using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngineInternal;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private LayerMask _PlateLayer;

    [Range(0.001f, 5f)]
    [SerializeField]
    private float _TickDuration;

    private TickSample _Sample;

    private Action DoAction;
    public Action TickAction;

    private Quaternion _BaseRotation;
    private Vector3 _BasePos;
    private Vector3 _Pivot;
    private Vector3 _MovementDir;
    private Vector3 _Direction;
    private Vector3 _ConvoyeurDir;

    private int _tickToWait = 0;
    private int _count;

    private Vector3 _TeleportPos;

    void Start()
    {
        _Direction = Vector3.forward;
        _Sample = CubeManager.GetInstance().CreateCoroutine(_TickDuration, this);
        SetModeMove();
    }

    void Update()
    {
        if (DoAction != null) DoAction();

    }

    private void CheckWall() 
    {
        Ray lRay = new Ray(transform.position, _Direction);

        if (Physics.Raycast(lRay, 1f))
        {
            _Direction = Quaternion.AngleAxis(90, Vector3.up) * _Direction;
            SetModeHitWall();
        }
    }

    #region Tile Stuff

    private void CheckTile() 
    {
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(lRay, out RaycastHit hitInfo,1f, _PlateLayer)) 
        {
            string tile = hitInfo.collider.tag.ToString();

            switch (tile) 
            {
                case "Arrow":
                    _Direction = hitInfo.collider.transform.rotation * Vector3.forward;
                    break;
                case "Convoyeur":
                    _ConvoyeurDir = hitInfo.collider.transform.rotation * Vector3.forward;
                    SetModeConvoyeur();
                    break;
                case "Turn":
                    _Direction = Quaternion.AngleAxis(90 * hitInfo.collider.gameObject.GetComponent<FractionneurPlate>().TurnSide,Vector3.up) * _Direction;
                    hitInfo.collider.gameObject.GetComponent<FractionneurPlate>().TurnSide *= -1;
                    break;
                case "Stop":
                    SetModeStop();
                    break;
                case "Teleporters":
                    _TeleportPos = hitInfo.collider.GetComponent<Teleporters>().teleporters.position;
                    SetModeTeleport();
                    break;

            }
        }
    }

    #endregion

    #region Tick Stuff
    private void HitWallTick()
    {
        if(_count >= _tickToWait)SetModeMove();
        _count++;
    }

    private void MovementTick() 
    {
        CheckTile();
        CheckWall();
        _Pivot = transform.position + new Vector3(_Direction.x, -1f, _Direction.z) / 2f;
        _BaseRotation = transform.rotation;
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (!Physics.Raycast(lRay, 1f)) SetModeFall();
    }

    private void FallTick() 
    {
        CheckTile();
        _BasePos = transform.position;
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(lRay, 1f)) SetModeMove();
    }

    private void ConvoyeurTick() 
    {
        _count++;
        if (_count >= _tickToWait)
        {
            _Direction = _MovementDir;
            SetModeMove();
        }
        MovementTick();
    }

    private void WaitTick() 
    {
        if (_count >= _tickToWait) 
        {
            SetModeMove();
        }
        _count++;
    }

    private void TeleportTick()
    {
        if (_count == 1) 
        {
            transform.position = _TeleportPos + Vector3.up * 1 * .5f;
        }
        if (_count >= _tickToWait) SetModeMove();
        _count++;
    }
    #endregion

    #region State Machine

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
        transform.position = _Pivot + Vector3.Slerp(new Vector3(-_Direction.x, 1f, -_Direction.z) / 2f, new Vector3(_Direction.x, 1f, _Direction.z) / 2f, _Sample._Ratio);
        transform.rotation = Quaternion.Lerp(_BaseRotation, Quaternion.AngleAxis(-90, Vector3.Cross(_Direction, Vector3.up)) * _BaseRotation, _Sample._Ratio);
    }

    private void SetModeFall() 
    {
        FallTick();
        DoAction = DoActionFall;
        TickAction = FallTick;
    }

    private void DoActionFall() 
    {
        transform.position = _BasePos + Vector3.Lerp(Vector3.zero,Vector3.down, _Sample._Ratio);
    }

    private void SetModeHitWall() 
    {
        _tickToWait = 1;
        _count = 0;
        DoAction = DoActionHitWall;
        TickAction = HitWallTick;
    }

    private void DoActionHitWall() 
    {
        
    }

    private void SetModeConvoyeur() 
    {
        _MovementDir = _Direction;
        _Direction = _ConvoyeurDir;

        DoAction = DoActionMove;
        TickAction = ConvoyeurTick;
        _tickToWait = 1;
        _count = 0;
    }

    private void SetModeStop() 
    {
        DoAction = DoActionStop;
        TickAction = WaitTick;

        _count = 0;
        _tickToWait = 1;
    }

    private void DoActionStop() 
    {
        
    }

    private void SetModeTeleport()
    {
        DoAction = DoActionTeleport;
        TickAction = TeleportTick;
        
        _count = 0;
        _tickToWait = 2;
    }

    private void DoActionTeleport()
    {
        
    }

    #endregion
}
