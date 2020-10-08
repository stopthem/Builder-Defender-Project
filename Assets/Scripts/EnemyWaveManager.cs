﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance {get; private set;}
    public event EventHandler OnWaveNumberChanged;
    private enum State{
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    private State state;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private Vector3 spawnPosition;
    private int remainingEnemySpawnAmount;
    private int waveNumber;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0,spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextEnemySpawnTimer = 3f;
    }
    private void Update()
    {
        switch(state){
            case State.WaitingToSpawnNextWave:
            nextWaveSpawnTimer -= Time.deltaTime;
            if (nextWaveSpawnTimer < 0f)
            {
                SpawnWave();
            }
            break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0, .2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDirection() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;
                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0,spawnPositionTransformList.Count)].position;
                            nextWaveSpawnTimer = 15f;
                        }
                    }
                }
            break;
        }      
    }
    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 4 + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this,EventArgs.Empty);
    }
    public int GetWaveNumber()
    {
        return waveNumber;
    }
    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
