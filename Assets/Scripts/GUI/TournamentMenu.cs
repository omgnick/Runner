using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TournamentMenu : BaseGuiElement {

	public UILabel tournamentPrize;
	public UILabel tournamentDescription;
	public UILabel tournamentTitle;
	public UIButton back;
	public UIButton results;
	public UIButton rules;
	public UIButton run;
	public UIGrid grid;
	public UILabel resultsTitle;
	public TournamentItem[] tournamentItems;
	public MainMenuPanel mainMenu;
	public UILabel tournamentEndsIn;
	private Hashtable tournamentData;
	private List<Hashtable> tournamentUsersData;
	private int tournamentEndTime;
	private int lastUpdateTime;
	private int updateInterval = 10;



	public void Refresh(){
		tournamentDescription.text = LanguageManager.GetStringByKey("tournament_description");
		tournamentTitle.text = LanguageManager.GetStringByKey("tournament_title");
		resultsTitle.text = LanguageManager.GetStringByKey("results_title");

		
		SetupButton(back, "OnBack", LanguageManager.GetStringByKey("back"));
		SetupButton(results, "OnResults", LanguageManager.GetStringByKey("results"));
		SetupButton(run, "OnRun", LanguageManager.GetStringByKey("run"));
		SetupButton(rules, "OnRules", LanguageManager.GetStringByKey("rules"));
	}



	private void RequestTournamentData() {
		lastUpdateTime = HTTPRequestManager.Instance.ServerTime;
		
		HTTPRequestManager.Instance.EventListener.AddEventListener(HTTPResponseEvent.TOURNAMENT_DATA, 
		                                                           OnTournamentDataRecieved);
		HTTPRequestManager.Instance.AddRequest("get_tournament_data", null);

	}



	public void OnBack(){
		mainMenu.Show();
		Hide ();
	}



	public void OnResults(){
		ShowResults();
	}



	public void OnRun(){
		Application.LoadLevel("TournamentRun");
	}



	public void Hide(){
		gameObject.SetActive(false);
	}



	public void Show(){
		gameObject.SetActive(true);
		Refresh();
		ShowRules();
	}



	private void ShowResults(){
		grid.gameObject.SetActive(true);
		resultsTitle.gameObject.SetActive(true);
		tournamentDescription.gameObject.SetActive(false);
		rules.gameObject.SetActive(true);
		results.gameObject.SetActive(false);
	}



	private void ShowRules(){
		grid.gameObject.SetActive(false);
		resultsTitle.gameObject.SetActive(false);
		tournamentDescription.gameObject.SetActive(true);
		rules.gameObject.SetActive(false);
		results.gameObject.SetActive(true);
	}



	private void OnRules(){
		ShowRules();
	}



	private void OnTournamentDataRecieved(HTTPResponseEvent ev){
		HTTPRequestManager.Instance.EventListener.RemoveEventListener(HTTPResponseEvent.TOURNAMENT_DATA, 
		                                                           OnTournamentDataRecieved);
		Hashtable data = ev.data;
		tournamentData = data["tournament"] as Hashtable;
		ArrayList list = data["tournament_users"] as ArrayList;
		//TODO: add server time
		tournamentEndTime = int.Parse(tournamentData["end_time"].ToString());

		tournamentUsersData = new List<Hashtable>();

		foreach(object hash in list)
			tournamentUsersData.Add(hash as Hashtable);

		tournamentPrize.text = tournamentData["total_prize"].ToString();

		for(int i = 0; i < tournamentItems.Length; i++){
			TournamentItem item = tournamentItems[i];
			
			if(tournamentUsersData.Count > i){
				Hashtable user = tournamentUsersData[i] as Hashtable;

				item.collectedGold.text = user["gold_collected"].ToString();
				item.playerName.text = user["user_name"].ToString();
			} else {
				item.collectedGold.text = "";
				item.playerName.text = "";
			}
		}
	}



	public void Update(){
		tournamentEndsIn.text = TimeLeftString;

		if(ShouldRequestTournamentData)
			RequestTournamentData();
	}



	private string TimeLeftString {
		get{
			return LanguageManager.GetStringByKey("tournament_end_in") + 
				(int)TimeLeft / 60 + 
					(((int)TimeLeft % 60) < 10 ? ":0"+((int)TimeLeft % 60) : ":" + ((int)TimeLeft % 60));
		}
	}



	private int TimeLeft {
		get{
			return System.Convert.ToInt32(Mathf.Max(0f, tournamentEndTime - HTTPRequestManager.Instance.ServerTime));
		}
	}



	private bool ShouldRequestTournamentData{
		get{
			return TimeLeft <= 0 && !HTTPRequestManager.Instance.IsInvoking("get_tournament_data") && 
				lastUpdateTime + updateInterval < HTTPRequestManager.Instance.ServerTime;
		}
	}




	public bool IsOpened {
		get{
			return gameObject.activeSelf;
		}
	}
}
