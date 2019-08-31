using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region DataBase classes

public class DataBase
{
	public static DataBase instance = new DataBase();
	// Array preserving all game game settings.
	public int[][] playerShipsInfo =
	{
		new[] { 1, 000, 1, 12, 0 },//Selected ship 1 - Cost Ship 1 - HP - Speed - Shield
		new[] { 0, 550, 2, 8, 0 }, //Selected ship 2 - Cost Ship 2 - HP - Speed - Shield
		new[] { 0, 950, 3, 6, 0 }  //Selected ship 3 - Cost Ship 3 - HP - Speed - Shield
	};
	// Upgrade price:
	public int costHP = 250;
	public int costSpeed = 500;
	public int costShield = 2500;
	// Our scores you can spend
	public int Score = 99999;
	//Score per game, added to the Score when you exit or win a game
	public int Score_Game = 0;

	public void GameLoadScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void SaveGame()
	{
		Score += Score_Game;
		for (int i = 0; i < playerShipsInfo.Length; i++)
		{
			for (int j = 0; j < playerShipsInfo[i].Length; j++)
				PlayerPrefs.SetInt("InfoSave" + i + j, playerShipsInfo[i][j]);
		}
		PlayerPrefs.SetInt("InfoSaveScore", Score);
	}

	public void LoadGameSave()
	{
		for (int i = 0; i < playerShipsInfo.Length; i++)
		{
			for (int j = 0; j < playerShipsInfo[i].Length; j++)
				playerShipsInfo[i][j] = PlayerPrefs.GetInt("InfoSave" + i + j);
		}
		Score = PlayerPrefs.GetInt("InfoSaveScore");
	}
}

#endregion 
public class MainMenu : MonoBehaviour
{
	public GameObject[] game_Panels;

	// Show player score.
    public Text Score;

    [Header("Shop panel")]
    // Which ship you can choose or buy
    public GameObject[] shop_Ships;
    //Text under the ship, showing the status of the ship(unlock or cost)
    public Text[] shop_Ship_Text;
    // ship purchase button(appears if the ship has not been purchased yet)
    public GameObject btn_Shop_Buy;
    // Ship purchase button (add the cost of the selected ship)
    public Text shop_Btn_Buy_Cost_Text;

    [Header("Upgrade panel")]
    // Ships images.
    public Sprite[] upgrade_Sprite_Ships;
    // Show the ship image that will upgrade.
    public GameObject upgrade_Sprite_Ship;
    // Sliders showing progress of improvement.
    public Slider[] upgrade_Sliders;
    // Show the cost of upgrades in the text of the buttons.
    public Text[] upgrade_Show_Cost;

    // Saves the number of the selected available ship(unlocked).
    int _index;
    // Saves the number of the selected ship that is not yet available to the player(not purchased).
    int _indexBuy;

    #region Buttons Save Load Debug Exit Choise...
    // btn SaveGame
    public void BtnSave()
    {
        //Call method save game
        DataBase.instance.SaveGame();
    }
    // btn LoadGame
    public void BtnLoadGameSave()
    {
        //Call method load game
        DataBase.instance.LoadGameSave();
    }

    //btn choice level game
    public void BtnChoiceLevelGame(string name)
    {
        //Call method loading scene by name
        DataBase.instance.GameLoadScene(name);
    }
    //btn Exit
    public void BtnExitGame()
    {
        //Save game
        BtnSave();
        //Exit
        Application.Quit();
    }
    #endregion

    void Start()
    {
	    if (PlayerPrefs.HasKey("InfoSaveScore"))
	        Debug.Log("found save game!!!");
        UpdateScore();
        ShopShipHighlighting();
    }

    public void UpdateScore()
    {
        Score.text = DataBase.instance.Score.ToString();
    }
    #region Shop...
    // Method of highlighting open and closed ships in different colors
    public void ShopShipHighlighting()
    {
        // In the cycle, check all available ships in the game
        for (int i = 0; i < DataBase.instance.playerShipsInfo.Length; i++)
        {
            // If the ship has been selected, make it the standard color, text under the ship is green
            if (DataBase.instance.playerShipsInfo[i][0] == 1)
            {
                shop_Ships[i].GetComponent<Image>().color = Color.white;
                shop_Ship_Text[i].color = Color.green;
                //Save selected ship.
                _index = i;
            }
            // The remaining ships will be made gray with red text below them
            else
            {
                shop_Ships[i].GetComponent<Image>().color = Color.gray;
                shop_Ship_Text[i].color = Color.red;
            }
            //Ship that does not have a price will have the text Open
            if (DataBase.instance.playerShipsInfo[i][1] == 0)
                shop_Ship_Text[i].text = "Open";
            else
                shop_Ship_Text[i].text = "Cost: " + DataBase.instance.playerShipsInfo[i][1];
        }
    }

