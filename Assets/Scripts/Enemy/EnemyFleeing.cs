using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : MonoBehaviour
{
    [SerializeField] GameObject _enemyParent;
    GameObject _target;
    [SerializeField] float RotateSpeed = 2000f;

    [SerializeField] bool _isFleeing;
    Rigidbody2D rb;
    //EnemyController _enemyController;

    // Start is called before the first frame update
    void Start()
    {
        rb = _enemyParent.GetComponent<Rigidbody2D>();
        //_enemyController = _enemyParent.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flee();
    }

    private void Flee()
    {
        
        if (_isFleeing)
        {
            if (_target != null)
            {
                Vector3 targetVector = _target.transform.position - _enemyParent.transform.position;

                float rotatingIndex = -1 * Vector3.Cross(targetVector, transform.up).z;

                rb.angularVelocity = rotatingIndex * RotateSpeed * Time.deltaTime;

                if (rb.angularVelocity < 0)
                {
                    _enemyParent.transform.Translate(Vector3.left * 2 * Time.deltaTime);

                }
                else if (rb.angularVelocity > 0)
                {
                   _enemyParent.transform.Translate(Vector3.right * 2 * Time.deltaTime);

                }
            }
        }
        else if (_isFleeing == false)
        {
            
            rb.gameObject.transform.rotation = Quaternion.Slerp(_enemyParent.transform.rotation, Quaternion.Euler(0, 0, 0), 8 * Time.deltaTime);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            _target = other.gameObject;
            _isFleeing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            _isFleeing = false;
        }
    }
}
