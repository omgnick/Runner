using UnityEngine;
using System.Collections;

public class User {

	private string networkId;
	private int id;
	private int jumpLevel = 0;
	private int hpLevel = 0;
	private int speedLevel = 0;
	private int gold = 0;
	private string name = "PLAYER";
	private int lastUpdateTime;



	public void LoadFromHashtable(Hashtable data){
		if(data.ContainsKey("network_id"))
			networkId = data["network_id"].ToString();
		
		if(data.ContainsKey("id"))
			id = int.Parse(data["id"].ToString());
		
		if(data.ContainsKey("jump_level"))
			jumpLevel = int.Parse(data["jump_level"].ToString());
		
		if(data.ContainsKey("hp_level"))
			hpLevel = int.Parse(data["hp_level"].ToString());
		
		if(data.ContainsKey("speed_level"))
			speedLevel = int.Parse(data["speed_level"].ToString());
		
		if(data.ContainsKey("gold"))
			gold = int.Parse(data["gold"].ToString());

		if(data.ContainsKey("name"))
			name = data["name"].ToString();
	}



	public static User CreateFromLocalSave(){
		User user = new User();

		if(PlayerPrefs.HasKey("jump_level"))
			user.jumpLevel = PlayerPrefs.GetInt("jump_level");

		if(PlayerPrefs.HasKey("hp_level"))
			user.hpLevel = PlayerPrefs.GetInt("hp_level");

		if(PlayerPrefs.HasKey("speed_level"))
			user.speedLevel = PlayerPrefs.GetInt("speed_level");

		if(PlayerPrefs.HasKey("gold"))
			user.gold = PlayerPrefs.GetInt("gold");

		if(PlayerPrefs.HasKey("name"))
			user.name = PlayerPrefs.GetString("name");

		if(PlayerPrefs.HasKey("last_update_time"))
			user.lastUpdateTime = PlayerPrefs.GetInt("last_update_time");

		return user;
	}



	public void Save(){
		PlayerPrefs.SetInt("jump_level", jumpLevel);
		PlayerPrefs.SetInt("hp_level", hpLevel);
		PlayerPrefs.SetInt("speed_level", speedLevel);
		PlayerPrefs.SetInt("gold", gold);
		PlayerPrefs.SetString("name", name);

		PlayerPrefs.SetInt("last_update_time",
		                   Config.IsOffline ? ++lastUpdateTime : HTTPRequestManager.Instance.ServerTime);
	}



	public string NetworkID{
		get{
			return networkId;
		}

		set{
			networkId = value;
		}
	}



	public int ID {
		get{
			return id;
		}
	}



	public int JumpLevel{
		get{
			return jumpLevel;
		}

		set{
			jumpLevel = value;
		}
	}



	public int SpeedLevel {
		get{
			return speedLevel;
		}

		set{
			speedLevel = value;
		}
	}



	public int Gold {
		get{
			return gold;
		}
		set{
			gold = value;
		}
	}



	public int HPLevel {
		get{
			return hpLevel;
		}
		set{
			hpLevel = value;
		}
	}



	public CharacterStats Stats {
		get{
			CharacterStats stats = new CharacterStats();
			stats.jumpHeight += 0.2f * jumpLevel;
			stats.maxSpeed += 3 * speedLevel;
			stats.turnSpeed += 0.5f * speedLevel;
			stats.hitpoints += hpLevel;
			return stats;
		}
	}



	public string Name{
		get{
			return name;
		}
	}



	public int LastUpdateTime{
		get{
			return lastUpdateTime;
		}

		set{
			lastUpdateTime = value;
		}
	}
}
