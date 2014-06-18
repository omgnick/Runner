using UnityEngine;
using System.Collections;

public class ShopMenu : BaseGuiElement {

	public UIButton back;
	public UIButton buySpeed;
	public UIButton buyJump;
	public UIButton buyHp;
	public UILabel speedPrice;
	public UILabel jumpPrice;
	public UILabel hpPrice;
	public UILabel playersMoney;
	public UILabel jumpLabel;
	public UILabel speedLabel;
	public UILabel hpLabel;
	public MainMenuPanel mainMenuPanel;



	public void Start(){
		SetupButton(back, "OnBack", LanguageManager.GetStringByKey("back"));
		SetupButton(buySpeed, "OnBuySpeed", (250 + 125 * Config.player.SpeedLevel).ToString());
		SetupButton(buyJump, "OnBuyJump", (100 + 100 * Config.player.JumpLevel).ToString());
		SetupButton(buyHp, "OnBuyHP", (400 + 200 * Config.player.HPLevel).ToString());
	}



	private void OnBack(){
		mainMenuPanel.Show();
		Hide();
	}



	public void Show(){
		gameObject.SetActive(true);
		Refresh();
	}



	public void Hide(){
		gameObject.SetActive(false);
	}



	private void Refresh(){
		

		if(Config.player.JumpLevel < 3){
			jumpPrice.text = (100 + 100 * Config.player.JumpLevel).ToString();
			jumpLabel.text = LanguageManager.GetStringByKey("jump") + " " + (Config.player.JumpLevel + 1);
			
			if(Config.player.Gold <= 100 + 100 * Config.player.JumpLevel){
				buyJump.isEnabled = false;
				jumpPrice.color = Color.red;
			} else {
				jumpPrice.color = Color.white;
				buyJump.isEnabled = true;
			}

		} else {
			buyJump.gameObject.SetActive(false);
			jumpLabel.text = LanguageManager.GetStringByKey("jump") + " " + (Config.player.JumpLevel);
		}

		if(Config.player.SpeedLevel < 3) {
			speedPrice.text = (250 + 125 * Config.player.SpeedLevel).ToString();
			speedLabel.text = LanguageManager.GetStringByKey("speed") + " " + (Config.player.SpeedLevel + 1);
			
			if(Config.player.Gold <= 250 + 125 * Config.player.SpeedLevel){
				buySpeed.isEnabled = false;
				speedPrice.color = Color.red;
			} else {
				speedPrice.color = Color.white;
				buySpeed.isEnabled = true;
			}
		} else {
			buySpeed.gameObject.SetActive(false);
			speedLabel.text = LanguageManager.GetStringByKey("speed") + " " + (Config.player.SpeedLevel);
		}

		if(Config.player.HPLevel < 3) {
			hpPrice.text = (400 + 200 * Config.player.HPLevel).ToString();
			hpLabel.text = LanguageManager.GetStringByKey("hp") + " " + (Config.player.HPLevel + 1);
			
			if(Config.player.Gold <= 400 + 200 * Config.player.HPLevel){
				buyHp.isEnabled = false;
				hpPrice.color = Color.red;
			} else {
				hpPrice.color = Color.white;
				buyHp.isEnabled = true;
			}

		} else {
			buyHp.gameObject.SetActive(false);
			hpLabel.text = LanguageManager.GetStringByKey("hp") + " " + (Config.player.HPLevel);
		}

		playersMoney.text = Config.player.Gold.ToString();
	}



	private void OnBuyJump (){
		HTTPRequestManager.Instance.AddRequest("buy_jump", null);
		Config.player.Gold -= 100 + 100 * Config.player.JumpLevel;
		Config.player.JumpLevel++;
		Refresh();
	}



	private void OnBuySpeed(){
		HTTPRequestManager.Instance.AddRequest("buy_speed", null);
		Config.player.Gold -= 250 + 125 * Config.player.SpeedLevel;
		Config.player.SpeedLevel++;
		Refresh();
	}



	private void OnBuyHP(){
		HTTPRequestManager.Instance.AddRequest("buy_hp", null);
		Config.player.Gold -= 400 + 200 * Config.player.HPLevel;
		Config.player.HPLevel++;
		Refresh();
	}
}
