using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    enum Side 
    {
        Left, Right, Forward, Back
    }

    public static List<Spawner> list = new List<Spawner>();

    [SerializeField] private Side _side;
    [SerializeField] private int nSpawnTick;
    [SerializeField] private int TICK_BEFORE_SPAWN;
    [SerializeField] private float _TickDuration;
    private int _countBeforeSpawn = 0;

    [SerializeField] public int N_CUBE = 0;
    [SerializeField] private GameObject _cubePrefab;
    private int _cubeSpawned = 0;

    private int _tickCount;

    private float _Angle;

    private float _Timer;

    private void OnEnable() => list.Add(this);

    private void OnDisable() => list.Remove(this);

    [SerializeField] private int cubeType = 0;

    void Start()
    {
        nSpawnTick += CubeMovement._tickToSpawn;

        GetComponentInChildren<MeshRenderer>().material.color = GameManager.GetInstance().colorType[cubeType % GameManager.GetInstance().colorType.Length];

        switch (_side)
        {
            case Side.Left:
                _Angle = -90;
                break;
            case Side.Right:
                _Angle = 90;
                break;
            case Side.Forward:
                _Angle = 0;
                break;
            case Side.Back:
                _Angle = 180;
                break;
            default:
                break;
        }

        
    }

    private void SpawnCube()
    { 
        if(_countBeforeSpawn< TICK_BEFORE_SPAWN) 
        {
            _countBeforeSpawn++;
            return;
        }
        if (_cubeSpawned < N_CUBE && _tickCount <= 0)
        {
            CubeMovement lCube = Instantiate(_cubePrefab, transform.position + Vector3.up * .5f, Quaternion.identity).GetComponent<CubeMovement>();
            lCube._Direction = Quaternion.AngleAxis(_Angle, Vector3.up) * Vector3.forward;
            lCube._TickDuration = _TickDuration;
            lCube.cubeType = cubeType;
            _cubeSpawned++;
            _tickCount = nSpawnTick;
        }
        _tickCount--;

    }

    void Update()
    {
        if (GameFlowManager.currentMode != GameFlowManager.Mode.Game) return;

        if (!GameManager.isPlaying) return;

        if (_Timer >= _TickDuration)
        {
            SpawnCube();
            _Timer = 0;
        }
        _Timer += Time.deltaTime * GameManager.gameSpeed;
    }

    public void ResetSpawner() 
    {
        _tickCount = 0;
        _cubeSpawned = 0;
        _countBeforeSpawn = 0;
        GameManager.isPlaying = false;
    }


}
