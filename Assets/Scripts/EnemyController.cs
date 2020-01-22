using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed = 1, _timeToDie = 10;
    PlayerController _player;
    [SerializeField] GameObject _enemyExplosion;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Player not found!");
            Application.Quit();
        }

        //Will destroy this gObj if not acheive other criteria in Destruction()
        Destroy(gameObject, _timeToDie);
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            Debug.Log("Enemy killed by Player'S bullet.");
            _player.UpdateScore(10);
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
            
        }

        if (other.tag == "Player")
        {
            Debug.Log("Enemy killed by Player.");
            _player.TakeDamage();
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Instantiate(_enemyExplosion, new Vector3(transform.position.x, transform.position.y - 0.1f, 0), Quaternion.identity);

    }

}
