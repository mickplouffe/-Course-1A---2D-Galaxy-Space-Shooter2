using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speedHori = 10, _speedVert = 10, _powerUpCooldown = 5, _defaultFireRate;
    [SerializeField] int _lifePoints = 3, _score;
    UIManager _uiManager;
    public float _fireRate = .4f;
    public bool _isShieldOn;
    public int currentAmunition = 0;
    float _canFire = 0;
    bool _routinePowerDownRunning, _routineSlowDownRunning;
    [SerializeField] GameObject[] _munitionsArr;
    [SerializeField] GameObject _activeAmunitions, _shield, _explosion, _damaged1, _damaged2;
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
        GetShield();
    }

    private void Mouvements()
    {
        float horizontalInput = Input.GetAxis("Horizontal"), verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput * _speedHori, verticalInput * _speedVert, 0) * Time.deltaTime);

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
            //# else if (transform.position.y < -11.3f)
            //# {
            //# transform.position = new Vector3(transform.position.x, 11.3f, 0);
            //# }

        #endregion

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.8f, 3), 0);
        if (transform.position.y > 5.5)
        {
            transform.Translate(new Vector3(horizontalInput , 0, 0) * Time.deltaTime);
        }
    }

    private void Amunitions(int amunition)
    {
        if (amunition != 0 && !_routinePowerDownRunning)
        {
            StartCoroutine(PowerDownRoutine());
        }

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
            StartCoroutine(SlowDownRoutine());
        }
        
    }

    private void PlayerFiring()
    {
        float firingInput = Input.GetAxis("Fire1");
        if ((firingInput == 1 || Input.GetKey(KeyCode.Space)) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            GameObject shot = Instantiate(_activeAmunitions, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        }
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
            _lifePoints -= dmg;
            _uiManager.UpdateLives(_lifePoints);
            GraphicDmg(_lifePoints);

            if (_lifePoints <= 0)
            {
                DeathProcedure();
            }
        }
        else
        {
            _isShieldOn = false;
        }

    }

    private void GetShield()
    {
        if (_isShieldOn)
        {
            _shield.SetActive(true);
        }
        else
        {
            _shield.SetActive(false);
        }

    }

    private void DeathProcedure()
    {
        Debug.LogWarning
            ("Player died!");
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
                StopCoroutine("PowerDownRoutine");
                _routinePowerDownRunning = false;
            }
            StartCoroutine("PowerDownRoutine");
            currentAmunition = 1;
        }

        if (upgrade == "SpdShot")
        {
            if (_routineSlowDownRunning)
            {
                StopCoroutine("SlowDownRoutine");
                _routineSlowDownRunning = false;
            }
            StartCoroutine("SlowDownRoutine");
            _fireRate = 0.1f;
        }

    }

    private void GraphicDmg(int _lifePoints)
    {
        switch (_lifePoints)
        {
            case 2:
                _damaged1.gameObject.SetActive(true);
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
