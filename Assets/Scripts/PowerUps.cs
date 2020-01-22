using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] float _fallingSpeed = 1, _timeToDie = 20;
    //[Range(0.0f, 1.0f)] public float _spawnChanceRate = 1;
    PlayerController _player;
    [SerializeField] AudioClip _getPowerUpSFX;
    [SerializeField] bool _isMagnetized;
    Vector2 PlayerDirection;
    Rigidbody2D rb;

    void Start()
    {
        Debug.Log("PowerUp" + gameObject.name +  " spawned.");

        Destroy(gameObject, _timeToDie);

        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Player not found!");
            //Application.Quit();
        }
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        MagnetizedByPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
        _player.UpdateScore(5);
        PowerUp();
        }

        if (other.tag == "PlayerMagneticField")
        {
            _isMagnetized = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerMagneticField")
        {
            _isMagnetized = false;
        }
    }

    private void MagnetizedByPlayer()
    {
        if (_isMagnetized)
        {
            PlayerDirection = -(transform.position - _player.transform.position).normalized;
            rb.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y -_fallingSpeed) * 200f * (Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(0, -_fallingSpeed);
        }
    }

    private void PowerUp()
    {
        //Shoud Change it to a switch/case. And create IDs to recude string verification
        if (gameObject.name.Contains("PowerUp_TripleShot"))
        {
            Debug.Log("PowerUp - Triple Shot Obtain");
            _player.SetPowerUp("TrpShot");
        }

        if (gameObject.name.Contains("PowerUp_LargeShot"))
        {
            Debug.Log("PowerUp - Large Shot Obtain");
            _player.SetPowerUp("LrgShot");
        }

        if (gameObject.name.Contains("PowerUp_SpeedShot"))
        {
            Debug.Log("PowerUp - Speed Shot Obtain");
            _player.SetPowerUp("SpdShot");
        }

        if (gameObject.name.Contains("PowerUp_Shield"))
        {
            Debug.Log("PowerUp - Shield Obtain");
            _player._isShieldOn = true;
            _player._shieldHitPoints = 3;
        }

        if (gameObject.name.Contains("PowerUp_Health_Up"))
        {
            Debug.Log("PowerUp - Health Regain");
            _player.TakeHeal(1);
        }

        if (gameObject.name.Contains("PowerUp_MunitionCharger"))
        {
            Debug.Log("PowerUp - Munition Charger");
            _player.RefillMunition();
        }

        if (_getPowerUpSFX != null)
        {
            AudioSource.PlayClipAtPoint(_getPowerUpSFX, transform.position);
        }

        DestroyAfterUse();

    }

    private void DestroyAfterUse()
    {
        Destroy(gameObject);
    }
}
