using UnityEngine;
using System.Collections;

public class TournamentRunEngine : RunEngine {

	private bool is_saved = false;

	override public void EndTheRun(){
		if(is_saved)
			return;

		is_saved = true;
		RunnerController runner = CharacterGenerator.Instance.MainCharachter.GetComponent<RunnerController>();
		
		HTTPRequestManager.Instance.AddRequest("tournament_run_ended", new Hashtable(){
			{"gold", runner.Coins}
		});
		
		Invoke("LoadMainMenu", 2f);
	}



	override protected void Start () {
		TournamentHudPanel panel = TournamentHudPanel.Instance as TournamentHudPanel;
		panel.timeLeft = 180f;
		panel.is_timer_enabled = true;

		base.Start();
	}
}
