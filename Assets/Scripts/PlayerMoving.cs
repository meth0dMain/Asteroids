using UnityEngine;
[System.Serializable]
public class Borders
{
	// Offset from the left border.
	public float minX_Offset = 1.1f;
	// Offset from the right border.
	public float maxX_Offset = -1.1f;
	// Offset from the bottom border.
	public float minY_Offset = 1.1f;
	// Offset from the top border.
	public float maxY_Offset = -1.1f;
	[HideInInspector]
	public float minX, maxX, minY, maxY;
}

public class PlayerMoving : MonoBehaviour
{
	public static PlayerMoving instance;
	public Borders borders;
	public int speed_Player = 10;
	Camera _camera;
	Vector2 _mouse_Position;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		_camera = Camera.main;
	}

	void Start()
	{
		ResizeBorders();
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			// Get 2D coordinates (xy) click on screen.
			_mouse_Position = _camera.ScreenToWorldPoint(Input.mousePosition);
			_mouse_Position.y += 1.5f;
			// Move our player to the 2D coordinates click.
			transform.position =
				Vector2.MoveTowards(transform.position, _mouse_Position, speed_Player * Time.deltaTime);
		}
		// If a player is trying to go abroad, do not let him.
		transform.position = new Vector2(Mathf.Clamp(transform.position.x, borders.minX, borders.maxX),
			Mathf.Clamp(transform.position.y, borders.minY, borders.maxY));
	}
	
	void ResizeBorders()
	{
		// Borders left.
		borders.minX = _camera.ViewportToWorldPoint(Vector2.zero).x + borders.minX_Offset;
		// Borders bottom.
		borders.minY= _camera.ViewportToWorldPoint(Vector2.zero).y + borders.minY_Offset;
		// Borders right.
		borders.maxX = _camera.ViewportToWorldPoint(Vector2.right).x + borders.maxX_Offset;
		// Borders top.
		borders.maxY = _camera.ViewportToWorldPoint(Vector2.up).y + borders.maxY_Offset;
	}
}