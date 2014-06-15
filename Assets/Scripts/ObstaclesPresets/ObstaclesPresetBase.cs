using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclesPresetBase {

	public virtual Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]>();
		}
	}

}
