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
            Debug.LogError("LevelDataExtralConfig is null");
        }
        for (int j = 0; j < levelDataExtralConfig.Length; j++)
        {
            Debug.Log("abc");
            int type = levelDataExtralConfig[j].type;
            int indexStand = levelDataExtralConfig[j].positionData.indextStand;
            int indexBird = levelDataExtralConfig[j].positionData.indexBird;
             int extralValues = levelDataExtralConfig[j].extralValues[0]; // thoi gian 
            if (type == 1)
            {
                var animal = listTreeSpawned[indexStand].AnimalsOnTree[indexBird];
                var obj = Instantiate(listTypeObstacles[0],animal.transform.position,Quaternion.identity);
                obj.transform.parent = animal.transform;
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