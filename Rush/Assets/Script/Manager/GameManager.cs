using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;

    private int _CubeToValid;

    public static bool isPlaying = false;

    public static bool isFinish;

    public static float gameSpeed = 1.0f;

    [SerializeField] public Color[] colorType;

    [SerializeField] private OrbitalCamera _cam;

    public static GameManager GetInstance() 
    {
        if(_Instance == null) return new GameManager();
        return _Instance;
    }

    private void Start()
    {
        if (_Instance != null) Destroy(this);
        else _Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying && GameFlowManager.currentMode == GameFlowManager.Mode.Game) isPlaying = true;
        else if (Input.GetKeyDown(KeyCode.Space) && isPlaying && GameFlowManager.currentMode == GameFlowManager.Mode.Game)
        {
            isPlaying = false;
            ResetCube();
            ResetSpawner();
            InitLevel();
            foreach (FractionneurPlate tile in FractionneurPlate.list) tile.TurnSide = 1;
        }
    }

    public void InitLevel() 
    {
        _CubeToValid = 0;
        foreach (Spawner spawner in Spawner.list) _CubeToValid += spawner.N_CUBE;
    }

    public void GameOver() 
    {
        GameHud.GetInstance().ShowLoseScreen();
        GameHud.isPause = true;
        isFinish = true;
        Time.timeScale = 0.0f;
    }

    public void CubeValid() 
    {
        _CubeToValid -= 1;
        if (_CubeToValid == 0)
        {
            GameHud.GetInstance().ShowWinScreen();
            isFinish = true;
        }
    }

    private void ResetCube() 
    {
        for (int i = CubeMovement.list.Count - 1; i >= 0; i--) Destroy(CubeMovement.list[i].gameObject);
    }

    private void ResetSpawner() 
    {
        foreach (Spawner spawner in Spawner.list) spawner.ResetSpawner();
    }

    public void ResetGame() 
    {
        ClearGame();
        GameFlowManager.GetInstance().SetModeGame(LvlManager.currentLvl);
    }

    public void ClearGame() 
    {
        _CubeToValid = 0;
        isPlaying = false;
        isFinish = false;
        ResetCube();
        ResetTiles();
        ResetSpawner();
        GameHud.GetInstance().ResetHud();
    }

    public void ResetTiles() 
    {
        for (int i = PlacableTiles.list.Count - 1; i >= 0; i--) Destroy(PlacableTiles.list[i].gameObject);
        GameHud.GetInstance().ResetHud();
        GameFlowManager.GetInstance().SetModeGame(LvlManager.currentLvl);
    }

    public void NextLevel() 
    {
        _cam.ResetCam();
        _cam.StopCam();
        VFXManager.GetInstance().ShowGame();
        isFinish = false;
        Time.timeScale = 1;
        _CubeToValid = 0;
        gameSpeed = 1.0f;
        isPlaying = false;
        for (int i = CubeMovement.list.Count - 1; i >= 0; i--) Destroy(CubeMovement.list[i].gameObject);
        ResetTiles();
        foreach (Spawner spawner in Spawner.list) spawner.ResetSpawner();
        GameHud.GetInstance().ResetHud();
        TilePlacer.GetInstance().ResetPlacer();
        GameFlowManager.GetInstance().SetModeGame(LvlManager.GetInstance().levels[LvlManager.GetInstance().levels.IndexOf(LvlManager.currentLvl) + 1]);
    }

    public void ResetLose() 
    {
        GameHud.GetInstance().ResetLose();
        ResetCube();
        ResetSpawner();
        InitLevel();
        foreach (FractionneurPlate tile in FractionneurPlate.list) tile.TurnSide = 1;
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }
}
