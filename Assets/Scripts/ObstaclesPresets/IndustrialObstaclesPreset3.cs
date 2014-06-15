using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset3 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL, new Vector2[] {
						new Vector2(0, 0),
						new Vector2(1, 0),
						new Vector2(2, 0),
						new Vector2(-1, 4),
						new Vector2(-2, 4),
						new Vector2(-2, 3),
					}
				},
			};
		}
	}
}
