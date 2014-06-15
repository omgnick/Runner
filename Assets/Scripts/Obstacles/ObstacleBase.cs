using UnityEngine;
using System.Collections;

public class ObstacleBase : CachedBehaviour {

	public virtual string PrefabPath {
		get{
			return "";
		}
	}



	public virtual void Renew(){
	}



	protected virtual void OnTriggerEnter(Collider collision){
	}
}
