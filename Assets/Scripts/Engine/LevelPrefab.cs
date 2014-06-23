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
	public float cellWidth;
	public float cellHeight;
	public int maxNumberOfObstacles;



	public virtual bool ShouldReplace{
		get{
			CachedBehaviour mainChar = CharacterGenerator.Instance.MainCharachter;

			return mainChar != null && 
				outDistance < Vector3.Distance(mainChar.CachedTransform.position, CachedTransform.position);
		}
	}



	public Vector3 GetWorldPositionByCell(Vector2 cell){

		if(cell.x > tracksNumber / 2)
			cell.x = cell.x - (tracksNumber / 2) * 2;

		return CachedTransform.position + startPosition + (new Vector3(cell.x * cellWidth, 0, cell.y * cellHeight));
	}
}
