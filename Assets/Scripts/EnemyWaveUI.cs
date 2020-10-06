using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    private TextMeshProUGUI waveNumberText, nextWaveSecondsText;
    private Camera mainCamera;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;
    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        nextWaveSecondsText = transform.Find("nextWaveSecondsText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWavePositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void Update()
    {
        HandleNextWaveMessage();
        
        HandleWaveSpawnPosition();

        HandleClosestEnemy();

    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText("Next wave in " + nextWaveSpawnTimer.ToString("F0") + "s");
        }
    }

    private void HandleWaveSpawnPosition()
    {
        Vector3 directionToTheNextSpawn = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        enemyWaveSpawnPositionIndicator.anchoredPosition = directionToTheNextSpawn * 30f;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(directionToTheNextSpawn));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }

    private void HandleClosestEnemy()
    {
        Enemy targetEnemy = null;
        float targetMaxRadius = 999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
        if (targetEnemy != null)
        {
            Vector3 directionToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            enemyClosestPositionIndicator.anchoredPosition = directionToClosestEnemy * 250f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(directionToClosestEnemy));

            float dirToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
            enemyClosestPositionIndicator.gameObject.SetActive(dirToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void SetMessageText(string message)
    {
        nextWaveSecondsText.SetText(message);
    }
    private void SetWaveNumberText(string text)
    {
        waveNumberText.SetText(text);
    }
}
