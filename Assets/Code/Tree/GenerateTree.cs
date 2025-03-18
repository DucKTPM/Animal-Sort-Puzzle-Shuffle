using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateTree : MonoBehaviour
{
    [SerializeField] private List<Tree> treeList = new List<Tree>();
    [SerializeField] private List<Tree> listTreeSpawned = new List<Tree>();
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<GameObject> listTypeObstacles = new List<GameObject>();
    [SerializeField] private List<GameObject> listItemObstacles = new List<GameObject>();
    public List<GameObject> ListTypeObstacles => listTypeObstacles;
    public List<Tree> ListTreeSpawned => listTreeSpawned;

    public void StartGenerateTree(LevelData levelData)
    {
        StartCoroutine(IeGenerateTree(levelData));
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

        Tree treeInstance = new Tree();
        treeInstance = treeList[type];
        var midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        midlePositonCamera.z = 0;
        bool leftRight = true;
        int count = 0;
        float yUp = 0.5f;
        float yDown = 0.4f;
        bool upDown = true;
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
             int extralValues = levelDataExtralConfig[j].extralValues[0]; // thoi gian , chim cam chia khoa
 
            if (type == 1)
            {
                var animal = listTreeSpawned[indexStand].AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count-indexBird-1];
                var obj = Instantiate(listTypeObstacles[0],animal.transform.position,Quaternion.identity);
                obj.transform.parent = animal.transform;
            }
            else if (type == 2)
            {   Debug.Log(indexStand);
                var animal = listTreeSpawned[indexStand].AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count-indexBird-1];
                var cage = Instantiate(listTypeObstacles[2],animal.transform.position,Quaternion.identity);
                cage.transform.parent = animal.transform;
                for (int i = 0; i < listTreeSpawned.Count; i++)
                {
                    foreach (var variaAnimal in listTreeSpawned[i].AnimalsOnTree)//key
                    {
                        if (variaAnimal.getNameAnimal() == extralValues.ToString())
                        {
                            var obj = Instantiate(listTypeObstacles[1],variaAnimal.transform.position,Quaternion.identity);
                            obj.transform.parent = variaAnimal.transform; 
                            goto endLoop;
                        }
                    }
                }
                endLoop: ;
            }
            else if (type == 3)
            {
                var animal = listTreeSpawned[indexStand].AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count-indexBird-1];
                var egg = Instantiate(listTypeObstacles[4],animal.transform.position,Quaternion.identity);
                egg.transform.parent = animal.transform;
                for (int i = 0; i < listTreeSpawned.Count; i++)
                {
                    foreach (var variaAnimal in listTreeSpawned[i].AnimalsOnTree)//hammer
                    {   
                        if (variaAnimal.getNameAnimal() == extralValues.ToString())
                        {
                          //  Debug.Log(extralValues);
                            var obj = Instantiate(listTypeObstacles[3],variaAnimal.transform.position,Quaternion.identity);
                            obj.transform.parent = variaAnimal.transform;
                         
                            goto endLoop;
                        }
                    }
                }
                endLoop: ;
                
            }
            else if (type == 4)
            {
                var animal = listTreeSpawned[indexStand].AnimalsOnTree[listTreeSpawned[indexStand].AnimalsOnTree.Count-indexBird-1];
                var clock = Instantiate(listTypeObstacles[5],animal.transform.position,Quaternion.identity);
                clock.transform.parent = animal.transform;
                for (int i = 0; i < levelDataExtralConfig[j].extralValues.Length; i+=2)
                {
                     int indexTree = levelDataExtralConfig[j].extralValues[i];
                     Debug.Log(indexTree);
                     int indexBirdSleep = levelDataExtralConfig[j].extralValues[i+1];
                     Debug.Log(indexBirdSleep);
                     listTreeSpawned[indexTree].AnimalsOnTree[indexBirdSleep].Sleep();
                }
            }
            else if (type == 5)
            {
                var treeCage = listTreeSpawned[indexStand];
                var cageTree = Instantiate(listTypeObstacles[6],treeCage.transform.position,Quaternion.identity);
                cageTree.transform.parent = treeCage.transform;
                int count = 0;
                foreach (var variaAnimal in treeCage.AnimalsOnTree)
                {
                    foreach (var treeSpawned in listTreeSpawned)
                    {
                        foreach (var animal in treeSpawned.AnimalsOnTree)
                        {
                            if (variaAnimal.name != animal.name && count <=1)
                            {
                                var obj = Instantiate(listTypeObstacles[1],animal.transform.position,Quaternion.identity);
                                obj.transform.parent = animal.transform;
                                count++;
                            }
                        }
                    }
                }
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