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
            if (activeBuildingType != null)
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
}
