using UnityEngine;
using System.Collections;

public class MainMenuEngine : CachedBehaviour {



	public void Start () {
		HTTPRequestManager.Instance.AddRequest("login", new Hashtable(){
			{"device_id", "1"}
		});
	}
}
