using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : CachedBehaviour where T : CachedBehaviour {

	private static GameObject singletonContainer;
	private static T instance;



	public static T Instance{
		get{
			if(instance == null){
				if(singletonContainer == null){
					singletonContainer = new GameObject();
					singletonContainer.name = "Singletons";
				}

				instance = singletonContainer.AddComponent<T>();
			}

			return instance;
		}
	}

}
