using UnityEngine;
using System.Collections;

public class MainMenuPanel : BaseGuiElement {

	public UIButton run;
	public UIButton shop;
	public UIButton tournament;
	public UIButton exit;


	public void Start(){
		SetupButton(run, "OnRun", LanguageManager.GetStringByKey("run"));
		SetupButton(shop, "OnShop", LanguageManager.GetStringByKey("shop"));
		SetupButton(tournament, "OnTournament", LanguageManager.GetStringByKey("tournament"));
		SetupButton(exit, "OnExit", LanguageManager.GetStringByKey("exit"));
	}



	private void OnRun(){
		MainMenuEngine.Instance.StartRegularRun();
	}



	private void OnShop(){
		Debug.Log("SHOP");
	}



	private void OnTournament(){
		Debug.Log("TOURNAMENT");
	}



	private void OnExit(){
		MainMenuEngine.Instance.ApplicationCloseRequest();
	}
}
