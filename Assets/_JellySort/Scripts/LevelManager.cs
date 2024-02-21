using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

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
    //public List<GameObject> ContainerSequence = new List<GameObject>();
    public int CurrentLevel = 1;
    public List<GameObject> ContainerList = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadLevel(CurrentLevel);
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
            ContainerList.Add(containerObject);

            //for (int j = 0; j < outerCount; j++)
            {
                ListWrapper<GameObject> wrapper = level.LevelInfo[i];
                int innerCount = wrapper.list.Count;

                for (int j = 0; j < innerCount; j++)
                {
                    //GameObject obj = wrapper.list[j];
                    GameObject obj = Instantiate(wrapper.list[j], new Vector3(xPos, 2 * (j+1), zPos), Quaternion.identity, containerObject.transform);
                    //ContainerList.Add(obj);
                    obj.transform.localScale = wrapper.list[j].transform.localScale;

                }
            }

            //Container container = containerObject.GetComponent<Container>();
            //container.maxCapacity = level.maxCapacityPerContainer;
        }


    }

    public void CheckGameState()
    {
        if (ContainerList.Count == 0)
        {
            return;
        }

        {
            
            // Get the script component of the first element
            var firstScript = ContainerList[0].GetComponent<Container>();

            // Check if all elements have the same script attached
            if (ContainerList.All(ContainerList => ContainerList.GetComponent<Container>() == firstScript))
            {
                // If all elements have the same script, check if SameColorFilled is true for all of them
                if(ContainerList.All(ContainerList => ContainerList.GetComponent<Container>().FilledWithSame))
                {   
                    print("Game Over!");
                }
            }

            // If not all elements have the same script attached, return false
            //return false;
        }

    }
}
