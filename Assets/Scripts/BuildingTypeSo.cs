﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSo : ScriptableObject
{
    public string nameString;
    public Transform Prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str +="<color=#"+ resourceAmount.resourceType.colorHex +">"+ resourceAmount.resourceType.nameShort + resourceAmount.amount + "</color>";
        }
        return str;
    }
}
