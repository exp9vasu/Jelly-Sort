using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject containerParent; // Reference to your Container prefab
    public LevelData[] levels;
    public GameObject[] Jelly;
    public int containerCount;
    public GameObject containerPrefab;

    public GameObject SelectedContainer;
    public GameObject SelectedCube;
    public List<GameObject> ContainerSequence = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadLevel(1);
    }

    // Method to load and generate levels
    public void LoadLevel(int levelIndex)
    {
        LevelData level = levels[levelIndex];

        int outerCount = level.LevelInfo.Count;
        containerCount = outerCount;

        float spacing = 2f;
        float zSpacing = 4f;

        // Generate containers based on level data
        for (int i = 0; i < outerCount; i++)
        {
            float xPos = (outerCount - 1) * -spacing / 2 + i * spacing;
            float zPos;


            if (i > 2)
            {
                zPos = 0;
            }
            else
            {
                zPos = -8;
            }

            GameObject containerObject = Instantiate(containerPrefab, new Vector3(xPos,0,zPos), Quaternion.identity, containerParent.transform);

            //for (int j = 0; j < outerCount; j++)
            {
                ListWrapper<GameObject> wrapper = level.LevelInfo[i];
                int innerCount = wrapper.list.Count;

                for (int j = 0; j < innerCount; j++)
                {
                    //GameObject obj = wrapper.list[j];
                    GameObject obj = Instantiate(wrapper.list[j], new Vector3(xPos, 2 * (j+1), zPos), Quaternion.identity, containerObject.transform);
                    obj.transform.localScale = wrapper.list[j].transform.localScale;

                }
            }

            //Container container = containerObject.GetComponent<Container>();
            //container.maxCapacity = level.maxCapacityPerContainer;
        }


    }
}
