using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform doorTransform;
    public GameObject requiredKey;

    public void Interact()
    {
        Destroy(doorTransform);
    }
}