using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public static HudManager _Instance;

    [SerializeField] private GameObject TCHud;
    [SerializeField] private GameObject LvlSelectorHud;
    [SerializeField] private GameObject GameHud;

    public static HudManager GetInstance() 
    {
        if(_Instance == null) _Instance = new HudManager();
        
        return _Instance;
    }

    private void Awake()
    {
        if (_Instance != null) Destroy(this);
        _Instance = this;
    }

    private void Start()
    {
    }

    public void ShowTC() 
    {
        LvlSelectorHud.SetActive(false);
        GameHud.SetActive(false);
        TCHud.SetActive(true);
    }

    public void ShowLvlSelector() 
    {
        LvlSelectorHud.SetActive(true);
        GameHud.SetActive(false);
        TCHud.SetActive(false);
    }

    public void ShowGame()
    {
        LvlSelectorHud.SetActive(false);
        GameHud.SetActive(true);
        TCHud.SetActive(false);
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }


}
