using UnityEngine;
using System.Collections;

public class PlayerPreferences {

	public static string NetworkID {
		get{
			if(PlayerPrefs.HasKey("network_id"))
				return PlayerPrefs.GetString("network_id");

			return "";
		}

		set{
			PlayerPrefs.SetString("network_id", value);
		}
	}



	public static string AuthKey{
		get{
			if(PlayerPrefs.HasKey("auth_key"))
				return PlayerPrefs.GetString("auth_key");

			return "";
		}
		
		set{
			PlayerPrefs.SetString("auth_key", value);
		}
	}



	public static bool HasAuthData{
		get{
			return PlayerPrefs.HasKey("auth_key") && PlayerPrefs.HasKey("network_id");
		}
	}
}
