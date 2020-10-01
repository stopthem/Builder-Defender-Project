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
    private BuildingTypeSo activeBuildingType;
    private BuildingTypeSOList buildingTypeList;
    private Camera mainCamera;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mainCamera = Camera.main;

        buildingTypeList = Resources.Load<BuildingTypeSOList>(typeof(BuildingTypeSOList).Name);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType,UtilsClass.GetMouseWorldPosition()))
            {
                Instantiate(activeBuildingType.Prefab,UtilsClass.GetMouseWorldPosition(),Quaternion.identity);
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
    private bool CanSpawnBuilding( BuildingTypeSo buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
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
                return true;
            }
        }
        return false;
        
    }
}
