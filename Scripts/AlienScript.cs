using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float shootingDelay;
    public float lastTimeShot = 0f;
    public float bulletSpeed;
    public Transform player;
    public GameObject bullet;
    public GameObject explosion;
    public SpriteRenderer spriteRenderer;
    public Collider2D _collider;
    public bool disabled;
    public int points;
    public float timeBeforeSpawning;

    public Transform startPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag ("Player").transform;

        NewLevel();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(disabled)
        {
            //if (Time.time > levelStartTime + timeBeforeSpawning)
            //{
            //    Disable();
            //}
            return;
        }
        if(Time.time > lastTimeShot + shootingDelay)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject newBullet = Instantiate(bullet, transform.position, q);

            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            lastTimeShot = Time.time;
        }
    }
    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    public void NewLevel()
    {
        Disable();
        timeBeforeSpawning = Random.Range(5f, 20f);
        Invoke("Enable", timeBeforeSpawning);
    }

    void Enable()
    {
        transform.position = startPosition.position;

        _collider.enabled = true;
        spriteRenderer.enabled = true;
        disabled = false;
    }

    private void Disable()
    {
        
        _collider.enabled = false;
        spriteRenderer.enabled = false;
        disabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("bullet"))
        {
            player.SendMessage("ScorePoints", points);

            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }
}
