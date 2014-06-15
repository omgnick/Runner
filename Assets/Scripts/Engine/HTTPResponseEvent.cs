using UnityEngine;
using System.Collections;

public class HTTPResponseEvent : BaseEvent {

	public Hashtable data{get; set;}



	public HTTPResponseEvent(Hashtable data){
		this.data = data;
	}

}
