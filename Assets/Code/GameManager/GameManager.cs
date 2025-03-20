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
    private int typeEffectItem = 0;
    public int TypeEffectItem { get => typeEffectItem; set => typeEffectItem = value; }
    public static GameManager Instance;
    private int valueTimeBomb = 0;
    private Bomb bomb;
    private Cage cage;
    private Animal animalOnCage;
   [SerializeField] private List<KeyUnlock> listKeyUnlock = new List<KeyUnlock>();
    public Animal AnimalOnEgg => animalOnEgg;
    
    private Animal animalOnEgg;
    private Tree treeLock;
    public Tree TreeLock { get => treeLock; set => treeLock = value; }
    public Animal AnimalOnCage => animalOnCage;
    private List<Animal> animalsSleep = new List<Animal>();
    public List<Animal> AnimalsSleep => animalsSleep;
    private void StartGame()
    {   
        Setup();
        LevelData levelData = levelDataManager.ReadLevelData();
        gameView.StartGenerateMapLevel(levelData);
        if (typeEffectItem == 1)
        {   
            coroutineType1 =  StartCoroutine(IeUpdateUserControlType1());
        }
        StartCoroutine(StartWaitWinGame());
    }

    private void Setup()
    {
        
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

        if (listKeyUnlock.Count !=0)
        {
            listKeyUnlock.Clear();
        }

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
        yield return new WaitUntil(() => valueTimeBomb <=0);
        Debug.Log("Game Over");
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
        if (listKeyUnlock.Count <=0)
        {
            treeLock.UnlockTree();
        }
    }
}
