using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowPanels : MonoBehaviour {

	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject controlPanel;
	public GameObject winPanel;

	public Button winContinue;
	public Button optionsBack;
	public Button controlsBack;

	private bool options = false;
	private bool controls = false;
	private bool win = false;

	public void Update(){
		var pointer = new PointerEventData (EventSystem.current);
		if (Globals.controllers [0] != null && (options || controls)) {
			switch (Globals.controllers [0].type) {
			case "ps4":
				if (Input.GetKeyDown (Globals.controllers [0].circle)) {
					if (options) 
						ExecuteEvents.Execute (optionsBack.gameObject, pointer, ExecuteEvents.pointerClickHandler);
					else if (controls) 
						ExecuteEvents.Execute (controlsBack.gameObject, pointer, ExecuteEvents.pointerClickHandler);

				}

				break;
			case "keyboard":
				if (Input.GetKeyDown ("escape")) {
					if (options) 
						ExecuteEvents.Execute (optionsBack.gameObject, pointer, ExecuteEvents.pointerClickHandler);
					 else if (controls) 
						ExecuteEvents.Execute (controlsBack.gameObject, pointer, ExecuteEvents.pointerClickHandler);

				}

				break;
			}
		}
		if (win) {
			for (int i = 0; i < Globals.controllers.Length; i++) {
				if (Globals.controllers [i] != null) {
					switch (Globals.controllers [i].type) {
					case "ps4":
						if (Input.GetKey (Globals.controllers [i].enter)) 
							ExecuteEvents.Execute (winContinue.gameObject, pointer, ExecuteEvents.pointerClickHandler);
						break;
					case "keyboard":
						if (Input.GetKey (KeyCode.Return))
							ExecuteEvents.Execute (winContinue.gameObject, pointer, ExecuteEvents.pointerClickHandler);
						break;
					}
				}
			}
		}
	}
	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		options = true;
//		optionsPanel.SetActive(true);
//		optionsTint.SetActive(true);
		var pointer = new PointerEventData (EventSystem.current);
		ExecuteEvents.Execute (optionsBack.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
	}

	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel()
	{
		options = false;
		optionsPanel.SetActive(false);
		optionsTint.SetActive(false);
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		menuPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}
	
	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		pausePanel.SetActive (true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		pausePanel.SetActive (false);
		optionsTint.SetActive(false);
	}

	public void ShowControlsPanel() {
		controls = true;
		controlPanel.SetActive (true);
		optionsTint.SetActive(true);
		var pointer = new PointerEventData (EventSystem.current);
		ExecuteEvents.Execute (controlsBack.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
	}

	public void HideControlPanel() {
		controls = false;
		controlPanel.SetActive (false);
		optionsTint.SetActive(false);
	}

	public bool inControlPanel() {
		return controls;
	}

	//Call this function to activate and display the Win panel during game play
	public void ShowWinPanel() {
		win = true;
		winPanel.SetActive (true);
		var pointer = new PointerEventData (EventSystem.current);
		ExecuteEvents.Execute (winContinue.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		//optionsTint.SetActive (true);
	}

	//Call this function to deactivate and hide the Win panel during game play
	public void HideWinPanel() {
		win = false;
		winPanel.SetActive (false);
		//optionsTint.SetActive (false);
	}
}
