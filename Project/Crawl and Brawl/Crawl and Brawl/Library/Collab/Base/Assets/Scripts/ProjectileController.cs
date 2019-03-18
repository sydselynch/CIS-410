using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	
	private Rigidbody2D rb;
	public GameObject explosion;
	public float speed;
	public float damage;
	public float knockback;
	public bool isExplosive;
	private bool canCreate = true;
	public float explosiveRadius;
	private float radiusPlusOffset;
	private int posOffset = 3;

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
		radiusPlusOffset = explosiveRadius + posOffset;

		foreach (Collider2D players in playersInRange) {
			if (players.tag == "Player") {
				playerScript = players.gameObject.GetComponent<PlayerController> ();
				playerScript.blood.Play ();

				distFromForce = players.transform.position - transform.position;
				closenessScale = (radiusPlusOffset - Mathf.Abs (distFromForce.x)) * (radiusPlusOffset - Mathf.Abs (distFromForce.y));
				healthScale = 1 + (110 - playerScript.health) * 0.02f;
				players.attachedRigidbody.AddForce (distFromForce * closenessScale * healthScale, ForceMode2D.Impulse);
				print(closenessScale/(radiusPlusOffset * radiusPlusOffset));
				print (damage);
				print (damage * (closenessScale / radiusPlusOffset * radiusPlusOffset));
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

}
