using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;
    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 5;
            ResourceAmount[] resourceAmountCost = new ResourceAmount[] {new ResourceAmount{ resourceType = goldResourceType, amount = repairCost}};
            if(ResourceManager.Instance.CanAfford(resourceAmountCost))
            { 
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            }
            else
            {
                TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.ToolTipTimer { timer = 2f});
            }
        });
    }
    private void OnMouseOver()
    {
        
    }
}
