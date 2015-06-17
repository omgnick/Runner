using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoldShopMenu : BaseGuiElement {

	public UIButton back;
	public UIGrid grid;
	public ShopSelectionMenu shopSelectionMenu;
	public UIScrollView scroll_view;
	public UILabel userGold;

	private List<GoldShopItem> shopItems = new List<GoldShopItem>();



	public void Start(){
		SetupButton(back, "OnBack", LanguageManager.GetStringByKey("back"));
		GameObject prefab = Resources.Load("Prefabs/GUI/GoldShopDialogItem") as GameObject;

		foreach(KeyValuePair<string, Hashtable> pair in Config.store_products){
			Hashtable productData = pair.Value;
			
			GameObject obj = NGUITools.AddChild(grid.gameObject, prefab);
			GoldShopItem item = obj.GetComponent<GoldShopItem>();
			item.Init(productData);
			item.Refresh();
			item.name = productData["gold"].ToString();;
			shopItems.Add(item);
		}


		grid.sorting = UIGrid.Sorting.Custom;
		grid.onCustomSort += GridSorting;
		grid.Reposition();
	}



	private int GridSorting(Transform l, Transform r){
		int li = int.Parse(l.gameObject.name);
		int ri = int.Parse(r.gameObject.name);

		return li < ri ? -1 : (li > ri ? 1 : 0);
	}



	public void Show(){
		gameObject.SetActive(true);
//		grid.Reposition();
//		scroll_view.ResetPosition();
		
		foreach(GoldShopItem item in shopItems)
			item.Refresh();
	}



	public void Hide(){
		gameObject.SetActive(false);
	}



	public void OnBack(){
		Hide();
		shopSelectionMenu.Show();
	}



	public void Update(){
		if(Config.player != null)
			userGold.text = Config.player.Gold.ToString();
	}
}
