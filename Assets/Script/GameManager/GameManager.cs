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
    [SerializeField] private Confetti confetti1;
    [SerializeField] private Confetti confetti2;
    public bool PauseGame = false;
    public int NumberUndo => numberUndo;
    public int NextLevel => nextLevel;
    bool flagCheckNextLevel = false;

    public GameObject totalPanel;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void RestartGame()
    {
        if (coroutineRestart == null && stateGame )
        {
       
            if (croutineWaitWinGame != null)
            {
                StopCoroutine(croutineWaitWinGame);
            }
            gameView.AnimalsCancelClicked();
            ClearLevelPlay();
            coroutineRestart = StartCoroutine(IeRestartGame());
            stateGame = false;
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
 
    public void Setup()
    {
        PauseGame= false;
        stateGame = true;
        menuGameOver.Hide();
        gameView.AnimalsCancelClicked();
        HideEffectWinGame();
        SetTextNextLevel();
        SetTextAddTree();
        menuWinGame.Hide();
        levelPopup.SetTextLevelPopup(levelDataManager.CurrentLevelIndex.ToString());
        SetTextCoin();
        SetTextUndo();
        typeEffectItem = 0;

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

        SetUpImageButton();
    }

    private IEnumerator IeUpdateUserControlType1()
    {
        yield return new WaitUntil(() => valueTimeBomb <= 0);
        if (croutineWaitWinGame != null)
        {
            StopCoroutine(croutineWaitWinGame);
            stateGame = false;
            croutineWaitWinGame = null;
        }
        bomb.SetEffectBum();
        yield return new WaitForSeconds(1f);
        ClearLevelPlay();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayLoseAudio();
        menuGameOver.Show();
    }

    private IEnumerator StartWaitWinGame()
    {
        yield return new WaitUntil(() => stateGame == false);
        
        if (flagCheckNextLevel == false)
        {
            
            yield return new WaitForSeconds(3f);
            AudioManager.instance.PlayWinAudio();
            ShowEffectWinGame();
            levelDataManager.NextLevelIndex();
            var dataAnimal = levelDataManager.ReadLevelData();
            int indexAnimal = dataAnimal.standConfig[0].idBirds[0];
            menuWinGame.SpawnAnimal(indexAnimal);
            ClearLevelPlay();
        }
        else
        {
           
            AudioManager.instance.PlayWinAudio();
            ShowEffectWinGame();
            levelDataManager.NextLevelIndex();
            var dataAnimal = levelDataManager.ReadLevelData();
            int indexAnimal = dataAnimal.standConfig[0].idBirds[0];
            menuWinGame.SpawnAnimal(indexAnimal);
            ClearLevelPlay();
            yield return new WaitForSeconds(0.5f);
        }
        menuWinGame.Show();
    }

    private Coroutine croutineWaitWinGame = null;
    private float _lastTimePlayCoinSound;
    private float GetTime() => Time.time;
    private float _minDeltaTimePlayCoinSound = 0.1f;

    public void AddCoin(int coin)
    {
        this.numberCoin += coin;
        if (GetTime() - _lastTimePlayCoinSound >= _minDeltaTimePlayCoinSound)
        {
            AudioManager.instance.PlayAddCoin();
            _lastTimePlayCoinSound = GetTime();
        }

        SetTextCoin();
    }


    private int numberCoin
    {
        get => PlayerPrefs.GetInt("numberCoin", 0);
        set
        {
            PlayerPrefs.SetInt("numberCoin", value);
            PlayerPrefs.Save();
        }
    }


    public void NextLevelUp()
    {
        if (nextLevel > 0 && flagCheckNextLevel == false && stateGame == true)
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
          
            // Color color3 = imgaeNextLevel.color;
            // color3.a = 1;
            // imgaeNextLevel.color = color3;
            // Color color = textNextLevel.color;
            // color.a = 1;
            // textNextLevel.color = color;
        }
        else
        {
            InvisibleNextLevel();
        }

        StartCoroutine(IeUndo());
        StartCoroutine(IeUpdateAddTree());
        StartCoroutine(IeUpdateNextLevel());
        //SetUnInvisbleAddTree();
    }

    private IEnumerator IeUpdateNextLevel()
    {
        flagCheckNextLevel = true;
       yield return new WaitForSeconds(1f);
       flagCheckNextLevel = false;
        while (true)
        {
            yield return new WaitUntil(() => stateGame && nextLevel > 0);
            UnInvisibleNextLevel();
            yield return new WaitUntil(() => stateGame == false && flagCheckNextLevel == true);
            InvisibleNextLevel();
        }
    }

    private void InvisibleNextLevel()
    {
        Color color2 = imgaeNextLevel.color;
        color2.a = 130 / 255f;
        imgaeNextLevel.color = color2;

        Color color = textNextLevel.color;
        color.a = 130 / 255f;
        textNextLevel.color = color;
    }

    private void UnInvisibleNextLevel()
    {
        Color color2 = imgaeNextLevel.color;
        color2.a = 1;
        imgaeNextLevel.color = color2;

        Color color = textNextLevel.color;
        color.a = 1;
        textNextLevel.color = color;
    }

    bool flagCheckUndo = false;

    private void Update()
    {
        if (gameView.Steps.Count <= 0)
        {
            SetInvisibleUndo();
        }

        if (gameView.Steps.Count > 0)
        {
            SetUnInvisibleUndo();
        }

        if (stateGame == false)
        {
            InvisibleNextLevel();
        }
    }

    private IEnumerator IeUndo()
    {
        while (true)
        { 
            yield return new WaitUntil(()=>!stateGame);
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

    private IEnumerator IeUpdateAddTree()
    {
        yield return new WaitUntil(() => stateGame == false );
        flagCheckAddTrees = true;
        SetInvisbleAddTree();
        yield return new WaitUntil(() => stateGame == true && addTrees >0);
        flagCheckAddTrees = false;
        SetUnInvisbleAddTree();
    }

    private void SetInvisbleAddTree()
    {
        Color color1 = imgaeAddTrees.color;
        color1.a = 130 / 255f;
        imgaeAddTrees.color = color1;
        Color color = textAddTrees.color;
        color.a = 130 / 255f;
        textAddTrees.color = color;
    }

    private void SetUnInvisbleAddTree()
    {
        Color color1 = imgaeAddTrees.color;
        color1.a = 1;
        imgaeAddTrees.color = color1;
        Color color = textAddTrees.color;
        color.a = 1;
        textAddTrees.color = color;
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


    private void ShowEffectWinGame()
    {
        confetti1.Show();
        confetti2.Show();
    }

    private void HideEffectWinGame()
    {
        confetti1.Hide();
        confetti2.Hide();
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
        if (listTreeSpawn.Count==0)
        {
            return false;
        }
        foreach (var tree in listTreeSpawn)
        {
            if (tree.AnimalsOnTree.Count != 0)
            {
                return false;
            }
        }
        Debug.Log("true");
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