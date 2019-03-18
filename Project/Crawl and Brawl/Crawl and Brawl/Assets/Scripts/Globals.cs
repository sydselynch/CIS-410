using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals{

	//Obtained from the start scene
	public static readonly int MAX_PLAYERS = 4;
	public static int NumPlayers { get; set; }
	public static GameObject[] Players { get; set; }
	public static Controller[] controllers = new Controller[4];
}
