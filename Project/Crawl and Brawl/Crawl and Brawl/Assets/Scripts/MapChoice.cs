using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MapChoice : MonoBehaviour {

	public void changeMap(string map){
		foreach (GameObject player in Globals.Players) {
			if (player != null) {
				player.SetActive (true);
			}
		}
		SceneManager.LoadScene (map);
	}
		
}
