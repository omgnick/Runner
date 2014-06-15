using UnityEngine;
using System.Collections;

public class CharacterGenerator : MonoSingleton<CharacterGenerator> {

	string prefab_path = "Prefabs/Characters/Runner";
	private CachedBehaviour mainCharacter;



	public void CreateCharacter(CharacterStats stats, ICharacterControllerInput input){
		Object obj = Resources.Load(prefab_path);
		GameObject obj_instance = Instantiate(obj) as GameObject;
		CachedBehaviour behavior = obj_instance.GetComponent<CachedBehaviour>();
		behavior.CachedTransform.position = LevelGenerator.Instance.StartPosition;
		behavior.CachedTransform.rotation = Quaternion.identity;
		behavior.CachedTransform.parent = CachedTransform;
		RunnerController controller = behavior.GetComponent<RunnerController>();
		controller.Init(stats);
		input.Init(controller);
		controller.StartRunning();
		mainCharacter = behavior;
	}



	public CachedBehaviour MainCharachter{
		get{
			return mainCharacter;
		}
	}
}
