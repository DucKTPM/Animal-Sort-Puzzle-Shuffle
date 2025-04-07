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
    [SerializeField] private List<Animal> listAnimalOnTree = new List<Animal>();
    [SerializeField] private int slot;
    [SerializeField] private float space = 0.5f;
    [SerializeField] private CageTree cageTree;
    [SerializeField] private bool lockTree = false;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private Coin coin;
    [SerializeField] private GameObject anchorSpawnCageTreeRight;
    [SerializeField] private GameObject anchorSpawnCageTreeLeft;
    
    public List<Animal> ListAnimalOnTree { get => listAnimalOnTree; set => listAnimalOnTree = value; }
    public GameObject AnchorSpawnCageTreeRight=> anchorSpawnCageTreeRight;
    public GameObject AnchorSpawnCageTreeLeft => anchorSpawnCageTreeLeft;
    public bool LockTree { get { return lockTree; } set { lockTree = value; } }
    public void SetCageTree(CageTree cageTree)
    {
        this.cageTree = cageTree;
    }
    public void SetSpace(float space)
    {
        this.space = space;
    }
    

    public void ScaleTree(float scale, float x)
    {
        treeView.transform.localScale = new Vector3(scale, scale, scale);
        var temp = boxCollider2D.size;
        boxCollider2D.size = new Vector2(temp.x+x, temp.y);
    }
    
    public List<Animal> AnimalsOnTree=> listAnimalOnTree;    

    private int currentAnimal = 0;

    private void OnEnable()
    {
        SetUpTree();
    }

    private void SetUpTree()
    {
        if (slot == 5)
        {
            space = 0.4f;
        }
        
        if (slot == 6)
        {
            space = 0.3f;
        }
    }



    public void StartSpawnAnimalOnTree(int[] idAnimals)
    {
        StartCoroutine(IeStartSpawnAnimal(idAnimals));
  
    }

    Vector3 position = new Vector3();
    public Vector3 PositionMove => position;

    private IEnumerator IeStartSpawnAnimal(int[] idAnimals)
    {
        var viewPostion = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPostion.x <= 0.5f)
        {
            position = anchorSpawnAninmalLeft.position;
        }
        else if (viewPostion.x > 0.5f)
        {
            transform.rotation = Quaternion.Euler(180f, 0, 180f);
            boxCollider2D.offset = new Vector2(1f, 0.3f);
            position = anchorSpawnAninmalRight.position;
        }

        for (int i = 0; i < idAnimals.Length; i++)
        {
            if (currentAnimal >= slot) break;
            if (viewPostion.x <= 0.5f)
            {
                var obj = Instantiate(animals[idAnimals[i]], position, Quaternion.identity, parent: transform);
                listAnimalOnTree.Add(obj);
                position.x += space;
                if (slot == 5)
                {
                    obj.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                }

                if (slot==6)
                {
                    obj.transform.localScale = new Vector3(0.70f, 0.70f, 1);
                }
            }else
            if (viewPostion.x > 0.5f)
            {
                var obj = Instantiate(animals[idAnimals[i]], position, Quaternion.identity, parent: transform);
                position.x -= space;
                listAnimalOnTree.Add(obj);
                if (slot == 5)
                {
                    obj.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                }

                if (slot==6)
                {
                    obj.transform.localScale = new Vector3(0.70f, 0.70f, 1);
                }
                
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

    public void StartEffectToAppear()
    {
        var destination = transform.position;
        var viewPostion = Camera.main.WorldToViewportPoint(destination);
        if (viewPostion.x <= 0.5f)
        {
            viewPostion.x -= space;
        }
        else if (viewPostion.x > 0.5f)
        {
            viewPostion.x += space;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewPostion);
        StartCoroutine(IeStartEffectToAppear(destination));
    }
    private void StartEffectToClose()
    {
        var destination = transform.position;
        var viewPostion = Camera.main.WorldToViewportPoint(destination);
        if (viewPostion.x <= 0.5f)
        {
            viewPostion.x -= space;
        }
        else if (viewPostion.x > space)
        {
            viewPostion.x += space;
        }
        destination = Camera.main.ViewportToWorldPoint(viewPostion);
        StartCoroutine(IeStartEffectToClose(destination));
    }
    private IEnumerator IeStartEffectToClose(Vector3 destination)
    {
        float speed = 5f;
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return new FixedUpdate();
        }

        transform.position = destination;
        gameObject.SetActive(false);
        Destroy(gameObject);
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

    
    private void MoveAnimalsTreeEmptys()
    {
            var checkSideViewPos = Camera.main.WorldToViewportPoint(transform.position);
            var listAnimalsMove = GameView.Instance.GetAnimalsClicked();
            StartCoroutine(IeMoveAnimalsEmpty(checkSideViewPos, listAnimalsMove));
           
        
    }

    private IEnumerator IeMoveAnimalsEmpty(Vector3 checkSideViewPos, List<Animal> listAnimalsMove)
    {
        lockTree = true;
        for (int i = 0; i <listAnimalsMove.Count; i++)
        {
            AnimalsOnTree.Add(listAnimalsMove[i]);
            listAnimalsMove[i].Jump(position,0.5f,this);
            if (checkSideViewPos.x <= 0.5f)
            {
                position.x += space;
            }
            else
            {
                position.x -= space;
            }
            GameView.Instance.RemoveAnimalOnTreeClicked(listAnimalsMove[i]);
                
                
        }
        if (CheckEnoghAnimalOnTree()!= true)
        {
            AddStepToList(listAnimalsMove,GameView.Instance.GetTree(),this);
        }
        GameManager.Instance.AnimalJump();
        yield return new WaitForSeconds(0.9f);
        lockTree = false;
       
    }

    private void AddStepToList(List<Animal> animals,Tree treeStart,Tree treeEnd)
    {
        Step step = new Step(animals,treeStart,treeEnd);
      
        GameView.Instance.AddStep(step);
    }

    public void SetPostionMove(Vector3 position)
    {
        this.position = position;
    }


    private void MoveAnimals()
    {   
        var checkSideViewPos = Camera.main.WorldToViewportPoint(transform.position);
        var listAnimalsMove = GameView.Instance.GetAnimalsClicked();
        int countToMove = Mathf.Min(slot - listAnimalOnTree.Count, listAnimalsMove.Count);
        if (countToMove <= 0) return;
        StartCoroutine(IeMoveAnimail(checkSideViewPos,listAnimalsMove,countToMove));
       
        
    }

    private IEnumerator IeMoveAnimail(Vector3 checkSideViewPos, List<Animal> listAnimalsMove, int countToMove)
    {
        lockTree=true;
        for (int i = 0; i < countToMove; i++)
        {
            AnimalsOnTree.Add(listAnimalsMove[i]);
            listAnimalsMove[i].Jump(position, 0.5f, this);
            Vector3 newPosition = position;
            if (checkSideViewPos.x <= 0.5f)
            {
                newPosition.x += space;
            }
            else
            {
                newPosition.x -= space;
            }

            position = newPosition;
            GameView.Instance.RemoveAnimalOnTreeClicked(listAnimalsMove[i]);
        } 

        if (CheckEnoghAnimalOnTree()!= true)
        {
            AddStepToList(listAnimalsMove,GameView.Instance.GetTree(),this);
        }
        GameManager.Instance.AnimalJump();
        yield return new WaitForSeconds(0.9f);
        lockTree = false;
    }

    private void OnMouseDown()
    {
        if (GameView.Instance.StateClickedTree == true && lockTree !=true && GameManager.Instance.StateGame == true ) // nếu có cây được chọn
        {
            if (GameView.Instance.GetTree() == this) // kiểm tra cây được chọn phải là cây này
            {
                GameView.Instance.AnimalsCancelClicked();
            }
            else if (listAnimalOnTree.Count > 0)// nếu cây  đã có  animal
            {
                if (!CheckSlotFull() && CheckAnimalsSameAnimalOnTree(GameView.Instance.GetAnimalClicked()))
                {
                  MoveAnimals();
                }
            }
            else // nếu không có cây không có animal
            {
                MoveAnimalsTreeEmptys();
            }
            GameView.Instance.AnimalsCancelClicked();
            if (CheckEnoghAnimalOnTree())
            {
                StartCoroutine(JumpOut());
            }
            GameView.Instance.AnimalsCancelClicked();
        }
        else if (GameView.Instance.StateClickedTree == false && listAnimalOnTree.Count !=0 && lockTree!= true && GameManager.Instance.StateGame == true) // chưa cây nào được chọn và cây đấy phải có chim
        {
          
            var listAnimalsCanMove = new List<Animal>();
            var nameTopInline = listAnimalOnTree[listAnimalOnTree.Count - 1].getNameAnimal();
            for (int i = listAnimalOnTree.Count - 1; i >= 0; i--)
            {
                if (nameTopInline == listAnimalOnTree[i].getNameAnimal())
                {
                    if (listAnimalOnTree[i].Cage != null || listAnimalOnTree[i].Egg != null || listAnimalOnTree[i].Sleeping == true)
                    {
                        break;
                    }
                    listAnimalsCanMove.Add(listAnimalOnTree[i]);
                }
                else break;
            }

            if (listAnimalsCanMove.Count > 0)
            {
                GameView.Instance.setStateClickedTree(true);
                GameView.Instance.SetTree(this);
                GameView.Instance.SetAnimalsClick(listAnimalsCanMove);
            }
           
        }
        else
        {
            GameView.Instance.AnimalsCancelClicked();
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
        lockTree = true;
        var coppyList = new List<Animal>(listAnimalOnTree);
        listAnimalOnTree.Clear();
        yield return new WaitForSecondsRealtime(1.5f);
        foreach (var animal in coppyList)
        {
            animal.SetAnimationSmile();
        }
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        targetPosition.z = 0;
      
        foreach (var animal in coppyList)
        {
            
            animal.Jump(targetPosition, 0.3f, this);
            var coin = Instantiate(this.coin,animal.transform.position,Quaternion.identity);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        foreach (var animal in coppyList)   
        {
            animal.gameObject.SetActive(false);
        }
        
        lockTree = false;
        GameManager.Instance.StartCheckWinGame();
    }

    private bool CheckEnoghAnimalOnTree()
    {
        if (listAnimalOnTree.Count != slot)
        {
            return false;
        }
        
        var animalHead = listAnimalOnTree[0].name;
        foreach (var animal in listAnimalOnTree)
        {
            if (animal.name != animalHead)
            {
                return false;
            }
        }

        foreach (var animal in listAnimalOnTree)
        {
            if (animal.GetBombInAnimal()!=null)
            {
                animal.Bomb.BombExploed();
            }
        }
        foreach (var animal in listAnimalOnTree)
        {
            if (animal.KeyUnlock!=null&& GameManager.Instance.TypeEffectItem ==2)
            {
                //Debug.Log("Unlocked");
               // GameManager.Instance.AnimalOnCage.UnlockCage();
                animal.HideKey();
            }

            if (animal.Hammer != null)
            {
                animal.HideHammer();
            }

            if (animal.Clock != null)
            {
                GameManager.Instance.AnimalWakeUp();
                animal.HideClock();
            }

            if (GameManager.Instance.TypeEffectItem == 5 && animal.KeyUnlock!=null)
            {
                GameManager.Instance.RemoveKeyUnlock(animal.KeyUnlock);
                animal.HideKey();
            }
        }
        GameView.Instance.ClearSteps();
        return true;
    }

    private bool CheckSlotFull()
    {
        return listAnimalOnTree.Count == slot;
    }


    private bool CheckAnimalsSameAnimalOnTree(Animal animal)
    {
        if (listAnimalOnTree[listAnimalOnTree.Count - 1].name == animal.name)
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
            position.x +=space;
        }

        if (viewPostion.x <= 0.5f)
        {
            position.x -= space;
        }

        listAnimalOnTree.Remove(animal);
    }

    private Coroutine coroutineShakeTree = null;

    public void ShakeTree(float duration = 0.3f, float angle = 0.5f)
    {
        if (coroutineShakeTree == null)
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
            float shakeAngle = Mathf.Sin(Time.time * 50f) * angle; // Dao động qua lại
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, shakeAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = origin;
        coroutineShakeTree = null;
    }

    public void CloseTree()
    {
        StartEffectToClose();
    }

    public void UnlockTree()
    {
        lockTree = false;
        StartCoroutine(IeWaitUnlockTree());
      
    }

    private IEnumerator IeWaitUnlockTree()
    {
        yield return new WaitForSeconds(1.5f);
        
        cageTree.gameObject.SetActive(false);
    }
}