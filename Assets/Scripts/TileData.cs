using UnityEngine;
using UnityEngine.Tilemaps;

// TileData that stores the different PowerUps information to avoid overflooding the scene with multiple gameObjects
// therefore saving resources.
//
// It stores the Tile to compare with the PowerUp TileMap position and the GameObject that should be spawned after
// casting the ?-Block (however not limiting to just that block but any other block that contains an specific powerup
// like invisible blocks or camoflaged blocks). The spawned object is spawned in a Spawn Position and passes a target 
// position (which can be one tile Up or Down depending on the cast collision direction). The Spawned Object has a spawn
// speed which determines how long it takes to reach the target position and a move speed that determines (in case it
// moves) how fast the object will move when spawned
[CreateAssetMenu]
public class TileData : ScriptableObject {
	public TileBase tile;
	public GameObject tileGameObject;

	[HideInInspector]
	public Vector3 targetPosition;
	
	public float spawnSpeed;
	public float moveSpeed;

	public void SpawnTile(Vector3 spawnPos, Vector3 targetPos) {
		Instantiate(tileGameObject, spawnPos, Quaternion.identity);
		targetPosition = targetPos;
	}
}
