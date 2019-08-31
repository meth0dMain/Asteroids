using UnityEngine;

public class Bonus : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		// If the entering collider is the player
		if (collision != null && collision.CompareTag("Player"))
		{
			if (PlayerShooting.instance.cur_Power_Level_Guns < PlayerShooting.instance.max_Power_Level_Guns)
				PlayerShooting.instance.cur_Power_Level_Guns++;
			Destroy(gameObject);
		}
	}
}
