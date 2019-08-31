using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[System.Serializable]
public class ShootingSettings
{
	[Range(0, 100)] public int shot_Chance;
	public float shot_Time_Min, shot_Time_Max;
}
public class Wave : MonoBehaviour
{
	public ShootingSettings shooting_Settings;
	[Space]
	public GameObject obj_Enemy;
	public int count_in_Wave;
	public float speed_Enemy;
	public float time_Spawn;
	public Transform[] path_Points;
	public bool is_return;

	// Test Wave
	// An infinite wave appears every 5 seconds to debug the wave
	[Header("Test wave!")] public bool is_Test_Wave;

	FollowThePath follow_Component;
	Enemy enemy_Component_Script;

	void Start()
	{
		StartCoroutine(CreateEnemyWave());
	}

	IEnumerator CreateEnemyWave()
	{
		for (int i = 0; i < count_in_Wave; i++)
		{
			// Create an instance of the prefab obj_Enemy in the obj_Enemy position and without rotation.
			GameObject new_enemy = Instantiate(obj_Enemy, obj_Enemy.transform.position,
				Quaternion.identity);
			// Try and find an FollowThePath script on the GameObject new_enemy
			follow_Component = new_enemy.GetComponent<FollowThePath>();
			// Specify the path that will move the new_enemy
			follow_Component.path_Points = path_Points;
			// Specify the speed with which the new enemy will move
			follow_Component.speed_Enemy = speed_Enemy;
			// Destroy the surviving enemies at the end of the path or send them to the beginning of the path.
			follow_Component.is_return = is_return;

			// Try and find an Enemy script on the gameObject new_enemy.
			enemy_Component_Script = new_enemy.GetComponent<Enemy>();
			// Specify shot chance a new enemy.
			enemy_Component_Script.shot_Chance = shooting_Settings.shot_Chance;
			// Specify time interval within which the shot occurs.
			enemy_Component_Script.shot_Time_Min = shooting_Settings.shot_Time_Min;
			enemy_Component_Script.shot_Time_Max = shooting_Settings.shot_Time_Max;
			new_enemy.SetActive(true);
			yield return new WaitForSeconds(time_Spawn);
		}
		if (is_Test_Wave)
		{
			// Infinitely generate the current wave every 5 seconds
			yield return new WaitForSeconds(5f);
			StartCoroutine(CreateEnemyWave());
		}
		// If is_return = false - destroy the enemy at the end of the path
		if (!is_return)
			Destroy(gameObject);
	}
	void OnDrawGizmos()
	{
		NewPositionByPath(path_Points);
	}

	void NewPositionByPath(Transform[] path)
	{
		var path_Position = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
			path_Position[i] = path[i].position;
		path_Position = Smoothing(path_Position);
		path_Position = Smoothing(path_Position);
		path_Position = Smoothing(path_Position);
		for (int i = 0; i < path_Position.Length - 1; i++)
			Gizmos.DrawLine(path_Position[i], path_Position[i + 1]);
	}

	Vector3[] Smoothing(Vector3[] path_Positions)
	{
		var new_Path_Position = new Vector3[(path_Positions.Length - 2) * 2 + 2];
		new_Path_Position[0] = path_Positions[0];
		new_Path_Position[new_Path_Position.Length - 1] = path_Positions[path_Positions.Length - 1];

		int j = 1;
		for (int i = 0; i < path_Positions.Length - 2; i++)
		{
			new_Path_Position[j] = path_Positions[i] + (path_Positions[i + 1] - path_Positions[i]) * 0.75f;
			new_Path_Position[j + 1] = path_Positions[i + 1] + (path_Positions[i + 2] - path_Positions[i + 1]) * 0.25f;
			j += 2;
		}
		return new_Path_Position;
	}
}
