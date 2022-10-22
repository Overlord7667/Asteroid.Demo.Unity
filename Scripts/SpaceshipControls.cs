using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceshipControls : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D _collider;
    public float thrust;
    public float turnThrust;
    private float thrustInput;
    private float turnInput;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public float bulletForce;
    public float deathForce;
    private bool hyperspace;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;

    public new AudioSource audio;
    public GameObject explosion;
    public GameObject bullet;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        hyperspace = true;
        scoreText.text = "Score " + score;
        livesText.text = "Lives " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

         if(Input.GetButtonDown ("Fire1"))
        {
            GameObject newBullet =  Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 5.0f);
        }

        if (Input.GetButtonDown("Hyperspace") && !hyperspace == false)
        {
            hyperspace = true;
            spriteRenderer.enabled = false;
            _collider.enabled = false;
            Invoke("Hyperspace",1f);
        }

        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust);

        Vector2 newPos = transform.position;
        if(transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }
        if(transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if(transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }
        transform.position = newPos;
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput);
        //rb.AddTorque(-turnInput);
    }

    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }

    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        
        spriteRenderer.enabled = true;
        spriteRenderer.color = inColor;
        Invoke("Invulnerable", 3f);
    }

    void Invulnerable()
    {
        _collider.enabled = true;
        spriteRenderer.color = normalColor;
    }

    void Hyperspace()
    {
        Vector2 newPosition = new Vector2(Random.Range(-21f, 21f),Random.Range(-13f, 13f));
        transform.position = newPosition;
        spriteRenderer.enabled = true;
        _collider.enabled = true;
         
        hyperspace = true;
    }

    void LoseLife()
    {
        lives--;
        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(newExplosion, 3f);
        livesText.text = "Lives " + lives;

        spriteRenderer.enabled = false;
        _collider.enabled = false;
        Invoke("Respawn", 3f);

        if (lives <= 0)
        {
            //GameOver
            GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.relativeVelocity.magnitude > deathForce)
        {
            LoseLife();
        }
        else
        {
            audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Beam"))
        {
            LoseLife();
        }
    }

    void GameOver()
    {
        CancelInvoke();
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
