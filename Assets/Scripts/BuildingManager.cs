using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {get; private set;}

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs{
        public BuildingTypeSo activeBuildingType;
    }

    [SerializeField] private Building hqBuilding;
    private BuildingTypeSo activeBuildingType;
    private BuildingTypeSOList buildingTypeList;
    private Camera mainCamera;
    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeSOList>(typeof(BuildingTypeSOList).Name);
    }
    private void Start()
    {
        mainCamera = Camera.main;

        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                if (CanSpawnBuilding(activeBuildingType,UtilsClass.GetMouseWorldPosition(),out string errorMessage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                
                    // Instantiate(activeBuildingType.Prefab,UtilsClass.GetMouseWorldPosition(),Quaternion.identity);
                    BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(),activeBuildingType);
                    SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show(" Cannot Afford " + activeBuildingType.GetConstructionResourceCostString(), new TooltipUI.ToolTipTimer {timer = 2f});
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.ToolTipTimer {timer = 2f});
                }
                
            }
            
        }
    }
    public void SetActiveBuildingType(BuildingTypeSo buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs{ activeBuildingType = activeBuildingType});
    }
    public BuildingTypeSo GetActiveBuildingType()
    {
        return activeBuildingType;
    }
    private bool CanSpawnBuilding( BuildingTypeSo buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage =" Area not clear ";
            return false;
        }
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = " Too close to another building with the same type ";
                    return false;
                }
            }
        }
        
        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                return true;
            }
        }
        errorMessage = " Too far from any other building ";
        return false;
        
        
    }
    public Building GetHQBuiliding()
    {
        return hqBuilding;
    }
}
