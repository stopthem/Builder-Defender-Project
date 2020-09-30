using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingTypeList")]
public class BuildingTypeSOList : ScriptableObject
{
    public List<BuildingTypeSo> list;
}
