using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    /*
     * Enemy Wave Spawner Script
     * Spawn enemies in waves
     * Uses IEnumerator from System.Collections
     */

    public static int EnemiesAlive = 0;

    [Header("Wave Settings")]
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Vector2 zRange;
    public float timeBetweenWaves = 5f;
    public float timeBetweenEnemies = 0.5f;
    public GameObject[] enemyPrefabs;

    [Header("UI Settings")]
    public Text enemiesLeftText;

    private float _countdown = 5f;
    private int _waveIndex;

    private Waves _wavesComp;
    private Wave[] _waves;
    private int _waveCount;

    private void Start()
    {
        EnemiesAlive = 0;
        _wavesComp = GetComponent<Waves>();
        _waves = _wavesComp.myWaves.waves;
        _waveCount = _waves.Length;
    }

    private void Update()
    {
        if (EnemiesAlive > 0) return;

        if (EnemiesAlive < 0) EnemiesAlive = 0;

        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
            return;
        }

        _countdown -= Time.deltaTime;

        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
    }

    private void FixedUpdate()
    {
        EnemiesAlive = FindObjectsOfType<Enemy>().Length;
        enemiesLeftText.text = "" + EnemiesAlive;
    }

    private IEnumerator SpawnWave()
    {
        _waveIndex++;
        PlayerStats.Waves = _waveIndex;

        Wave currentWave = _waves[(_waveIndex - 1) % _waveCount];
        int multiply = (_waveIndex - 1) / _waveCount + 1;


        for (var i = currentWave.enemies.Length - 1; i >= 0; i--)
        {
            for (var j = 0; j < currentWave.enemies[i] * multiply; j++)
            {
                SpawnEnemy(i);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
    }

    private void SpawnEnemy(int type)
    {
        Instantiate(enemyPrefabs[type], new Vector3(spawnPoint.position.x, spawnPoint.position.y, Random.Range(zRange.x, zRange.y)), spawnPoint.rotation);
        //EnemiesAlive++;
    }
}
