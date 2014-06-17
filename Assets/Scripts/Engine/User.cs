using UnityEngine;
using System.Collections;

public class User {

	private string networkId;
	private int id;
	private int jumpLevel = 0;
	private int hpLevel = 0;
	private int speedLevel = 0;
	private int gold = 0;



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
	}



	public void OnRecievedFromServer(HTTPResponseEvent ev){
		LoadFromHashtable(ev.data["user"] as Hashtable);
	}



	public string NetworkID{
		get{
			return networkId;
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
	}



	public int SpeedLevel {
		get{
			return speedLevel;
		}
	}



	public int Gold {
		get{
			return gold;
		}
	}



	public int HPLevel {
		get{
			return hpLevel;
		}
	}
}
