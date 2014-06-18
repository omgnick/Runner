using UnityEngine;
using System.Collections;

public class BaseGuiElement : CachedBehaviour {

	public void SetupButton(UIButton button, string callback, string text){
		button.onClick.Clear();
		button.onClick.Add(new EventDelegate(this, callback));

		UILabel label = button.GetComponentInChildren<UILabel>();

		if(label != null){
			label.text = text;
		}
	}
}
