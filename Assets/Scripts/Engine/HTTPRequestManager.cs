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

	private Queue<Request> requests = new Queue<Request>();
	private EventListener<HTTPResponseEvent> eventListener;



	public EventListener<HTTPResponseEvent> EventListener{
		get{
			if(eventListener == null)
				eventListener = new EventListener<HTTPResponseEvent>();

			return eventListener;
		}
	}



	public string RequestURL {
		get{
			return "http://www.runner.com";
		}
	}



	private void Start() {
		StartCoroutine(ProcessRequest());
	}



	private IEnumerator ProcessRequest() {

		while(true){

			if(requests.Count > 0){
				Request request = requests.Dequeue();

				WWW response = new WWW(RequestURL, request.Form);

				yield return response;

				if(!string.IsNullOrEmpty(response.error)){
					Debug.LogError(response.error);
				} else {
					Debug.Log(response.text);
					Hashtable data = response.text.hashtableFromJson();

					foreach(string key in data.Keys)
						eventListener.DispatchEvent(key, new HTTPResponseEvent(data));
				}
			}


			yield return new WaitForSeconds(0.1f);
		}

	}



	public void AddRequest(string command, Hashtable data) {
		Request request = new Request();

		request.Command = command.ToLower();
		request.Parameters = data;

		requests.Enqueue(request);
	}
}
