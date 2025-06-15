using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform holdPoint;
    private GameObject heldObject;
    public float moveForce = 250f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
                {
                    if (hit.collider.CompareTag("Pickup"))
                    {
                        PickupObject(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            Rigidbody rb = pickObj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.freezeRotation = true;
            heldObject = pickObj;
            rb.drag = 10;
            heldObject.transform.parent = holdPoint;
        }
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.drag = 1;
        heldObject.transform.parent = null;
        heldObject = null;
    }

    void FixedUpdate()
    {
        if (heldObject != null)
        {
            Vector3 moveDirection = holdPoint.position - heldObject.transform.position;
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }
}
