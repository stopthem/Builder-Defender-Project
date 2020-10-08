using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSo buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairButton;
    private void Awake()
    {
        buildingDemolishBtn = transform.Find("BuildingDemolishBtn");
        buildingRepairButton = transform.Find("BuildingRepairButton");
        HideBuildingDemolishBtn();
        HideBuildingRepairButton();
    }
    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDied += HealthSystem_OnDied;
        
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        if (healthSystem.IsFullHealth())
        {
            HideBuildingRepairButton();
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        ShowBuildingRepairButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            healthSystem.Damage(50);
        }
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();
    }
    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }
    private void ShowBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(true);
        }
    }
    private void HideBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(false);
        }
    }
    private void ShowBuildingRepairButton()
    {
        if (buildingRepairButton != null)
        {
            buildingRepairButton.gameObject.SetActive(true);
        }
    }
    private void HideBuildingRepairButton()
    {
        if (buildingRepairButton != null)
        {
            buildingRepairButton.gameObject.SetActive(false);
        }
    }
}
