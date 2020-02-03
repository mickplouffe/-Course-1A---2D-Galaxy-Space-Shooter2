using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_OuterShell : MonoBehaviour
{
    public GameObject colliderObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            colliderObj = other.gameObject;

        }
    }
}
