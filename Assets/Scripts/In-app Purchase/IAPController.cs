using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_ANDROID
public class IAPController : MonoSingleton<IAPController> {

	private List<GoogleSkuInfo> validProducts;
	private List<GooglePurchase> unavailableProducts = new List<GooglePurchase>();
	private List<GooglePurchase> itemsToConsume = new List<GooglePurchase>();
	private State state;
	private string productInProgressId;
	private int itemsConsumeInProgressNumber;

	public System.Action<bool> onPurchaseCompleted;
	public System.Action<GooglePurchase> onPurchaseSuccessfulyCompleted;
	public SimpleDelegate onInitCompleted;
	public SimpleDelegate onPurchaseFailedToReward;
	private SimpleDelegate onPrepareForUseCompleted;



	public enum State : int {
		NotLoaded = 0,
		Loading,
		Loaded,
		FailedToLoad,
		FailedToInitialize
	};



	#region Initialization

	public void Init () {
        if (IsInited) {
            RequestProductData();
            return;
        }

		string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnn3gnWtau+ecW0kpaA2S2cdDtQ88WmSzBmHuZ4Y92obvGCjdXrVmQdkj5Ro2S3yiq6jd700BEZxG+pjZuaIlWZucTcJycf/UMR6d1COUVygCurk22pztemWUHGroHKKhxd7brLJRK2wnoCEaOGEiGP0YDNXZIoEmfN7wDfxtRGFk+q7ckUYru754LaOYgWPcinNUO7DCfo4KdHYerSebby3dVBc+9npePM7wMYi5C642VZjgBhND+Oety7wyYbh8S/XGRdYdKny8I7qx7DCWs+Fs0cPrvNIHOfMrxLuhsQKyYoE69cexbXSeIR+/HtERpH43hX+RsIQg4vfdlc1AMQIDAQAB";

		if(string.IsNullOrEmpty(publicKey)) {
			state = State.FailedToLoad;
			
			if(onInitCompleted != null)
				onInitCompleted();

		} else {

			if(state == State.NotLoaded){
				GoogleIABManager.billingNotSupportedEvent += OnBillingNotSupported;
				GoogleIABManager.billingSupportedEvent += OnBillingSupported;
				GoogleIABManager.queryInventorySucceededEvent += OnInventoryLoaded;
				GoogleIABManager.queryInventoryFailedEvent += OnFailedToLoadInventory;
				GoogleIABManager.purchaseSucceededEvent += OnPurchaseSucceeded;
				GoogleIABManager.purchaseFailedEvent += OnPurchaseFailed;
				GoogleIABManager.consumePurchaseSucceededEvent += OnConsumePurchaseSucceeded;
				GoogleIABManager.consumePurchaseFailedEvent += OnConsumePurchaseFailed;

				HTTPRequestManager.Instance.EventListener.AddEventListener(HTTPResponseEvent.GOOGLE_IAB_DEVELOPER_PAYLOAD,
				                                                           OnRequestPayloadResponse);
				
				HTTPRequestManager.Instance.EventListener.AddEventListener(HTTPResponseEvent.GOOGLE_IAB_PURCHASE_REWARDER,
				                                                           OnConsumeProductServerResponse);
			}

			state = State.Loading;
			GoogleIAB.enableLogging(true);
			GoogleIAB.init(publicKey);
		}
	}



