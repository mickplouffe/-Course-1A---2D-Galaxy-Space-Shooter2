using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject _enemy, _enemyContainer, _powerUpContainer;
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] bool _isEnemyOn = true, _isPowerupOn = true;
    [SerializeField] float _maxWaitEnemySpawnTime = .1f, _maxWaitPowerUpSpawnTime = 5;
    [SerializeField] int randomNumber = 0;
    int[] rarityArray = { 45, 25, 10, 8, 4, 3};
    int _powerUpToSpawn = 0;

    public void StartGame()
    {
        StartCoroutine(nameof(spawnEnemyRoutine));
        StartCoroutine(spawnPowerUpSpawner(_maxWaitPowerUpSpawnTime));
        StartCoroutine(nameof(SpawnEnemyQuickerAfterTime));
    }

    public void OnPlayerDeath()
    {
        _isEnemyOn = false;
        _isPowerupOn = false;

        Destroy(_enemyContainer, 1f);
        Destroy(_powerUpContainer, 1f);
    }

    private void CalculateSpawnChance()
    {
        // I HATE that way of doing. I need to redo it. But it is working for the purpose of the courses 1A.
        //int result = 0, total = 0, randomNumber = 0;
        
        int total = 0;

        foreach (var item in rarityArray)
        {
            total += item;
        }

        randomNumber = Random.Range(0, total);

        for (int i = 0; i < rarityArray.Length; i++)
        {
            if (randomNumber <= rarityArray[i])
            {
                _powerUpToSpawn = i;
                return;
            }
            else
            {
                randomNumber -= rarityArray[i];
            }
        }
    }

    IEnumerator spawnEnemyRoutine()
    {
        while (_isEnemyOn) 
        {
            float spawnX = Random.Range(-9.2f, 9.5f);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(spawnX, 8), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_maxWaitEnemySpawnTime);
                
        }
    }

    IEnumerator spawnPowerUpSpawner(float waitSpawnTime)
    {
        while (_isPowerupOn)
        {
            CalculateSpawnChance();
            float spawnX = Random.Range(-9.5f, 9.5f);
            float spawnTime = Random.Range(0.3f, waitSpawnTime);
            
            GameObject newPowerUp = Instantiate(_powerUps[_powerUpToSpawn], new Vector3(spawnX, 8), Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(waitSpawnTime);
        }
    }

    IEnumerator SpawnEnemyQuickerAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (_maxWaitEnemySpawnTime > 0.15f)
            {
                _maxWaitEnemySpawnTime -= 0.05f;
            }
            else
            {
                StopCoroutine(nameof(SpawnEnemyQuickerAfterTime));
            }
        }
    }
}
