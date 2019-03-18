using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MapScroll : MonoBehaviour {

	public RectTransform panel;
	public Button[] maps;
	public RectTransform center;

	private float time1 = 0, cooldown = 0.2f;
	private float[] distance;
	private int bttnDist;
	private int buttonPos;
	private int count;


	void Start() {
		int mapLen = maps.Length;

		bttnDist = (int) Mathf.Abs (maps [1].GetComponent<RectTransform> ().anchoredPosition.x - maps [0].GetComponent<RectTransform> ().anchoredPosition.x);

		distance = new float[mapLen];
		distance [0] = 0;
		for (int i = 1; i < maps.Length; ++i) {
			distance [i] = distance [i - 1] + bttnDist;
		}
	}

	void Update() {
		if (Globals.controllers [0] != null) {
			var pointer = new PointerEventData (EventSystem.current);
			switch (Globals.controllers [0].type) {
			case "keyboard":
				if (Input.GetKeyDown (Globals.controllers [0].left)) {
					if (buttonPos < 0) {
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						count--;
						buttonPos += 650;
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerEnterHandler);


					}
				}
				if (Input.GetKeyDown (Globals.controllers [0].right)) {
					if (buttonPos > -2600) {
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						count++;
						buttonPos -= 650;
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerEnterHandler);

					}
				}
				if (Input.GetKeyDown(KeyCode.Return))
					ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				break;
			case "ps4":
				if (Input.GetAxis (Globals.controllers [0].horiz) < -.85 && Time.time > time1) {
					if (buttonPos < 0) {
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerExitHandler);

						buttonPos += 650;
						count--;
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
						time1 = Time.time + cooldown;
					}
				}
				if (Input.GetAxis (Globals.controllers [0].horiz) > .85 && Time.time > time1) {
					if (buttonPos > -2600) {
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerExitHandler);
						buttonPos -= 650;
						count++;
						//ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
						time1 = Time.time + cooldown;
					}
				}
				if (Input.GetKeyDown(Globals.controllers[0].enter))
					ExecuteEvents.Execute (maps[count].gameObject, pointer, ExecuteEvents.pointerClickHandler);
				break;
			}
				
			LerpToButton (buttonPos);
		}
		
	}

	public void LerpToButton (int pos){ 
		panel.anchoredPosition = new Vector2 (Mathf.Lerp (panel.anchoredPosition.x, pos, Time.deltaTime * 5), panel.anchoredPosition.y);
	}

	public void setButtonPos(int pos) {
		buttonPos = pos;
	}

}
