using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private string nameAnimal;
    [SerializeField] private SpriteRenderer animalRenderer;
    [SerializeField] private Bomb bomb;
    public Bomb Bomb => bomb;
    [SerializeField] private bool sleeping = false;
    [SerializeField] Cage cage;
    public Cage Cage => cage;

    [SerializeField] private KeyUnlock keyUnlock;
    public KeyUnlock KeyUnlock => keyUnlock;
    private Egg egg;
    public Egg Egg => egg;
    private Hammer hammer;
    public Hammer Hammer => hammer;
    private Clock clock;
    public Clock Clock => clock;
    public bool Sleeping => sleeping;
    private void OnEnable()
    {
        sleeping = false;
    }


    public void SetClock(Clock clock)
    {
        this.clock = clock;
    }

    public void SetHammer(Hammer hammer)
    {
        this.hammer = hammer;
    }

    public void BreakEgg()
    {
        egg.BreakEgg();
        egg = null;
    }

    public void HideHammer()
    {
        hammer.gameObject.SetActive(false);
    }

    public void SetEgg(Egg egg)
    {
        this.egg = egg;
    }

    public void SetKeyUnlocked(KeyUnlock keyUnlock)
    {
        this.keyUnlock = keyUnlock;
    }

    public void HideKey()
    {
        keyUnlock.HideKey();
    }

    public void UnlockCage()
    {
        cage.UnLockCage();
        cage = null;
    }

    public void SetCage(Cage cage)
    {
        this.cage = cage;
    }

    public void SetBombInAnimal(Bomb bomb)
    {
        this.bomb = bomb;
    }

    public Bomb GetBombInAnimal()
    {
        return this.bomb;
    }

    public string getNameAnimal()
    {
        return this.nameAnimal;
    }

    public void Click()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        animalRenderer.color = Color.yellow;
    }

    public void RemoveClickedAnimal()
    {
        if (animalRenderer!=null)
        {
            animalRenderer.color = Color.white;
        }
      
    }

    public void Jump(Vector3 endPosition, float h, Tree anchorTree)
    {
        gameObject.transform.SetParent(anchorTree.transform);
        StartCoroutine(IeJump(transform.position, endPosition, h, anchorTree));
    }

    private IEnumerator IeJump(Vector3 startPosition, Vector3 endPosition, float heightMultiplier, Tree anchorTree)
    {
        float elapsedTime = 0f;
        float duration = 1f;
        float maxHeight = Vector3.Distance(startPosition, endPosition) * heightMultiplier; // Độ cao tối đa

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;


            Vector3 position = Vector3.Lerp(startPosition, endPosition, t);

            // Tính toán vị trí Y theo quỹ đạo parabol
            position.y += maxHeight - 4 * maxHeight * (t - 0.5f) * (t - 0.5f);

            transform.position = position;
            yield return null;
        }

        transform.position = endPosition; // Đảm bảo đặt đúng vị trí kết thúc
        anchorTree.ShakeTree();
    }

    public void Sleep()
    {
        sleeping = true;
        Click();
    }

    public void SetSleep(bool b)
    {
        sleeping = b;
    }

    public void WakeUp()
    {
        sleeping = false;
        RemoveClickedAnimal();
    }

    public void HideClock()
    {
        if (clock != null)
        {
            clock.gameObject.SetActive(false);
        }
    }
}