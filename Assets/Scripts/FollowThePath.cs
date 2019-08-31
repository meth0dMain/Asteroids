using UnityEngine;

public class FollowThePath : MonoBehaviour
{
	// An array of waypoints along which the enemy moves in a wave.
	[HideInInspector] public Transform[] path_Points;
	// The speed at which the enemy moves.
	[HideInInspector] public float speed_Enemy;
	// Destroy the surviving enemies at the end of the path or send them to the beginning of the path.
	[HideInInspector] public bool is_return;
	// Store vector3 of all waypoints Debug
	[HideInInspector] public Vector3[] _new_Position;
	int cur_Pos;

	void Start()
	{
		_new_Position = NewPositionByPath(path_Points);
		// Send the current opponent to the starting point.
		transform.position = _new_Position[0];
	}

	void Update()
	{
		// Move the current enemy to the points of the path at a given speed.
		transform.position = Vector3.MoveTowards(transform.position, _new_Position[cur_Pos], speed_Enemy * Time.deltaTime);
		// If the current enemy has reached the point of the path
		if (Vector3.Distance(transform.position, _new_Position[cur_Pos]) < 0.2f)
		{
			//Set the next waypoint.
			cur_Pos++;
			// If the current enemy reaches the last point and is_return = true, send the enemy to the starting waypoint
			if (is_return && Vector3.Distance(transform.position, _new_Position[_new_Position.Length - 1]) < 0.3f)
				cur_Pos = 0;
		}
		// if the current enemy reaches the last point and is_return = false, destroy the enemy
		if (Vector3.Distance(transform.position, _new_Position[_new_Position.Length - 1]) < 0.2f && !is_return)
			Destroy(gameObject);
	}

	Vector3[] NewPositionByPath(Transform[] pathPos)
	{
		var pathPositions = new Vector3[pathPos.Length];
		for (int i = 0; i < path_Points.Length; i++)
		{
			pathPositions[i] = pathPos[i].position;
		}
		pathPositions = Smoothing(pathPositions);
		pathPositions = Smoothing(pathPositions);
		pathPositions = Smoothing(pathPositions);
		return pathPositions;
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