	override public void OnDestroy(){
	    GoogleIABManager.billingSupportedEvent -= OnBillingSupported;
	    GoogleIABManager.billingNotSupportedEvent -= OnBillingNotSupported;
	    GoogleIABManager.queryInventorySucceededEvent -= OnInventoryLoaded;
	    GoogleIABManager.queryInventoryFailedEvent -= OnFailedToLoadInventory;
	    GoogleIABManager.purchaseSucceededEvent -= OnPurchaseSucceeded;
	    GoogleIABManager.purchaseFailedEvent -= OnPurchaseFailed;
	    GoogleIABManager.consumePurchaseSucceededEvent -= OnConsumePurchaseSucceeded;
	    GoogleIABManager.consumePurchaseFailedEvent -= OnConsumePurchaseFailed;

		HTTPRequestManager.Instance.EventListener.RemoveEventListener(HTTPResponseEvent.GOOGLE_IAB_DEVELOPER_PAYLOAD,
		                                                           OnRequestPayloadResponse);
		
		HTTPRequestManager.Instance.EventListener.RemoveEventListener(HTTPResponseEvent.GOOGLE_IAB_DEVELOPER_PAYLOAD,
		                                                           OnRequestPayloadResponse);
	}



	private void OnBillingNotSupported(string error){
		state = State.FailedToInitialize;

		Debug.Log(error);

		if(onInitCompleted != null)
			onInitCompleted();
	}



	private void OnBillingSupported(){
		RequestProductData();
	}

	#endregion

	#region Load Inventory



	private void RequestProductData() {

        List<string> products = new List<string>();

        //Getting full list of products in all AB groups
	    foreach (Hashtable product in Config.store_products.Values) {
	        products.Add(product["productId"] as string);
	    }

        GoogleIAB.queryInventory(products.ToArray());
    }



    private void OnInventoryLoaded(List<GooglePurchase> purchasedProducts, List<GoogleSkuInfo> validProducts){
		state = State.Loaded;
	
		this.validProducts = validProducts;
		this.unavailableProducts = purchasedProducts.FindAll( product => product.purchaseState 
		                                                   != GooglePurchase.GooglePurchaseState.Purchased);


		itemsToConsume = purchasedProducts.FindAll( product => product.purchaseState 
		                                           == GooglePurchase.GooglePurchaseState.Purchased);


        Debug.Log("[IAP] Items to consume: " + itemsToConsume.Count);

        ConsumeIfNeeded();

		if(onInitCompleted != null)
			onInitCompleted();
	}

    public string getPriceLabel(string id) {
        if(validProducts != null) 
			foreach (GoogleSkuInfo product in validProducts) {
	            if (product.productId == id) {
	                return product.price;
	            }
			}

        return "No price";
    }

	/// <summary>
	/// Get product info by id , Returns GoogleSkuInfo (price, Currancy and more)
	/// </summary>
	/// <param name="id">Identifier.</param>
	private GoogleSkuInfo product_info_by_id (string id) {
		if(validProducts != null) 
			foreach (GoogleSkuInfo product in validProducts) {
				if (product.productId == id)
					return product;
 			}
		return null;
	}

	private void OnFailedToLoadInventory(string error){
		state = State.FailedToLoad;

		Debug.Log(error);

		if(onInitCompleted != null)
			onInitCompleted();
	}



	#endregion

	#region Purchase Product

    public void PurchaseConsumableProduct(string key) {
		Hashtable product = Config.store_products[key] as Hashtable;
		string productId = product["productId"] as string;
		productInProgressId = productId;
		

		if(IsValidProduct(productId) && CanPurchase(productId))
			HTTPRequestManager.Instance.AddRequest("request_google_iab_payload", new Hashtable(){
				{"product_id", productId},
			});
		else if (onPurchaseCompleted != null) 
			onPurchaseCompleted(false);
	}

			                             


	private void OnPurchaseSucceeded(GooglePurchase purchase){
		itemsToConsume.Add (purchase);


		Hashtable requestData = new Hashtable();


		requestData[purchase.productId] = (new Hashtable(){ 
			{"purchase_signature", purchase.signature},
			{"purchase_data", purchase.originalJson},
			{"payload", purchase.developerPayload},
		}).toJson();

		Hashtable parameters = new Hashtable(){
			{"purchases", requestData.toJson()},
		};
		
		HTTPRequestManager.Instance.AddRequest("google_iab_purchases", parameters);
	}



