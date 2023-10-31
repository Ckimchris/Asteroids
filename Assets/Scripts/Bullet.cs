using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public Rigidbody2D myRigidBody { get; private set; }
    public float lifeTime = 1f;
    public float speed = 500f;

    // Start is called before the first frame update
    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }


    public void Shoot(Vector2 direction)
    {
        myRigidBody.AddForce(direction * speed);

        Destroy(gameObject, lifeTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet as soon as it collides with anything
        Destroy(gameObject);
    }

}
