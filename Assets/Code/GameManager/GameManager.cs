using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image imgaeAddTrees;
    [SerializeField] private TextMeshProUGUI textUndo;
    [SerializeField] private Image imgaeUndo;
    [SerializeField] private int numberUndo = 0;
    [SerializeField] private int nextLevel = 0;
    [SerializeField] private TextMeshProUGUI textNextLevel;
    [SerializeField] private Image imgaeNextLevel;
    [SerializeField] private MenuWinGame menuGameOver;
    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private List<Animal> listAnimals;
    public int NumberUndo => numberUndo;
    public int NextLevel => nextLevel;
    bool flagCheckNextLevel = false;

    public GameObject totalPanel;

    private int numberCoin
    {
        get => PlayerPrefs.GetInt("numberCoin", 0);
        set => PlayerPrefs.SetInt("numberCoin", value);
    }
    
    public void NextLevelUp()
    {
        if (nextLevel > 0 && flagCheckNextLevel == false)
        {
            stateGame = false;
            nextLevel--;
            flagCheckNextLevel = true;
            if (nextLevel < 0 || flagCheckNextLevel == true)
            {
                Color color = imgaeNextLevel.color;
                color.a = 130 / 255f;
                imgaeNextLevel.color = color;
                Color colort = textNextLevel.color;
                colort.a = 130 / 255f;
                textNextLevel.color = color;
            }
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

    bool flagCheckAddTrees = false;

    public void AddTreeOnScene()
    {
        if (addTrees > 0 && flagCheckAddTrees == false && stateGame == true)
        {
            generateTree.AddTree();
            addTrees--;
            Color color = imgaeAddTrees.color;
            color.a = 130f / 255f;
            imgaeAddTrees.color = color;
            Color colort = textAddTrees.color;
            textAddTrees.color = color;
            colort.a = 130f / 255f;
            SetTextAddTree();
            flagCheckAddTrees = true;
        }
    }

    public void SetUpImageButton()
    {
        if (addTrees > 0)
        {
            flagCheckAddTrees = false;
            Color color1 = imgaeAddTrees.color;
            color1.a = 1;
            imgaeAddTrees.color = color1;
            Color color = textAddTrees.color;
            color.a = 1;
            textAddTrees.color = color;
        }

        if (numberUndo > 0)
        {
            flagCheckAddTrees = false;
            SetInvisibleUndo();
        }

        if (nextLevel > 0)
        {
            flagCheckNextLevel = false;
            Color color3 = imgaeNextLevel.color;
            color3.a = 255;
            imgaeNextLevel.color = color3;
            Color color = textNextLevel.color;
            color.a = 255;
            textNextLevel.color = color;
        }

        StartCoroutine(IeUndo());
    }

    bool flagCheckUndo = false;

    private IEnumerator IeUndo()
    {
        while (true)
        {
            yield return new WaitUntil(() => gameView.Steps.Count > 0);
            SetUnInvisibleUndo();
            yield return new WaitUntil(() => gameView.Steps.Count == 0);
            SetInvisibleUndo();
        }
    }

    private void SetUnInvisibleUndo()
    {
        Color color2 = imgaeUndo.color;
        color2.a = 1;
        imgaeUndo.color = color2;

        Color color = textUndo.color;
        color.a = 1;
        textUndo.color = color;
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

    private List<Animal> animalOnEgg;

    public void SetAnimalOnEgg(Animal animal)
    {
        if (animalOnEgg == null)
        {
            Debug.LogError("Animal is null! Cannot add to list.");
            return;
        }

        animalOnEgg.Add(animal);
    }

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
        if (coroutineRestart == null && stateGame == true)
        {
            stateGame = false;
            gameView.AnimalsCancelClicked();
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

        croutineWaitWinGame = StartCoroutine(StartWaitWinGame());
    }

    private Coroutine croutineWaitWinGame = null;

    public void AddCoin(int coin)
    {
        this.numberCoin += coin;
        SetTextCoin();
    }

    private void Setup()
    {
        gameView.AnimalsCancelClicked();
        SetUpImageButton();
        SetTextNextLevel();
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
        animalOnEgg = new List<Animal>();

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
        if (flagCheckNextLevel == false)
        {
            yield return new WaitForSeconds(1.5f);
        }

        levelDataManager.NextLevelIndex();
      var dataAnimal = levelDataManager.ReadLevelData();
      int indexAnimal =  dataAnimal.standConfig[0].idBirds[0];
      menuWinGame.SpawnAnimal(indexAnimal);
        ClearLevelPlay();
        
        menuWinGame.Show();
    }

    private void ClearLevelPlay()
    {
        if (generateTree.ListTreeSpawned != null)
        {
            generateTree.CloseTreeSpawned();
        }
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
        if (numberUndo <= 0)
        {
            SetInvisibleUndo();
        }
    }

    public void SetInvisibleUndo()
    {
        Color color1 = imgaeUndo.color;
        color1.a = 130 / 255f;
        imgaeUndo.color = color1;
        Color color = textUndo.color;
        color.a = 130 / 255f;
        textUndo.color = color;
    }
}