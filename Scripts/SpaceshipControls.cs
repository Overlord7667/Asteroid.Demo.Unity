using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceshipControls : MonoBehaviour
{
    public Rigidbody2D rb;
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

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    //public GameObject newHighScorePanel;
    //public InputField highScoreInput;
    public GameManager gm;

    public AudioSource audio;

    public GameObject explosion;
    public GameObject bullet;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

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

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;

        Invoke("Invulnerable", 6f);
    }

    void Invulnerable()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if(col.relativeVelocity.magnitude > deathForce)
        {
            lives--;

            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion,2f);
            livesText.text = "Lives " + lives;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Respawn", 3f);

            if(lives <=0)
            {
                //GameOver
                GameOver();
            }
        }
        else
        {
            audio.Play();
        }
    }
    void GameOver()
    {
        CancelInvoke();
            gameOverPanel.SetActive(true);

        //if(gm.CheckForHighScore(score))
        //{
        //    newHighScorePanel.SetActive(true);
        //}
        //else
        //{
        //}
    }
    //public void HighScoreInput()
    //{
    //    string newInput = highScoreInput.text;
    //    Debug.Log(newInput);
    //}

    public void GoToMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Button was Pressed");
    }
}
