using UnityEngine;
using System.Collections;

public class CharacterGenerator : MonoSingleton<CharacterGenerator> {

	string prefab_path = "Prefabs/Characters/Runner";
	private CachedBehaviour mainCharacter;



	public void CreateCharacterAfterLevelIsBuilt(CharacterStats stats, ICharacterControllerInput input){
		StartCoroutine(DeferredCharacterCreation(stats, input));
	}



	private IEnumerator DeferredCharacterCreation(CharacterStats stats, ICharacterControllerInput input){
		while(!LevelGenerator.Instance.IsGenerated)
			yield return new WaitForSeconds(0.1f);

		Object obj = Resources.Load(prefab_path);
		GameObject obj_instance = Instantiate(obj) as GameObject;
		CachedBehaviour behavior = obj_instance.GetComponent<CachedBehaviour>();
		behavior.CachedTransform.position = LevelGenerator.Instance.StartPosition;
		behavior.CachedTransform.rotation = Quaternion.identity;
		RunnerController controller = behavior.GetComponent<RunnerController>();
		controller.Init(stats);
		input.Init(controller);
		controller.StartRunning();
		mainCharacter = behavior;
	

		return true;
	}



	public CachedBehaviour MainCharachter{
		get{
			return mainCharacter;
		}
	}
}
