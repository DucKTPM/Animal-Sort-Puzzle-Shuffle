using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool StateGame = false;
    [SerializeField] private GameView gameView;
    [SerializeField] private LevelDataManager levelDataManager;

    private void OnEnable()
    {
        StartGame();
    }

    private void StartGame()
    {
        LevelData levelData = levelDataManager.ReadLevelData();
        gameView.StartGenerateMapLevel(levelData);
    }
}
