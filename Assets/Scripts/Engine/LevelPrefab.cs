using UnityEngine;
using System.Collections;


public class LevelPrefab : CachedBehaviour {

	public string file;
	public Vector3 worldMargin;
	public Vector3 moveDirection;
	public Vector3 startPosition;
	public GameObject prefab;
	public float outDistance;
	public int tracksNumber;
	public float trackLength;



	public virtual bool ShouldReplace{
		get{
			CachedBehaviour mainChar = CharacterGenerator.Instance.MainCharachter;

			return mainChar != null && 
				outDistance < Vector3.Distance(mainChar.CachedTransform.position, CachedTransform.position);
		}
	}
}
