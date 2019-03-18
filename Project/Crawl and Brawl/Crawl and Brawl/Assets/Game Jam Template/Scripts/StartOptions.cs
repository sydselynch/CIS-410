using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartOptions : MonoBehaviour {


	public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip

	[HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	[HideInInspector] public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
	[HideInInspector] public Animator animMenuAlpha;					//Reference to animator that will fade out alpha of MenuPanel canvas group
	 public AnimationClip fadeColorAnimationClip;						//Animation clip fading to color (black default) when changing scenes
	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha

	public PlayMusic playMusic;										//Reference to PlayMusic script
	private float fastFadeIn = .015f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	private int contr1Num, contr2Num, contr3Num, contr4Num, menIndex = 0;
	private bool Controller1 = false, Controller2 = false, Controller3 = false, Controller4 = false;
	public Button[] leftButton;
	public Button[] rightButton;
	public Button[] menuButtons;
	private float time1 = 0f,time3 = 0f,time4 = 0f,cooldown = .25f;
	private int totPlayers = 0;
	public Dropdown numPlayers;											//How many players playing
	private Transform[] UICharSelectors = new Transform[Globals.MAX_PLAYERS];	//The UI of the character selector
	private Transform[] UIPreCharSelectors = new Transform[4]; //
	private GameObject[] charChoices;
	private Button startButton;

	public static StartOptions Instance;


	void Awake() {

		if (Instance) {
			DestroyImmediate (gameObject);
		} else {

			Instance = this;

			//Adds listener to update how many player choices there should be
			/*totPlayers.onValueChanged.AddListener (delegate {
				PlayerChangeCheck ();
			});*/

			//Puts references to each character selector in UICharSelectors
			Transform charSelector = transform.FindChild ("MenuPanel").FindChild ("CharacterSelector");
			Transform charPreSelector = transform.FindChild ("MenuPanel").FindChild ("PreCharacterSelector");
			for (int i = 0; i < UICharSelectors.Length; ++i) {
				UICharSelectors [i] = charSelector.GetChild (i);
			}

			for (int i = 0; i < UIPreCharSelectors.Length; ++i) {
				UIPreCharSelectors [i] = charPreSelector.GetChild (i);
			}

			//Get a reference to ShowPanels attached to UI object
			showPanels = GetComponent<ShowPanels> ();

			//Get a reference to PlayMusic attached to UI object
			playMusic = GetComponent<PlayMusic> ();

			//Find a the start button
			startButton = transform.FindChild("MenuPanel").FindChild("MenuButtons").FindChild("Start").GetComponent<Button>();
			startButton.interactable = false;
		}

	}
	public void Update(){
		if (!showPanels.inControlPanel()) {
			if (Input.GetKeyDown ("right shift") && !Controller1) {
				Globals.controllers [totPlayers] = new Controller (totPlayers, 0);
				contr1Num = totPlayers;
				totPlayers++;
				Controller1 = true;
				PlayerChangeCheck ();
			} else if (Input.GetKeyDown ("left shift") && !Controller2) {
				Globals.controllers [totPlayers] = new Controller (totPlayers, 1);
				contr2Num = totPlayers;
				totPlayers++;
				Controller2 = true;
				PlayerChangeCheck ();
			} else if (Input.GetKeyDown ("joystick 1 button 9") && !Controller3) {
				Globals.controllers [totPlayers] = new Controller (totPlayers, 2);
				contr3Num = totPlayers;
				totPlayers++;
				Controller3 = true;
				PlayerChangeCheck ();
			} else if (Input.GetKeyDown ("joystick 2 button 9") && !Controller4) {
				Globals.controllers [totPlayers] = new Controller (totPlayers, 3);
				contr4Num = totPlayers;
				totPlayers++;
				Controller4 = true;
				PlayerChangeCheck ();
			}
			var pointer = new PointerEventData (EventSystem.current);
			if (Input.GetKeyDown ("left") && Controller1) {
				ExecuteEvents.Execute (leftButton [contr1Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
			} else if (Input.GetKeyDown ("right") && Controller1) {
				ExecuteEvents.Execute (rightButton [contr1Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
			} else if (Input.GetKeyDown ("a") && Controller2) {
				ExecuteEvents.Execute (leftButton [contr2Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
			} else if (Input.GetKeyDown ("d") && Controller2) {
				ExecuteEvents.Execute (rightButton [contr2Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
			} else if (Input.GetAxis ("PS4Horizontal") < -.85 && Controller3 && Time.time > time3) {
				ExecuteEvents.Execute (leftButton [contr3Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				time3 = Time.time + cooldown;
			} else if (Input.GetAxis ("PS4Horizontal") > .85 && Controller3 && Time.time > time3) {
				ExecuteEvents.Execute (rightButton [contr3Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				time3 = Time.time + cooldown;
			} else if (Input.GetAxis ("PS4Horizontal2") < -.85 && Controller4 && Time.time > time4) {
				ExecuteEvents.Execute (leftButton [contr4Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				time4 = Time.time + cooldown;
			} else if (Input.GetAxis ("PS4Horizontal2") > .85 && Controller4 && Time.time > time4) {
				ExecuteEvents.Execute (rightButton [contr4Num].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				time4 = Time.time + cooldown;
			}
			if (Globals.controllers [0] != null && totPlayers >= 2) {
				switch (Globals.controllers [0].type) {
				case "ps4":
					if (Input.GetAxis (Globals.controllers [0].vert) < -.85 && Time.time > time1 && menIndex > 0) {
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						time1 = Time.time + cooldown;
						menIndex--;
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
					} else if (Input.GetAxis (Globals.controllers [0].vert) > .85 && Time.time > time1) {
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						time1 = Time.time + cooldown;
						menIndex++;
						menIndex %= menuButtons.Length;
						ExecuteEvents.Execute (menuButtons [menIndex % menuButtons.Length].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
					} else if (Input.GetKeyDown (Globals.controllers [0].enter))
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerClickHandler);
					break;
				case "keyboard":
					if (Input.GetKeyDown (Globals.controllers [0].upBut) && Time.time > time1 && menIndex > 0) {
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						menIndex--;
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
					} else if (Input.GetKeyDown (Globals.controllers [0].downBut) && Time.time > time1) {
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						menIndex++;
						menIndex %= menuButtons.Length;
						ExecuteEvents.Execute (menuButtons [menIndex % menuButtons.Length].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
					} else if (Input.GetKeyDown (KeyCode.Return))
						ExecuteEvents.Execute (menuButtons [menIndex].gameObject, pointer, ExecuteEvents.pointerClickHandler);
					break;
				} 
			}
		}
	}


	public void StartButtonClicked() {
		//Assigns game globals like number of players or player selections
		if (totPlayers >= 2) {
			Globals.NumPlayers = totPlayers; //int.Parse(numPlayers.options [numPlayers.value].text);
			//Assigns the character choices
			Globals.Players = new GameObject[Globals.NumPlayers];
			for (int i = 0; i < Globals.NumPlayers; ++i) {
				Globals.Players [i] = UICharSelectors [i].GetComponent<PlayerSelector> ().getCurrentCharacter ();
			}

			//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
			//To change fade time, change length of animation "FadeToColor"
			if (changeMusicOnStart) {
				playMusic.FadeDown (fadeColorAnimationClip.length);
			}

			//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
			if (changeScenes) {
				//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
				Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);

				//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
				animColorFade.SetTrigger ("fade");
			} 

			//If changeScenes is false, call StartGameInScene
			else {
				//Call the StartGameInScene function to start game without loading a new scene.
				StartGameInScene ();
			}
		}
	}

    void OnEnable() {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

    //Once the level has loaded, check if we want to call PlayLevelMusic
    void SceneWasLoaded(Scene scene, LoadSceneMode mode) {
		//if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
		if (changeMusicOnStart)
		{
			playMusic.PlayLevelMusic ();
		}	
	}


	public void LoadDelayed() {
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//Hide the main menu UI element
		showPanels.HideMenu ();

		//Load the selected scene, by scene index number in build settings
		SceneManager.LoadScene ("MapSelector");
	}

	public void HideDelayed() {
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}

	public void StartGameInScene() {
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			//Wait until game has started, then play new music
			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
		}
		//Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");
		Invoke("HideDelayed", fadeAlphaAnimationClip.length);
	}


	public void PlayNewMusic() {
		//Fade up music nearly instantly without a click 
		playMusic.FadeUp (fastFadeIn);
		//Play music clip assigned to mainMusic in PlayMusic script
		playMusic.PlaySelectedMusic (1);
	}
		
	public void PlayerChangeCheck() {
		int curPlayers = totPlayers; //int.Parse(numPlayers.options [numPlayers.value].text);
		if (totPlayers >= 2) {
			startButton.interactable = true;
			var pointer = new PointerEventData (EventSystem.current);
			ExecuteEvents.Execute (menuButtons [0].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		}
		
		//Shows curPlayers amount of character selectors
		for (int i = 0; i < curPlayers; ++i) {
			UICharSelectors [i].gameObject.SetActive (true);
			UIPreCharSelectors [i].gameObject.SetActive (false);
		}

		//Hides Globals.MAX_PLAYERS - curPlayers amount of character selectors
		for (int i = curPlayers; i < Globals.MAX_PLAYERS; ++i) {
			UICharSelectors [i].gameObject.SetActive (false);
			UIPreCharSelectors [i].gameObject.SetActive (true);
		}
	}
}
