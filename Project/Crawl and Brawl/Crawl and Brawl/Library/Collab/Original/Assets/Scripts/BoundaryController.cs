using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundaryController : MonoBehaviour {

	private CameraController cameraController;
	public GameObject playerUI;
	private GameObject[] playerIcons;
	private PlayerController temp;
	public ParticleSystem deathExplosion;
	public AudioSource[] audioSources;

	void Start() {
		audioSources = GetComponents<AudioSource> ();
		cameraController = gameObject.transform.parent.GetComponent<CameraController> ();
		playerIcons = new GameObject[playerUI.transform.childCount];
		for (int i = 0; i < playerIcons.Length; ++i){
			playerIcons[i] = playerUI.transform.GetChild (i).gameObject;
		}
	}
		
	void OnTriggerExit2D(Collider2D collision) {
		switch (collision.gameObject.tag) {
		case "Player":
				temp = collision.GetComponent<PlayerController> ();
				temp.playable = false;
				print (collision.transform.name + " has died");
				collision.gameObject.SetActive (false);
				playerIcons [temp.controls.playerNum].SetActive (false);

				Vector3 playerPos = collision.transform.position;
				Instantiate (deathExplosion, playerPos, Quaternion.identity);
				temp.blood.Play ();
				audioSources [0].Play ();
				cameraController.ShakeCamera ();
				Globals.NumPlayers -= 1;
				Globals.Players [temp.controls.playerNum].SetActive (false);
				if (Globals.NumPlayers < 2) {
					playerUI.SetActive (false);
				}
				break;

			case "damage":
				Destroy (collision.gameObject);
				break;

			case "Environmental Spawn":
				Destroy (collision.gameObject);
				break;

		}
	}
}
