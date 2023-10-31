using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class UFO : MonoBehaviour
{
    public PlayerController ship;
    public Bullet bulletPrefab;
    public Rigidbody2D myRigidBody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public AudioSource fireSFX;
    public float fireSpreadAngle = 1f;

    public float ufoSpeed = 0.2f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();

        StartCoroutine(MoveTowardsPlayerRoutine());

        StartCoroutine(FireAtPlayer());

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FindPlayer()
    {
        if(ship == null)
        {
            ship = GameObject.Find("Ship").GetComponent<PlayerController>();
        }
    }

    IEnumerator FireAtPlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.Shoot(((Random.Range(-fireSpreadAngle, fireSpreadAngle) * ship.transform.position) - transform.position));
            fireSFX.Play();
        }

    }

    IEnumerator MoveTowardsPlayerRoutine()
    {

        while(true)
        {
            var direction = Vector3.zero;
            if (Vector3.Distance(transform.position, ship.transform.position) > 3f)
            {
                direction = ship.transform.position - transform.position;
#if UNITY_EDITOR
                myRigidBody.AddRelativeForce(direction.normalized * 0.1f, ForceMode2D.Force);
#else
                myRigidBody.AddRelativeForce(direction.normalized * ufoSpeed, ForceMode2D.Force);
#endif
            }
            else
            {
                SetVelocity();

                Vector3 dir = (ship.transform.position - transform.position).normalized;
                Vector3 cross = Vector3.Cross(Vector3.up, dir).normalized;

                myRigidBody.AddForce(dir * (Mathf.Pow(1f, 2)) / Vector3.Distance(transform.position, ship.transform.position));

            }
            yield return null;
        }
    }


    void SetVelocity()
    {
        Vector3 dir = (ship.transform.position - transform.position).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, dir).normalized;

        myRigidBody.velocity = cross * 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Game.Instance.OnUFODestroyed(this);

            Destroy(gameObject);
        }
    }
}
