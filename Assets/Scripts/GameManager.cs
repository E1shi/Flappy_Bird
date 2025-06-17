using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public Text highScoreText;
    public GameObject restart;
    public GameObject startGame;


    private int score;
    private int highScore = 0;


    public void Awake()
    {
        Application.targetFrameRate = 60;
        Pause();
        startGame.SetActive(true);
        restart.SetActive(false);
        scoreText.gameObject.SetActive(false);

        player.transform.position = Vector3.zero;

        // Load high score dari PlayerPrefs saat game mulai
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    void Update()
    {
        // Jika game sedang pause (belum mulai / game over), cek input
        if (Time.timeScale == 0f && IsStartInput())
        {
            startGame.SetActive(false);
            restart.SetActive(true);
            scoreText.gameObject.SetActive(true);
            Play();
        }
    }

    // Cek semua input mulai game (Tap, Klik Mouse, Spasi)
    private bool IsStartInput()
    {
        // Keyboard (Spasi)
        if (Input.GetKeyDown(KeyCode.Space))
            return true;

        // Mouse / Touch
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // Jika tidak menekan UI (opsional, hapus jika ingin tap di UI juga memulai game)
            if (!EventSystem.current.IsPointerOverGameObject())
                return true;
        }
        return false;
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        restart.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        AudioSource scoreAudio = GetComponent<AudioSource>();
        scoreAudio.Play();
    }

    public void GameOver()
    {
        restart.SetActive(true);

        // Cek jika score sekarang > high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Simpan ke PlayerPrefs
        }

        // Update teks high score
        highScoreText.text = "High Score\n" + highScore;
        highScoreText.gameObject.SetActive(true); // Tampilkan high score

        Pause();
        PlayerPrefs.Save();
    }
    
    
}
