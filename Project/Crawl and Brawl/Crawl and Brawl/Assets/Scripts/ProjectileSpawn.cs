using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour {

	public Sprite[] projectiles;
	public int size;
	public bool fast;
	public GameObject projectile;
	private GameObject curProjectile;
	private Rigidbody2D curRb;
	private int projectileSpin = 1000;
	public int spawnTime;
	private float timer;
	private Vector2 randVec2;
	public bool itemIsFireball;

	void Start(){
		spawnTime = Random.Range (1, 1);
		if (fast)
			spawnTime = spawnTime / 15;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= spawnTime) {
			spawnTime = Random.Range (1, 1);
			if (fast)
				spawnTime = spawnTime / 10;
			timer = 0;
			if (itemIsFireball) {
				SpawnFireball ();
			} else {

				SpawnProjectile ();
			}
		}
	}

	void SpawnProjectile(){
		curProjectile = (GameObject) Instantiate (projectile,transform.position,transform.rotation);
		curProjectile.GetComponent<SpriteRenderer> ().sprite = projectiles [Random.Range(0,size)];
		curRb = curProjectile.GetComponent<Rigidbody2D> ();
		randVec2 = new Vector2 (Random.Range (-1000.0f, 1000.0f), Random.Range (-100.0f, 100.0f));
		curRb.AddForce (randVec2);
		curRb.AddTorque (projectileSpin);
	}

	void SpawnFireball() {
		curProjectile = (GameObject) Instantiate (projectile,transform.position,transform.rotation);
		curRb = curProjectile.GetComponent<Rigidbody2D> ();
		curRb.AddForce (new Vector2(-2,1) * 5000);
		curRb.AddTorque (1500);
	}
}
