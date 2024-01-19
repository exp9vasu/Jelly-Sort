// CubeScript.cs
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public delegate void CubeEnteredStackEvent(GameObject cube, int stackIndex);
    public delegate void CubeExitedStackEvent(GameObject cube, int stackIndex);

    public static event CubeEnteredStackEvent OnCubeEnteredStack;
    public static event CubeExitedStackEvent OnCubeExitedStack;

    private void OnTriggerEnter(Collider other)
    {
        StackArea stackArea = other.GetComponent<StackArea>();
        if (stackArea != null)
        {
            int stackIndex = stackArea.StackIndex;
            OnCubeEnteredStack?.Invoke(gameObject, stackIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StackArea stackArea = other.GetComponent<StackArea>();
        if (stackArea != null)
        {
            int stackIndex = stackArea.StackIndex;
            OnCubeExitedStack?.Invoke(gameObject, stackIndex);
        }
    }
}