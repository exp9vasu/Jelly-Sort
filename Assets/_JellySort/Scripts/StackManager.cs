// StackManager.cs
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    private Dictionary<int, List<GameObject>> stackedCubesByStack = new Dictionary<int, List<GameObject>>();
    private Dictionary<string, List<int>> stackIndicesByColor = new Dictionary<string, List<int>>();
    private Dictionary<string, int> sameColorStackCount = new Dictionary<string, int>();
    private Dictionary<string, int> totalSameColorStackCount = new Dictionary<string, int>();

    public List<StackArea> StackAreaList = new List<StackArea>();
    public int fullStackCount;

    private void OnEnable()
    {
        CubeScript.OnCubeEnteredStack += HandleCubeEnteredStack;
        CubeScript.OnCubeExitedStack += HandleCubeExitedStack;
    }

    private void OnDisable()
    {
        CubeScript.OnCubeEnteredStack -= HandleCubeEnteredStack;
        CubeScript.OnCubeExitedStack -= HandleCubeExitedStack;
    }

    private void HandleCubeEnteredStack(GameObject cube, int stackIndex, string cubeColor)
    {
        if (!stackedCubesByStack.ContainsKey(stackIndex))
        {
            stackedCubesByStack[stackIndex] = new List<GameObject>();
        }

        stackedCubesByStack[stackIndex].Add(cube);

        // Track stack indices by color
        if (!stackIndicesByColor.ContainsKey(cubeColor))
        {
            stackIndicesByColor[cubeColor] = new List<int>();
        }
        stackIndicesByColor[cubeColor].Add(stackIndex);

        Debug.Log($"Cube named \"{cubeColor}\" entered stack {stackIndex}. Stack count: {stackedCubesByStack[stackIndex].Count}");

        // Check if three cubes in the stack have the same color
        CheckStackColor(stackIndex, cubeColor);
    }

    private void HandleCubeExitedStack(GameObject cube, int stackIndex, string cubeColor)
    {
        if (stackedCubesByStack.ContainsKey(stackIndex))
        {
            stackedCubesByStack[stackIndex].Remove(cube);

            // Remove stack index from the color dictionary
            if (stackIndicesByColor.ContainsKey(cubeColor))
            {
                stackIndicesByColor[cubeColor].Remove(stackIndex);
            }

            Debug.Log($"Cube named \"{cubeColor}\" exited stack {stackIndex}. Stack count: {stackedCubesByStack[stackIndex].Count}");

            // Check if three cubes in the stack have the same color
            CheckStackColor(stackIndex, cubeColor);
        }
    }

    private void CheckStackColor(int stackIndex, string cubeColor)
    {
        if (stackedCubesByStack.ContainsKey(stackIndex) && stackedCubesByStack[stackIndex].Count >= 3)
        {
            // Get the first three cubes in the stack
            List<GameObject> firstThreeCubes = stackedCubesByStack[stackIndex].GetRange(0, 3);

            // Check if all three cubes have the same color
            bool allSameColor = firstThreeCubes.TrueForAll(cube => cube.GetComponent<CubeScript>().cubeColor == cubeColor);

            if (allSameColor)
            {

                Debug.Log($"Three cubes in stack {stackIndex} have the same color ({cubeColor})");

                // Increment the count for the same colored stack
                if (!sameColorStackCount.ContainsKey(cubeColor))
                {
                    sameColorStackCount[cubeColor] = 1;
                }
                else
                {
                    sameColorStackCount[cubeColor]++;
                }

                // Increment the total count for the same colored stack across all stack areas
                if (!totalSameColorStackCount.ContainsKey(cubeColor))
                {
                    totalSameColorStackCount[cubeColor] = 1;
                }
                else
                {
                    totalSameColorStackCount[cubeColor]++;
                }

                Debug.Log($"Count of same colored stacks ({cubeColor}) in this stack area: {sameColorStackCount[cubeColor]}");
                Debug.Log($"Total count of same colored stacks ({cubeColor}) across all stack areas: {totalSameColorStackCount[cubeColor]}");

                // Mark the stack area as filled
                MarkStackAreaAsFilled(stackIndex);
            }
            else
            {
                // If cubes are not of the same color, mark the stack area as unfilled
                MarkStackAreaAsUnfilled(stackIndex);
            }
        }
    }

    private void MarkStackAreaAsFilled(int stackIndex)
    {
        GameObject stackAreaObject = GameObject.Find($"StackArea_{stackIndex}");
        if (stackAreaObject != null)
        {
            StackArea stackArea = stackAreaObject.GetComponent<StackArea>();
            if (stackArea != null)
            {
                //stackArea.isFilled = true;
                Debug.Log($"Stack Area {stackIndex} is now filled.");
            }
        }
    }

    private void MarkStackAreaAsUnfilled(int stackIndex)
    {
        GameObject stackAreaObject = GameObject.Find($"StackArea_{stackIndex}");
        if (stackAreaObject != null)
        {
            StackArea stackArea = stackAreaObject.GetComponent<StackArea>();
            if (stackArea != null)
            {
                //stackArea.isFilled = false;
                Debug.Log($"Stack Area {stackIndex} is now unfilled.");
            }
        }
    }
}
