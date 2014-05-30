using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : CachedBehaviour {

	public int MaxPrefabsNumber {get; set;}

	private List<LevelPrefab> prefabs = new List<LevelPrefab>();
	private Queue<Transform> queue = new Queue<Transform>();
	private Transform last_created;



	public void AddPrefab(LevelPrefab prefab){
		prefabs.Add(prefab);
	}



	public void StartBuildingLevel(){
		StartCoroutine(BuildLevel());
	}



	private IEnumerator BuildLevel(){

		for(int i = queue.Count; i < MaxPrefabsNumber; i++){

		}

		return null;
	}



	private void CreateRandomPrefab(){
		LevelPrefab prefab = prefabs[Random.Range(0, prefabs.Count - 1)];

		if(prefab.prefab == null){
			prefab.prefab = Resources.Load(prefab.file) as GameObject; 
		}

		GameObject obj = Instantiate(prefab.prefab) as GameObject;

		if(last_created == null){
			obj.transform.position = Vector3.zero;
			obj.transform.rotation = Quaternion.Euler(Vector3.zero);
			obj.transform.localScale = Vector3.one;
		} else {
			LevelPrefab last_prefab = last_created.GetComponent<LevelPrefab>();
			Vector3 position = last_created.position + prefab.distance; 
			obj.transform.position = position;
			obj.transform.rotation = Quaternion.Euler(Vector3.zero);
			obj.transform.localScale = Vector3.one;
		}

		last_created = obj.transform;
	}
}
