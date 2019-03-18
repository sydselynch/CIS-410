using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller {
	private string[] contrType = {"keyboard", "keyboard", "ps4", "ps4"};
	private string[] playerJump = {"up", "w","", ""};
	private string[] playerJump2 = { "", "", "joystick 1 button 1", "joystick 2 button 1" };
	private string[] playerJump3 = { "", "", "joystick 1 button 3", "joystick 2 button 3" };
	private string[] verticals = { "", "", "PS4Vertical", "PS4Vertical2" };
	private string[] horizontals = {"Horizontal", "HorizontalWASD", "PS4Horizontal", "PS4Horizontal2"};
	private string[] playerAtt = {"down", "s","joystick 1 button 5", "joystick 2 button 5"};
	private string[] throwBut = {"right shift", "left shift", "joystick 1 button 7", "joystick 2 button 7"};
	private string[] startBut = { "escape", "escape", "joystick 1 button 9", "joystick 2 button 9" };

	public string start;
	public string horiz;
	public string vert;
	public string jumpBut;
	public string jumpBut2;
	public string jumpBut3;
	public string attackBut;
	public string upBut;
	public string downBut;
	public string left;
	public string right;
	public string throwButton;
	public string enter;
	public int playerNum;
	public int contrNum;
	public string type;
	public string circle;
	public  Controller(int play, int contr){
		playerNum = play;
		contrNum = contr;
		upBut = playerJump [contrNum];
		downBut = playerAtt [contrNum];
		horiz = horizontals [contrNum];
		attackBut = playerAtt [contrNum];
		jumpBut = playerJump [contrNum];
		jumpBut2 = playerJump2 [contrNum];
		jumpBut3 = playerJump3 [contrNum];
		throwButton = throwBut [contrNum];
		type = contrType [contrNum];
		start = startBut [contrNum];
		if (type == "keyboard" && contrNum == 0) {
			left = "left";
			right = "right";
		} else if (type == "keyboard" && contrNum == 1) {
			left = "a";
			right = "d";
		}
		if (type == "ps4" && contrNum == 2) {
			enter = "joystick 1 button 1";
			circle = "joystick 1 button 2";
		} else if (type == "ps4" && contrNum == 3) {
			enter = "joystick 2 button 1";
			circle = "joystick 2 button 2";
		}
		vert = verticals [contrNum];
	}

}
