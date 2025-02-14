using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public GameObject closedDoor;
    public GameObject openedDoor;

    public void OpenDoor()
    {
        closedDoor.SetActive(false);
        openedDoor.SetActive(true);
    }

    public void CloseDoor()
    {
        closedDoor.SetActive(true);
        openedDoor.SetActive(false);
    }
}
