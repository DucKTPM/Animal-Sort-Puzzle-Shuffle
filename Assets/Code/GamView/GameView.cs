using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private GenerateTree generateTree;
    [SerializeField] private Tree tree;
    [SerializeField] private List<Animal> animalsClicked;
    public static GameView Instance;
    private bool stateClickedTree = false;
    public bool StateClickedTree => stateClickedTree;
    private void OnEnable()
    {
        Instance = this;
    }

    public void RemoveAnimalOnTreeClicked(Animal animal)
    {
        tree.RemoveAnimal(animal);
    }

    public Tree GetTree()
    {
        return tree;
    }
    public void SetTree(Tree tree)
    {
        this.tree = tree;
    }
    public Animal GetAnimalClicked()
    {
        if (animalsClicked.Count != 0)
        {
            return animalsClicked[0];
        }   
        return null;
    }

    public List<Animal> GetAnimalsClicked()
    {
        return animalsClicked;
    }
    
    public void SetAnimalsClick(List<Animal> animals)
    {
        if (animalsClicked != null)
        {
            foreach (var animal in animalsClicked)
            {
                animal.RemoveClickedAnimal();
            }
            animalsClicked.Clear();
            animalsClicked = animals;
            foreach (var animal in animalsClicked)
            {
                animal.Click();
            }
            stateClickedTree = true;
        }
     
    }

   

    public void AnimalsCancelClicked()
    {
        if (animalsClicked != null)
        {
            foreach (var VARIABLE in animalsClicked)
            {
                VARIABLE.RemoveClickedAnimal();
            }
        }
        tree = null;
        animalsClicked.Clear();
        stateClickedTree = false;
    }
    public void StartGenerateMapLevel(LevelData levelData)
    {
        if (levelData != null)
        {
            generateTree.StartGenerateTree(levelData);
        }
        else
        {
            Debug.Log("Not found level data");
        }
    }
    
}
