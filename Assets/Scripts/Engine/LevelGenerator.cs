using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoSingleton<LevelGenerator> {

	public int MaxPrefabsNumber {get; set;}
	public int MaxObstaclesNumber {get; set;}

	private List<LevelPrefab> prefabs = new List<LevelPrefab>();
	private List<LevelObstacle> obstecles = new List<LevelObstacle>();
	private Queue<Transform> queue = new Queue<Transform>();
	private Transform lastInQueue;



	public void LoadPrefabs(List<string> names) {
		foreach(string name in names) {
			GameObject obj = Resources.Load(name) as GameObject;
			LevelPrefab prefab = obj.GetComponent<LevelPrefab>();
			prefabs.Add(prefab);
		}
	}



	public void StartBuildingLevel(){
		StartCoroutine("BuildLevel");
	}



	private IEnumerator BuildLevel(){
		InstantiatePrefabs();

		while(true){
			Transform first = queue.Peek();
			LevelPrefab prefab = first.GetComponent<LevelPrefab>();

			while(!prefab.ShouldReplace){
				yield return new WaitForSeconds(0.1f);
			}

			ReplaceFirstInQueue();
		}
	}



	private void ReplaceFirstInQueue(){
		Transform first = queue.Dequeue();
		LevelPrefab prefab = first.GetComponent<LevelPrefab>();
		first.position = lastInQueue.position + prefab.worldMargin;
		lastInQueue = first;
		queue.Enqueue(lastInQueue);
	}



	private void InstantiatePrefabs(){
		Vector3 last_created_position = Vector3.zero;

		for(int i = 0; i < MaxPrefabsNumber; i++){
			LevelPrefab prefab = prefabs[i % prefabs.Count];

			if(lastInQueue != null)
				last_created_position += prefab.worldMargin;

			GameObject obj = Instantiate(prefab.gameObject, last_created_position, Quaternion.identity) 
				as GameObject;

			lastInQueue = obj.transform;
			queue.Enqueue(obj.transform);
		}
	}



	public Vector3 StartPosition{
		get{
			return prefabs[0].startPosition;
		}
	}



	public bool IsGenerated {
		get{
			return queue.Count >= MaxPrefabsNumber;
		}
	}



	public int TracksNumber {
		get{
			return prefabs[0].tracksNumber;
		}
	}



	public float TrackLength {
		get{
			return prefabs[0].trackLength;
		}
	}
}
