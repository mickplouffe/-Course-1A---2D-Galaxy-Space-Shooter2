using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed = 1, _timeToDie = 10;
    [Range(0, 100)] public int _spawnChanceRate = 1;
    PlayerController _player;
    [SerializeField] GameObject _enemyExplosion, _shield;
    [SerializeField] bool _isShieldOn;
    [SerializeField] int _shieldHitPoints;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player != null)
        {
            //to check
        }

        //Will destroy this gObj if not acheive other criteria in Destruction()
        Destroy(gameObject, _timeToDie);
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        CalculateShield();
        Destruction();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isShieldOn)
        {
            if (other.tag =="PlayerBullet")
            {
                Destroy(other.gameObject);
                _player.UpdateScore(5);
                _shieldHitPoints--;

            }

            if (other.tag == "Player")
            {
                _player.UpdateScore(5);
                _player.TakeDamage();
                _shieldHitPoints--;

            }
        }
        else
        {
            if (other.tag == "PlayerBullet")
            {
                Debug.Log("Enemy killed by Player's bullet.");
                _player.UpdateScore(10);
                _player.RefillMunition(1);
                Destroy(other.gameObject);
                ExplodeAndDestroy();
            }

            if (other.tag == "Player")
            {
                Debug.Log("Enemy killed by Player.");
                _player.UpdateScore(10);
                _player.TakeDamage();
                ExplodeAndDestroy();
            }
        }
        
    }

    public void ExplodeAndDestroy()
    {
        Instantiate(_enemyExplosion, new Vector3(transform.position.x, transform.position.y - 0.1f, 0), Quaternion.identity);
        Destroy(gameObject);
    }

    private void CalculateShield()
    {
        if (_shield != null)
        {
            if (_isShieldOn && _shieldHitPoints > 0)
            {
                _shield.SetActive(true);
            }
            else
            {
                _shield.SetActive(false);
                _isShieldOn = false;

            }
        }
        
    }

    public void ChangeSpeed(float setSpeed)
    {
        _speed = setSpeed;
    }

    private void Destruction()
    {
        if (transform.position.y > 12 || transform.position.y < -8 || transform.position.x > 14 || transform.position.x < -14)
        {
            Destroy(gameObject);
        }
    }
}
