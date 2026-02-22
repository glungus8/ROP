using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class wave_manager : MonoBehaviour
{
    public static wave_manager instance;
    public TextMeshProUGUI displayText;
    public string difficulty;
    public int waves;
    public GameObject exitPortal;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float waveTimer = 5f;

    int wave = 0;
    int enemiesAlive = 0;

    void Awake()
    {
        instance = this;
        if (exitPortal != null) exitPortal.SetActive(false);
    }

    void Start()
    {
        displayText = GameObject.Find("wave_text").GetComponent<TextMeshProUGUI>();

        //najde spawnpointy
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            List<Transform> points = new List<Transform>();
            foreach (Transform point in transform) points.Add(point);
            spawnPoints = points.ToArray();
        }

        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (wave < waves)
        {
            wave++;

            float timer = waveTimer;
            while (timer > 0)
            {
                displayText.text = "Wave " + wave + " in " + Mathf.Ceil(timer) + "s";
                yield return new WaitForSeconds(1f);
                timer -= 1f;
            }

            if (displayText != null) displayText.text = "";

            SpawnWave();

            //ceka se nez vsichni enemy umrou
            while (enemiesAlive > 0)
            {
                yield return null;
            }
        }

        FinishLevel();
    }

    void SpawnWave()
    {
        int count = wave + 5;
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)]; //vybere si random spawnpoint
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; //vybere si random enemaka
        Instantiate(prefab, sp.position, Quaternion.identity); //spawne enemaka
        enemiesAlive++;
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }

    void FinishLevel()
    {
        if (displayText != null) displayText.text = "LEVEL COMPLETED";
        if (exitPortal != null) exitPortal.SetActive(true);
    }

    public int GetReward() => difficulty == "Hard" ? 100 : (difficulty == "Medium" ? 50 : 20);
}