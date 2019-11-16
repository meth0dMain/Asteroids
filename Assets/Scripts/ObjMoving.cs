using UnityEngine;

public class ObjMoving : MonoBehaviour
{
	public float speed;

   void Update()
   {
	   transform.Translate(Time.deltaTime * speed * Vector3.up);
   }
}
