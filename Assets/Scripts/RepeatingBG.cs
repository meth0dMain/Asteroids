using UnityEngine;

public class RepeatingBG : MonoBehaviour
{
	public float vertical_Size;
	Vector2 _offset_Up;

	void Update()
	{
		// If the sprite is completely gone
		if (transform.position.y < -vertical_Size)
			RepeatBackground();
	}

	// Moves two sprites one after the other, creating endless background.
	void RepeatBackground()
	{
		// Set the offset twice the height of the sprite.
		_offset_Up = new Vector2(0, vertical_Size * 2f); // 2f -> 2 sprites total
		// Set a new position for the sprite.
		transform.position = (Vector2) transform.position + _offset_Up;
	}
}
