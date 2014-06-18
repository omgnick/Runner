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
				{"back", "Назад"},
				{"jump", "Стрибок"},
				{"speed", "Швидкість"},
				{"hp", "Життя"},
				{"tournament_description", "Виконайте забіг. У вас буде 3 хвилини, зберіть як умога більше монет. " +
					"Зібрані монети під час забігу йдуть до призового фонду. Гравці з кращими результатами отримують " +
					"монети з призового фонду по завершенню турніру. "},
				{"tournament_title", "Призовий фонд турніра"},
				{"results_title", "Кращі результати"},
				{"results", "Результати"},
				{"rules", "Правила"},
				{"change_name", "Змінити ім'я"}
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
