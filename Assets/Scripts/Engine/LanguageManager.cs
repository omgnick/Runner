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
				{"back", "Back"},
				{"jump", "Jump"},
				{"speed", "Speed"},
				{"hp", "Lifes"},
				{"tournament_description", "Follow the race. You will have 3 minutes, gather as quickly as many coins. "+
					"The collected coins during the race go to the prize pool. Players with the best results get the " +
					"coins of the prize pool at the end of the tournament. "},
				{"tournament_title", "Prize Pool"},
				{"results_title", "Best Results"},
				{"results", "Results"},
				{"rules", "Rules"},
				{"change_name", "Change Name"},
				{"tournament_end_in", "Tournament ends in: "}
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
				{"change_name", "Змінити ім'я"},
				{"tournament_end_in", "Турнір закінчиться через: "},
				{"network_ok", "Зв'язок є"},
				{"network_no", "Зв'язку не має"},
				{"run_results_title", "Результати"},
				{"take_gold", "Забрати"},
				{"gold_small", "Гірсть золота"},
				{"gold_medium", "Купа золота"},
				{"gold_large", "Величезна купа золота"},
				{"gold_shop_button_title", "ЗОЛОТО"},
				{"buy", "КУПИТИ"},
				{"gold_item_price", "Ціна: {0}"},
				{"gold_item_reward", "ЗОЛОТО: {0}"},
				{"skills_shop_button_title", "НАВИЧКИ"}
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
