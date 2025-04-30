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
    [SerializeField] private List<Step> steps = new List<Step>();
    public List<Step> Steps => steps;
    private void OnEnable()
    {
        Instance = this;
    }
    public void ClearSteps()
    {
        steps.Clear();
    }
    public void AddStep(Step step)
    {
        steps.Add(step);
    }

    public void UndoAction()
    {
        int numberUndo = GameManager.Instance.NumberUndo;
        if (numberUndo > 0 && steps.Count > 0 && GameManager.Instance.StateGame == true)
        {
           
            var step = steps[steps.Count - 1];
            var checkSideViewPos = Camera.main.WorldToViewportPoint(step.TreeStart.transform.position);
            Vector3 newPosition = step.TreeStart.PositionMove;
            for (int i = step.Animals.Count-1; i >=0; i--)
            {
                step.TreeStart.AnimalsOnTree.Add( step.Animals[i]);
                step.Animals[i].Jump(step.TreeStart.PositionMove,0.5f,step.TreeStart,true);
                step.TreeEnd.RemoveAnimal( step.Animals[i]);
               
                if (checkSideViewPos.x <= 0.5f)
                {
                    newPosition.x += 0.5f;
                }
                else
                {
                    newPosition.x -= 0.5f;
                }
                step.TreeStart.SetPostionMove(newPosition);
            }
            // foreach (var animal in step.Animals)
            // {
            //     step.TreeStart.AnimalsOnTree.Add(animal);
            //     animal.Jump(step.TreeStart.PositionMove,0.5f,step.TreeStart,true);
            //     step.TreeEnd.RemoveAnimal(animal);
            //    
            //     if (checkSideViewPos.x <= 0.5f)
            //     {
            //         newPosition.x += 0.5f;
            //     }
            //     else
            //     {
            //         newPosition.x -= 0.5f;
            //     }
            //     step.TreeStart.SetPostionMove(newPosition);
            //
            // }

            steps.Remove(step);
            GameManager.Instance.UndoClick();
        }
        
    }
    
    public void setStateClickedTree(bool state)
    {
        stateClickedTree = state;
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
        AudioManager.instance.PlayClickSound();
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
            foreach (var animal in animalsClicked)
            {
                animal.RemoveClickedAnimal();
            }
            animalsClicked.Clear();
        }
        tree = null;
        stateClickedTree = false;
       // AudioManager.instance.PlayClickSound();
    }
    public void StartGenerateMapLevel(LevelData levelData)
    {
        if(steps.Count>0)
        {
            steps.Clear();
        }
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