	private void OnPurchaseFailed(string error){
		productInProgressId = null;

		Debug.Log(error);

//        Config.request_queue.add_queued_command("fail_transaction", new Hashtable(){
//           {"data", error}
//          });

		if(onPurchaseCompleted != null)
			onPurchaseCompleted(false);
	}



	public bool IsValidProduct(string productId){
#if UNITY_EDITOR
        return true;
#else
		return validProducts != null && validProducts.Find( x => x.productId == productId ) != null;
#endif
	}



	public bool CanPurchase(string productId){
		return (itemsToConsume == null || itemsToConsume.Find( x => x.productId == productId ) == null)
			&& (unavailableProducts == null || unavailableProducts.Find( x => x.productId == productId ) == null);
	}



	public void OnRequestPayloadResponse(HTTPResponseEvent ev){

		Hashtable data = ev.data["google_iab_purchase_payload"] as Hashtable;
		string productId = data["product_id"] as string;
		string payload = data["payload"] as string;

		GoogleIAB.purchaseProduct(productId, payload);
	}



	private bool IsOldNotConsumedProduct(string productId){
		return !string.IsNullOrEmpty(productInProgressId) && productId == productInProgressId;
	}



	#endregion



	#region Consume Product



	public bool HasNotConsumedProducts {
		get{
			return itemsToConsume != null && itemsToConsume.Count > 0;
		}
	}



	private void TerminateConsume(){
		productInProgressId = null;
		itemsConsumeInProgressNumber = 0;
		
		if(onPurchaseCompleted != null)
			onPurchaseCompleted(false);
		
		if(onPrepareForUseCompleted != null){
			onPrepareForUseCompleted();
			onPrepareForUseCompleted = null;
		} 

	}



	private void OnConsumePurchaseFailed(string error){
		Debug.Log("Failed to consume product. "+error);
		
//		Config.request_queue.add_queued_command("fail_transaction", new Hashtable(){
//			{"data", error}
//		});
		
		TerminateConsume();
	}
	

	
	private void OnConsumePurchaseSucceeded(GooglePurchase purchase){
		Debug.Log("Product "+purchase.productId+" successfully consumed!");

		if(!IsOldNotConsumedProduct(purchase.productId)) {
			if(onPurchaseSuccessfulyCompleted != null)
				onPurchaseSuccessfulyCompleted(purchase);
		} else
			productInProgressId = null;

		if(HasNotConsumedProducts){
			itemsToConsume.RemoveAll(p => p.productId == purchase.productId);
			itemsConsumeInProgressNumber--;

			if(itemsConsumeInProgressNumber <= 0 && onPrepareForUseCompleted != null){
				onPrepareForUseCompleted();
				onPrepareForUseCompleted = null;
			}
		}

        /*Config.request_queue.add_queued_command("update_transaction", new Hashtable(){
			{"status", "finish"},
		});*/
		
		if(onPurchaseCompleted != null)
			onPurchaseCompleted(true);
	}



	private string GetGoodsKeyById(string id){
        Dictionary<string, Hashtable> goods = Config.store_products;
        foreach (KeyValuePair<string, Hashtable> en in goods) {
			Hashtable good = en.Value;

			if(good.ContainsKey("productId") && good["productId"] as string == id)
				return en.Key as string;
		}

		return id;
	}



