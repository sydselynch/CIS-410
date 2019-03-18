using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	
	private Rigidbody2D rb;
	public GameObject explosion;
	public GameObject explosion_sound;
	public AudioClip explosionSound;
	public AudioSource audioSources;
	public float speed;
	public float damage;
	public float knockback;
	public bool isExplosive;
	private bool canCreate = true;
	public float explosiveRadius;
	private float radiusPlusOffset;

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = transform.right * speed;
		if (isExplosive) {
			tag = "Explosive";
		} else {
			tag = "damage";
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//Damage or stun players here
		if (collision.gameObject.tag == "damage") {
			
		} else if (collision.gameObject.transform.tag != "Explosion" && canCreate) {
			if (transform.tag == "Explosive") {
				canCreate = false;
				explode ();
			} else {
				Destroy (gameObject);
			}
		}
	}

	private void explode() {
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
				closenessScale = ((explosiveRadius - Mathf.Abs (distFromForce.x)) * (explosiveRadius - Mathf.Abs (distFromForce.y))) / 2;
				healthScale = 1 + (100 - playerScript.health) * 0.03f;
				players.attachedRigidbody.AddForce (distFromForce * 2 * Mathf.Abs(closenessScale) * healthScale, ForceMode2D.Impulse);
				playerScript.loseHealth (damage * ((closenessScale)/ (explosiveRadius * explosiveRadius)));
			}
		}
		GameObject expl_sound = Instantiate (explosion_sound) as GameObject;
		GameObject expl = Instantiate (explosion) as GameObject;
		var particleFX = expl.GetComponent<ParticleSystem> ().main;
		particleFX.startSpeed = explosiveRadius;
		particleFX.startSize = explosiveRadius;

		expl.transform.position = transform.position;
		expl.SetActive (true);
		Destroy (gameObject);
	}

}
