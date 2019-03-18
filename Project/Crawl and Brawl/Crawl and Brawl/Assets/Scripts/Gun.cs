using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons {

	public bool automatic;

	public GameObject bullet;

	public int bulletAmt;
	public float bulletTransOffset;

	public int shotsFired;
	public float fireRate;
	private float nextFire = 0;
	public float spreadRate;
	private Transform curBullet;

	public AudioSource gunshot1;
	public AudioClip emptyGun1;
	public AudioClip emptyGun2;
	public ParticleSystem muzzleFlash;

	public override void attack(int facing) {
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		gunshot1 = audioSources [0];

		if (bulletAmt > 0 && Time.time > nextFire) { // Shoot bullet
			gunshot1.Play ();
			--bulletAmt;
			nextFire = Time.time + fireRate;

			for (int i = 0; i < shotsFired; i++) {	
				curBullet = gameObject.transform.GetChild (0);											//Picks the first child (a bullet)
				curBullet.GetComponent<ProjectileController> ().damage = damage;
				curBullet.GetComponent<ProjectileController> ().speed *= facing;
				curBullet.GetComponent<ProjectileController> ().knockback = knockback;
				Physics2D.IgnoreCollision(curBullet.parent.parent.GetComponent<Collider2D>(), curBullet.GetComponent<Collider2D>());
				curBullet.SetParent (null);															//Detaches bullet from parent (gun)
				curBullet.rotation = Quaternion.AngleAxis (Random.Range (-spreadRate, spreadRate), Vector3.forward);	//Adjusted for spread
				curBullet.position += Vector3.right * bulletTransOffset * facing;	//Moves the bullet to the front of the gun
				curBullet.GetComponent<Rigidbody2D>().velocity = transform.right * curBullet.GetComponent<ProjectileController>().speed;
				curBullet.tag = "Nothing";
				curBullet.gameObject.SetActive (true);

				//Kickback added to player
				transform.parent.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-facing, 0.5f) * knockback);
			}

			Instantiate (muzzleFlash, curBullet.position+(Vector3.right*3*facing), Quaternion.identity);

		} else if (bulletAmt <= 0 && Time.time > nextFire) { //Throw gun
			audioSources[Random.Range(1, 3)].PlayDelayed(0.15f);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (thrown) {
			if (collision.gameObject.tag == "Player") {
				Destroy (gameObject);
			}
		}
	}
}
