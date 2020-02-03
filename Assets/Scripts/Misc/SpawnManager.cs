using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] bool _isSpawningMaster, _isSpawningEnemy, _isSpawningPickup;
    [SerializeField] float _minEnemySpawnDelay = 0, _maxEnemySpawnDelay = 1, _minPickupSpawnDelay = 0, _maxPickupSpawnDelay = 1;
    [SerializeField] int _waves = 0, _WaveDelaySeconds = 1, _enemyInWave = 10, _enemySpawned = 0;
    [SerializeField] GameObject[] _enemyToSpawn, _pickupToSpawn;
    [SerializeField] GameObject _boss1, _enemyContainer, _pickupContainer;
    [SerializeField] UIManager _uiManager;

    bool _isSpawnEnemyRoutine, _isSpawnPickupRoutine;
    int _nextToSpawn;

    // Update is called once per frame
    void Update()
    {
        SpawningCheck();
        WaveChecker();
    }

    /// <summary>
    /// Start Spawn All the spawnable. Enemy and Pickup.
    /// </summary>
    public void StartSpawn()
    {
        StartSpawn(true, true, true);
    }

    /// <summary>
    /// Allow selecting which Spawnable that will be used. Master, Enemy, Pickup.
    /// </summary>
    public void StartSpawn(bool master, bool enemy, bool pickup)
    {
        _isSpawningMaster = master; _isSpawningEnemy = enemy; _isSpawningPickup = pickup;
    }

    /// <summary>
    /// Check if the SpawnManager can spawn any spawnable.
    /// </summary>
    private void SpawningCheck()
    {
            if (_isSpawningEnemy && _isSpawningMaster && _isSpawnEnemyRoutine == false)
            {
                StartCoroutine(nameof(SpawnEnemyRoutine));
                _isSpawnEnemyRoutine = true;
            }
            else if ((_isSpawningEnemy == false || _isSpawningMaster == false) && _isSpawnEnemyRoutine)
            {
                StopCoroutine(nameof(SpawnEnemyRoutine));
                _isSpawnEnemyRoutine = false;
            }

            if (_isSpawningPickup && _isSpawningMaster && _isSpawnPickupRoutine == false)
            {
                StartCoroutine(nameof(SpawnPickupRoutine));
                _isSpawnPickupRoutine = true;                
            }
            else if ((_isSpawningPickup == false || _isSpawningMaster == false) && _isSpawnPickupRoutine)
            {
                StopCoroutine(nameof(SpawnPickupRoutine));
                _isSpawnPickupRoutine = false;
            }
    }

    private void CalculateRarity(GameObject[] spawnable)
    {
        int total = 0, i = 0, j = 0, oldValue;
        int chance = 0;
        int[,] positionToWeight = new int[spawnable.Length, 2];

        foreach (var item in spawnable)
        {
            oldValue = total;
            if (item.tag == "Pickup")
            {
                chance = item.GetComponent<Pickup>()._spawnChanceRate;
            }
            else if (item.tag == "Enemy")
            {
                chance = item.GetComponent<EnemyController>()._spawnChanceRate;
            }
            
            total += chance;

            positionToWeight[i, 0] = oldValue;
            positionToWeight[i, 1] = total - 1;
            //positionToWeight[i, 2] = chance;
            i++;
        }

        int randomNumber = Random.Range(0, total);

        foreach (var item in positionToWeight)
        {
            
            if (randomNumber >= positionToWeight[j,0] && randomNumber <= positionToWeight[j, 1])
            {
                _nextToSpawn = j;
                return;
            }
            j++;
        }    
    }

    public void OnPlayerDeath()
    {
        _isSpawningMaster = false;

        foreach (Transform item in _pickupContainer.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in _enemyContainer.transform)
        {
            item.GetComponent<EnemyController>().ExplodeAndDestroy();
        }        
    }

    public void NextWave()
    { //TBH I was tired. So I made it quick. Just Working. I should have make it cleaner.
        StartCoroutine(nameof(NextWaveRoutine));
    }

    public int GetWave()
    {
        return _waves;
    }

    private void WaveChecker()
    {
        if (_enemySpawned > _enemyInWave)
        {
            _isSpawningMaster = false;
            _enemySpawned = 0;

            NextWave();

        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            float spawnPositionX = Random.Range(-9.2f, 9.5f);
            CalculateRarity(_enemyToSpawn);
            GameObject newEnemy = Instantiate(_enemyToSpawn[_nextToSpawn], new Vector3(spawnPositionX, 8), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemySpawned++;

            yield return new WaitForSeconds(_maxEnemySpawnDelay);
        }
    }
    
    IEnumerator SpawnPickupRoutine()
    {
        while (true)
        {
            //CalculateSpawnChance();
            float spawnPositionX = Random.Range(-9.5f, 9.5f);
            CalculateRarity(_pickupToSpawn);
            GameObject newPowerUp = Instantiate(_pickupToSpawn[_nextToSpawn], new Vector3(spawnPositionX, 8), Quaternion.identity);
            newPowerUp.transform.parent = _pickupContainer.transform;
            yield return new WaitForSeconds(_maxPickupSpawnDelay);
        }
    }

    IEnumerator NextWaveRoutine()
    {
        while (true)
        {
            _waves++;

            yield return new WaitForSeconds(_WaveDelaySeconds);

            switch (_waves)
            {
                case 0:
                    NextWave();
                    Debug.LogError("_waves = Case 0 Error");
                    break;
                case 1:
                    _uiManager.UpdateWave();

                    _enemyInWave = 25;
                    _maxEnemySpawnDelay = 1.5f;
                    _minEnemySpawnDelay = 1f;
                    StartSpawn();
                    _WaveDelaySeconds = 7;

                    break;
                case 2:
                    _uiManager.UpdateWave();

                    _enemyInWave = 50;
                    _maxEnemySpawnDelay = .4f;
                    _minEnemySpawnDelay = 0.1f;
                    _isSpawningMaster = true;
                    _WaveDelaySeconds = 10;
                    break;
                case 3:
                    _uiManager.UpdateWave();

                    Instantiate(_boss1, new Vector3(0, 4.2f), Quaternion.identity);
                    Debug.Log("Wave boss");
                    StartSpawn(true, false, true);
                    StopCoroutine(nameof(NextWaveRoutine));

                    break;
                default:
                    _waves = 1;
                    _uiManager.UpdateWave();

                    break;
            }
            StopCoroutine(nameof(NextWaveRoutine));
            break;
        }

    }



    /// <summary>
    /// Is disabled for now.
    /// </summary>
    IEnumerator SpawnEnemyQuickerAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (true)//_maxWaitEnemySpawnTime > 0.15f)
            {
                //_maxWaitEnemySpawnTime -= 0.05f;
            }
            else
            {
                //StopCoroutine(nameof(SpawnEnemyQuickerAfterTime));
            }
        }
    }
}

