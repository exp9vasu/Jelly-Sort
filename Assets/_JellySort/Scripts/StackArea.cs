// StackArea.cs
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StackArea : MonoBehaviour
{
    public int StackIndex;
    public bool isFilledWithSameColor; // Indicates if the stack area is filled with cubes of the same color
    public List<CubeScript> stackList = new List<CubeScript>();

    public int cubeCount;

    private void OnTriggerEnter(Collider other)
    {
        CubeScript cubeScript = other.GetComponent<CubeScript>();

        if (cubeScript != null)
        {
            // Cube entered the stack area
            cubeCount++;

            if (!stackList.Contains(cubeScript))
            {
                stackList.Add(cubeScript);
            }
            CheckAndMarkFilled();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CubeScript cubeScript = other.GetComponent<CubeScript>();

        if (cubeScript != null)
        {
            // Cube exited the stack area
            cubeCount--;

            if (stackList.Contains(cubeScript))
            {
                stackList.Remove(cubeScript);
            }
            CheckAndMarkFilled();
        }
    }

    private void CheckAndMarkFilled()
    {
        if (cubeCount >= 3)
        {
            
        }
    }

}
