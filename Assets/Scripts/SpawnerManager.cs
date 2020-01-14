using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject _enemy, _enemyContainer, _powerUpContainer;
    [SerializeField] GameObject[] _powerUp;
    [SerializeField] bool _isEnemyOn = true, _isPowerupOn = true;
    [SerializeField] float _waitEnemySpawnTime = .1f, _waitPowerUpSpawnTime = 5;

    public void StartGame()
    {
        StartCoroutine(spawnEnemyRoutine(_waitEnemySpawnTime));
        StartCoroutine(spawnPowerUpSpawner(_waitPowerUpSpawnTime));
    }

    public void OnPlayerDeath()
    {
        _isEnemyOn = false;
        _isPowerupOn = false;

        Destroy(_enemyContainer, 1f);
        Destroy(_powerUpContainer, 1f);
    }
    IEnumerator spawnEnemyRoutine(float waitSpawnTime)
    {
        while (_isEnemyOn) 
        {
            float spawnX = Random.Range(-9.2f, 9.5f);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(spawnX, 8), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitSpawnTime);
                
        }
    }

    IEnumerator spawnPowerUpSpawner(float waitSpawnTime)
    {
        while (_isPowerupOn)
        {
            
            float spawnX = Random.Range(-9.5f, 9.5f);
            GameObject newPowerUp = Instantiate(_powerUp[Random.Range(0, _powerUp.Length)], new Vector3(spawnX, 8), Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(waitSpawnTime);
        }
    }
}
