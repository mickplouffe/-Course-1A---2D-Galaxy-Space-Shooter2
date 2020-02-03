using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject _Amunitions;
    [SerializeField] float _minFireRate = 0.5f, _maxFireRate = 5, _fireRate = 3;
    float _canFire;
    //bool _firstCylcleSkip = false;

    // Start is called before the first frame update
    void Start()
    {
        _fireRate = Random.Range(_minFireRate, _maxFireRate);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Shooting();

    }

    private void Shooting()
    {        
        if ((Time.time > _canFire))
        {
            _fireRate = Random.Range(_minFireRate, _maxFireRate);
            _canFire = Time.time + _fireRate;
            GameObject shot = Instantiate(_Amunitions, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.Euler(0, 0, 180));

        }
    }
}
