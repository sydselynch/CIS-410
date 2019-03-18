using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Weapons {

	public bool explosive;
	public GameObject explosion;
	private bool isCountingDown = false;
	public float explosiveRadius;
	private int posOffset = 1;
	public float radiusPlusOffset;
	public float grenadeTimer;

	public override void startThrowing() {
		if (explosive && ! isCountingDown) {
			isCountingDown = true;
			StartCoroutine("explodeCountdown");							//Starts the timer for explosion
		}
		base.startThrowing ();
	}

	private IEnumerator explodeCountdown() {
		while (grenadeTimer > 0) {
			yield return new WaitForSeconds (1);
			grenadeTimer -= 1;
		}
		if (transform.parent != null) {
			transform.parent.GetComponent<PlayerController>().hideWeaponUI ();
		}
		explode();
	}

	public void explode() {
		Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
		Vector2 distFromForce;
		float closenessScale, healthScale;
		PlayerController playerScript;
		radiusPlusOffset = explosiveRadius + posOffset;

		foreach (Collider2D players in playersInRange) {
			if (players.tag == "Player") {
				playerScript = players.gameObject.GetComponent<PlayerController> ();
				playerScript.blood.Play ();

				distFromForce = players.transform.position - transform.position;
				closenessScale = (radiusPlusOffset - Mathf.Abs (distFromForce.x)) * (radiusPlusOffset - Mathf.Abs (distFromForce.y));
				healthScale = 1 + (110 - playerScript.health) * 0.02f;
				players.attachedRigidbody.AddForce (distFromForce * closenessScale * healthScale, ForceMode2D.Impulse);
				playerScript.loseHealth (damage * (closenessScale / (radiusPlusOffset * radiusPlusOffset)));
			}
		}

		GameObject expl = Instantiate (explosion) as GameObject;
		var particleFX = expl.GetComponent<ParticleSystem> ().main;
		particleFX.startSpeed = explosiveRadius;
		particleFX.startSize = explosiveRadius;

		expl.transform.position = transform.position;
		expl.SetActive (true);
		Destroy (gameObject);
	}

	public override void throwItem (int facing) {
		if (explosive) {
			transform.parent.GetComponent<PlayerController>().hideWeaponUI ();
			throwableClone = Instantiate (gameObject) as GameObject;
//			Physics2D.IgnoreCollision (throwableClone.GetComponent<Collider2D> (), transform.parent.GetComponent<Collider2D> ());
			throwableClone.GetComponent<Throwable> ().thrown = true;
			throwableClone.GetComponent<Throwable> ().StartCoroutine ("explodeCountdown");
			throwableClone.transform.localScale = transform.lossyScale;				//Resets scale back to original prefab scale
			throwableClone.layer = 11; 												//Changes it to Gun layer

			throwableClone.transform.position = transform.position + (transform.right * 2 * facing);

			Rigidbody2D cloneRb = throwableClone.AddComponent<Rigidbody2D> ();
			cloneRb.AddForce(Vector2.right * weaponThrowSpd * facing * throwForce);
			cloneRb.AddTorque (weaponSpinSpeed * throwForce);

			Destroy (gameObject);	
		} else {
			base.throwItem (facing);
		}


	}
}
