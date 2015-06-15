using UnityEngine;
using System.Collections;

public class ConfigUpdateEvent : BaseEvent {

	public const string IsOfflineUpdated = "isOffline";

	private object newValue;



	public ConfigUpdateEvent(object newValue){
		this.newValue = newValue;
	}



	public T GetNewValue<T>() {

		if(typeof(T).IsClass)
			return newValue is T ? (T)newValue : default(T);
		else 
			return (T)newValue;
	}
}
