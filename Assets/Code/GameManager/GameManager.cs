using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool stateGame = false;
    [SerializeField] private GameView gameView;
    [SerializeField] private LevelDataManager levelDataManager;
    [SerializeField] private GenerateTree generateTree;
    [SerializeField] private List<KeyUnlock> listKeyUnlock = new List<KeyUnlock>();
    [SerializeField] private LevelPopup levelPopup;
    [SerializeField] private MenuWinGame menuWinGame;
    [SerializeField] private int addTrees = 0;
    [SerializeField] private TextMeshProUGUI textAddTrees;
    [SerializeField] private TextMeshProUGUI textUndo;
    [SerializeField] private int numberUndo = 0;
    [SerializeField] private int nextLevel = 0;
    [SerializeField] private TextMeshProUGUI textNextLevel;
    [SerializeField] private MenuWinGame menuGameOver;
    public GameObject totalPanel;

    [SerializeField]
    private int numberCoin
    {
        get=>PlayerPrefs.GetInt("numberCoin",0);
        set=>PlayerPrefs.SetInt("numberCoin",value);
    }
    [SerializeField] private TextMeshProUGUI textCoin;
    
    public int NumberUndo => numberUndo;
        
    public int NextLevel => nextLevel;

    public void NextLevelUp()
    {
        if (nextLevel>0)
        {
            stateGame = false;
            nextLevel--;
           
            
        }
   
    }

    public void SetTextCoin()
    {
        textCoin.text = numberCoin.ToString();
    }

    public void SetTextNextLevel()
    {
        textNextLevel.text = nextLevel.ToString();
    }
    

    public void AddTreeOnScene()
    {
        if (addTrees >0)
        {
            generateTree.AddTree();
            addTrees--;
            SetTextAddTree();
        }
    }
    public bool StateGame
    {
        get => stateGame;
        set => stateGame = value;
    }

    public int TypeEffectItem
    {
        get => typeEffectItem;
        set => typeEffectItem = value;
    }

    private void SetTextAddTree()
    {
        textAddTrees.text = addTrees.ToString();
    }

    public static GameManager Instance;
    private int valueTimeBomb = 0;
    private Bomb bomb;
    private Cage cage;
    private Animal animalOnCage;
    private int typeEffectItem = 0;
    public Animal AnimalOnEgg => animalOnEgg;
    private Animal animalOnEgg;
    private Tree treeLock;

    public Tree TreeLock
    {
        get => treeLock;
        set => treeLock = value;
    }

    public Animal AnimalOnCage => animalOnCage;
    private List<Animal> animalsSleep = new List<Animal>();
    public List<Animal> AnimalsSleep => animalsSleep;


    private Coroutine coroutineRestart = null;

    public void RestartGame()
    {
        if (coroutineRestart == null)
        {
            ClearLevelPlay();
            coroutineRestart = StartCoroutine(IeRestartGame());
        }
          
    }

    public void RestartGameOver()
    {
        menuGameOver.Hide();
       StartGame();
    }

    private IEnumerator IeRestartGame()
    {
        yield return new WaitForSeconds(1f);
        StartGame();
        yield return new WaitForSeconds(1f);
        coroutineRestart = null;
    }

    public void StartGame()
    {   
        Setup();
        LevelData levelData = levelDataManager.ReadLevelData();
        gameView.StartGenerateMapLevel(levelData);
        if (typeEffectItem == 1)
        {
            coroutineType1 = StartCoroutine(IeUpdateUserControlType1());
        }

        if (croutineWaitWinGame != null)
        {
            StopCoroutine(croutineWaitWinGame);
            croutineWaitWinGame = null;
        }
        croutineWaitWinGame =  StartCoroutine(StartWaitWinGame());
    }
    private Coroutine croutineWaitWinGame = null;

    public void AddCoin(int coin)
    {
        this.numberCoin += coin;
        SetTextCoin();
    }
    private void Setup()
    {   SetTextNextLevel();
        SetTextAddTree();
        menuWinGame.Hide();
        levelPopup.SetTextLevelPopup(levelDataManager.CurrentLevelIndex.ToString());
        SetTextCoin();
        SetTextUndo();
        typeEffectItem = 0;
        stateGame = true;
        bomb = null;
        cage = null;
        animalOnCage = null;
        animalOnEgg = null;
        
        if (animalsSleep.Count != 0)
        {
            animalsSleep.Clear();
        }

        if (listKeyUnlock.Count != 0)
        {
            listKeyUnlock.Clear();
        }
    }

    private void SetTextUndo()
    {
        textUndo.text = numberUndo.ToString();
    }

    public void AddAnimalsSleep(Animal animal)
    {
        animalsSleep.Add(animal);
    }

    public void SetAnimalOnEgg(Animal animalOnEgg)
    {
        this.animalOnEgg = animalOnEgg;
    }


    public void SetAnimalOnCage(Animal animal)
    {
        animalOnCage = animal;
    }

    private void OnEnable()
    {
        Instance = this;
        StartGame();
    }

    public void SetValueBomb(int value)
    {
        valueTimeBomb = value;
    }

    public void SetBomb(Bomb bomb)
    {
        this.bomb = bomb;
    }

    public void AnimalJump()
    {
        if (typeEffectItem == 1)
        {
            valueTimeBomb--;
            bomb.SetValueBomb(valueTimeBomb);
        }
        else if (typeEffectItem == 2)
        {
        }
    }


    public void SetTypeEffectItem(int type)
    {
        typeEffectItem = type;
    }

    private Coroutine coroutineType1;

    public void StopCrountineType1()
    {
        if (coroutineType1 != null)
        {
            StopCoroutine(coroutineType1);
            coroutineType1 = null;
        }
    }


    private IEnumerator IeUpdateUserControlType1()
    {
        yield return new WaitUntil(() => valueTimeBomb <= 0);
        bomb.SetEffectBum();
        
        yield return new WaitForSeconds(1f);
        ClearLevelPlay();
        yield return new WaitForSeconds(1f);
        menuGameOver.Show();
    }

    private IEnumerator StartWaitWinGame()
    {
        yield return new WaitUntil(() => stateGame == false);
        ClearLevelPlay();
        levelDataManager.NextLevelIndex();
        menuWinGame.Show();
    }

    private void ClearLevelPlay()
    {
        if (generateTree.ListTreeSpawned!=null)
        {
            generateTree.CloseTreeSpawned();
        }
       
        // generateTree.ClearTreeSpawned();
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
        yield return new WaitUntil(() => CheckWin(generateTree.ListTreeSpawned));
        stateGame = false;
    }

    public bool CheckWin(List<Tree> listTreeSpawn)
    {
        foreach (var tree in listTreeSpawn)
        {
            if (tree.AnimalsOnTree.Count != 0)
            {
                return false;
            }
        }

        stateGame = false;
        return true;
    }

    public void AnimalWakeUp()
    {
        foreach (Animal animal in AnimalsSleep)
        {
            animal.WakeUp();
        }
    }

    public void AddKeyUnlock(KeyUnlock keyUnlock)
    {
        listKeyUnlock.Add(keyUnlock);
    }

    public void RemoveKeyUnlock(KeyUnlock keyUnlock)
    {
        listKeyUnlock.Remove(keyUnlock);
        if (listKeyUnlock.Count <= 0)
        {
            treeLock.UnlockTree();
        }
    }

    public void UndoClick()
    {
        numberUndo--;
        SetTextUndo();
    }
}