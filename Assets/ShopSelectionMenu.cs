using UnityEngine;
using System.Collections;

public class ShopSelectionMenu : BaseGuiElement {

	public UIButton skillsShopMenuButton;
	public UIButton goldShopMenuButton;
	public UIButton back;
	public ShopMenu skillsShop;
	public MainMenuPanel mainMenu;
	public GoldShopMenu goldShop;




	public void Start(){
		SetupButton(goldShopMenuButton, "OnGoldShop", LanguageManager.GetStringByKey("gold_shop_button_title"));
		SetupButton(back, "OnBack", LanguageManager.GetStringByKey("back"));
		SetupButton(skillsShopMenuButton, "OnSkillsShop", LanguageManager.GetStringByKey("skills_shop_button_title"));
	}



	private void OnGoldShop(){
		goldShop.Show();
		Hide();
	}



	private void OnSkillsShop(){
		skillsShop.Show();
		Hide();
	}



	public void Show(){
		gameObject.SetActive(true);
	}



	public void Hide(){
		gameObject.SetActive(false);
	}



	public void OnBack(){
		Hide();
		mainMenu.Show();
	}

}
