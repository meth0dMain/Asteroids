using UnityEngine;

public class DestroyAll : MonoBehaviour
{
	// Reference to the BoxCollider2D component.
	BoxCollider2D _boundare_Collider;
	// Vector for storing sizes Camera
	Vector2 _viewport_Size;

	void Awake()
	{
		_boundare_Collider = GetComponent<BoxCollider2D>();
	}

	void Start()
	{
		ResizeCollider2D();
	}
	// The size of the collider is adjusted to the size of the camera.
	void ResizeCollider2D()
	{
		// Get the size of the upped right corner and multiply by 2.
		if (Camera.main != null)
			_viewport_Size = Camera.main.ViewportToWorldPoint(new Vector2(1, 1) * 2);
		// Increase the width by 1.5
		_viewport_Size.x *= 1.5f;
		// Increase the height by 1.5
		_viewport_Size.y *= 1.5f;
		// Change the size.
		_boundare_Collider.size = _viewport_Size;
	}

	public void OnTriggerExit2D(Collider2D coll)
	{
		// Destroy the collider if correct tag.
		if (coll.CompareTag("Planet"))
			Destroy(coll.gameObject);
		else if (coll.CompareTag("Bullet"))
			Destroy(coll.gameObject);
	}
}
