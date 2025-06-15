using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour,IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
    }

}
