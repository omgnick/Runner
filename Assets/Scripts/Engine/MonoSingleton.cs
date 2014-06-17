using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : CachedBehaviour where T : CachedBehaviour {

	private static T instance;



	public static T Instance{
		get{
			if(instance == null){

				instance = GameObject.FindObjectOfType<T>();

				if(instance == null){
					instance = (new GameObject()).AddComponent<T>();
					instance.name = typeof(T).ToString();
					instance.transform.parent = SingletonContainer.Instance;
				}
			}

			return instance;
		}
	}

}



public class SingletonContainer {
	private static Transform instance;

	public static Transform Instance{
		get{

			if(instance == null) {
				GameObject obj = GameObject.Find("Singletons");

				if(obj == null) {
					instance = (new GameObject()).transform;
					instance.name = "Singletons";
				} else 
					instance = obj.transform;

				Object.DontDestroyOnLoad(instance);
			}

			return instance;
		}
	}
}
