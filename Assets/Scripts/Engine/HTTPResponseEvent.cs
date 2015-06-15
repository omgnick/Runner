using UnityEngine;
using System.Collections;

public class HTTPResponseEvent : BaseEvent {

	public const string RECIEVED_USER = "user";
	public const string AUTH_KEY = "auth_key";
	public const string TOURNAMENT_DATA = "tournament";
	public const string TOURNAMENT_USERS = "tournament_users";
	public const string GOOGLE_IAB_DEVELOPER_PAYLOAD = "google_iab_purchase_payload";
	public const string GOOGLE_IAB_PURCHASE_REWARDER = "google_iab_purchases_rewarded";



	public Hashtable data{get; set;}



	public HTTPResponseEvent(Hashtable data){
		this.data = data;
	}

}
