using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingTypeUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSo> ignoreBuildingTypeList;
    private Dictionary<BuildingTypeSo, Transform> btnTransformDictionary;
    
    private Transform arrowBtn;
    private void Awake()
    {
        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeSOList buildingTypeList = Resources.Load<BuildingTypeSOList>(typeof(BuildingTypeSOList).Name);

        btnTransformDictionary = new Dictionary<BuildingTypeSo, Transform>();

        int index = 0;

        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = +120f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * offsetAmount, 0);

        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30f);

        arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
        BuildingManager.Instance.SetActiveBuildingType(null);
        });
        index ++;
        foreach (BuildingTypeSo buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType))
            {
                continue;
            }
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = +120f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * offsetAmount, 0);

            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });
            btnTransformDictionary[buildingType] = btnTransform;
            index++;
        }
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnactiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
       
    }
    private void BuildingManager_OnactiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }
    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSo buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }
        BuildingTypeSo activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
        
    }
    
}
