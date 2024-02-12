using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;

public class Container : MonoBehaviour
{
    public int currentCapacity;
    public int maxCapacity;
    public List<GameObject> ContainerStack = new List<GameObject>();

    // Method to add a cube to the container
    public void AddCube()
    {
        if (currentCapacity < maxCapacity)
        {
            // Instantiate and position cube inside the container
            // Update current capacity
            currentCapacity++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ContainerStack.Contains(other.gameObject))
        {
            ContainerStack.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (ContainerStack.Count > 0)
        //{
        //    ContainerStack.RemoveAt(ContainerStack.Count-1);
        //}
    }

    private void OnMouseDown()
    {
        //Debug.Log("Stack Clicked");
        //if(LevelManager.instance.SelectedContainer == null)
        {
            LevelManager.instance.SelectedContainer = this.gameObject;
            LevelManager.instance.ContainerSequence.Add(this.gameObject);
        }

        if (ContainerStack.Count > 0 && LevelManager.instance.SelectedCube == null /*&& LevelManager.instance.SelectedCube == this.gameObject*/)
        {
            PopCube();
        }
        else if (LevelManager.instance.SelectedCube /*&& LevelManager.instance.SelectedCube == this.gameObject*/)
        {
            InsertCube();
        }
    }

    public void PopCube()
    {
        {
            print("POP");

            LevelManager.instance.SelectedCube = ContainerStack[ContainerStack.Count - 1].gameObject;
            ContainerStack[ContainerStack.Count - 1].transform.GetComponent<Rigidbody>().isKinematic = true;
            LeanTween.move(ContainerStack[ContainerStack.Count - 1], new Vector3(transform.position.x, /*transform.position.y + 5*/4, transform.position.z), 1);
        }
    }
    GameObject selectedCube;
    public void InsertCube()
    {
        
        {

            selectedCube = LevelManager.instance.SelectedCube;

            print("INSERT");
            ContainerStack.Add(selectedCube);

            //if (LevelManager.instance.ContainerSequence.Count < 2)
            //{
            LeanTween.move(
                //selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, selectedCube.transform.position.y + 5, selectedCube.transform.position.z), 1).
                selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, /*transform.position.y+5*/4, selectedCube.transform.position.z), 1)
                .setOnComplete(FinalCubeDeposit);
            //}
            //else
            {
                //LeanTween.move(
                //    //selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, selectedCube.transform.position.y + 5, selectedCube.transform.position.z), 1).
                //    selectedCube.gameObject, new Vector3(LevelManager.instance.ContainerSequence[LevelManager.instance.ContainerSequence.Count - 1].transform.position.x,
                //    4, LevelManager.instance.ContainerSequence[LevelManager.instance.ContainerSequence.Count - 1].transform.position.z), 1)
                //    .setOnComplete(FinalCubeDeposit);
            }
        }

        void FinalCubeDeposit()
        {
            //LeanTween.move(selectedCube.gameObject, new Vector3(transform.position.x, 4, transform.position.z), 1);

            ContainerStack[ContainerStack.Count - 1].transform.GetComponent<Rigidbody>().isKinematic = false;
            LevelManager.instance.SelectedCube = null;
            selectedCube = null;
            LevelManager.instance.SelectedContainer = null;

        }

    }
}

    

