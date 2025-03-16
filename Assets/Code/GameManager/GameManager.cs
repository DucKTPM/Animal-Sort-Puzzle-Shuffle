using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool stateGame = false;
    [SerializeField] private GameView gameView;
    [SerializeField] private LevelDataManager levelDataManager;
    [SerializeField] private GenerateTree generateTree;
    public static GameManager Instance;
    private void OnEnable()
    {
        Instance = this;
        StartGame();
    }

    private void StartGame()
    {
        stateGame = true;
        LevelData levelData = levelDataManager.ReadLevelData();
        gameView.StartGenerateMapLevel(levelData);
        StartCoroutine(StartWaitWinGame());
    }

    private IEnumerator StartWaitWinGame()
    {
        yield return new WaitUntil(() => stateGame == false);
        generateTree.CloseTreeSpawned();
    }

    Coroutine coroutineCheckWin = null;
    public void StartCheckWinGame()
    {
        if (coroutineCheckWin == null)
        {
            StartCoroutine(IeWaitWinGame());
        }
        else
        {
            StopCoroutine(coroutineCheckWin);
            coroutineCheckWin = null;
        }
       
    }
    private IEnumerator IeWaitWinGame()
    {
        yield return new WaitUntil(()=>CheckWin(generateTree.ListTreeSpawned));
        Debug.Log("Win Game");
    }

    public bool CheckWin(List<Tree> listTreeSpawn)
    {
        foreach (var tree in listTreeSpawn)
        {

            Debug.Log(tree.AnimalsOnTree.Count);
            if (tree.AnimalsOnTree.Count != 0)
            {
                return false;
            }
        }
        stateGame = false;
        return true;    
    }
}
