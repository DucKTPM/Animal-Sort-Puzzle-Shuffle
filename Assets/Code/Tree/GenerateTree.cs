using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateTree : MonoBehaviour
{
    [SerializeField] private List<Tree> treeList = new List<Tree>();
    [SerializeField] private List<Tree> listTreeSpawned = new List<Tree>();
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<GameObject> listTypeObstacles = new List<GameObject>();
    [SerializeField] private List<GameObject> listItemObstacles = new List<GameObject>();
    [SerializeField] private Bomb bomb;
    [SerializeField] private KeyUnlock key;
    [SerializeField] private Cage cage;
    [SerializeField] private Egg egg;
    [SerializeField] private Hammer hammer;
    [SerializeField] private Clock clock;
    [SerializeField] private CageTree cageTree;
    public List<GameObject> ListTypeObstacles => listTypeObstacles;
    public List<Tree> ListTreeSpawned => listTreeSpawned;

    public void ClearTreeSpawned()
    {
        for (int i = 0; i < listTreeSpawned.Count; i++)
        {
            Destroy(listTreeSpawned[i].gameObject);
        }
    }

    public void StartGenerateTree(LevelData levelData)
    {
        if (listTreeSpawned.Count != 0)
            listTreeSpawned.Clear();
        StartCoroutine(IeGenerateTree(levelData));
    }

    bool leftRight = true;
    int count = 0;
    float yUp = 0.5f;
    float yDown = 0.4f;
    bool upDown = true;
    Tree treeInstance = new Tree();

    public void AddTree()
    {
        int[] idAnimals = new int[0];
        var midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        midlePositonCamera.z = 0;
        if (upDown)
        {
            if (leftRight)
            {
                midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0f, yUp, 0));
                midlePositonCamera.z = 0;
                var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                listTreeSpawned.Add(obj);
                leftRight = !leftRight;
                count++;
                obj.StartSpawnAnimalOnTree(idAnimals);
            }
            else
            {
                midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yUp, 0));
                midlePositonCamera.z = 0;
                var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                listTreeSpawned.Add(obj);
                leftRight = !leftRight;
                count++;
                obj.StartSpawnAnimalOnTree(idAnimals);
            }

            if (count == 2)
            {
                yUp += 0.1f;
                count = 0;
                upDown = !upDown;
            }
        }
        else
        {
            if (leftRight)
            {
                midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0f, yDown, 0));
                midlePositonCamera.z = 0;
                var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity); ;
                listTreeSpawned.Add(obj);
                leftRight = !leftRight;
                count++;
                obj.StartSpawnAnimalOnTree(idAnimals);
            }
            else
            {
                midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yDown, 0));
                midlePositonCamera.z = 0;
                var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                listTreeSpawned.Add(obj);
               
                leftRight = !leftRight;
                count++;
                obj.StartSpawnAnimalOnTree(idAnimals);
            }

            if (count == 2)
            {
                yDown -= 0.1f;
                count = 0;
                upDown = !upDown;
            }
        }
    }

    private IEnumerator IeGenerateTree(LevelData levelData)
    {
        int slot = 0;
        int type = 0;
        int sumTrees = levelData.standConfig.Length;
        int[] idAnimals;
        foreach (var standConfig in levelData.standConfig)
        {
            slot = standConfig.standData.numSlot;
            type = standConfig.standData.type;
        }

        slot = Mathf.Max(slot, 4);
        treeInstance = treeList[type];
        var midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        midlePositonCamera.z = 0;
        leftRight = true;
        count = 0;
        yUp = 0.5f;
        yDown = 0.4f;
        upDown = true;

        for (int i = 0; i < sumTrees; i++)
        {
            idAnimals = levelData.standConfig[i].idBirds;
            treeInstance.SetSlot(slot);

            if (upDown)
            {
                if (leftRight)
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0f, yUp, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
                    listTreeSpawned.Add(obj);
                    leftRight = !leftRight;
                    count++;
                    if (slot == 5)
                    {
                        obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                        obj.SetSpace(0.4f);
                        obj.ScaleTree(0.8f,3/4);
                    }

                    if (slot==6)
                    {
                        obj.transform.localScale = new Vector3(0.65f, 0.65f, 0.75f);
                        obj.SetSpace(0.3f);
                       // obj.ScaleTree(0.9f,3f/4);
                    }
                }
                else
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yUp, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
                    listTreeSpawned.Add(obj);
                    leftRight = !leftRight;
                    count++;
                    if (slot == 5)
                    {
                        obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                        obj.SetSpace(0.4f);
                    }

                    if (slot==6)
                    {
                        obj.transform.localScale = new Vector3(0.65f, 0.65f, 0.75f);
                        obj.SetSpace(0.3f);
                    }
                }

                if (count == 2)
                {
                    yUp += 0.1f;
                    count = 0;
                    upDown = !upDown;
                }
            }
            else
            {
                if (leftRight)
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0f, yDown, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
                    listTreeSpawned.Add(obj);
                    leftRight = !leftRight;
                    count++;
                    if (slot == 5)
                    {
                        obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                        obj.SetSpace(0.4f);
                    }

                    if (slot==6)
                    {
                        obj.transform.localScale = new Vector3(0.65f, 0.65f, 0.75f);
                        obj.SetSpace(0.3f);
                    }
                }
                else
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yDown, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
                    listTreeSpawned.Add(obj);

                    leftRight = !leftRight;
                    count++;
                    if (slot == 5)
                    {
                        obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                        obj.SetSpace(0.4f);
                    }

                    if (slot==6)
                    {
                        obj.transform.localScale = new Vector3(0.65f, 0.65f, 0.75f);
                        obj.SetSpace(0.3f);
                    }
                 
                }

                if (count == 2)
                {
                    yDown -= 0.1f;
                    count = 0;
                    upDown = !upDown;
                }
            }
        }

        SpawnObstace(levelData.extralsConfig);
        yield return null;
    }

  
    private void SpawnObstace(ExtralsConfig[] levelDataExtralConfig)
    {
        if (levelDataExtralConfig == null)
        {
            Debug.Log("LevelDataExtralConfig is null");
        }
        else
        {
            for (int j = 0; j < levelDataExtralConfig.Length; j++)
            {
                int type = levelDataExtralConfig[j].type;
                int indexStand = levelDataExtralConfig[j].positionData.indexStand;
                int indexBird = levelDataExtralConfig[j].positionData.indexBird;
                int extralValues = levelDataExtralConfig[j].extralValues[j]; // thoi gian , chim cam chia khoa

                if (type == 1)
                {
                    var animal = listTreeSpawned[indexStand]
                        .AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count - indexBird - 1];
                    var obj = Instantiate(this.bomb, animal.transform.position, Quaternion.identity);
                    obj.transform.parent = animal.transform;
                    obj.SetValueBomb(extralValues);
                    animal.SetBombInAnimal(obj);
                    GameManager.Instance.SetValueBomb(extralValues);
                    GameManager.Instance.SetBomb(obj);
                    GameManager.Instance.SetTypeEffectItem(type);
                }
                else if (type == 2)
                {
                    Debug.Log(indexStand);
                    var animal = listTreeSpawned[indexStand]
                        .AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count - indexBird - 1];
                    var cage = Instantiate(this.cage, animal.transform.position, Quaternion.identity);
                    cage.transform.parent = animal.transform;
                    animal.SetCage(cage);
                    gameManager.SetAnimalOnCage(animal);
                    for (int i = 0; i < listTreeSpawned.Count; i++)
                    {
                        foreach (var variaAnimal in listTreeSpawned[i].AnimalsOnTree) //key
                        {
                            if (variaAnimal.getNameAnimal() == extralValues.ToString())
                            {
                                var obj = Instantiate(key, variaAnimal.transform.position, Quaternion.identity);
                                obj.transform.parent = variaAnimal.transform;
                                variaAnimal.SetKeyUnlocked(obj);
                                goto endLoop;
                            }
                        }
                    }

                    GameManager.Instance.SetTypeEffectItem(type);
                    endLoop: ;
                }
                else if (type == 3)
                {
                    var animal = listTreeSpawned[indexStand]
                        .AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count - indexBird - 1];
                    var egg = Instantiate(this.egg, animal.transform.position, Quaternion.identity);
                    egg.transform.parent = animal.transform;
                    animal.SetEgg(egg);
                    gameManager.SetAnimalOnEgg(animal);

                    for (int i = 0; i < listTreeSpawned.Count; i++)
                    {
                        foreach (var variaAnimal in listTreeSpawned[i].AnimalsOnTree) //hammer
                        {
                            if (variaAnimal.getNameAnimal() == extralValues.ToString())
                            {
                                var obj = Instantiate(hammer, variaAnimal.transform.position, Quaternion.identity);
                                obj.transform.parent = variaAnimal.transform;
                                variaAnimal.SetHammer(obj);
                                goto endLoop;
                            }
                        }
                    }

                    endLoop: ;
                }
                else if (type == 4)
                {
                    var animal = listTreeSpawned[indexStand]
                        .AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count - indexBird - 1];
                    var clock = Instantiate(this.clock, animal.transform.position, Quaternion.identity);
                    clock.transform.parent = animal.transform;
                    animal.SetClock(clock);
                    for (int i = 0; i < levelDataExtralConfig[j].extralValues.Length; i += 2)
                    {
                        int indexTree = levelDataExtralConfig[j].extralValues[i + 1];
                        Debug.Log(indexTree);
                        int indexBirdSleep = levelDataExtralConfig[j].extralValues[i];
                        Debug.Log(indexBirdSleep);
                        listTreeSpawned[indexTree].AnimalsOnTree[indexBirdSleep].Sleep();
                        gameManager.AddAnimalsSleep(listTreeSpawned[indexTree].AnimalsOnTree[indexBirdSleep]);
                    }
                }
                else if (type == 5)
                {
                    var treeCage = listTreeSpawned[indexStand];
                    var cageTree = Instantiate(this.cageTree, treeCage.transform.position, Quaternion.identity);
                    cageTree.transform.parent = treeCage.transform;
                    treeCage.LockTree = true;
                    GameManager.Instance.SetTypeEffectItem(type);
                    GameManager.Instance.TreeLock = treeCage;
                    treeCage.SetCageTree(cageTree);
                    int count = 0;
                    List<Animal> animalsSpawned = new List<Animal>();
                    bool flag = false;
                    foreach (var tree in listTreeSpawned)
                    {
                        foreach (var animal in tree.AnimalsOnTree)
                        {
                            flag = true;
                            foreach (var animaCage in treeCage.AnimalsOnTree)
                            {
                                if (animaCage.getNameAnimal() == animal.getNameAnimal())
                                {
                                    flag = false;
                                }
                            }

                            if (flag)
                                animalsSpawned.Add(animal);
                        }
                    }

                    Animal animalTemp = new Animal();
                    foreach (var animal in animalsSpawned)
                    {
                        if (count <= 1 && animalTemp.getNameAnimal() != animal.getNameAnimal())
                        {
                            var obj = Instantiate(key, animal.transform.position, Quaternion.identity);
                            obj.transform.parent = animal.transform;
                            gameManager.AddKeyUnlock(obj);
                            animal.SetKeyUnlocked(obj);
                            animalTemp = animal;
                            count++;
                        }
                    }
                    // foreach (var variaAnimal in treeCage.AnimalsOnTree)
                    // {
                    //     foreach (var treeSpawned in listTreeSpawned)
                    //     {
                    //         foreach (var animal in treeSpawned.AnimalsOnTree)
                    //         {
                    //             if (variaAnimal.name != animal.name && count <=1)
                    //             {
                    //                 
                    //             }
                    //         }
                    //     }
                    // }
                }
            }
        }
    }

    public void CloseTreeSpawned()
    {
        foreach (var tree in listTreeSpawned)
        {
            tree.CloseTree();
        }
    }
}