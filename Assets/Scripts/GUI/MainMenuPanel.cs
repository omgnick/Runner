using UnityEngine;
using System.Collections;

public class MainMenuPanel : BaseGuiElement {

	public UIButton run;
	public UIButton shop;
	public UIButton tournament;
	public UIButton exit;
	public UIButton changeName;
	public ShopMenu shopMenu;
	public TournamentMenu tournamentMenu;
	public ChangeNameMenu changeNameMenu;


	public void Start(){
		SetupButton(run, "OnRun", LanguageManager.GetStringByKey("run"));
		SetupButton(shop, "OnShop", LanguageManager.GetStringByKey("shop"));
		SetupButton(tournament, "OnTournament", LanguageManager.GetStringByKey("tournament"));
		SetupButton(exit, "OnExit", LanguageManager.GetStringByKey("exit"));
		SetupButton(changeName, "OnChangeName", LanguageManager.GetStringByKey("change_name"));
	}



	private void OnRun(){
		MainMenuEngine.Instance.StartRegularRun();
	}



	private void OnShop(){
		shopMenu.Show();
		Hide ();
	}



	private void OnTournament(){
		tournamentMenu.Show();
		Hide ();
	}



	private void OnExit(){
		MainMenuEngine.Instance.ApplicationCloseRequest();
	}



	public void Hide(){
		gameObject.SetActive(false);
	}



	public void Show(){
		gameObject.SetActive(true);
	}



	private void OnChangeName(){
		changeNameMenu.Show();;
		Hide ();
	}
}
