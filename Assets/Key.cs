using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public GameObject door;
    public GameObject door1;
    public void Interact()
    {
        gameObject.SetActive(false);
        door.SetActive(false);
        door1.SetActive(false);
        
    }

}
