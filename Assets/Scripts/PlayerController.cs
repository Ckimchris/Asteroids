using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidBody { get; private set; }
    public Bullet bulletPrefab;
    public SwitchShipSprite shipSprite;
    public Transform nose;
    public AudioSource fireSFX;
    public AudioSource thrustSFX;

    public float thrustSpeed = 2f;
    public bool thrusting { get; private set; }

    public float turnDirection { get; private set; } = 0f;
    public float rotationSpeed = 0.025f;

    public float respawnDelay = 3f;
    public float respawnInvulnerability = 3f;

    private Coroutine flashingRoutine;

    // Start is called before the first frame update
    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Turn off collisions for a few seconds after spawning to ensure the
        // player has enough time to safely move away from asteroids
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }

    // Update is called once per frame
    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            myRigidBody.AddForce(transform.up * thrustSpeed);
            shipSprite.ChangeToMoving();
            thrustSFX.Play();
        }
        else
        {
            shipSprite.ChangeToIdle();

        }

        if (turnDirection != 0f)
        {
            myRigidBody.AddTorque(rotationSpeed * turnDirection);
        }
    }

    public void Fire()
    {
        Bullet bullet = Instantiate(bulletPrefab, nose.position, transform.rotation);
        bullet.Shoot(transform.up);
        fireSFX.Play();
    }

    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        flashingRoutine = StartCoroutine(FlashingState());
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        StopCoroutine(flashingRoutine);
    }

    IEnumerator FlashingState()
    {
        var timer = 0f;
        var spriteRend = shipSprite.GetComponent<SpriteRenderer>();
        while(timer < respawnInvulnerability)
        {
            timer += Time.deltaTime;

            if(spriteRend.enabled)
            {
                spriteRend.enabled = false;
            }
            else
            {
                spriteRend.enabled = true;

            }

            yield return new WaitForSeconds(0.25f);
        }
        spriteRend.enabled = true;
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            myRigidBody.velocity = Vector3.zero;
            myRigidBody.angularVelocity = 0f;
            Game.Instance.OnPlayerDeath(this);
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            myRigidBody.velocity = Vector3.zero;
            myRigidBody.angularVelocity = 0f;
            Game.Instance.OnPlayerDeath(this);
        }
    }
}
