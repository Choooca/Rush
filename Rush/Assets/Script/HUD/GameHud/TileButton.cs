using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    public enum TileType 
    {
        Arrow,
        Convoyor,
        Turn,
        Stop
    }

    public enum Side 
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [HideInInspector] public TileType _tileType;
    [HideInInspector] public Side _side;

    private GameObject _ArrowPrefab;
    private GameObject _ConvoyorPrefab;
    private GameObject _TurnPrefab;
    private GameObject _StopPrefab;


    private Button _Button;

    private GameObject _tile;
    private float _Angle;

    private TilePlacer _tilePlacer;

    [SerializeField] private RectTransform _Rect;

    [SerializeField] private TextMeshProUGUI text;

    public int _Num = 1;

    public static List<TileButton> buttonList = new List<TileButton>();

    private OrbitalCamera mainCamOrbital;
    private Quaternion _baseRotation;

    private void Awake()
    {
        _ArrowPrefab = Resources.Load("Prefabs/Tiles/Arrow") as GameObject;
        _ConvoyorPrefab = Resources.Load("Prefabs/Tiles/Conveyor") as GameObject;
        _TurnPrefab = Resources.Load("Prefabs/Tiles/Turnstile") as GameObject;
        _StopPrefab = Resources.Load("Prefabs/Tiles/Stopper") as GameObject;

        _Button = GetComponentInChildren<Button>();

        _Button.onClick.AddListener(SpawnTile);

        _tilePlacer = TilePlacer.GetInstance();

        buttonList.Add(this);

        mainCamOrbital = Camera.main.GetComponent<OrbitalCamera>();
    }

    public void Init() 
    {
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
            case TileType.Stop:
                _tile = _StopPrefab;
                break;
        }

        switch (_side)
        {
            case Side.Left:
                _Angle = 90;
                break;
            case Side.Right:
                _Angle = -90;
                break;
            case Side.Top:
                _Angle = 180;
                break;
            case Side.Bottom:
                _Angle = 0;
                break;
        }

        text.text = _Num.ToString();

        _Rect.Rotate(new Vector3(0, 0, _Angle + 90 ));
        _baseRotation = _Rect.rotation;
    }

    private void Update()
    {
        _Rect.transform.rotation =_baseRotation * Quaternion.AngleAxis(mainCamOrbital.horizontalAngle , Vector3.back);
    }

    public void RemoveTile() 
    {
        _Num -= 1;
        text.text = _Num.ToString();
    }

    public void AddTile() 
    {
        _Num+= 1;
        text.text = _Num.ToString();
    }

    public void SpawnTile()
    {
        if(_Num > 0) _tilePlacer.ChangeCurrentButton(this, CreateTile());
    }

    public GameObject CreateTile() 
    {
        GameObject lTile = Instantiate(_tile);
        PlacableTiles lPlacableTile = lTile.gameObject.GetComponent<PlacableTiles>();
        lPlacableTile.myButton = this;
        lTile.transform.rotation = Quaternion.AngleAxis(_Angle , Vector3.down);
        lTile.GetComponent<BoxCollider>().enabled = false;
        lTile.GetComponentInChildren<MeshRenderer>().enabled = false;
        return lTile;
    }

    private void OnDestroy()
    {
        buttonList.Remove(this);
    }


}   
