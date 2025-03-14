using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Tree : MonoBehaviour
{
    [SerializeField] private List<Animal> animals = new List<Animal>();
    [SerializeField] private Transform anchorSpawnAninmalLeft;
    [SerializeField] private Transform anchorSpawnAninmalRight;
    [SerializeField] private GameObject treeView;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private List<Animal> animalsCopy = new List<Animal>();
    [SerializeField] private int slot;

    private int currentAnimal = 0;
    // private int[] arrayIdAnimals;
    // private void OnEnable()
    // {
    //   // StartSpawnAnimalOnTree(arrayIdAnimals);
    //    //StartEffectToAppear();
    // }

    public void StartSpawnAnimalOnTree(int[] idAnimals)
    {
        StartCoroutine(IeStartSpawnAnimal(idAnimals));
    }

    Vector3 position = new Vector3();

    private IEnumerator IeStartSpawnAnimal(int[] idAnimals)
    {
        var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
        //  Vector3 position = new Vector3();
        if (viewPostion.x <= 0.5f)
        {
            position = anchorSpawnAninmalLeft.position;
        }
        else if (viewPostion.x > 0.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
            boxCollider2D.offset = new Vector2(1f, -boxCollider2D.offset.y);
            position = anchorSpawnAninmalRight.position;
        }

        for (int i = 0; i < idAnimals.Length; i++)
        {
            if (currentAnimal >= slot) break;
            if (viewPostion.x <= 0.5f)
            {
                var obj = Instantiate(animals[idAnimals[i]], position, Quaternion.identity, parent: transform);
                animalsCopy.Add(obj);
                position.x += 0.375f;
            }

            if (viewPostion.x > 0.5f)
            {
                position.x -= 0.375f;
                var obj = Instantiate(animals[idAnimals[i]], position, Quaternion.identity, parent: transform);
                animalsCopy.Add(obj);
            }

            currentAnimal++;
        }


        StartEffectToAppear();
        yield return null;
    }

    // public void SetArrayIdAnimals(int[] arrayIdAnimals)
    // {
    //     this.arrayIdAnimals = arrayIdAnimals;
    //
    // }
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }

    private void StartEffectToAppear()
    {
        var destination = transform.position;
        var viewPostion = Camera.main.WorldToViewportPoint(destination);
        if (viewPostion.x <= 0.5f)
        {
            viewPostion.x -= 0.5f;
        }
        else if (viewPostion.x > 0.5f)
        {
            viewPostion.x += 0.5f;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewPostion);
        StartCoroutine(IeStartEffectToAppear(destination));
    }

    private IEnumerator IeStartEffectToAppear(Vector3 destination)
    {
        float speed = 5f;
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return new FixedUpdate();
        }

        transform.position = destination;
    }

    private bool stateClicked = false;

    private void OnMouseDown()
    {
        if (GameView.Instance.StateClickedTree == true)
        {
            if (GameView.Instance.GetTree() == this)
            {
                GameView.Instance.AnimalsCancelClicked();
            }
            else if (animalsCopy.Count > 0)
            {
                if (!CheckSlotFull() && CheckAnimalsSameAnimalOnTree(GameView.Instance.GetAnimalClicked()))
                {
                    Debug.Log("Cant move");
                    var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
                    var animal = GameView.Instance.GetAnimalsClicked();
                    for (int i = 0; i <= slot - animalsCopy.Count; i++)
                    {
                        if (viewPostion.x > 0.5f)
                        {
                            position.x -= 0.375f;
                        }

                        animalsCopy.Add(animal[i]);
                        animal[i].Jump(position);
                        if (viewPostion.x <= 0.5f)
                        {
                            position.x += 0.375f;
                        }

                        GameView.Instance.RemoveAnimalOnTreeClicked(animal[i]);
                    }
                }
            }
            else
            {
                Debug.Log("Cant move");
                var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
                var animal = GameView.Instance.GetAnimalsClicked();
                for (int i = 0; i < animal.Count; i++)
                {
                    if (viewPostion.x > 0.5f)
                    {
                        position.x -= 0.375f;
                    }

                    animalsCopy.Add(animal[i]);
                    animal[i].Jump(position);
                    if (viewPostion.x <= 0.5f)
                    {
                        position.x += 0.375f;
                    }

                    GameView.Instance.RemoveAnimalOnTreeClicked(animal[i]);
                }
                //    GameView.Instance.AnimalsCancelClicked();
            }

            if (CheckEnoghAnimalOnTree())
            {
                Debug.Log("Enough");
                JumpOut();
            }

            GameView.Instance.AnimalsCancelClicked();
        }
        else if (!stateClicked)
        {
            stateClicked = true;
            var listAnimalsCanMove = new List<Animal>();
            var nameTopInline = animalsCopy[animalsCopy.Count - 1].getNameAnimal();

            for (int i = animalsCopy.Count - 1; i >= 0; i--)
            {
                if (nameTopInline == animalsCopy[i].getNameAnimal())
                {
                    listAnimalsCanMove.Add(animalsCopy[i]);
                }
                else break;
            }

            GameView.Instance.SetTree(this);
            GameView.Instance.SetAnimalsClick(listAnimalsCanMove);
        }
        else
        {
            GameView.Instance.AnimalsCancelClicked();
            stateClicked = false;
        }
    }

    private void JumpOut()
    {
    }

    private bool CheckEnoghAnimalOnTree()
    {
        var animalHead = animalsCopy[0].name;
        foreach (var animal in animalsCopy)
        {
            if (animal.name != animalHead)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckSlotFull()
    {
        Debug.Log(animalsCopy.Count == slot);
        return animalsCopy.Count == slot;
    }


    private bool CheckAnimalsSameAnimalOnTree(Animal animal)
    {
        if (animalsCopy[animalsCopy.Count - 1].name == animal.name)
        {
            return true;
        }

        return false;
    }

    public void RemoveAnimal(Animal animal)
    {
        var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPostion.x > 0.5f)
        {
            position.x += 0.375f;
        }

        if (viewPostion.x <= 0.5f)
        {
            position.x -= 0.375f;
        }

        animalsCopy.Remove(animal);
        //  animal.gameObject.SetActive(false);
    }
}