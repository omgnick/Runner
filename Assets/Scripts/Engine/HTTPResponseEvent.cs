using UnityEngine;
using System.Collections;

public class HTTPResponseEvent : BaseEvent {

	public const string RECIEVED_USER = "user";
	public const string AUTH_KEY = "auth_key";



	public Hashtable data{get; set;}



	public HTTPResponseEvent(Hashtable data){
		this.data = data;
	}

}
