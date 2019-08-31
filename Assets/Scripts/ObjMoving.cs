using UnityEngine;

public class ObjMoving : MonoBehaviour
{
	public float speed;

   void Update()
   {
       // Move the object vertically with a given speed
       transform.Translate(Time.deltaTime * speed * Vector3.up);
   }
}
