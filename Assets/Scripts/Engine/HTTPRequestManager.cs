using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class HTTPRequestManager : MonoSingleton<HTTPRequestManager> {

#region Classes
	private class Request {

		public Hashtable Parameters {get; set;}
		public string Command {get; set;}



		public WWWForm Form {
			get{

				WWWForm form = new WWWForm();

				form.AddField("command", Command);

				Hashtable parameters = Parameters;

				if(PlayerPreferences.HasAuthData){
					parameters["network_id"] = PlayerPreferences.NetworkID;
					parameters["auth_key"] = PlayerPreferences.AuthKey;
				}

				foreach(DictionaryEntry en in parameters){
					if(en.Value is byte[])
						form.AddBinaryData(en.Key.ToString(), en.Value as byte[]);
					else
						form.AddField(en.Key.ToString(), en.Value.ToString());
				}


				form.AddField("sig", Signature);

				//form.headers["content-type"] = "application/x-www-form-urlencoded";
				

				return form;
			}
		}



		private string Signature{
			get{
				string sig = "12#$WKNssa&@asd><";
				sig += "command"+Command;

				foreach(DictionaryEntry en in Parameters){
					sig += en.Key.ToString() + en.Value.ToString();
				}

				MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

				byte[] ascii_bytes = System.Text.Encoding.UTF8.GetBytes(sig);
				byte[] hashed_bytes = MD5CryptoServiceProvider.Create().ComputeHash(ascii_bytes);
				return System.BitConverter.ToString(hashed_bytes).Replace("-", "").ToLower();
			}
		}
	}
#endregion

	private List<Request> requests = new List<Request>();
	private EventListener<HTTPResponseEvent> eventListener;
	//TODO: put it into TimeManager
	private int serverTime;
	


	public EventListener<HTTPResponseEvent> EventListener{
		get{
			if(eventListener == null)
				eventListener = new EventListener<HTTPResponseEvent>();

			return eventListener;
		}
	}



	public string RequestURL {
		get{
			return "http://192.168.1.33";
		}
	}



	private void Start() {
		StartCoroutine(ProcessRequest());
	}



	private IEnumerator ProcessRequest() {

		while(true){

			if(requests.Count > 0){
				Request request = requests[0];

				Debug.Log("[WWW Processing] "+RequestURL);

				WWW response = new WWW(RequestURL, request.Form);
				
				yield return response;

				if(!string.IsNullOrEmpty(response.error)){
					Debug.Log("[WWW Error] " + response.error);

					Config.IsOffline = true;

				} else {

					Config.IsOffline = false;

					Debug.Log("[WWW Response] " + response.text);
					Hashtable data = response.text.hashtableFromJson();

					if(data != null && data.ContainsKey("permanent")){
						Hashtable permanent = data["permanent"] as Hashtable;
						serverTime = Mathf.FloorToInt(int.Parse(permanent["time"].ToString()) - Time.realtimeSinceStartup);
					}

					if(data != null){
						foreach(string key in data.Keys){
							eventListener.DispatchEvent(key, new HTTPResponseEvent(data));
						}
					}
				}
			
				requests.RemoveAt(0);
				yield return new WaitForEndOfFrame();
				
			} else 
				yield return new WaitForEndOfFrame();
		}

	}



	public void AddRequest(string command, Hashtable data) {
		Request request = new Request();

		request.Command = command.ToLower();
		request.Parameters = data ?? new Hashtable();

		requests.Add(request);
	}



	public int ServerTime {
		get{
			return serverTime + (int)Time.realtimeSinceStartup;
		}
	}



	public bool IsInQueue(string command){
		return requests.Find( r => r.Command == command) != null;
	}
}
