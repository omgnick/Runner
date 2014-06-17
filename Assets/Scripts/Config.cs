using UnityEngine;
using System.Collections;

public class Config {

	public static float worldGravity = -5f;
	public static User player;



	public static bool ShouldInitialize {
		get{
			return player == null;
		}
	}
}
