using UnityEngine;
using System.Collections;

public class GoldShopItem : BaseGuiElement {

	public UILabel reward;
	public UILabel price;
	public UISprite icon;
	public UIButton buy;
	private Hashtable productData;



	public void Init(Hashtable productData) {
		this.productData = productData;
		SetupButton(buy, "OnBuy", LanguageManager.GetStringByKey("buy"));
	}



	public void Refresh(){
		string productId = productData["productId"] as string;
		
		if(IAPController.Instance.IsValidProduct(productId)) {
			icon.spriteName = productData["icon"] as string;
			reward.text = string.Format(LanguageManager.GetStringByKey("gold_item_reward"), (int)productData["gold"]);
			price.text = string.Format(LanguageManager.GetStringByKey("gold_item_price"), 
			                           IAPController.Instance.getPriceLabel(productId));
		} else {
			buy.gameObject.SetActive(false);
			price.gameObject.SetActive(false);
			reward.gameObject.SetActive(false);
		}
	}



	public void OnBuy() {
		IAPController.Instance.PurchaseConsumableProduct(productData["productId"] as string);
	}
}
