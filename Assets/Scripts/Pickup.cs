using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] float _fallingSpeed = 1, _timeToDie = 20;
    [Range(0, 100)] public int _spawnChanceRate = 1;
    [SerializeField] int _pickupID;
    PlayerController _player;
    [SerializeField] AudioClip _getPickupSFX;
    [SerializeField] bool _isMagnetized;
    Vector2 PlayerDirection;
    Rigidbody2D rb;

    void Start()
    {
        Debug.Log("Pickup" + gameObject.name +  " spawned.");

        Destroy(gameObject, _timeToDie);

        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Player not found!");
            //Application.Quit();
        }

        if (_player != null)
        {
            rb = GetComponent<Rigidbody2D>();

        }

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
        PickupCheck();
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

    private void PickupCheck()
    {
        switch (_pickupID)
        {
            case 0:
                Debug.Log("Pickup - Triple Shot Obtain");
                _player.SetPowerUp("TrpShot");
                break;
            case 1:
                Debug.Log("Pickup - Large Shot Obtain");
                _player.SetPowerUp("LrgShot");
                break;
            case 2:
                Debug.Log("Pickup - Speed Shot Obtain");
                _player.SetPowerUp("SpdShot");
                break;
            case 3:
                Debug.Log("Pickup - Shield Obtain");
                _player._isShieldOn = true;
                _player._shieldHitPoints = 3;
                break;
            case 4:
                Debug.Log("Pickup - Health Regain");
                _player.TakeHeal(1);
                break;
            case 5:
                Debug.Log("Pickup - Munition Charger");
                _player.RefillMunition();
                break;
            case 6:
                Debug.Log("Pickup - Homing Shot Obtain");
                _player.SetPowerUp("hmgShot");
                break;
            case 7:
                Debug.Log("Pickup Nagative - Remove 100 pts");
                _player.UpdateScore(-105);
                break;
            default:
                Debug.LogWarning("Pickup with ID: " + _pickupID + " is not setup correctly.");
                break;
        }
        /*//Shoud Change it to a switch/case. And create IDs to recude string verification
        if (gameObject.name.Contains("Pickup_TripleShot"))
        {
            Debug.Log("Pickup - Triple Shot Obtain");
            _player.SetPowerUp("TrpShot");
        }

        if (gameObject.name.Contains("Pickup_LargeShot"))
        {
            Debug.Log("Pickup - Large Shot Obtain");
            _player.SetPowerUp("LrgShot");
        }

        if (gameObject.name.Contains("Pickup_SpeedShot"))
        {
            Debug.Log("Pickup - Speed Shot Obtain");
            _player.SetPowerUp("SpdShot");
        }

        if (gameObject.name.Contains("Pickup_Shield"))
        {
            Debug.Log("Pickup - Shield Obtain");
            _player._isShieldOn = true;
            _player._shieldHitPoints = 3;
        }

        if (gameObject.name.Contains("Pickup_Health_Up"))
        {
            Debug.Log("Pickup - Health Regain");
            _player.TakeHeal(1);
        }

        if (gameObject.name.Contains("Pickup_MunitionCharger"))
        {
            Debug.Log("Pickup - Munition Charger");
            _player.RefillMunition();
        } */

        if (_getPickupSFX != null)
        {
            AudioSource.PlayClipAtPoint(_getPickupSFX, transform.position);
        }

        DestroyAfterUse();

    }

    private void DestroyAfterUse()
    {
        Destroy(gameObject);
    }
}
