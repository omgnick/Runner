using UnityEngine;
using System.Collections;

public class HudPanel : MonoSingleton<HudPanel> {

	public UILabel coinsNumber;
	public UILabel lifesNumber;



	public void SetCoinsNumber(int number){
		coinsNumber.text = number.ToString();
	}



	public void SetLifesNumber(int number){
		lifesNumber.text = number.ToString();
	}
}
