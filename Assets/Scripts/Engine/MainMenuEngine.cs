using UnityEngine;
using System.Collections;

public class MainMenuEngine : MonoSingleton<MainMenuEngine> {

	public bool isQuiting = true;


	public void Start () {
		if(Config.ShouldInitialize){
			Config.player = User.CreateFromLocalSave();

			HTTPRequestManager.Instance.EventListener.AddEventListener(HTTPResponseEvent.RECIEVED_USER,
			                                                           OnAuthorizationCompleted);

			Hashtable loginRequestData = new Hashtable();

			if(!PlayerPreferences.HasAuthData){
				loginRequestData["device_id"] = SystemInfo.deviceUniqueIdentifier;
				HTTPRequestManager.Instance.EventListener.AddEventListener(HTTPResponseEvent.AUTH_KEY, OnRecievedAuthKey);
			}

			HTTPRequestManager.Instance.AddRequest("login", loginRequestData);

			IAPController.Instance.PrepareForUse(null);
		}
	}



	private void OnRecievedAuthKey(HTTPResponseEvent ev){
		if(!ev.data.ContainsKey("auth_key"))
			return;

		HTTPRequestManager.Instance.EventListener.RemoveEventListener(HTTPResponseEvent.AUTH_KEY, OnRecievedAuthKey);
		
		string auth_key = ev.data["auth_key"].ToString();

		if(!string.IsNullOrEmpty(auth_key)){
			PlayerPreferences.AuthKey = auth_key;

			if(ev.data.ContainsKey("user")){
				Hashtable user = ev.data["user"] as Hashtable;
				PlayerPreferences.NetworkID = user["network_id"].ToString();
			}
		}

	}



	private void OnAuthorizationCompleted(HTTPResponseEvent ev){
		Hashtable user = ev.data["user"] as Hashtable;

		if(Config.player.LastUpdateTime <= int.Parse(user["last_update_time"].ToString())){
			Config.player.LoadFromHashtable(user);
			Config.player.Save();
		} else {
			Config.player.NetworkID = user["network_id"].ToString();

			HTTPRequestManager.Instance.AddRequest("update_user", new Hashtable(){
				{"gold", Config.player.Gold},
				{"hp_level", Config.player.HPLevel},
				{"speed_level", Config.player.SpeedLevel},
				{"jump_level", Config.player.JumpLevel},
				{"name", Config.player.Name},
				{"last_update_time", Config.player.LastUpdateTime},
			});
		}
	}



	private void OnApplicationQuit() {
		isQuiting = true;
	}



	private void OnDestroy(){
		if(!isQuiting)
			HTTPRequestManager.Instance.EventListener.RemoveEventListener(HTTPResponseEvent.RECIEVED_USER,
			                                                              OnAuthorizationCompleted);
	}



	public void ApplicationCloseRequest(){
		Application.Quit();
	}



	public void StartRegularRun(){
		Application.LoadLevel("Run");
	}
			                                                          
}
