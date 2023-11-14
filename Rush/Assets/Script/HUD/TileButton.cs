using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    enum TileType 
    {
        Arrow,
        Convoyor,
        Turn,
        Teleport,
        Stop
    }

    enum Side 
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [SerializeField] private TileType _tileType;
    [SerializeField] private Side _side;

    [SerializeField] private GameObject _ArrowPrefab;
    [SerializeField] private GameObject _ConvoyorPrefab;
    [SerializeField] private GameObject _TurnPrefab;
    [SerializeField] private GameObject _TeleportPrefab;
    [SerializeField] private GameObject _StopPrefab;


    private Button _Button;

    private GameObject _tile;
    private float _Angle;

    private TilePlacer _tilePlacer;

    private RectTransform _Rect;

    [SerializeField] private TextMeshProUGUI text;

    private int _Num = 1;

    private void Start()
    {
        text.text = _Num.ToString();

        _Rect = GetComponent<RectTransform>();
        _Button = GetComponent<Button>();
        _Button.onClick.AddListener(SpawnTile);

        _tilePlacer = TilePlacer.GetInstance();

        switch (_tileType)
        {
            case TileType.Arrow:
                _tile = _ArrowPrefab;
                break;
            case TileType.Convoyor:
                _tile = _ConvoyorPrefab;
                break;
            case TileType.Turn:
                _tile = _TurnPrefab;
                break;
            case TileType.Teleport:
                _tile = _TeleportPrefab;
                break;
            case TileType.Stop:
                _tile = _StopPrefab;
                break;
        }

        switch (_side)
        {
            case Side.Left:
                _Angle = 180;
                break;
            case Side.Right:
                _Angle = 0;
                break;
            case Side.Top:
                _Angle = -90;
                break;
            case Side.Bottom:
                _Angle = 90;
                break;
        }

        _Rect.Rotate(new Vector3(0,0, _Angle - 90));
    }

    public void RemoveTile() 
    {
        _Num -= 1;
        text.text = _Num.ToString();
    }

    private void SpawnTile()
    {
        if(_Num > 0) _tilePlacer.ChangeCurrentObject(this, Instantiate(_tile), _Angle);

        
    }
}   
