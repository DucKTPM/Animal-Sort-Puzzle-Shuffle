using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTree : MonoBehaviour
{
    [SerializeField] private List<Tree> treeList = new List<Tree>();

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
                    leftRight = !leftRight;
                    count++;
                }
                else
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yUp, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
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
                    leftRight = !leftRight;
                    count++;
                }
                else
                {
                    midlePositonCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, yDown, 0));
                    midlePositonCamera.z = 0;
                    var obj = Instantiate(treeInstance, midlePositonCamera, Quaternion.identity);
                    obj.StartSpawnAnimalOnTree(idAnimals);
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

        yield return null;
    }
}