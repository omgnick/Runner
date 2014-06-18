using UnityEngine;
using System.Collections;

public class ChangeNameMenu : BaseGuiElement {

	public MainMenuPanel mainMenu;
	public UIButton changeName;
	public UILabel name;



	public void Start(){
		SetupButton(changeName, "OnChangeName", LanguageManager.GetStringByKey("change_name"));
	}


	public void Show(){
		gameObject.SetActive(true);
		name.text = Config.player.Name;
	}



	public void Hide(){
		gameObject.SetActive(false);
		mainMenu.Show();
	}



	private void OnChangeName(){
		if(name.text != Config.player.Name)
			HTTPRequestManager.Instance.AddRequest("change_name", new Hashtable(){
				{"changed_name", name.text}
			});

		Hide();
	}
}
