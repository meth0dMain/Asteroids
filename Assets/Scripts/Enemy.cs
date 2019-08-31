using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
	public int enemy_heath;
	public int score_Value;
	[Space]
	public GameObject obj_Bullet;
	public float shot_Time_Min, shot_Time_Max;
	public int shot_Chance;

	[Header("BOSS")]
	public bool is_Boss;
	public GameObject obj_Bullet_Boss;
	public float time_Bullet_Boss_Spawn;
	public float _timer_Shot_Boss;
	public int shot_Chance_Boss;

	void Start()
	{
		// If the current object is not the boss, make only one shot
		if (!is_Boss)
		{
			// Call the OpenFire in the time interval between shot_Time_Min and shot_Time_Max
			Invoke("OpenFire", Random.Range(shot_Time_Min, shot_Time_Max));
		}
	}

	void Update()
	{
		// If the current object is the boss
		if (is_Boss)
		{
			if (Time.time > _timer_Shot_Boss)
			{
				_timer_Shot_Boss = Time.time + time_Bullet_Boss_Spawn;
				OpenFire();
				OpenFireBoss();
			}
		}
	}
	void OpenFireBoss()
	{
		// If random value less than shot chance, boss makes extra shot
		if (Random.value < (float)shot_Chance_Boss / 100)
		{
			for (int zZz = -40; zZz < 40; zZz += 10)
			{
				// Create an instance of the prefab obj_Bullet_Boss in the boss position and
				// rotates zZz degrees around the z axis
				Instantiate(obj_Bullet_Boss, transform.position, Quaternion.Euler(0, 0, zZz));
			}
		}
	}
	void OpenFire()
	{
		// If the random value less than shot chance, making the shot
		if (Random.value < (float) shot_Chance / 100)
		{
			// Create an instance of the prefab obj_Bullet in the enemy position and without rotation.
			Instantiate(obj_Bullet, transform.position, Quaternion.identity);
		}
	}

	public void GetDamage(int damage)
	{
		enemy_heath -= damage;
		if (enemy_heath <= 0)
			Destruction();
	}
	void Destruction()
	{
		LevelController.instance.ScoreInGame(score_Value);
		Destroy(gameObject);
	}

	// If enemy collides with the player, player receives damage.
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("Player"))
		{
			GetDamage(1);
			Player.instance.GetDamage(1);
		}
		if (coll.CompareTag("Shield"))
		{
			GetDamage(1);
			Player.instance.GetDamageShield(1);
		}
	}
}
