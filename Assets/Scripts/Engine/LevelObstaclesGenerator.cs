using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelObstaclesGenerator : MonoSingleton<LevelObstaclesGenerator> {

	public int PresetsPoolSize {get; set;}

	protected Dictionary<string, Queue<ObstacleBase>> obstaclesPool;
	protected List<ObstaclesPresetBase> obstaclesPresets;
	protected Queue<LevelPrefab> levelPrefabsToFill = new Queue<LevelPrefab>();
	protected Dictionary<LevelPrefab, List<ObstacleBase>> usedObstacles = 
		new Dictionary<LevelPrefab, List<ObstacleBase>>();



	public List<ObstaclesPresetBase> ObstaclePresets{
		set{
			obstaclesPresets = value;
		}
	}



	public void CreateObstaclesPool(){
		Dictionary<string, int> maxObstaclesNumberPerPreset = new Dictionary<string, int>();

		if(PresetsPoolSize == 0)
			PresetsPoolSize = LevelGenerator.Instance.MaxPrefabsNumber;

		foreach(ObstaclesPresetBase preset in obstaclesPresets){
			foreach(KeyValuePair<string, Vector2[]> pair in preset.ObstaclesPositions){
				//Writing max number of items per preset
				if(!maxObstaclesNumberPerPreset.ContainsKey(pair.Key))
					maxObstaclesNumberPerPreset[pair.Key] = pair.Value.Length;
				else if(maxObstaclesNumberPerPreset[pair.Key] < pair.Value.Length)
					maxObstaclesNumberPerPreset[pair.Key] = pair.Value.Length;
			}
		}

		int obstaclesInPoolNumer = 1;
		obstaclesPool = new Dictionary<string, Queue<ObstacleBase>>();
		//Creating pools
		foreach(KeyValuePair<string, int> obstacle in maxObstaclesNumberPerPreset){
			obstaclesInPoolNumer = obstacle.Value * PresetsPoolSize;
			GameObject prefab = Resources.Load(obstacle.Key) as GameObject;
			Queue<ObstacleBase> pool = new Queue<ObstacleBase>();

			for(int i = 0; i < obstaclesInPoolNumer; i++){
				GameObject instance = Instantiate(prefab) as GameObject;
				instance.SetActive(false);
				instance.transform.parent = CachedTransform;
				pool.Enqueue(instance.GetComponent<ObstacleBase>());
			}

			obstaclesPool[obstacle.Key] = pool;
		}
	}



	public void AddPrefabLevelToFillQueue(LevelPrefab prefab) {
		levelPrefabsToFill.Enqueue(prefab);
	}



	public bool TryFillLevelPrefab(){
		if(usedObstacles.Count <= PresetsPoolSize){

			ObstaclesPresetBase preset = GetRandomPreset();
			LevelPrefab prefab = levelPrefabsToFill.Dequeue();
			List<ObstacleBase> used = new List<ObstacleBase>();

			foreach(KeyValuePair<string, Vector2[]> pair in preset.ObstaclesPositions){
				int random = Random.Range(0, prefab.tracksNumber);

				foreach(Vector2 cell in pair.Value) {
					ObstacleBase obstacle = obstaclesPool[pair.Key].Dequeue();
					used.Add(obstacle);
					Vector2 rnd_cell = cell;
					rnd_cell.x += random;
					obstacle.CachedTransform.position = prefab.GetWorldPositionByCell(rnd_cell);
					obstacle.Renew();
					obstacle.CachedGameObject.SetActive(true);
				}
			}

			usedObstacles[prefab] = used;
			return true;
		} 

		return false;
	}



	public void FreeObstacles(LevelPrefab prefab){
		if(usedObstacles.ContainsKey(prefab)){

			foreach(ObstacleBase obstacle in usedObstacles[prefab])
				obstaclesPool[obstacle.PrefabPath].Enqueue(obstacle);

			usedObstacles.Remove(prefab);
		}
	}



	public void FillLevelPrefabs(){
		while(TryFillLevelPrefab());
	}



	public ObstaclesPresetBase GetRandomPreset(){
		return obstaclesPresets[Random.Range(0, obstaclesPresets.Count)];
	}
}
