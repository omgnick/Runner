using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager {

	public static string[] languages = new string[]{"ru", "en", "ua"};

	private static string currentLanguage = "ua";
	


	private static Dictionary<string, Dictionary<string, string>> values =
		new Dictionary<string, Dictionary<string, string>>() {
		{"ru", new Dictionary<string, string>() {
				{"run", "Забег"},
				{"shop", "Магазин"},
				{"tournament", "Турнир"},
				{"exit", "Выход"},
			}
		},

		{"en", new Dictionary<string, string>() {
				{"run", "Run"},
				{"shop", "Shop"},
				{"tournament", "Tournament"},
				{"exit", "Exit"},
			}
		},

		{"ua", new Dictionary<string, string>() {
				{"run", "Забіг"},
				{"shop", "Магазин"},
				{"tournament", "Турнір"},
				{"exit", "Вихід"},
			}
		},
	};




	public static string GetStringByKey(string key){
		key = key.ToLower();

		if(values[CurrentLanguage].ContainsKey(key))
			return values[CurrentLanguage][key];
	
		return key;
	}



	public static string CurrentLanguage {
		get{
			return currentLanguage;
		}
		set{
			currentLanguage = value;
		}
	}
}
