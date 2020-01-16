using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speedHori = 10, _speedVert = 10, _powerUpCooldown = 5, _defaultFireRate, _boostBy = 2, _boostCurrentCharge = 100;
    [SerializeField] float _boostChargingRate = 5, _boostDechargingRate = 20, _amunitionCharger = 15;
    [SerializeField] int _lifePoints = 3, _score, _tempLifePoints;
    [SerializeField] bool _isBoosting, _boostRecharging;
    UIManager _uiManager;
    public float _fireRate = .4f;
    public bool _isShieldOn;
    public int currentAmunition = 0, _shieldHitPoints = 3;
    float _canFire = 0;
    bool _routinePowerDownRunning, _routineSlowDownRunning;
    [SerializeField] GameObject[] _munitionsArr;
    [SerializeField] GameObject _activeAmunitions, _shield, _explosion, _damaged1, _damaged2, _rechargingEffect;
    SpawnerManager _spawnerManager;

    // Start is called before the first frame update
    void Start()
    {
        _defaultFireRate = _fireRate;
   
        transform.position = new Vector3(0, -3, 0);
        _spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();
        if (_spawnerManager == null)
        {
            Debug.LogError("Spawner Manager not found!");
        }

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        Mouvements();
        PlayerFiring();
        Amunitions(currentAmunition);
        CalculateShield();
    }

    private void Mouvements()
    {

        CalculateBoost();

        float horizontalInput = Input.GetAxis("Horizontal"), verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput * _speedHori * _boostBy, verticalInput * _speedVert * _boostBy, 0) * Time.deltaTime);

        #region 1A-Warp_Removed/Commented
        //The Course 1A Demonstrater to make the Warp from the side Pac-Man Like. However I don,t really like this and I think this is
        //Disturbing during GamePlay.
        //I Commented the Course approach and keep the Clamp as for now.
        //I may do it again but with 2 identical ships to make illusion on warp around and not a Teleportation
        //09-01-2020 dd-mm-yyyy

        //# //The number here is where will it betriggered and is think is the MainCamera DOES NOT change position.
        //# if (transform.position.x > 11.3f)
        //# {
        //# transform.position = new Vector3(-11.3f, transform.position.y, 0);
        //# }
        //# else if (transform.position.x < -11.3f)
        //# {
        //# transform.position = new Vector3(11.3f, transform.position.y, 0);

        //# }

        #endregion

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.8f, 3), 0);
        if (transform.position.y > 5.5)
        {
            transform.Translate(new Vector3(horizontalInput , 0, 0) * Time.deltaTime);
        }
    }

    private void CalculateBoost()
    {
        if (_boostCurrentCharge < 0)
        {
            _boostCurrentCharge = 0;
            _boostRecharging = true;
            _rechargingEffect.SetActive(_boostRecharging);

        }
        else if (_boostCurrentCharge > 100)
        {
            _boostCurrentCharge = 100;
            _boostRecharging = false;
            _rechargingEffect.SetActive(_boostRecharging);

        }

        if (Input.GetAxis("Fire3") == 1 && _isBoosting == false && _boostCurrentCharge > 0 && _boostRecharging == false)
        {
            _isBoosting = true;
            _boostBy = 2;
        }
        else if ((Input.GetAxis("Fire3") == 0 || _boostCurrentCharge <= 0) && _isBoosting == true)
        {
            _isBoosting = false;
            _boostBy = 1;
        }

        if (_isBoosting && _boostCurrentCharge > 0 && _boostRecharging == false)
        {
            _boostCurrentCharge -= _boostDechargingRate * Time.deltaTime;
        }
        else if (_isBoosting == false && _boostCurrentCharge < 100)
        {
            _boostCurrentCharge += _boostChargingRate * Time.deltaTime;
        }

        _uiManager.UpdateBoost(Mathf.RoundToInt(_boostCurrentCharge), _boostRecharging);


    }

    private void Amunitions(int amunition)
    {
        if (amunition < _munitionsArr.Length && amunition >= 0)
        {
            _activeAmunitions = _munitionsArr[amunition];
        }
        else
        {
            _activeAmunitions = _munitionsArr[0];
            Debug.LogError("Array Overflow - Value is not valide");
        }

        if (_fireRate != _defaultFireRate && !_routineSlowDownRunning)
        {
            StartCoroutine(nameof(SlowDownRoutine));
        }
        
    }

    private void PlayerFiring()
    {
       
        float firingInput = Input.GetAxis("Fire1");
        if ((firingInput == 1 || Input.GetKey(KeyCode.Space)) && Time.time > _canFire)
        {
            if (_amunitionCharger > 0)
            {
                _canFire = Time.time + _fireRate;
                GameObject shot = Instantiate(_activeAmunitions, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                _amunitionCharger--;
            }
            _uiManager.UpdateAmunitionCharger(_amunitionCharger);
        }
    }

    public void  RefillMunition()
    {
        _amunitionCharger = 15; //variable COnstante
        _uiManager.UpdateAmunitionCharger(_amunitionCharger);
    }

    public void UpdateScore(int Addscore)
    {
        _score += Addscore;
        _uiManager.UpdateScore(_score);
    }

    public void TakeDamage()
    {
        TakeDamage(1);

    }

    public void TakeDamage(int dmg)
    {
        if (_isShieldOn == false)
        {
            Debug.LogWarning("TakeDamage Update");

            _lifePoints -= dmg;
            _uiManager.UpdateLives(_lifePoints);
            _uiManager.ScreenShake();
            GraphicDmg(_lifePoints);

            if (_lifePoints <= 0)
            {
                _lifePoints = 0;
                DeathProcedure();
            }
        }
        else
        {
            _shieldHitPoints -= dmg;
            CalculateShield();
        }
        Debug.LogWarning("TakeDamage Finished");
    }

    public void TakeHeal(int healAmount)
    {
        _tempLifePoints = _lifePoints + healAmount;
        if (_tempLifePoints <= 3 && _tempLifePoints > 0)
        {
            _lifePoints += healAmount;
            _uiManager.UpdateLives(_lifePoints);
            GraphicDmg(_lifePoints);
        }
        else if (_tempLifePoints > 3)
        {
            _lifePoints = 3;
        }

    }

    private void CalculateShield()
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

        switch (_shieldHitPoints)
        {
            case 1:
                _shield.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 2:
                _shield.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 3:
                _shield.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            default:
                _shield.SetActive(false);
                _isShieldOn = false;
                break;
        }



    }

    private void DeathProcedure()
    {
        Debug.LogWarning("Player died!");
        _uiManager.GameOver();

        _spawnerManager.OnPlayerDeath();
        Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SetPowerUp(string upgrade)
    {
        if (upgrade == "TrpShot")
        {
            if (_routinePowerDownRunning)
            {
                StopCoroutine(nameof(PowerDownRoutine));
                _routinePowerDownRunning = false;
            }
            StartCoroutine(nameof(PowerDownRoutine));
            currentAmunition = 1;
        }

        if (upgrade == "LrgShot")
        {
            if (_routinePowerDownRunning)
            {
                StopCoroutine(nameof(PowerDownRoutine));
                _routinePowerDownRunning = false;
            }
            StartCoroutine(nameof(PowerDownRoutine));
            currentAmunition = 2;
        }

        if (upgrade == "SpdShot")
        {
            if (_routineSlowDownRunning)
            {
                StopCoroutine(nameof(SlowDownRoutine));
                _routineSlowDownRunning = false;
            }
            StartCoroutine(nameof(SlowDownRoutine));
            _fireRate = 0.1f;
        }

    }

    private void GraphicDmg(int _lifePoints)
    {
        switch (_lifePoints)
        {
            case 3:
                _damaged1.gameObject.SetActive(false);
                _damaged2.gameObject.SetActive(false);
                break;
            case 2:
                _damaged1.gameObject.SetActive(true);
                _damaged2.gameObject.SetActive(false);
                break;
            case 1:
                _damaged2.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator PowerDownRoutine()
    {
        _routinePowerDownRunning = true;
        yield return new WaitForSeconds(_powerUpCooldown);
        currentAmunition = 0;
        _routinePowerDownRunning = false;
    }

    IEnumerator SlowDownRoutine()
    {
        _routineSlowDownRunning = true;
        yield return new WaitForSeconds(_powerUpCooldown);
        _fireRate = _defaultFireRate;
        _routineSlowDownRunning = false;
    }
}
