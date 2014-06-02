using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunEngine : MonoBehaviour {

	private void Start () {
		List<string> prefabs = new List<string>() {"Prefabs/Levels/Industrial/SemiWalls"};

		LevelGenerator.Instance.LoadPrefabs(prefabs);
		LevelGenerator.Instance.MaxPrefabsNumber = 10;
		LevelGenerator.Instance.StartBuildingLevel();

		CharacterStats stats = new CharacterStats();
		stats.turnDistance = LevelGenerator.Instance.TrackLength;
		stats.maxTurnsNumber = LevelGenerator.Instance.TracksNumber / 2;

#if UNITY_EDITOR
		ICharacterControllerInput input = RunUserInput.Instance;
#elif UNITY_ANDROID
		ICharacterControllerInput input = RunTouchUserInput.Instance;
#endif

		CharacterGenerator.Instance.CreateCharacterAfterLevelIsBuilt(stats, input);
	}
}
