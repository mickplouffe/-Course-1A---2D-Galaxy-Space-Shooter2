using UnityEngine;

public class EnemySmartBackShot : MonoBehaviour
{
    [SerializeField] GameObject _Amunitions, _detectionRange;
    [SerializeField] float _minFireRate = 0.5f, _maxFireRate = 5, _fireRate = 3;
    bool _playerDetected;
    float _canFire;

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDetected = _detectionRange.GetComponent<PlayerFinderByCollision>().CheckPlayerTriggerCollision();

        CalculateShooting();
    }

    private void CalculateShooting()
    {

        if ((Time.time > _canFire) && _playerDetected)
        {
            _fireRate = Random.Range(_minFireRate, _maxFireRate);
            _canFire = Time.time + _fireRate;
            GameObject shot = Instantiate(_Amunitions, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.Euler(0, 0, 0));
            shot.GetComponent<Bullets>()._bulletSpeed = 3;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerDetected = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerDetected = false;
        }
    }

}
