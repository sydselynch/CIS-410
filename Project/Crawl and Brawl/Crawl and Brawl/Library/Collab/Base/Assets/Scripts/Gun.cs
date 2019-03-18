using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons {

	public bool automatic;

	public GameObject bullet;

	public int bulletAmt;
	public float fireRate;
	private float nextFire = 0;

	private int gunThrowSpd = 50;
	private int gunSpinSpd = -1000;

	private bool thrown = false;

	public AudioClip gunshot1;
	public AudioSource audiosource;

	public override void attack(int facing) {
		if (bulletAmt > 0 && Time.time > nextFire) { // Shoot bullet
			audiosource.Play ();
			--bulletAmt;
			nextFire = Time.time + fireRate;
			GameObject curFiredBullet = (GameObject) Instantiate (bullet);
			curFiredBullet.GetComponent<ProjectileController> ().damage = damage;
			curFiredBullet.transform.position = transform.position + Vector3.right * 3.5f * facing;
			curFiredBullet.GetComponent<ProjectileController> ().speed *= facing;

		} else if (bulletAmt <= 0 && Time.time > nextFire) { //Throw gun
			throwWeapon(facing);
		}
	}

	public void throwWeapon(int facing) {
		transform.parent = null;
		GameObject thrownGun = (GameObject) Instantiate(gameObject,transform.position+(transform.right * 2 * facing), transform.rotation);//clone as workaround with rigidbody glitch
		thrownGun.GetComponent<PolygonCollider2D> ().enabled = true;
		thrownGun.GetComponent<Gun>().thrown = true;
		Rigidbody2D rb = thrownGun.GetComponent<Rigidbody2D> ();
		rb.velocity = transform.right * gunThrowSpd * facing;
		rb.AddTorque (gunSpinSpd);
		thrownGun.transform.tag = "Thrown Gun";
		damage /= 4;
		Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (thrown) {
			Destroy (gameObject);
		}
	}
}
