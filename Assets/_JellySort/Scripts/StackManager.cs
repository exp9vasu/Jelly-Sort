// StackManager.cs
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    private Dictionary<int, List<GameObject>> stackedCubesByStack = new Dictionary<int, List<GameObject>>();

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

    private void HandleCubeEnteredStack(GameObject cube, int stackIndex)
    {
        if (!stackedCubesByStack.ContainsKey(stackIndex))
        {
            stackedCubesByStack[stackIndex] = new List<GameObject>();
        }

        stackedCubesByStack[stackIndex].Add(cube);
        Debug.Log($"Cube entered stack {stackIndex}. Stack count: {stackedCubesByStack[stackIndex].Count}");
    }

    private void HandleCubeExitedStack(GameObject cube, int stackIndex)
    {
        if (stackedCubesByStack.ContainsKey(stackIndex))
        {
            stackedCubesByStack[stackIndex].Remove(cube);
            Debug.Log($"Cube exited stack {stackIndex}. Stack count: {stackedCubesByStack[stackIndex].Count}");
        }
    }
}
