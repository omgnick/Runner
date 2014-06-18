using UnityEngine;
using System.Collections;

public class TournamentHudPanel : HudPanel {

	public UILabel timer;
	public float timeLeft;
	public bool is_timer_enabled = false;



	public void Update(){

		if(is_timer_enabled) {
			if(timeLeft < 0){
				timeLeft = 0;
				TournamentRunEngine.Instance.EndTheRun();
				is_timer_enabled = false;
			}

			timer.text = (int)timeLeft / 60 + ((int)timeLeft % 60) < 10 ? ":0"+((int)timeLeft % 60) : ":" + ((int)timeLeft % 60);
			timeLeft -= Time.deltaTime;
		}


		
	}
}
