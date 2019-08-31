using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    public int player_Health = 1;
    public GameObject obj_Shield;
    public int shield_Health = 1;
    Slider _slider_hp_Player;
    Slider _slider_hp_Shield;

    void Awake()
    {
        // Setting up the references.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        // Look for an object with tag hp_Player and take the Slider component from it
        _slider_hp_Player = GameObject.FindGameObjectWithTag("sl_HP").GetComponent<Slider>();
        // Look for an object with tag hp_Shield and take the Slider component from it
        _slider_hp_Shield = GameObject.FindGameObjectWithTag("sl_Shield").GetComponent<Slider>();
    }

    void Start()
    {
        _slider_hp_Player.value = (float)player_Health / 15; //slider has a range from 0 to 1
        if (shield_Health != 0)
        {
            //Show shield.
            obj_Shield.SetActive(true);
            _slider_hp_Shield.value = (float)shield_Health / 6; //slider has a range from 0 to 1
        }
        // If the shield has no life
        else
        {
            obj_Shield.SetActive(false);
            _slider_hp_Shield.value = 0;
        }
    }
    public void GetDamageShield(int damage)
    {
        shield_Health -= damage;
        _slider_hp_Shield.value = (float)shield_Health / 10;
        if (shield_Health <= 0)
            obj_Shield.SetActive(false);
    }

    public void GetDamage(int damage)
    {
        player_Health -= damage;
        _slider_hp_Player.value = (float)player_Health / 10;
        if (player_Health <= 0)
            Destruction();
    }
    void Destruction()
    {
        Destroy(gameObject);
    }
}
