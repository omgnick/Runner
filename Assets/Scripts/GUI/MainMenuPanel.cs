using UnityEngine;
using System.Collections;

public class MainMenuPanel : BaseGuiElement {

	public UIButton run;
	public UIButton shop;
	public UIButton tournament;
	public UIButton exit;
	public UIButton changeName;
	public ShopSelectionMenu shopSelectionMenu;
	public TournamentMenu tournamentMenu;
	public ChangeNameMenu changeNameMenu;
	public UILabel network;



	public void Start(){
		SetupButton(run, "OnRun", LanguageManager.GetStringByKey("run"));
		SetupButton(shop, "OnShop", LanguageManager.GetStringByKey("shop"));
		SetupButton(tournament, "OnTournament", LanguageManager.GetStringByKey("tournament"));
		SetupButton(exit, "OnExit", LanguageManager.GetStringByKey("exit"));
		SetupButton(changeName, "OnChangeName", LanguageManager.GetStringByKey("change_name"));

		Config.dispatcher.AddEventListener(ConfigUpdateEvent.IsOfflineUpdated, OnOfflineStatusChanged);
		UpdateNetworkLabel();
	}



	private void OnDestroy(){
		Config.dispatcher.RemoveEventListener(ConfigUpdateEvent.IsOfflineUpdated, OnOfflineStatusChanged);
	}



	private void OnRun(){
		MainMenuEngine.Instance.StartRegularRun();
	}



	private void OnShop(){
		shopSelectionMenu.Show();
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



	private void OnOfflineStatusChanged(ConfigUpdateEvent ev){
		bool isOnline = !ev.GetNewValue<bool>();
		tournament.enabled = isOnline;

		if(tournamentMenu.IsOpened && !isOnline){
			tournamentMenu.Hide();
			Show();
		}

		UpdateNetworkLabel();
	}



	private void UpdateNetworkLabel(){
		network.text = LanguageManager.GetStringByKey(Config.IsOffline ? "network_no" : "network_ok");
		network.color = Config.IsOffline ? Color.red : Color.green;
	}
}
