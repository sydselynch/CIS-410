using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool isOver = false;
	private StartOptions startScript;
	private PlayerController temp;
	private Transform WinPanel;
	public Button continueButt;

	void Awake() {
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();

		WinPanel = transform.FindChild ("WinPanel");

		continueButt = WinPanel.FindChild ("ContinueButton").GetComponent<Button>();
	}

	public void Update() {

		if (Globals.NumPlayers < 2 && !isOver && !startScript.inMainMenu) {
			isOver = true;
			for (int i = 0; i < Globals.Players.Length; i++) {
				if (Globals.Players [i].activeSelf == true) {
					temp = Globals.Players [i].GetComponent<PlayerController> ();
//					Debug.Log (temp);
					WinPanel.FindChild ("Winner Text").GetComponent<Text> ().text = ("Player " + (temp.controls.playerNum + 1) + " Wins!");
					WinPanel.FindChild ("Winner").GetComponent<Image> ().sprite = temp.playerIcon;

				} else {
					Globals.Players [i].SetActive (true); //Fixes bug where players don't spawn next time
				}
			}
			startScript.playMusic.stop ();
			showPanels.ShowWinPanel ();
			continueButt.interactable = false;
			StartCoroutine ("winCooldown");
		}
	}

	public void setIsOver(bool cond) {
		isOver = cond;
	}

	IEnumerator winCooldown() {
		yield return new WaitForSeconds (1);
		continueButt.interactable = true;
	}

}
