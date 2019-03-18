using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour {

	public Button[] pauseButtons;
	private ShowPanels showPanels;	//Reference to the ShowPanels script used to hide and show UI panels
	private int menIndex = 0;
	private bool isPaused;//Boolean to check if the game is paused or not
	private string type;
	private string exitKey;
	private int controllerNum;
	private float[] time = new float[4];
	private float cooldown = 0.25f, time1 = 0f;
	private StartOptions startScript;					//Reference to the StartButton script
	
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();
	}

	// Update is called once per frame
	void Update ()  {
		for (int i = 0; i < Globals.controllers.Length; i++) {
			if (Globals.controllers [i] != null && !isPaused) {
				if (Input.GetKey (Globals.controllers [i].start) && !startScript.inMainMenu && Time.time > time[i]) {
					time[i] = Time.time + cooldown;
					controllerNum = i;
					DoPause ();
				}
			}
		}
		

			//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
			/*if (Input.GetButtonDown ("Cancel") && !isPaused && !startScript.inMainMenu) 
			{
				//Call the DoPause function to pause the game
				DoPause();
			} */
		if(Globals.controllers[controllerNum] != null){
			
			//If the button is pressed and the game is paused and not in main menu
			if (Input.GetKeyDown (Globals.controllers[controllerNum].start) && isPaused && !startScript.inMainMenu) 
			{
				//Call the UnPause function to unpause the game
				UnPause ();
			}
			if (isPaused && !startScript.inMainMenu) {
				var pointer = new PointerEventData (EventSystem.current);
				if (Globals.controllers [controllerNum] != null) {
					switch (Globals.controllers [controllerNum].type) {
					case "ps4":
						if ((Input.GetAxis (Globals.controllers [controllerNum].vert) < -.85) && Time.time > time1 && menIndex > 0) {
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
							time1 = Time.time + cooldown;
							Debug.Log ("go up");
							menIndex--;
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
						} else if ((Input.GetAxis (Globals.controllers [controllerNum].vert) > .85) && Time.time > time1) {
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
							Debug.Log ("go down");
							time1 = Time.time + cooldown;
							menIndex++;
							Debug.Log (menIndex);
							menIndex %= pauseButtons.Length;
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
							Debug.Log ("end go down");
						} 
						else if (Input.GetKeyDown(Globals.controllers[controllerNum].enter))
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerClickHandler);
						break;
					case "keyboard":
						if (Input.GetKeyDown ("up") && Time.time > time1 && menIndex > 0) {
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
							menIndex--;
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
						} else if (Input.GetKeyDown ("down") && Time.time > time1) {
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
							menIndex++;
							menIndex %= pauseButtons.Length;
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
						} else if (Input.GetKeyDown (KeyCode.Return))
							ExecuteEvents.Execute (pauseButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerClickHandler);
						break;
					}				
				}

			}
		}
	}


	public void DoPause()
	{
		
		//Set isPaused to true
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		//call the ShowPausePanel function of the ShowPanels script
		showPanels.ShowPausePanel ();
		var pointer = new PointerEventData (EventSystem.current);
		ExecuteEvents.Execute (pauseButtons [0].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
	}


	public void UnPause()
	{
		//Set isPaused to false
		menIndex = 0;
		isPaused = false;
		//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		//call the HidePausePanel function of the ShowPanels script
		showPanels.HidePausePanel ();
	}


}
