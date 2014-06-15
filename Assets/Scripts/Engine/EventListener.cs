using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventListener<T> {

	Dictionary<string, OnEventDelegate<T>> listeners = new Dictionary<string, OnEventDelegate<T>>();



	public void AddEventListener(string eventName, OnEventDelegate<T> listener){
		if(listeners.ContainsKey(eventName))
			listeners[eventName] += listener;
		else
			listeners.Add(eventName, listener);
	}



	public void DispatchEvent(string eventName, T ev){
		if(listeners.ContainsKey(eventName))
			listeners[eventName](ev);
	}



	public void RemoveEventListener(string eventName, OnEventDelegate<T> listener){
		if(listeners.ContainsKey(eventName))
			listeners[eventName] -= listener;
	}
}



public class BaseEvent {
}



public delegate void OnEventDelegate<T>(T ev);