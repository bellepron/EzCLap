using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSceneController : MonoBehaviour//, IEndGameObserver
{
    // #region Field Declarations

    // [Header("Enemy & Power Prefabs")]
    // [Space]
    // // [SerializeField] private AI_NavMesh opponentPrefab;
    // // [SerializeField] private Mortar mortarPrefab;
    // [SerializeField] private InputHandler player;

    // [Header("Level Definitions")]
    // [Space]
    // public List<LevelDefinition> levels;
    // public LevelDefinition currentLevel;

    // #endregion

    // #region Startup
    // void Start()
    // {
    //     if (Globals.currentLevelIndex >= levels.Count)
    //         Globals.currentLevelIndex = 0;

    //     StartLevel(Globals.currentLevelIndex);

    //     GameManager.Instance.AddObserver(this);
    // }
    // #endregion

    // #region Level Management

    // private void StartLevel(int levelIndex)
    // {
    //     currentLevel = levels[levelIndex];
    // }

    // // public void GameEnd()
    // // {
    // //     if (!Globals.isPlayerWin)
    // //     {
    // //         if (Globals.currentLevelIndex == levels.Count - 1)
    // //         {
    // //             Globals.currentLevelIndex = 0;
    // //         }
    // //         //StopAllCoroutines();

    // //         StartLevel(Globals.currentLevelIndex);
    // //     }

    // //     if (Globals.isPlayerWin)
    // //     {
    // //         if (Globals.currentLevelIndex == levels.Count - 1)
    // //         {
    // //             Globals.currentLevelIndex = 0;
    // //         }
    // //         else
    // //             Globals.currentLevelIndex++;
    // //         //StopAllCoroutines();

    // //         StartLevel(Globals.currentLevelIndex);
    // //     }
    // // }

    // // // public void EndLevel()
    // // // {
    // // //     currentLevelIndex++;
    // // //     StopAllCoroutines();

    // // //     //If last level the game over
    // // //     if (currentLevelIndex < levels.Count)
    // // //     {
    // // //         StartLevel(currentLevelIndex);
    // // //     }
    // // // }

    // #endregion

    // #region Spawning 
    // public void Spawn_SpawnManager()
    // {
    //     // SpawnManager spawnManager = Instantiate(Resources.Load("Spawn Manager"), Vector3.zero, Quaternion.identity) as SpawnManager;
    //     GameObject spawnManagerObject = Instantiate(Resources.Load("Spawn Manager"), Vector3.zero, Quaternion.identity) as GameObject;
    //     SpawnManager spawnManager = spawnManagerObject.GetComponent<SpawnManager>();

    //     spawnManager.spawnInterval = currentLevel.enemySpawnInterval;
    //     spawnManager.hydraSpeed = currentLevel.hydraSpeed;
    //     spawnManager.chimeraSpeed = currentLevel.chimeraSpeed;
    //     spawnManager.StartSpawning();
    // }

    // #endregion

    // public void GameEnd()
    // {
    //     Globals.currentLevelIndex++;
    // }
}