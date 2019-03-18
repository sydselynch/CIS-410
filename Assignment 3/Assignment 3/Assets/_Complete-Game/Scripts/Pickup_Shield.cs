using UnityEngine;
using System.Collections;

public class Pickup_Shield : MonoBehaviour
{
	void OnTriggerExit (Collider other) 
	{
        Done_PlayerController.shield = true;
        Destroy(gameObject);
	}
}