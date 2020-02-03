using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    [SerializeField] GameObject _enemyParent;
    GameObject _target;
    [SerializeField] float RotateSpeed = 2000f;

    [SerializeField] bool _isFollowingPlayer;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = _enemyParent.GetComponent<Rigidbody2D>();
        _target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        
        if (_isFollowingPlayer)
        {
            if (_target != null)
            {
                Vector3 targetVector = _target.transform.position - _enemyParent.transform.position;

                float rotatingIndex = -1 * Vector3.Cross(targetVector, transform.up).z;

                rb.angularVelocity = -1 * rotatingIndex * RotateSpeed * Time.deltaTime;
            }
        }
        else if (_isFollowingPlayer == false)
        {
            
            rb.gameObject.transform.rotation = Quaternion.Slerp(_enemyParent.transform.rotation, Quaternion.Euler(0, 0, 0), 8 * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _isFollowingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _isFollowingPlayer = false;
        }
    }
}
