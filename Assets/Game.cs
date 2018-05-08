using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum GameState { Menu, Game, GameOver }
    public GameState State;
    public GameObject Player1;
    public GameObject Player2;
    public AIAgent AI;
    public Text StartText;
    public Text TitleText;
    public Text GameOverText;
    public Text Player1ScoreText;
    public Text Player2ScoreText;
    public GameObject[] StartFruit;
    public Brain Brain;

    private GameObject[] balls;
    private int player1Score;
    private int player2Score;
    private int numberOfBalls;
    private bool multiPlayerMode;

    public void Start()
    {
        balls = new GameObject[24];
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = Resources.Load<GameObject>(i.ToString());
        }

        Player1.SetActive(false);
        Player2.SetActive(false);

        if (Brain.brainType == BrainType.External)
        {
            Start2(true);
        }
    }

    public void Update()
    {
        if (State == GameState.Menu || State == GameState.GameOver)
        {
            if (Input.GetButton("Fire1"))
            {
                Start1();
            }
            else if (Input.GetButton("Fire2"))
            {
                Start2(false);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Start2(true);
            }
        }
    }

    private void Reset()
    {
        Player1.SetActive(false);
        Player2.SetActive(false);
        State = GameState.Menu;
        player1Score = 0;
        player2Score = 0;
        numberOfBalls = 0;
        StartText.enabled = false;
        GameOverText.enabled = false;
        foreach(var startFruit in StartFruit)
        {
            startFruit.SetActive(false);
        }

        if (Brain.brainType == BrainType.External)
        {
            AI.Done();
        }
    }

    public void Start1()
    {
        Reset();
        Player1ScoreText.enabled = true;
        Player2ScoreText.enabled = false;
        TitleText.enabled = false;
        Player1.SetActive(true);
        AI.enabled = false;
        multiPlayerMode = false;
        State = GameState.Game;

        Drop("");
    }

    public void Start2(bool AIEnabled)
    {
        Reset();
        Player1ScoreText.enabled = true;
        Player2ScoreText.enabled = true;
        TitleText.enabled = false;
        if (Brain.brainType != BrainType.External) Player1.SetActive(true);
        Player2.SetActive(true);
        AI.enabled = AIEnabled;
        multiPlayerMode = true;
        State = GameState.Game;

        Drop("");
    }

    public void Drop(string name)
    {
        int randomItem = (int)UnityEngine.Random.Range(0, balls.Length);
        GameObject.Instantiate(balls[randomItem], new Vector3(UnityEngine.Random.Range(-20, 20), 30, 0), Quaternion.identity);

        AudioSource hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        if (name != "")
        {
            hitSound.Play();
        }

        numberOfBalls++;

        if (name == "Player1")
        {
            if (multiPlayerMode)
            {
                player1Score++;
            }
            else if (numberOfBalls - 1 > player1Score)
            {
                player1Score = numberOfBalls - 1;
            }
        }
        else if (name == "Player2")
        {
            player2Score++;
            if (Brain.brainType == BrainType.External)
            {
                AI.SetReward(10f);
                AI.Done();
            }
        }

        UpdateScore();

        if (player1Score == 100)
        {
            GameOver("Game Over player 1 wins");
        }
        if (Brain.brainType != BrainType.External && player2Score == 100)
        {
            GameOver("Game Over player 2 wins");
        }
    }

    public void Miss()
    {
        numberOfBalls--;

        if (Brain.brainType == BrainType.External)
        {
            Player2.GetComponent<Agent>().SetReward(-0.0001f);
            Player2.GetComponent<Agent>().Done();
        }

        if (numberOfBalls <= 0)
        {
            if (Brain.brainType == BrainType.External)
            {
                Start2(true);
                return;
            }

            if (multiPlayerMode)
            {
                if (player1Score == player2Score)
                {
                    GameOver("Game Over - Draw");
                }
                else
                {
                    string winner = (player1Score > player2Score) ? "1" : "2";
                    GameOver("Game Over player " + winner + " wins");
                }
            }
            else
            {
                GameOver("Game Over you scored " + player1Score);
            }
        }
    }

    private void GameOver(string text)
    {
        GameOverText.text = text;
        GameOverText.enabled = true;
        State = GameState.GameOver;
        Player1.SetActive(false);
        Player2.SetActive(false);
        StartText.enabled = true;
    }


    private void UpdateScore()
    {
        Player1ScoreText.text = "Score: " + (player1Score);

        if (multiPlayerMode)
        {
            Player2ScoreText.text = "Score: " + (player2Score);
        }
    }

}
