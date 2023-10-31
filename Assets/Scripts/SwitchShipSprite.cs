using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShipSprite : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite movingSprite;
    private SpriteRenderer ship;
    // Start is called before the first frame update
    void Awake()
    {
        ship = GetComponent<SpriteRenderer>();
    }

    public void ChangeToIdle()
    {
        ship.sprite = idleSprite;
    }

    public void ChangeToMoving()
    {
        ship.sprite = movingSprite;

    }
}
