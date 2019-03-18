using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour {

	public GameObject[] characterChoices;
	public int selectorNum;
	private int charCtr = 0;
	private GameObject curCharacter;
	private Image curImage;
	public GameObject[] otherSelectors = new GameObject[3];
	private bool validCharacter = true;

	public void Start() {
		curCharacter = characterChoices [selectorNum-1];
		curImage = transform.FindChild ("Current Player Icon").GetComponent<Image>();
		curImage.sprite = curCharacter.GetComponent<PlayerController> ().playerIcon;

	}

	public void LeftButtonClicked(){
		do {
			validCharacter = true;
			--charCtr;
			if (charCtr < 0) {
				charCtr = characterChoices.Length - 1;
			}

			for (int i = 0; i < otherSelectors.Length; ++i) {
				if (otherSelectors[i].activeSelf){
					if (characterChoices[charCtr] == otherSelectors [i].GetComponent<PlayerSelector> ().getCurrentCharacter ()) {
						validCharacter = false;
					}
				}
			}
		} while(!validCharacter);
		SelectCharacter ();
	}

	public void RightButtonClicked(){
		do {
			validCharacter = true;
			++charCtr;
			charCtr %= characterChoices.Length;
			for (int i = 0; i < otherSelectors.Length; ++i) {
				if (otherSelectors[i].activeSelf){
					if (characterChoices[charCtr] == otherSelectors [i].GetComponent<PlayerSelector> ().getCurrentCharacter ()) {
						validCharacter = false;
					}
				}
			}
		} while(!validCharacter);
		SelectCharacter ();
	}

	private void SelectCharacter() {
		curCharacter = characterChoices [charCtr];
		curImage.sprite = curCharacter.GetComponent<PlayerController> ().playerIcon;
	}

	public GameObject getCurrentCharacter (){
		return curCharacter;
	}
}
