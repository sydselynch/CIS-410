using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void GoBackToMain() {
		SceneManager.LoadScene ("Intro");
		GetComponent<StartOptions> ().inMainMenu = true;
		GetComponent<ShowPanels> ().ShowMenu ();
		GetComponent<WinScreen> ().setIsOver(false);
		GetComponent<PlayMusic> ().PlayLevelMusic ();
	}
}
