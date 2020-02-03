using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinderByCollision : MonoBehaviour
{
    [SerializeField] bool _playerFound;

    public bool CheckPlayerTriggerCollision()
    {
        return _playerFound;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerFound = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerFound = false;
        }
    }
}
