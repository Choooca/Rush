using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlManager : MonoBehaviour
{

    private static LvlManager _Instance;

    [SerializeField] public List<GameObject> levels;

    public static GameObject currentLvl;

    private void Awake()
    {
        if (_Instance != null) Destroy(this);
        else _Instance = this;
    }

    public static LvlManager GetInstance() 
    {
        if (_Instance == null) return new LvlManager();
        return _Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public LevelSetting LoadLvl(GameObject pLvlToLoad) 
    {
        currentLvl = pLvlToLoad;
        LevelSetting pLevelSetting = null;
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] == pLvlToLoad)
            {
                levels[i].gameObject.SetActive(true);
                GameManager.GetInstance().InitLevel();
                pLevelSetting = levels[i].GetComponent<LevelSetting>();
            }
            else levels[i].gameObject.SetActive(false);

        }
        return pLevelSetting;
    }

    public void UnloadLevel() 
    {
        if (currentLvl == null) return;
        currentLvl.SetActive(false);
        currentLvl = null;
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }
}
