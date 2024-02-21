using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public GameObject boxPrefab;
    public int m, n;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBoxes(m, n);
    }

    // Update is called once per frame
    void Update()
    {
        
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

                // Spawn box at calculated coordinates
                Instantiate(boxPrefab, new Vector3(x, y, z), Quaternion.identity);
            }
        }
    }

}
