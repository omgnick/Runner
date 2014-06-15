using UnityEngine;
using System.Collections;

public class ObstacleGoldCoin : ObstacleBase {

	public ParticleSystem sparkles;
	public GameObject coin;
	public int coinsNumber = 1;



	public override string PrefabPath {
		get{
			return ObstaclesList.INDUSTRIAL_GOLD_COIN;
		}
	}
	
	
	
	protected override void OnTriggerEnter(Collider collider){
		RunnerController runner = collider.GetComponent<RunnerController>();

		if(runner != null){
			runner.Coins += coinsNumber;
			HudPanel.Instance.SetCoinsNumber(runner.Coins);
		}

		coin.SetActive(false);
		sparkles.Play();
	}
	
	
	
	public override void Renew(){
		coin.SetActive(true);
	}



	protected virtual int GoldAdd{
		get{
			return 1;
		}
	}
}
