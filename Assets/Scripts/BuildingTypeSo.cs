using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSo : ScriptableObject
{
    public string nameString;
    public Transform Prefab;
}
