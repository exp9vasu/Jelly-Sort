using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "JellySort/LevelData", order = 1)]

[Serializable]
public class ListWrapper<T>
{
    public List<T> list;
}
public class LevelData : ScriptableObject
{
    public int maxCapacityPerContainer;
    public int numberOfContainers;

    public List<ListWrapper<GameObject>> LevelInfo = new List<ListWrapper<GameObject>>(); 

}