	private void ConsumeAll() {
        Debug.Log("[IAP] Trying to consume...");
//        if (!Config.is_connected) return;
//        Debug.Log("[IAP] Game connected, consuming");

		if(itemsToConsume.Count > 0) {
			Hashtable requestData = new Hashtable();

			foreach(GooglePurchase purchase in itemsToConsume){
				string productKey = GetGoodsKeyById(purchase.productId);

                if (!string.IsNullOrEmpty(productKey)) {
                    requestData[purchase.productId] = (new Hashtable(){ 
						{"purchase_signature", purchase.signature},
						{"purchase_data", purchase.originalJson},
						{"payload", purchase.developerPayload},
					}).toJson();


                    Debug.Log("[IAP] Consuming " + purchase.productId);
                }
                else {
                    Debug.Log("[IAP] Consume:" + productKey + " not found");
                }
			}


			if(requestData.Count > 0) {
				itemsConsumeInProgressNumber = requestData.Count;

				Hashtable parameters = new Hashtable(){
					{"purchases", requestData.toJson()},
				};

				HTTPRequestManager.Instance.AddRequest("google_iab_purchases", parameters);

				return;
			}
		}

		if (onPrepareForUseCompleted != null){
			onPrepareForUseCompleted();
			onPrepareForUseCompleted = null;
		}
	}

    /// <summary>
    /// Consume items if need
    /// call it on start up when game connected to server and get user
    /// </summary>
    public void ConsumeIfNeeded() {
        if (HasNotConsumedProducts) {
            if (itemsConsumeInProgressNumber <= 0)
                ConsumeAll();
        }
    }

    /// <summary>
    /// Only dev function
    /// Foce consume all items
    /// </summary>
	public void ForceConsumeAll(){
//		if(!Config.is_dev)
//			return;

		if(itemsToConsume.Count > 0) {
			string[] skus = new string[itemsToConsume.Count];

			for(int i = 0; i < itemsToConsume.Count; i++)
				skus[i] = itemsToConsume[i].productId;

			itemsConsumeInProgressNumber = skus.Length;
			GoogleIAB.consumeProducts(skus);
		}
	}



	public void OnConsumeProductServerResponse(HTTPResponseEvent ev){
		ArrayList data = ev.data["google_iab_purchases_rewarded"] as ArrayList;

		List<string> skus = new List<string>();

		foreach(Hashtable info in data){
			bool isConsumedByServer = bool.Parse(info["is_consumed"].ToString());

			if(isConsumedByServer)
				skus.Add(info["product_id"] as string);
		}

		if(skus.Count != data.Count && onPurchaseFailedToReward != null)
			onPurchaseFailedToReward();

		itemsConsumeInProgressNumber = skus.Count;

		if(skus.Count == 0) {
			TerminateConsume();
			Debug.Log("Google IAB, server rejected consume");
		} else
			GoogleIAB.consumeProducts(skus.ToArray());
	}

	#endregion



	#region Ready For Use



	public bool IsReadyForUse {
		get{
			return IsInited && !HasNotConsumedProducts;
		}
	}



	public void PrepareForUse(SimpleDelegate on_complete){

		if(on_complete != null)
			onPrepareForUseCompleted += on_complete;

		if(!IsBillingSupported){
			if(onPrepareForUseCompleted != null)
				onPrepareForUseCompleted();
		} else if(IsLoading){
			onInitCompleted += OnReadyForUse;
		} else if(!IsInited){
			onInitCompleted += OnReadyForUse;
			Init();
		} else if(HasNotConsumedProducts) {
			if(itemsConsumeInProgressNumber <= 0)
				ConsumeAll();
		} else if(onPrepareForUseCompleted != null)
			onPrepareForUseCompleted();

	}



	private void OnReadyForUse (){
		onInitCompleted -= OnReadyForUse;

		if(IsInited && HasNotConsumedProducts)
			ConsumeAll();
		else if(onPrepareForUseCompleted != null) {
			onPrepareForUseCompleted();
			onPrepareForUseCompleted = null;
		}
	}



	#endregion


	public bool IsLoading {
		get {
#if UNITY_EDITOR
            return false;
#else

			return state == State.Loading;
#endif
        }
	}



	public bool IsInited {
		get{
			return !IsLoading && state != State.NotLoaded && state != State.FailedToLoad;
		}
	}



	public bool IsBillingSupported {
		get{
			return state != State.FailedToInitialize;
		}
	}
}

public delegate void SimpleDelegate();


#endif