    public void ShopCheckPlayerShip(int num)
    {
        // If the ship has no price, you can choose it
        if (DataBase.instance.playerShipsInfo[num][1] == 0)
        {
            // Delete information about previously selected ships
            for (int i = 0; i < DataBase.instance.playerShipsInfo.Length; i++)
            {
                DataBase.instance.playerShipsInfo[i][0] = 0;
            }
            // Save information about the selected ship...
            DataBase.instance.playerShipsInfo[num][0] = 1;
            _index = num;
            // Hiding the buy ship button(it's already open)
            btn_Shop_Buy.SetActive(false);
        }
        // If the ship has a price and a score is enough to buy it
        if (DataBase.instance.playerShipsInfo[num][1] != 0 &&
            DataBase.instance.playerShipsInfo[num][1] <= DataBase.instance.Score)
        {
            // Show the buy ship button.
            btn_Shop_Buy.SetActive(true);
            // Show cost selected ship.
            shop_Btn_Buy_Cost_Text.text = "Buy " + DataBase.instance.playerShipsInfo[num][1];
            // Remember the number of the selected ship that is not yet purchased
            _indexBuy = num;
        }
        // If the ship has a price and the score is not enough to buy it
        if (DataBase.instance.playerShipsInfo[num][1] != 0 &&
            DataBase.instance.playerShipsInfo[num][1] > DataBase.instance.Score)
        {
            // Hiding the buy ship button
            btn_Shop_Buy.SetActive(false);
        }
        ShopShipHighlighting();
    }

    public void Btn_ShopBuyShip()
    {
        // Now this ship can be chosen.
        _index = _indexBuy;
        // Update the score
        DataBase.instance.Score = DataBase.instance.Score - DataBase.instance.playerShipsInfo[_index][1];
        // Remove the price of the ship (if there is no price, then the ship is unlocked)
        DataBase.instance.playerShipsInfo[_index][1] = 0;
        UpdateScore();
        ShopCheckPlayerShip(_index);
        // Delete information about previously selected ships
        for (int i = 0; i < DataBase.instance.playerShipsInfo.Length; i++)
	        DataBase.instance.playerShipsInfo[i][0] = 0;
        // Save information about the selected ship
        DataBase.instance.playerShipsInfo[_index][0] = 1;
        // Call the method highlighting ship in the store (make a normal ship color)
        ShopShipHighlighting();
    }
    #endregion
    #region Upgrade

    public void UpgradesGetInformation()
    {
        // Show the ship that will upgrade
        upgrade_Sprite_Ship.GetComponent<Image>().sprite = upgrade_Sprite_Ships[_index];
        // Show update cost HP
        upgrade_Show_Cost[0].text = "Cost: " + DataBase.instance.costHP;
        // Show update cost Speed
        upgrade_Show_Cost[1].text = "Cost: " + DataBase.instance.costSpeed;
        // Show update cost Shield
        upgrade_Show_Cost[2].text = "Cost: " + DataBase.instance.costShield;
        // Show update progress HP (max 15 updates)
        upgrade_Sliders[0].value = (float)DataBase.instance.playerShipsInfo[_index][2] / 15;
        // Show update progress Speed (max 20 updates)
        upgrade_Sliders[1].value = (float)DataBase.instance.playerShipsInfo[_index][3] / 20;
        // Show update progress Shield (max 6 updates)
        upgrade_Sliders[2].value = (float)DataBase.instance.playerShipsInfo[_index][4] / 6;
    }

    public void Btn_Upgrade(int index)
    {
        //Upgrade HP
        //Selected 1 index; score is enough for HP upgrade; upgrade is not the max value
        if (index == 0 && DataBase.instance.Score > DataBase.instance.costHP && DataBase.instance.playerShipsInfo[_index][2] < 15)
        {
            upgrade_Show_Cost[0].text = "Cost: " + DataBase.instance.costHP;
            DataBase.instance.Score -= DataBase.instance.costHP;
            DataBase.instance.playerShipsInfo[_index][2] += 1;
            upgrade_Sliders[0].value += (float)1 / 15;
        }
        //Upgrade Speed
        //Selected 2 index; score is enough for Speed upgrade; upgrade is not the max value
        if (index == 1 && DataBase.instance.Score > DataBase.instance.costSpeed && DataBase.instance.playerShipsInfo[_index][3] < 20)
        {
            upgrade_Show_Cost[1].text = "Cost: " + DataBase.instance.costSpeed;
            DataBase.instance.Score -= DataBase.instance.costSpeed;
            DataBase.instance.playerShipsInfo[_index][3] += 1;
            upgrade_Sliders[1].value += (float)1 / 20;
        }
        //Upgrade Shield
        //Selected 3 index; score is enough for Shield upgrade; upgrade is not the max value
        if (index == 2 && DataBase.instance.Score > DataBase.instance.costShield && DataBase.instance.playerShipsInfo[_index][4] < 6)
        {
            upgrade_Show_Cost[2].text = "Cost: " + DataBase.instance.costShield;
            DataBase.instance.Score -= DataBase.instance.costShield;
            upgrade_Sliders[2].value += (float)1 / 6;
            DataBase.instance.playerShipsInfo[_index][4] += 1;
        }
        UpdateScore();
    }
    #endregion
    public void Show_Change_Panel(int index)
    {
	    // Check which ship is selected. (if not checked, the player can choose a ship that has not bought yet and use it)
	    ShopShipHighlighting();
	    game_Panels[index].SetActive(true);
    }

    public void Hide_Change_Panel(int index)
    {
	    // Save the game(the player could buy a ship or upgrade it, as soon as he closes the panel, progress is saved)
	    BtnSave();
	    game_Panels[index].SetActive(false);
    }
}
