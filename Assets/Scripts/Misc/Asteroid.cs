using UnityEngine;

public class Asteroid : MonoBehaviour
{
    PlayerController _player;
    SpawnManager _spawnManager;
    [SerializeField] GameObject _explosion;
    
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Player not found!");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawner Manager not found!");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, .3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            Explode();
            Destroy(other.gameObject);
            _spawnManager.NextWave();

            Destroy(gameObject);

        }

        if (other.tag == "Player")
        {
            _player.TakeDamage();
            Explode();
            _spawnManager.NextWave();

            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

    }
}
