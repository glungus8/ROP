using UnityEngine;
using System.Collections;
using TMPro;

public class wave_manager : MonoBehaviour
{
    public static wave_manager instance;
    public TextMeshProUGUI displayText, summaryText;
    public string difficulty;
    public int waves;
    public GameObject exitPortal;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    float waveTimer = 3f, r;
    bool waitingForMod = false;
    int wave = 0;
    int enemiesAlive = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //skip levelu jen tak pro rychlejsi testovani :)
        {
            FinishLevel();
        }
    }
    void Awake()
    {
        instance = this;
        if (exitPortal != null) exitPortal.SetActive(false);
    }

    void Start()
    {
        displayText = GameObject.Find("info").GetComponent<TextMeshProUGUI>();
        summaryText = GameObject.Find("info2").GetComponent<TextMeshProUGUI>();

        //odmena podle lvl
        r = difficulty == "Hard" ? 100f : (difficulty == "Medium" ? 50f : 20f);

        //najde spawnpointy
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = GetComponentsInChildren<Transform>();
        }

        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (wave < waves)
        {
            wave++;

            float timer = waveTimer;
            while (timer > 0) //cas mezi wave
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

            //mod kazdy 3 vlny
            if (wave % 1 == 0 && wave < waves)
            {
                waitingForMod = true;
                modifier_ui.instance.OpenMenu();
                while (waitingForMod) yield return null;
            }
        }
        FinishLevel();
    }

    public void ModSelected()
    {
        waitingForMod = false;
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
        if (displayText != null) displayText.text = "LEVEL COMPLETED\n";

        if (summaryText != null) summaryText.text = "BASE REWARD " + r
                + "c\nMULT " + modifier_manager.instance.rewardMultiplier
                + "x\n==============="
                + "\nREWARD " + GetReward() + "c";
        if (exitPortal != null) exitPortal.SetActive(true);
    }

    public int GetReward()
    {
        float bonus = r;
        //* nasobek z modu
        if (modifier_manager.instance != null)
        {
            bonus *= modifier_manager.instance.rewardMultiplier;
        }

        int reward = Mathf.Max(5, Mathf.RoundToInt(bonus)); //zaokrouhleni + odmena musi byt aspon 5
        return reward;
    }
}