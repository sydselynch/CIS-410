using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public bool playable; //Mainly for the Demo, but can check to declare an unplayable character

	public int playernum;

	public float health = 100.0f;
	private int hitsTaken = 0;

	//Audio Variables
	private AudioSource[] audioSources;
	public AudioSource footStep;
	public AudioSource jumpSound;
	public AudioSource punch1;

	//Movement Variables
	private bool isGrounded;
	public float jump;
	private int jumpCount;
	private float jumpCoolDown = .2f;
	private float nextJump = 0;
	private Rigidbody2D rb;
	public float maxSpeed;
	public float speed;
	public Vector2 knockbackAngle;

	//Weapon Variables
	public GameObject curWeapon;
	private Transform curWeaponTrans;
	private Transform gunTrans;
	private int facing = -1;
	private float curZRotation;
	private Weapons weaponScript;
	private bool pickedUpItem = false;

	// Particle Effects
	public ParticleSystem movementDustRight;
	public ParticleSystem movementDustLeft;
	public ParticleSystem blood;

	//Player appearance
	public Sprite playerIcon;
	private Animator animator;


	//Movement parameters
	public Controller controls;

	//Player UI
	private Transform rotatableCanvas;											//For UI elements that rotate with the player
	private Transform staticCanvas;												//For UI elements that do not rotate with the player
	private Text ammoCount;
	private GameObject noAmmoTxt;
	private RectTransform healthBar;
	private RectTransform powerArrow;

	public float scale;
	public float canvasScale;


	// Use this for initialization
	void Start() {
		jumpCount = 0;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator> ();
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		footStep = audioSources [0];
		jumpSound = audioSources [1];
		punch1 = audioSources [2];

		gunTrans = transform.FindChild ("GunPos").transform;

		//Gets player UI elements that will not be rotated
		staticCanvas = transform.FindChild ("StaticPlayerCanvas");
		healthBar = staticCanvas.FindChild ("Healthbar").FindChild("Health").GetComponent<RectTransform>();
		ammoCount = staticCanvas.FindChild ("AmmoCount").GetComponent<Text>();

		//Gets player UI elements that will rotate (noAmmoTxt's child is an exception)
		rotatableCanvas = transform.FindChild ("RotatablePlayerCanvas");
		noAmmoTxt = rotatableCanvas.FindChild ("OutOfAmmoText").gameObject;
		powerArrow = rotatableCanvas.FindChild ("PowerArrow").GetComponent<RectTransform> ();
	}

	private void Update() {
		if (playable) {
			
			//Jump code
			Vector3 vertJump = new Vector3 (0.0f, jump, 0.0f);
			if (controls.type == "ps4") {
//				Debug.Log (Input.GetAxis (vert));
				if ((Input.GetAxis (controls.vert) < -0.85 || Input.GetKeyDown (controls.jumpBut2) || Input.GetKeyDown (controls.jumpBut3)) && jumpCount < 2 && Time.time > nextJump) {
					nextJump = Time.time + jumpCoolDown;
					jumpSound.Play ();
					rb.AddForce (vertJump);
					jumpCount++;
					isGrounded = false;
				}
			}
			else if (Input.GetKeyDown (controls.jumpBut) && jumpCount < 2 && Time.time > nextJump) {
				nextJump = Time.time + jumpCoolDown;
				jumpSound.Play ();
				rb.AddForce (vertJump);
				jumpCount++;
				isGrounded = false;
			}

			//Attack code TODO Improve code structure (not quite sure the best way)
			if (curWeapon != null) {
				if (Input.GetKey (controls.throwButton)) {
					//Builds up power as the player holds down the throw button
//					Debug.Log("Input key throw but");	
					weaponScript.startThrowing ();				

					//Scales power arrow by throwforce
					powerArrow.localScale = new Vector2(0.1f + weaponScript.getThrowForce() / 5, 0.1f + weaponScript.getThrowForce());
					powerArrow.gameObject.SetActive (true);
				} else if (Input.GetKeyUp (controls.throwButton) && !pickedUpItem) {
					//Throws current item
					weaponScript.throwItem (facing);
					curWeapon = null;
					hideWeaponUI ();
				} else if (curWeapon.GetComponent<Gun> () != null && curWeapon.GetComponent<Gun> ().automatic) {
					if (Input.GetKey (controls.attackBut)) {
						Attack ();
					}
				} else {
					if (Input.GetKeyDown (controls.attackBut)) {
						Attack ();
					}
				}
			}

			if (Input.GetKey (controls.throwButton)) {
				pickedUpItem = false;
			}
				
			//Movement code
			float moveHorizontal = Input.GetAxis (controls.horiz);

			if (moveHorizontal > 0) {
				animator.SetTrigger ("Move");
				transform.localScale = new Vector2(-scale, scale);
				staticCanvas.GetComponent<RectTransform>().localScale = new Vector2(-canvasScale, canvasScale);
				rotatableCanvas.transform.GetChild(0).GetChild(0).localScale = new Vector2(-0.1f, 0.1f);
				facing = 1;
				if (isGrounded && !footStep.isPlaying) {
					footStep.Play ();
					movementDustRight.Play ();
				}
			} else if (moveHorizontal < 0) {
				animator.SetTrigger ("Move");
				transform.localScale = new Vector2(scale, scale);
				staticCanvas.GetComponent<RectTransform>().localScale = new Vector2(canvasScale, canvasScale);
				rotatableCanvas.transform.GetChild(0).GetChild(0).localScale = new Vector2(0.1f, 0.1f);
				facing = -1;

				if (isGrounded && !footStep.isPlaying) {
					footStep.Play ();
					movementDustLeft.Play ();
				}
			} else {
				animator.SetTrigger ("Idle");
				footStep.Stop ();
			}

			Vector2 movement = new Vector2 (moveHorizontal * speed * Time.deltaTime, 0.0f);
			rb.AddForce (movement);

			// Limits the speed of the character to maxSpeed
			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
		}
	}

	private void increaseBlood(int bursts) {
		int num = bursts * 50;
		var em = blood.emission;
		em.SetBursts (
			new ParticleSystem.Burst[] {
				new ParticleSystem.Burst(0.1f, (short)num)
			});
	}

	private void OnCollisionEnter2D(Collision2D other) {
		AudioSource[] audioSources = GetComponents<AudioSource> ();

		//Resets jump count if there is an object is below player
		if (Vector3.Dot (other.contacts[0].normal, Vector3.up) > 0.5) { 
			isGrounded = true;
			jumpCount = 0;
		}

		switch (other.gameObject.tag) {
			case "damage":
				hitsTaken++;
				loseHealth (other.gameObject.GetComponent<ProjectileController> ().damage);
				float velocity = other.relativeVelocity.x;
				velocity *= Mathf.Abs (1 / velocity); 									// Normalize magnetude to 1.
				Vector2 Angle = new Vector2 ((velocity * knockbackAngle.x), knockbackAngle.y);
				rb.AddForce (Angle * other.gameObject.GetComponent<ProjectileController> ().knockback * ((101 - health) / 10));
				increaseBlood (hitsTaken);
				blood.Play ();
					//Instantiate (blood, transform.position, Quaternion.identity);
				audioSources [Random.Range (3, 5)].Play ();
				if (other.gameObject.layer == 14) {
					Destroy (other.gameObject);
				}
				break;

//			case "Explosive":
//				loseHealth (50.0f);
//				float expdirection = (transform.position.x - other.gameObject.transform.position.x);
//				expdirection *= Mathf.Abs (1 / expdirection); 							    // Normalize magnetude to 1.
//				Vector2 expAngle = new Vector2 ((expdirection * knockbackAngle.x), knockbackAngle.y);
//				rb.AddForce (expAngle * 300, ForceMode2D.Impulse);
//				blood.Play ();
//				break;
//

			case "Nothing":
				hitsTaken++;
				loseHealth (other.gameObject.GetComponent<ProjectileController>().damage);
				float direction = (transform.position.x - other.gameObject.transform.position.x);
				direction *= Mathf.Abs (1 / direction); 							    // Normalize magnetude to 1.
				Vector2 newAngle = new Vector2 ((direction * knockbackAngle.x), knockbackAngle.y);
				rb.AddForce (newAngle * other.gameObject.GetComponent<ProjectileController>().knockback);
				increaseBlood(hitsTaken);
				blood.Play ();
				//Instantiate (blood, transform.position, Quaternion.identity);
				audioSources[Random.Range (3, 5)].Play ();
				break;

		case "Thrown Item":
			hitsTaken++;
			float gunVelocity = other.relativeVelocity.x;
			if (gunVelocity != 0) {
				gunVelocity = gunVelocity * Mathf.Abs (1 / gunVelocity); // Normalize magnetude to 1.
				Vector2 gunAngle = new Vector2 ((gunVelocity * knockbackAngle.x), knockbackAngle.y);
				rb.AddForce (gunAngle * other.relativeVelocity.x * other.relativeVelocity.x / 3);
			} else {
				float tempDirection = (transform.position.x - other.gameObject.transform.position.x);
				tempDirection *= Mathf.Abs (1 / tempDirection);
				Vector2 tempAngle = new Vector2 ((tempDirection * knockbackAngle.x), knockbackAngle.y);
				rb.AddForce (tempAngle * 3000);
			}
				break;
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "ItemSpawn") {
			if (curWeapon == null && Input.GetKeyUp (controls.throwButton) && other.GetComponent<ItemSpawnController>().curItemExists()) {
				pickedUpItem = true;
				pickUp (other.GetComponent<ItemSpawnController>().pickupItem());
			}
		}
	}
		

	private void pickUp(GameObject weapon) {
		curWeapon = weapon;
		curWeapon.layer = 12; //Changes it to Held Gun layer
		weaponScript = curWeapon.GetComponent<Weapons> ();
		weaponScript.spawnpoint.GetComponent<ItemSpawnController> ().stopParticleEffect ();
		curWeaponTrans = curWeapon.transform;

		// Remove the items collision with character as well as physics
		curWeapon.GetComponent<Collider2D> ().isTrigger = false;
		Physics2D.IgnoreCollision (curWeapon.GetComponent<Collider2D> (), GetComponent<Collider2D> ());

		Destroy (curWeapon.GetComponent<Rigidbody2D> ());

		// Set the correct position
		curWeaponTrans.SetParent(transform);
		curWeaponTrans.localEulerAngles = new Vector3(transform.rotation.x,transform.rotation.y, 0); //Resets the z angle to 0
		curWeaponTrans.position = gunTrans.position;
		if (transform.localScale.x > 0) {
			curWeaponTrans.localScale = new Vector2 (-curWeaponTrans.localScale.x, curWeaponTrans.localScale.y);
		}

	}

	private void Attack() {
		if (curWeapon != null) {
			weaponScript.attack (facing);	
			if (curWeapon.GetComponent<Throwable> () == null) {
				ammoCount.text = "Ammo: " + curWeapon.GetComponent<Gun> ().bulletAmt;	

				//Displays Out of ammo text and plays empty noise if there are no more bullets
				if (curWeapon.GetComponent<Gun> () != null && curWeapon.GetComponent<Gun> ().bulletAmt <= 0) {
					noAmmoTxt.SetActive (true);																									
				}
			}
		} 
	}

	public void loseHealth(float playerDamage) {
		if (health >= playerDamage) {
			health -= playerDamage;
		} else {
			health = 0;
		}
		healthBar.offsetMax = new Vector2 (health, healthBar.offsetMax.y);
	}

	public void hideWeaponUI() {
		ammoCount.gameObject.SetActive (false);
		powerArrow.gameObject.SetActive (false);
		noAmmoTxt.gameObject.SetActive (false);
	}
}
