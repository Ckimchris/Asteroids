using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public int score { get; private set; }
    public int lives { get; private set; }

    [SerializeField] private PlayerController player;
    [SerializeField] private AsteroidManager asteroidManager;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private TextMeshProUGUI gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI instructionsDisplay;
    [SerializeField] private Button QuitButton;

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource breakSFX;
    [SerializeField] private AudioSource respawnSFX;

    private float revengeValue = 0;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PreGame();
    }

    private void Update()
    {
        if(!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NewGame();
            }
        }
    }

    private void PreGame()
    {
        isPlaying = false;
        gameOverUI.gameObject.SetActive(false);
        gameOverScoreUI.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        instructionsDisplay.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
    }

    private void NewGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        UFO[] ufos = FindObjectsOfType<UFO>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        for (int i = 0; i < ufos.Length; i++)
        {
            Destroy(ufos[i].gameObject);
        }

        isPlaying = true;
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        instructionsDisplay.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
        gameOverScoreUI.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);

        SetScore(0);
        SetLives(3);

        Respawn();

        if (!bgm.isPlaying)
        {
            bgm.Play();
        }

        asteroidManager.SpawnRoutine();

    }

    private void GameOver()
    {
        asteroidManager.Cancel();

        isPlaying = false;

        gameOverUI.gameObject.SetActive(true);
        gameOverScoreUI .gameObject.SetActive(true);
        gameOverScoreUI.text = "Score: " + score.ToString();
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        instructionsDisplay.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);

    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "Lives: " + lives.ToString();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        respawnSFX.Play();
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        breakSFX.Play();
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();

        if (asteroid.size < 0.7f)
        {
            SetScore(score + 100); // small asteroid
            SetRevengeCounter(100);
        }
        else if (asteroid.size < 1.4f)
        {
            SetScore(score + 50); // medium asteroid
            SetRevengeCounter(50);

        }
        else
        {
            SetScore(score + 25); // large asteroid
            SetRevengeCounter(25);

        }
    }

    public void SetRevengeCounter(int score)
    {
        this.revengeValue = this.revengeValue + score;

        if (revengeValue >= 1000)
        {
            asteroidManager.SpawnUFO();
            revengeValue = 0;
        }

    }

    public void OnUFODestroyed(UFO ufo)
    {
        breakSFX.Play();
        explosionEffect.transform.position = ufo.transform.position;
        explosionEffect.Play();

        SetScore(score + 200);

    }

    public void OnPlayerDeath(PlayerController player)
    {
        player.gameObject.SetActive(false);
        deathSFX.Play();
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        SetLives(lives - 1);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), player.respawnDelay);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
