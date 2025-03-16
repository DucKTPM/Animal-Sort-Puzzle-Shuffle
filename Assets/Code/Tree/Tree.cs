using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
                position.x += 0.4f;
            }

            if (viewPostion.x > 0.5f)
            {
                position.x -= 0.4f;
                
                var obj = Instantiate(animals[idAnimals[i]], position, Quaternion.identity, parent: transform);

                animalsCopy.Add(obj);
            }

            currentAnimal++;
        }


        StartEffectToAppear();
        yield return null;
    }
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
        if (GameView.Instance.StateClickedTree == true) // nếu có cây được chọn
        {
            if (GameView.Instance.GetTree() == this) // kiểm tra cây được chọn phải là cây này
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
                        animal[i].Jump(position,0.5f,this);
                        if (viewPostion.x <= 0.5f)
                        {
                            position.x += 0.375f;
                        }

                        GameView.Instance.RemoveAnimalOnTreeClicked(animal[i]);
                    }
                  //  ShakeTree();
           
                }
            }
            else
            {
                var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
                var animal = GameView.Instance.GetAnimalsClicked();
                for (int i = 0; i < animal.Count; i++)
                {
                    if (viewPostion.x > 0.5f)
                    {
                        position.x -= 0.375f;
                    }

                    animalsCopy.Add(animal[i]);
                    animal[i].Jump(position,0.5f,this);
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
                StartCoroutine(JumpOut());
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

    private IEnumerator JumpOut()
    {
        var targetPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (targetPosition.x > 0.5f)
        {
            targetPosition.x += 1;
            position.x = anchorSpawnAninmalLeft.position.x;
        }
        else
        {
            targetPosition.x -= 1;
            position.x = anchorSpawnAninmalRight.position.x;
        }
       
        yield return new WaitForSecondsRealtime(1.5f);

        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        targetPosition.z = 0;
        foreach (var animal in animalsCopy)
        {
            animal.Jump(targetPosition, 0.3f,this);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var animal in animalsCopy)
        {
            animal.gameObject.SetActive(false);
        }
        
        animalsCopy.Clear();
        
    }

    private bool CheckEnoghAnimalOnTree()
    {
        if (animalsCopy.Count != slot)
        {
            return false;
        }
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

    private Coroutine coroutineShakeTree = null;
    public void ShakeTree(float duration = 0.3f, float angle = 0.5f)
    {
        if (coroutineShakeTree== null)
        {
            coroutineShakeTree = StartCoroutine(Shake(duration, angle));
        }
      
    }

    private IEnumerator Shake(float duration, float angle)
    {
        var origin = transform.rotation;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float shakeAngle = Mathf.Sin(Time.time * 50f) * angle;  // Dao động qua lại
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, shakeAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = origin; 
        coroutineShakeTree = null;
    }
}