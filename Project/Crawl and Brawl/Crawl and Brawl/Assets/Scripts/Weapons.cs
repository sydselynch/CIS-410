using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour {
	public enum type {
		fist,
		gun,
		sword,
		throwable
	}

	[HideInInspector] public GameObject spawnpoint;
		
	public float damage;
	public float knockback;

	protected bool thrown = false;
	protected bool throwing = false;
	protected float throwForce = 0;
	protected float maxForce = 2;
	protected GameObject throwableClone;

	protected int weaponThrowSpd = 3000;
	protected int weaponSpinSpeed = -500;

	public virtual void Start () {}

	public virtual void attack (int facing) {}

	public virtual void startThrowing () {
		if (throwForce > maxForce) {
			throwForce = maxForce;
		} else {
			throwForce += Time.deltaTime;
		}
	}

	public float getThrowForce(){
		return throwForce;
	}

	public virtual void throwItem (int facing) {
		
		throwableClone = Instantiate (gameObject) as GameObject;
		Physics2D.IgnoreCollision (throwableClone.GetComponent<Collider2D> (), transform.parent.GetComponent<Collider2D> ());
		throwableClone.transform.localScale = transform.lossyScale;				//Resets scale back to original prefab scale
		throwableClone.GetComponent<Weapons> ().thrown = true;
		throwableClone.transform.tag = "Thrown Item";
		throwableClone.layer = 11; 												//Changes it to Gun layer
		
		throwableClone.transform.position = transform.position + (transform.right * 2 * facing);

		Rigidbody2D cloneRb = throwableClone.AddComponent<Rigidbody2D> ();
		cloneRb.AddForce(Vector2.right * weaponThrowSpd * facing * throwForce);
		cloneRb.AddTorque (weaponSpinSpeed * throwForce);

		Destroy (gameObject);													// Destroy the original, but the above sclone is the thrown item 
	}

	public Animator animator;


}
