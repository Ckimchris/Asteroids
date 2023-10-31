using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScreenWrap : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        float rightSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        float topOfSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        float bottomOfSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).y;

        if (screenPos.x <= 0 && myRigidbody.velocity.x < 0)
        {
            transform.position = new Vector2(rightSideOfScreen, transform.position.y);
        }

        else if(screenPos.x >= Screen.width && myRigidbody.velocity.x > 0)
        {
            transform.position = new Vector2(leftSideOfScreen, transform.position.y);
        }
        else if (screenPos.y >= Screen.height && myRigidbody.velocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, bottomOfSideOfScreen);
        }
        else if (screenPos.y <= 0 && myRigidbody.velocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, topOfSideOfScreen);
        }
    }
}
