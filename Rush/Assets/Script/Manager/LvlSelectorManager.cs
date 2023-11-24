using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlSelectorManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs = new List<GameObject>();
    [SerializeField] private GameObject lvlPreviewsContainer;
    [SerializeField] private List<string> lvlNames;
    private int currentLvl;

    [SerializeField] private TextMeshProUGUI _lvlName;

    [SerializeField] private float _levelSpeed = 5;
    [SerializeField] private float _Distance = 900 ;

    private void Start()
    {
        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            GameObject lvlPreviewContainer = new GameObject();
            lvlPreviewContainer.transform.SetParent(lvlPreviewsContainer.transform);
            lvlPreviewContainer.layer = 5;

            RectTransform lvlPreviewTransform = lvlPreviewContainer.AddComponent<RectTransform>();
            lvlPreviewTransform.localPosition = CalculateSpherCoord(_Distance, 0, ((360*i) / levelPrefabs.Count + 90));
            lvlPreviewTransform.localScale = Vector3.one * 25f;

            GameObject lPreview = Instantiate(levelPrefabs[i], lvlPreviewContainer.transform);
            lPreview.gameObject.SetActive(true);
            lPreview.GetComponent<LvlPreview>().enabled = true;
        }
        _lvlName.text = lvlNames[currentLvl];

    }

    private void Update()
    {

    }

    private Vector3 CalculateSpherCoord(float pDistance, float pVerticalAngle, float pHorizontalAngle) 
    {
        float x = pDistance * Mathf.Cos(Mathf.Deg2Rad * 0) * Mathf.Cos(Mathf.Deg2Rad * pHorizontalAngle);
        float y = pDistance * Mathf.Sin(Mathf.Deg2Rad * 0);
        float z = pDistance * Mathf.Cos(Mathf.Deg2Rad * 0) * Mathf.Sin(Mathf.Deg2Rad * pHorizontalAngle);

        return new Vector3(x, y, z);
    }

    public void PreviousLevel() 
    {
        lvlPreviewsContainer.transform.Rotate(new Vector3(0, -360 / levelPrefabs.Count, 0));
        currentLvl--;
        if (currentLvl == -1) currentLvl = levelPrefabs.Count - 1;
        _lvlName.text = lvlNames[currentLvl];
    }

    public void NextLevel()
    {
        lvlPreviewsContainer.transform.Rotate(new Vector3(0,360/levelPrefabs.Count,0));
        currentLvl++;
        currentLvl = currentLvl % levelPrefabs.Count;
        _lvlName.text = lvlNames[currentLvl];
    }

    public void Play() 
    {
        GameFlowManager.GetInstance().SetModeGame(levelPrefabs[currentLvl]);
    }
}
