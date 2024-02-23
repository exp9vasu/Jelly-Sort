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
    public int CurrentLevel;
    public List<GameObject> ContainerList = new List<GameObject>();
    public bool GameOver;
    private int RepeatingLevel=3;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameOver = false;

        CurrentLevel = PlayerPrefs.GetInt("LevelIndex");

        //Handle Level Limit Case
        if (CurrentLevel > levels.Length-1)
        {
            PlayerPrefs.SetInt("LevelIndex", RepeatingLevel);
            CurrentLevel = PlayerPrefs.GetInt("LevelIndex");
        }

        LoadLevel(CurrentLevel);
        UIManager.instance.LevelIndex.text = "LEVEL " + (CurrentLevel+1).ToString() ;
    }

    float spacing = 2f;
    int zSpacing = 6;
    float xPos;
    float zPos;

    // Method to load and generate levels
    public void LoadLevel(int levelIndex)
    {
        LevelData level = levels[levelIndex];
        int outerCount = level.LevelInfo.Count;
        containerCount = outerCount;

        int m = levels[CurrentLevel].m;
        int n = levels[CurrentLevel].n;
        int o = levels[CurrentLevel].o;

       

        LevelRowGenerator(0, m, levelIndex, -4-zSpacing);
        LevelRowGenerator(m, m+n, levelIndex, -4);
        LevelRowGenerator(m+n, m+n+o, levelIndex, -4+zSpacing);

    }

    public void LevelRowGenerator(int start, int row, int levelIndex, int ZPos)
    {

        LevelData level = levels[levelIndex];


        // Generate containers based on level data
        for (int i = start; i < row; i++)
        {
            xPos = ((row + start) - 1) * -spacing / 2 + i * spacing;

            //SpawnBoxes(levels[CurrentLevel].m, levels[CurrentLevel].n);

            zPos = ZPos;

            //zPos = ((row + start) - 1) * -zSpacing / 2 + i * zSpacing;

            GameObject containerObject = Instantiate(containerPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, containerParent.transform);
            ContainerList.Add(containerObject);

            //for (int j = 0; j < outerCount; j++)
            {
                ListWrapper<GameObject> wrapper = level.LevelInfo[i];
                int innerCount = wrapper.list.Count;

                for (int j = 0; j < innerCount; j++)
                {
                    //GameObject obj = wrapper.list[j];
                    GameObject obj = Instantiate(wrapper.list[j], new Vector3(xPos, 2 * (j + 1), zPos), Quaternion.identity, containerObject.transform);
                    
                    if(wrapper.list[j].GetComponent<Container>())
                    {
                        ContainerList.Add(wrapper.list[j]);
                    }
                    obj.transform.localScale = wrapper.list[j].transform.localScale;

                }
            }

            //Container container = containerObject.GetComponent<Container>();
            //container.maxCapacity = level.maxCapacityPerContainer;
        }
    }

    public void SpawnBoxes(int m, int n)
    {
        float spacing = 2f; // Spacing between boxes along the x-axis
        float height = 0f; // Height difference between rows along the y-axis
        float depth = 2f; // Spacing between boxes along the z-axis

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                float x = spacing * j; // Calculate x-coordinate
                float y = height * i; // Calculate y-coordinate
                float z = depth * i; // Calculate z-coordinate

                xPos = x;
                zPos = z;

                // Spawn box at calculated coordinates
                //Instantiate(boxPrefab, new Vector3(x, y, z), Quaternion.identity);
            }
        }
    }
    public List<GameObject> newList;

    public void CheckGameState()
    {
        //if (ContainerList.Count == 0)
        //{
        //    return;
        //}

        {
            foreach (GameObject element in ContainerList)
            {
                if (element.GetComponent<Container>().ContainerStack.Count > 0 && !newList.Contains(element))
                {
                    newList.Add(element);
                }
                else
                if (element.GetComponent<Container>().ContainerStack.Count == 0 && newList.Contains(element))
                {
                    newList.Remove(element);
                }
            }

            CheckIsFilledSame(newList);
        }

    }

    void CheckIsFilledSame(List<GameObject> elements)
    {
        if (GameOver)
        {
            return;
        }

        // If the list is empty, return
        if (elements.Count == 0)
        {
            return;
        }

        // Check if all elements have the same isFilledSame boolean property
        foreach (GameObject element in elements)
        {
            // Get the script component attached to the element
            Container script = element.GetComponent<Container>();

            // If the script is null or the isFilledSame property is false, return
            if (script == null || !script.FilledWithSame)
            {
                return;
            }
        }

        GameOver = true;
        Invoke("ShowWin",2);
    }

    public void ShowWin()
    {
        if (!GameOver)
        {
            return;
        }

        // If all elements have isFilledSame set to true, print "Game Over!"
        UIManager.instance.LivePanel.SetActive(false);
        UIManager.instance.WinPanel.SetActive(true);
        Debug.Log("Game Over!");
    }

    public void LoadNextLevel()
    {
        ResetLevel();

        GameOver = false;

        //Handle Level Limit Case
        if (CurrentLevel > levels.Length - 2)
        {
            PlayerPrefs.SetInt("LevelIndex", RepeatingLevel - 1);
            CurrentLevel = PlayerPrefs.GetInt("LevelIndex");
        }

        int temp = PlayerPrefs.GetInt("LevelIndex");
        PlayerPrefs.SetInt("LevelIndex", temp+1);

        CurrentLevel = PlayerPrefs.GetInt("LevelIndex");

        LoadLevel(CurrentLevel);
        UIManager.instance.LevelIndex.text = "LEVEL " + (CurrentLevel + 1).ToString();
        
        UIManager.instance.LivePanel.SetActive(true);
        UIManager.instance.WinPanel.SetActive(false);

    }

    public void RetryLevel()
    {
        ResetLevel();

        GameOver = false;

        CurrentLevel = PlayerPrefs.GetInt("LevelIndex");

        LoadLevel(CurrentLevel);
        UIManager.instance.LevelIndex.text = "LEVEL " + (CurrentLevel + 1).ToString();

        UIManager.instance.LivePanel.SetActive(true);
        UIManager.instance.WinPanel.SetActive(false);

    }

    public void ResetLevel()
    {
        // Destroy all existing containers
        foreach (GameObject container in ContainerList)
        {
            Destroy(container);
        }
        ContainerList.Clear(); // Clear the list of containers
        newList.Clear();
    }
}
