using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;

public class Container : MonoBehaviour
{
    public int currentCapacity;
    public int maxCapacity;
    public List<GameObject> ContainerStack = new List<GameObject>();
    public bool FilledWithSame;
    public GameObject ConfettiPrefab;

    private void Start()
    {
        //transform.GetComponent<BoxCollider>().enabled = false;
        //Invoke("EnableBoxCollider", 3);    
    }

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
        if (ContainerStack.Count > 0)
        {
            ContainerStack.RemoveAt(ContainerStack.Count - 1);
        }
    }

    void EnableBoxCollider()
    {
        transform.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnMouseDown()
    {
        if (LevelManager.instance.GameOver)
        {
            return;
        }

        //Debug.Log("Stack Clicked");
        //if(LevelManager.instance.SelectedContainer == null)
        {
            LevelManager.instance.SelectedContainer = this.gameObject;
            //LevelManager.instance.ContainerSequence.Add(this.gameObject);
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
        if (LevelManager.instance.SelectedCube)
        {
            return;
        }

        {
            //print("POP");

            LevelManager.instance.SelectedCube = ContainerStack[ContainerStack.Count - 1].gameObject;
            selectedCube = ContainerStack[ContainerStack.Count - 1].gameObject;
            //ContainerStack[ContainerStack.Count - 1].transform.GetComponent<Rigidbody>().isKinematic = true;
            selectedCube.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            selectedCube.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            selectedCube.transform.GetComponent<Rigidbody>().isKinematic = true;

            LeanTween.move(ContainerStack[ContainerStack.Count - 1], new Vector3(selectedCube.transform.position.x,
               LevelManager.instance.levels[LevelManager.instance.CurrentLevel].maxCapacityPerContainer + 1, selectedCube.transform.position.z), 1);
        }
    }
    GameObject selectedCube;

    public void DropBack()
    {
        if (LevelManager.instance.SelectedContainer.GetComponent<Container>() == this && LevelManager.instance.SelectedCube !=null)
        {
            LevelManager.instance.SelectedCube.GetComponent<Rigidbody>().isKinematic = false;
            LevelManager.instance.SelectedCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            LevelManager.instance.SelectedCube = null;
        }
    }

    public void InsertCube()
    {

        if (ContainerStack.Count >= LevelManager.instance.levels[LevelManager.instance.CurrentLevel].maxCapacityPerContainer)
        {
            //if (selectedCube)
            //{
            //    selectedCube.GetComponent<Rigidbody>().isKinematic = false;
            //    selectedCube = null;
            //}
            DropBack();
            return;
            
        }

        if(ContainerStack.Count > 0)
        {
            if(LevelManager.instance.SelectedCube.GetComponentInParent<CubeScript>().CubeIndex 
                != ContainerStack[ContainerStack.Count - 1].GetComponentInParent<CubeScript>().CubeIndex)
            {
                DropBack();
                return;
            }
        }

        {

            selectedCube = LevelManager.instance.SelectedCube;

            //print("INSERT");
            ContainerStack.Add(selectedCube);

            //if (LevelManager.instance.ContainerSequence.Count < 2)
            //{
            LeanTween.move(
                //selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, selectedCube.transform.position.y + 5, selectedCube.transform.position.z), 1).
                selectedCube.gameObject, new Vector3(transform.position.x, LevelManager.instance.levels[LevelManager.instance.CurrentLevel].maxCapacityPerContainer + 1, transform.position.z), 1)
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
            if (selectedCube)
            {
                //LeanTween.move(selectedCube.gameObject, new Vector3(transform.position.x, 4, transform.position.z), 1);
                //LeanTween.move(
                //    //selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, selectedCube.transform.position.y + 5, selectedCube.transform.position.z), 1).
                //    selectedCube.gameObject, new Vector3(selectedCube.transform.position.x, /*transform.position.y+5*/4, selectedCube.transform.position.z), 1);
                //    //.setOnComplete(FinalCubeDeposit);

                //ContainerStack[ContainerStack.Count - 1].transform.GetComponent<Rigidbody>().isKinematic = false;
                selectedCube.transform.GetComponent<Rigidbody>().isKinematic = false;
                LevelManager.instance.SelectedCube = null;
                selectedCube = null;
                LevelManager.instance.SelectedContainer = null;

                //CheckSameIndex(ContainerStack);

                LevelManager.instance.CheckGameState();
                //CheckSameBox();

                Invoke("AdjustCubes", 2.5f);

            }
        }

    }

    public List<GameObject> tempList = new List<GameObject>();

    public void CheckSameIndex(List<GameObject> cubes)
    {
        // If there are no cubes, return false
        if (cubes.Count == 0)
        {
            //FilledWithSame = true;
            return;
        }

        // Get the index of the first cube
        int firstCubeIndex = cubes[0].GetComponentInParent<CubeScript>().CubeIndex;

        // Check if all cubes have the same index
        for (int i = 1; i < cubes.Count; i++)
        {
            CubeScript cubeScript = cubes[i].GetComponentInParent<CubeScript>();


            if (cubeScript.CubeIndex != firstCubeIndex)
            {
                FilledWithSame = false;
                return;
                //ConfettiPrefab.SetActive(false);

            }
            // Check if the cube has CubeScript attached and CubeIndex is present
            else if (cubeScript != null && cubeScript.CubeIndex == firstCubeIndex)
            {
                if (ContainerStack.Count == LevelManager.instance.levels[LevelManager.instance.CurrentLevel].maxCapacityPerContainer)
                {
                    // If any cube has a different index, return false
                    //return false;
                    FilledWithSame = true;
                    //Invoke("ShootPrefab", 1);
                }
            }
           
        }

        // All cubes have the same index
        //return true;
        //FilledWithSame = true;

    }

    //private void Update()
    //{
    //    //CheckSameBox();
    //}

    public void CheckSameBox()
    {
        CheckSameIndex(ContainerStack);

        if (LevelManager.instance.GameOver && ContainerStack.Count > 0)
        {
            Invoke("ShootPrefab", 1);
        }
    }

    public void AdjustCubes()
    {
        for (int i = 0; i < ContainerStack.Count; i++)
        {
            LeanTween.move(ContainerStack[i].gameObject, new Vector3(transform.position.x, ContainerStack[i].transform.position.y, transform.position.z), 1);

        }
    }

    public void ShootPrefab()
    {
        ConfettiPrefab.SetActive(true);
    }

    private void Update()
    {
        CheckSameBox();

        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Loop through all the touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Check if the touch phase is began
                if (touch.phase == TouchPhase.Began)
                {
                    // Handle the touch
                    HandleTouch(touch.position);
                }
            }
        }
    }

    void HandleTouch(Vector2 touchPosition)
    {
        // Convert touch position to a ray from the camera
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit has a BoxCollider
            Container container = hit.collider.GetComponent<Container>();
            if (container == this)
            {
                // Object with BoxCollider is clicked, you can perform actions here
                Debug.Log("Object with BoxCollider clicked: " + hit.collider.gameObject.name);
                OnMouseDown();
            }
        }
    }
}



