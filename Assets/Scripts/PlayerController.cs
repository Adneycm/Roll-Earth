using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    // Player
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;

    // Text
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highScoreText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    // UI
    public GameObject MainMenuButton;
    public GameObject PlayAgainButton;
    AudioManager audioManager;

    static private float highScore;
    private float highScore_aux;
    [SerializeField] float timer;
    private bool gameOver;
    private bool gameWin;
    public float FallingThreshold = -15f;
    [SerializeField] private GameObject wall;
    public Vector3 targetPosition = new Vector3(0, -5, 11);
    public float duration = 4.0f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        highScore_aux = 0;
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        highScoreText.enabled = false;
        MainMenuButton.SetActive(false);
        PlayAgainButton.SetActive(false);
        gameOver = false;
        gameWin = false;
    }

    private void OnMove(InputValue movementValue)
    {
        if (!gameOver)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        audioManager.PlaySFX(audioManager.collectItemSFX);
        if (count >= 12 & !gameOver & !gameWin)
        {
            StartCoroutine(MoveWallSmoothly(targetPosition, duration));
        }

        if (count >= 20 & !gameOver & !gameWin)
        {
            int minutes;
            int seconds;
            timeText.color = Color.green;
            if (Mathf.FloorToInt(highScore / 60) < 1 & Mathf.FloorToInt(highScore % 60) < 1)
            {
                highScore = highScore_aux;
                minutes = Mathf.FloorToInt(highScore / 60);
                seconds = Mathf.FloorToInt(highScore % 60);
                highScoreText.text = "New High Score: " + $"{minutes}:{seconds:D2}";
                highScoreText.color = Color.green;
                highScoreText.enabled = true;
            }
            else if (highScore_aux < highScore)
            {
                highScore = highScore_aux;
                minutes = Mathf.FloorToInt(highScore / 60);
                seconds = Mathf.FloorToInt(highScore % 60);
                highScoreText.text = "New High Score: " + $"{minutes}:{seconds:D2}";
                highScoreText.color = Color.green;
                highScoreText.enabled = true;
            } else
            {
                minutes = Mathf.FloorToInt(highScore / 60);
                seconds = Mathf.FloorToInt(highScore % 60);
                highScoreText.text = "High Score: " + $"{minutes}:{seconds:D2}";
                highScoreText.color = Color.green;
            }
            highScore_aux = 0;
            gameWin = true;

            highScoreText.enabled = true;
            winTextObject.SetActive(true);
            MainMenuButton.SetActive(true);
            PlayAgainButton.SetActive(true);
        }
    }

    private IEnumerator MoveWallSmoothly(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = wall.transform.position;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            wall.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wall.transform.position = targetPosition; // Ensure the object ends exactly at the target position
    }


    private void FixedUpdate()
    {
        if (!gameOver)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag("Asteroid") & !gameWin)
        {
            timeText.color = Color.red;
            GameOver();
        }
    }

    private void Update()
    {
        if (!gameOver & !gameWin)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                highScore_aux += Time.deltaTime;
            }
            else if (timer < 0)
            {
                timer = 0;
                timeText.color = Color.red;
                GameOver();
            }
            
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timeText.text = "Time: " + $"{minutes}:{seconds:D2}";
        }

        if (rb.velocity.y < FallingThreshold)
        {
            timeText.color = Color.red;
            Destroy(gameObject);
            if (!gameWin)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        loseTextObject.SetActive(true);
        MainMenuButton.SetActive(true);
        PlayAgainButton.SetActive(true);

        highScore_aux = 0;
        int minutes = Mathf.FloorToInt(highScore / 60);
        int seconds = Mathf.FloorToInt(highScore % 60);
        highScoreText.text = "High Score: " + $"{minutes}:{seconds:D2}";
        highScoreText.color = Color.red;
        highScoreText.enabled = true;

        gameOver = true;
    }
}
