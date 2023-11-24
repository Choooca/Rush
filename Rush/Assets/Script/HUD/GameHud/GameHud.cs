using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    private static GameHud _Instance;

    [SerializeField] private GameObject TileButtonContainer;
    [SerializeField] private GameObject tileButtonPrefab;

    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    public static bool isPause = false;

    private Sprite _ArrowSprite;
    private Sprite _StopperSprite;
    private Sprite _TurnSprite;
    private Sprite _ConveyorSprite;

    private List<GameObject> buttonList = new List<GameObject>();

    [SerializeField] private Slider _SpeedSlider;

    public static GameHud GetInstance() 
    {
        if (_Instance == null) return new GameHud();
        return _Instance;
    }

    private void OnEnable()
    {
        isPause = false;
        PauseMenu.SetActive(false);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    private void Awake()
    {
        if (_Instance != null) Destroy(this);
        else _Instance = this;

        _ArrowSprite = Resources.Load<Sprite>("Texture/Tiles/Arrow");
        _ConveyorSprite = Resources.Load<Sprite>("Texture/Tiles/Conveyor");
        _StopperSprite = Resources.Load<Sprite>("Texture/Tiles/Stopper");
        _TurnSprite = Resources.Load<Sprite>("Texture/Tiles/Turnstile");
    }

    public void AddButton(int pNum, TileButton.Side pSide, TileButton.TileType pImage) 
    {
        Sprite lSpriteToLoad = null;

        switch (pImage)
        {
            case TileButton.TileType.Arrow:
                lSpriteToLoad = _ArrowSprite;
                break;
            case TileButton.TileType.Convoyor:
                lSpriteToLoad = _ConveyorSprite;
                break;
            case TileButton.TileType.Turn:
                lSpriteToLoad = _TurnSprite;
                break;
            case TileButton.TileType.Stop:
                lSpriteToLoad = _StopperSprite;
                break;
            default:
                break;
        }

        GameObject lButton = Instantiate(tileButtonPrefab, TileButtonContainer.transform);
        lButton.GetComponentInChildren<Image>().sprite = lSpriteToLoad;
        lButton.GetComponent<TileButton>()._Num = pNum;
        lButton.GetComponent<TileButton>()._side = pSide;
        lButton.GetComponent<TileButton>()._tileType = pImage;
        lButton.GetComponent<TileButton>().Init();

        buttonList.Add(lButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
            isPause = !isPause;
            if (PauseMenu.activeInHierarchy) Time.timeScale = 0.0f;
            else Time.timeScale = 1.0f;
        }
    }

    public void ShowLoseScreen() 
    {
        LoseScreen.SetActive(true);
    }

    public void ChangeSpeed() 
    {
        GameManager.gameSpeed = 1 + _SpeedSlider.value;
    }

    public void GoMenu() 
    {
        GameFlowManager.GetInstance().SetModeTitleCard();
        LvlManager.GetInstance().UnloadLevel();
        GameManager.GetInstance().ClearGame();
    }

    public void ResetHud() 
    {
        foreach(GameObject lButton in buttonList) DestroyImmediate(lButton);
        isPause = false;
        Time.timeScale = 1.0f;
        _SpeedSlider.value = 0.0f;
        PauseMenu.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    public void ResetTiles() 
    {
        if (GameManager.isPlaying) return;
        GameManager.GetInstance().ResetTiles();
    }

    public void ShowWinScreen() 
    {
        WinScreen.SetActive(true);
        isPause = !isPause;
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }
}
