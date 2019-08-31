using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage;
	public bool is_Enemy_Bullet;

	void Destruction()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (is_Enemy_Bullet && coll.CompareTag("Player"))
		{
			Player.instance.GetDamage(damage);
			Destruction();
		}
		else if (!is_Enemy_Bullet && coll.CompareTag("Enemy"))
		{
			coll.GetComponent<Enemy>().GetDamage(damage);
			Destruction();
		}
		else if (is_Enemy_Bullet && coll.CompareTag("Shield"))
		{
			Player.instance.GetDamageShield(damage);
			Destruction();
		}
	}
}
