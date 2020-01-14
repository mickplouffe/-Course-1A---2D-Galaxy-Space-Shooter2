using UnityEngine;

public class Asteroid : MonoBehaviour
{
    PlayerController _player;
    SpawnerManager _spawnerManager;
    [SerializeField] GameObject _explosion;
    
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Player not found!");
        }

        _spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();

        if (_spawnerManager == null)
        {
            Debug.LogError("Spawner Manager not found!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, .3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);

        }

        if (other.tag == "Player")
        {
            _player.TakeDamage();
            Explode();
            Destroy(gameObject);
        }

        _spawnerManager.StartGame();
    }

    private void Explode()
    {
        Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

    }
}
