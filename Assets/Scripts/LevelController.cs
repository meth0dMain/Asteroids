using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class EnemyWaves
{
    // Time when the wave will appear on the stage
    public float timeToStart;
    // The Enemy Wave prefab to be spawned.
    public GameObject wave;
    // This wave after which the game will end.
    public bool is_Last_Wave;
}

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    //Array of player ships
    public GameObject[] playerShip;
    // Reference to the EnemyWaves.
    public EnemyWaves[] enemyWaves;
    // Finish the game or not
    bool is_Final;

    //Pause menu.
    public GameObject panel;
    //Show and hide pause menu.
    bool _isPause;
    // Buttons pause menu (Exit, Return, Restart)
    public GameObject[] btnPause;
    // Show player score in game.
    public Text text_Score;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Time.timeScale = 1;
        // Create selected player ship...
        for (int i = 0; i < DataBase.instance.playerShipsInfo.Length; i++)
        {
            // If the value in the array = 1, this ship must be created, with all upgrades
            if (DataBase.instance.playerShipsInfo[i][0] == 1)
                LoadPlayer(i);
        }
        // Create all enemy waves
        for (int i = 0; i < enemyWaves.Length; i++)
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave, enemyWaves[i].is_Last_Wave));
    }
    void Update()
    {
        // If is_Final = true, there are no objects with the Enemy tag left and the pause button is not pressed...
        if (is_Final && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !_isPause)
        {
            // You win game!
            Debug.Log("Win");
            GamePause();
            btnPause[1].SetActive(false);
        }
        if (Player.instance.player_Health <= 0 && !_isPause)
        {
            // You lose game!
            Debug.Log("Lose");
            GamePause();
        }
    }
    public void ScoreInGame(int score)
    {
        // Score per game that can be saved
        DataBase.instance.Score_Game += score;
        // Show the player points
        text_Score.text = "Score: " + DataBase.instance.Score_Game;
    }

    //Method loads the player's ship with its upgrades
    public void LoadPlayer(int ship)
    {
        Instantiate(playerShip[ship]);
        Player.instance.player_Health = DataBase.instance.playerShipsInfo[ship][2];
        PlayerMoving.instance.speed_Player = DataBase.instance.playerShipsInfo[ship][3];
        Player.instance.shield_Health = DataBase.instance.playerShipsInfo[ship][4];
    }

    public void GamePause()
    {
        // Call pause panel
        if (!_isPause)
        {
            _isPause = true;
            Time.timeScale = 0;
            panel.SetActive(true);
            //If the player is not dead, the return button instead of the restart button
            if (Player.instance.player_Health >= 1)
            {
                btnPause[0].SetActive(false);
                btnPause[1].SetActive(true);
            }
            //If the player is dead, the restart button instead of the return button
            else
            {
                btnPause[0].SetActive(true);
                btnPause[1].SetActive(false);
            }
        }
        //Hide pause panel
        else
        {
            _isPause = false;
            // time in the game is normal
            Time.timeScale = 1;
            panel.SetActive(false);
        }
    }

    public void BtnRestartGame()
    {
        // Reset Score_game
        DataBase.instance.Score_Game = 0;
        // time in the game is normal
        Time.timeScale = 1;
        //Loading current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BtnExitGame()
    {
        DataBase.instance.SaveGame();
        DataBase.instance.GameLoadScene("Menu");
    }

    IEnumerator CreateEnemyWave(float delay, GameObject Wave, bool Final)
    {
        // If creation wave time ! = 0 and player is alive -> create wave
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance.player_Health >= 1)
            Instantiate(Wave);
        if (Final)
            is_Final = true;
    }
}
