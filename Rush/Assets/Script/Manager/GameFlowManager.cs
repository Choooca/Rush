using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager _Instance;

    public enum Mode 
    {
        TiltedCard,
        LevelSelector,
        Game
    }

    public static Mode currentMode { get; private set; }

    public static GameFlowManager GetInstance() 
    {
        if(_Instance == null) _Instance = new GameFlowManager();
        return _Instance;
    }

    private void Start()
    {
        if (_Instance != null) Destroy(this);
        _Instance = this;
        HudManager.GetInstance().ShowTC();
    }

    public void SetModeTitleCard() 
    {
        HudManager.GetInstance().ShowTC();
        currentMode = Mode.TiltedCard;
    }
    public void SetModeLevelSelector()
    {
        HudManager.GetInstance().ShowLvlSelector();
        currentMode = Mode.LevelSelector;
    }

    public void SetModeGame(GameObject pLevelToLoad)
    {
        if (pLevelToLoad == null) return;
        currentMode = Mode.Game;
        HudManager.GetInstance().ShowGame();
        TileScriptableObject lvlSetting = LvlManager.GetInstance().LoadLvl(pLevelToLoad).tileSettings;

        for (int i = 0; i < lvlSetting.nTileList.Count; i++)
        {
            GameHud.GetInstance().AddButton(lvlSetting.nTileList[i], lvlSetting.sideList[i], lvlSetting.tileList[i]);

        }

    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }


}
