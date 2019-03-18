using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class GameController : MonoBehaviour {
	
	public GameObject[] players;						//All the players in the game

	public GameObject playerStatusUI;					//Referenced to display the icons of the players at the top of the UI

	public GameObject spawnpoints;						//Stores all of the different spawn points
	private List<Vector3> spawns;

	public bool inSpace;
	public bool onIceMap;


	private PlayerController temp;

	// Use this for initialization
	void Start () {
		// Spawn location, if none then adds vectors ([0,3],0,0)
		if (spawnpoints != null) {
			spawns = new List<Vector3> ();
			foreach (Transform spawn in spawnpoints.transform) {
				spawns.Add (spawn.position);
			}
		}												

		//Initialized to inactive so they can be reassigned with new images
		GameObject[] playerIcons = new GameObject[playerStatusUI.transform.childCount];		
		for (int i = 0; i < playerIcons.Length; ++i) {
			playerIcons [i] = playerStatusUI.transform.GetChild (i).gameObject;
			playerIcons [i].SetActive (false);
		}

		//If gone through the intro scene you will have a non-null global players array
		if (Globals.Players != null) {														
			foreach (GameObject player in players) {
				Destroy (player);
			}
			players = new GameObject[Globals.Players.Length];
			for (int i = 0; i < players.Length; ++i) {
				players [i] = Instantiate (Globals.Players [i]);
			}
		}

		//Initialize player controls, spawns, and gui elements
		for (int i = 0; i < players.Length; i++) {
			if (spawns != null){
				players [i].transform.position = spawns[i]; 
			}
			temp = players [i].GetComponent<PlayerController> ();
			playerIcons [i].GetComponent<Image>().sprite = temp.playerIcon;

			players [i].GetComponent<PlayerController> ().controls = Globals.controllers[i];
			playerIcons [i].gameObject.SetActive(true);

			players[i].transform.FindChild("StaticPlayerCanvas").FindChild ("PlayerLabel").GetComponent<Text>().text = "Player " + (i + 1);
			Globals.Players [i] = players [i];
		}

		//Space modifier
		if (inSpace) {
			Physics2D.gravity = new Vector2 (0, -3.0f);
		} else {
			Physics2D.gravity = new Vector2 (0, -9.81f);
		}
		if (onIceMap) {
			foreach (GameObject player in Globals.Players) { 
				player.GetComponent<PlayerController> ().speed = 13300;
			}
		} else if (!onIceMap) {
			foreach (GameObject player in Globals.Players) { 
				player.GetComponent<PlayerController> ().speed = 19000;
			}
		}
			
	}
		
}
