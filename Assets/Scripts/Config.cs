using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Config {

	public static float worldGravity = -5f;
	public static User player;
	public static EventListener<ConfigUpdateEvent> dispatcher = new EventListener<ConfigUpdateEvent>();
	private static bool isOffline;



	public static bool IsOffline {
		get{
			return isOffline;
		}

		set{
			if(isOffline != value){
				isOffline = value;
				dispatcher.DispatchEvent(ConfigUpdateEvent.IsOfflineUpdated, new ConfigUpdateEvent(isOffline));
			}
		}
	}



	public static bool ShouldInitialize {
		get{
			return player == null;
		}
	}



	public static Dictionary<string, Hashtable> store_products = new Dictionary<string, Hashtable>{
		{"gold500", new Hashtable() {
			{"productId", "gold500"},
			{"gold", 500},
			{"name", "gold_small"},
			{"icon", "coin-icon"}
		}},
		{"gold1500", new Hashtable() {
			{"productId", "gold1500"},
			{"gold", 1500},
			{"name", "gold_medium"},
			{"icon", "gold_medium"}
		}},
		{"gold3000", new Hashtable() {
			{"productId", "gold3000"},
			{"gold", 3000},
			{"name", "gold_large"},
			{"icon", "gold_large"}
		}},
	};
}
