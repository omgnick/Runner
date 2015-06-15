using UnityEngine;
using System.Collections;

public class ObstacleBarrel : ObstacleBase {

	public GameObject barrelNormal;
	public GameObject barrelExploded;
	public ParticleSystem explosionParticle;



	public override string PrefabPath {
		get{
			return ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL;
		}
	}



	private bool IsExploded {
		get{
			return barrelExploded.activeSelf;
		}
	}



	protected override void OnTriggerEnter(Collider collider){
		if(!IsExploded){
			CachedAnimation.Play();
			barrelNormal.gameObject.SetActive(false);
			barrelExploded.gameObject.SetActive(true);
			explosionParticle.Play();

			RunnerController runner = collider.GetComponent<RunnerController>();

			if(runner != null) {
				runner.TakeDamage(1);
				runner.Slow(0.8f);
				CameraController.Instance.PlayCameraHit();
				HudPanel.Instance.SetLifesNumber(runner.Hitpoins);
			}
		}
	}



	public override void Renew(){
		barrelNormal.gameObject.SetActive(true);
		barrelExploded.gameObject.SetActive(false);
	}
}
