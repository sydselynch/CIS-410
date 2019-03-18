using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Weapons {

	public bool explosive;
	public GameObject explosion;
	public GameObject explosion_sound;
	private bool isCountingDown = false;
	public float explosiveRadius;
	public float grenadeTimer;
	public AudioClip explosionSound;
	public AudioSource audioSources;

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
		audioSources = gameObject.GetComponent<AudioSource>();
		audioSources.Play ();
		explode();
	}

	public void explode() {
		Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
		Vector2 distFromForce;
		float closenessScale, healthScale;
		PlayerController playerScript;
		foreach (Collider2D players in playersInRange) {
			if (players.tag == "Player") {
				playerScript = players.gameObject.GetComponent<PlayerController> ();
				playerScript.blood.Play ();
				//Instantiate(playerScript.blood, playerScript.transform.position, Quaternion.identity);

				distFromForce = players.transform.position - transform.position;
				closenessScale = ((explosiveRadius - Mathf.Abs (distFromForce.x)) * (explosiveRadius - Mathf.Abs (distFromForce.y))) / 5;
				print (closenessScale);
				healthScale = 1 + (100 - playerScript.health) * 0.03f;
				players.attachedRigidbody.AddForce (distFromForce * 1.5f * Mathf.Abs(closenessScale) * healthScale, ForceMode2D.Impulse);
				playerScript.loseHealth (damage * ((closenessScale+100)/ (explosiveRadius * explosiveRadius)));
			}
		}
		GameObject expl_sound = Instantiate (explosion_sound) as GameObject;
		Destroy (expl_sound, 3);
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
