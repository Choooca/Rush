using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private LayerMask _PlateLayer;

    [Range(0.001f, 5f)]
    [SerializeField]
    public float _TickDuration;
    private float _currentTickDuration;
    private float _Ratio;

    [SerializeField] private LayerMask _groundMask;

    private Animator _Animator;
    [SerializeField] private GameObject _CubeGraphics;

    private Action DoAction;
    public UnityAction TickAction;

    private bool wasOnConvoyor = false;

    private Quaternion _BaseRotation;
    private Vector3 _BasePos;
    private Vector3 _Pivot;
    private Vector3 _MovementDir;
    public Vector3 _Direction;
    private Vector3 _ConvoyeurDir;

    private int _tickToWait = 0;
    private int _count;

    [SerializeField] private LayerMask planeLayer;

    private float _Timer;

    public static int _tickToSpawn = 8;

    public static List<CubeMovement> list = new List<CubeMovement>();

    private Vector3 _TeleportPos;

    [HideInInspector] public int cubeType;

    void Start()
    {
        SetModeSpawn();
        list.Add(this);
        GetComponentInChildren<Renderer>().material.color = GameManager.GetInstance().colorType[cubeType % GameManager.GetInstance().colorType.Length];
        _Animator = GetComponentInChildren<Animator>();
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) GameManager.GetInstance().GameOver();
        if (other.gameObject.layer != gameObject.layer) return;
        if (other.gameObject.GetComponent<BoxCollider>().isTrigger && GetComponent<BoxCollider>().isTrigger)
        {
            GameManager.GetInstance().GameOver();
        }
    }

    void Update()
    {
        if (DoAction != null) DoAction();

        if (_Timer >= _currentTickDuration)
        {
            TickAction();
            _Timer = 0;
        }
        _Animator.speed = GameManager.gameSpeed;
        _Timer += Time.deltaTime * GameManager.gameSpeed ;
        _Ratio = _Timer / _currentTickDuration;

    }

    private void SetPivotAndRot()
    {
        _Pivot = transform.position + new Vector3(_Direction.x, -1f, _Direction.z) / 2f;
        _BaseRotation = transform.rotation;
    }

    private void CheckWall()
    {
        while (Physics.Raycast(transform.position, _Direction, 1f, _groundMask))
        {
            _Direction = Quaternion.AngleAxis(90, Vector3.up) * _Direction;
            SetModeHitWall();
        }
    }

    #region Tile Stuff

    private bool CheckTile()
    {
        Ray lRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(lRay, out RaycastHit hitInfo, 1.1f, _PlateLayer))
        {
            string tile = hitInfo.collider.tag.ToString();

            switch (tile)
            {
                case "Target":
                    if ((hitInfo.collider.GetComponent<Target>()).cubeType != cubeType) break;
                    GameManager.GetInstance().CubeValid();
                    Destroy(gameObject);
                    break;
                case "Arrow":
                    _Direction = hitInfo.collider.transform.rotation * Vector3.forward;
                    break;
                case "Convoyeur":
                    _ConvoyeurDir = hitInfo.collider.transform.rotation * Vector3.forward;
                    wasOnConvoyor = true;
                    SetModeConvoyeur();
                    break;
                case "Turn":
                    _Direction = Quaternion.AngleAxis(90 * hitInfo.collider.gameObject.GetComponent<FractionneurPlate>().TurnSide, Vector3.up) * _Direction;
                    hitInfo.collider.gameObject.GetComponent<FractionneurPlate>().TurnSide *= -1;
                    CheckWall();
                    break;
                case "Stop":
                    SetModeStop();
                    break;
                case "Teleporters":
                    _TeleportPos = hitInfo.collider.GetComponent<Teleporters>().teleporters.position;
                    SetModeTeleport();
                    break;

            }
            return true;
        }
        return false;
    }

    #endregion

    #region Tick Stuff

    private void SpawnTick()
    {
        if (_count >= _tickToSpawn)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            SetModeMove();
            SetPivotAndRot();
        }
        _count++;
    }

    private void MovementTick()
    {
        if (wasOnConvoyor)
        {
            _Direction = _MovementDir;
            wasOnConvoyor = false;
        }

        if (!CheckTile()) CheckWall();
        SetPivotAndRot();
        Ray lRay = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(lRay, 1.1f, _groundMask)) SetModeFall();
    }

    private void FallTick()
    {
        CheckTile();

        _BasePos = transform.position;
        Ray lRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(lRay, 1.1f, _groundMask))
        {
            _Pivot = transform.position + new Vector3(_Direction.x, -1f, _Direction.z) / 2f;
            _BaseRotation = transform.rotation;
            if(TickAction == FallTick)SetModeMove();
            CheckWall();
        }
    }

    private void ConvoyeurTick()
    {
        if (_count == 0)
        {
            if (!CheckTile()) CheckWall();
            DoAction = DoActionVoid;
            MovementTick();
        }
        if (_count == 1)
        {
            SetModeMove();
            MovementTick();
        }
        _count++;
    }

    private void WaitTick()
    {
        if (_count >= _tickToWait)
        {
            MovementTick();
            SetModeMove();
        }
        _count++;
    }

    private void StopTick() 
    {
        if(_count >= _tickToWait)
        {
            SetModeMove();
            CheckWall();
        }
        _count++;
    }

    private void TeleportTick()
    {
        if (_count == 1)
        {
            transform.position = _TeleportPos + Vector3.up * 1 * .5f;
        }
        if(_count == 2) 
        {
            CheckWall();
            _Pivot = transform.position + new Vector3(_Direction.x, -1f, _Direction.z) / 2f;
            _BaseRotation = transform.rotation;
            Ray lRay = new Ray(transform.position, Vector3.down);
            if (!Physics.Raycast(lRay, 1.1f, _groundMask))
            {
                SetModeFall();
            }
            DoAction = DoActionMove;
        }
        if (_count >= _tickToWait)
        {
            MovementTick();
            SetModeMove();
        }
        _count++;
    }
    #endregion

    #region State Machine

    private void SetModeVoid()
    {
        DoAction = DoActionVoid;
    }

    private void DoActionVoid() { }

    private void SetModeSpawn()
    {
        _currentTickDuration = _TickDuration;
        TickAction = SpawnTick;
        _count = 0;

        DoAction = DoActionSpawn;
    }

    private void DoActionSpawn() { }

    private void SetModeMove()
    {
        _currentTickDuration = _TickDuration;
        DoAction = DoActionMove;
        TickAction = MovementTick;
    }

    private void DoActionMove()
    {
        transform.position = _Pivot + Vector3.Slerp(new Vector3(-_Direction.x, 1f, -_Direction.z) / 2f, new Vector3(_Direction.x, 1f, _Direction.z) / 2f, Mathf.Clamp(_Ratio, 0, 1));
        transform.rotation = Quaternion.Lerp(_BaseRotation, Quaternion.AngleAxis(-90, Vector3.Cross(_Direction, Vector3.up)) * _BaseRotation, Mathf.Clamp(_Ratio, 0, 1));
    }

    private void SetModeFall()
    {
        FallTick();
        _currentTickDuration = _TickDuration / 2f;
        DoAction = DoActionFall;
        TickAction = FallTick;
    }

    private void DoActionFall()
    {
        transform.position = _BasePos + Vector3.Lerp(Vector3.zero, Vector3.down, Mathf.Clamp(_Ratio, 0, 1));
    }

    private void SetModeHitWall()
    {
        _Animator.SetTrigger("HitWall");
        _tickToWait = 1;
        _count = 0;
        DoAction = DoActionVoid;
        TickAction = WaitTick;
    }

    private void SetModeConvoyeur()
    {
        _MovementDir = _Direction;
        _Direction = _ConvoyeurDir;

        TickAction = ConvoyeurTick;
        DoAction = DoActionMove;

        _tickToWait = 1;
        _count = 0;
    }

    private void SetModeStop()
    {
        DoAction = DoActionVoid;
        TickAction = StopTick;

        _count = 0;
        _tickToWait = 1;
    }

    private void SetModeTeleport()
    {
        if (TickAction == ConvoyeurTick) _Direction = _MovementDir;
        DoAction = DoActionVoid;
        TickAction = TeleportTick;

        _count = 0;
        _tickToWait = 3;
    }

    #endregion

    private void OnDestroy()
    {
        list.Remove(this);
    }

}
