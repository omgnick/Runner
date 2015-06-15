using UnityEngine;
using System.Collections;

public class ResultDialog : BaseGuiElement {

	public UILabel result;
	public UILabel title;
	public UILabel okLabel;
	public UIButton ok;



	public void Start(){
		title.text = LanguageManager.GetStringByKey("run_results_title");
		SetupButton(ok, "OnOk", LanguageManager.GetStringByKey("take_gold"));
	}



	public int GoldCollected {
		set{
			result.text = value.ToString();
		}
	}



	public void OnOk(){
		RunEngine.Instance.EndTheRun();
	}



	public void Show(){
		gameObject.SetActive(true);
	}



	public void Hide(){
		gameObject.SetActive(false);
	}

}
