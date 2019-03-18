using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Globals.MAX_PLAYERS; ++i) {
			if (i < Globals.NumPlayers) {
				transform.GetChild (i).GetComponent<Image> ().sprite = Globals.Players [i].GetComponent<PlayerController> ().playerIcon;
			} else {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}
}
