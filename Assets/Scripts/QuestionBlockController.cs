using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class QuestionBlockController : MonoBehaviour {

	public Tilemap powerUpTilemap;
	public TileBase[] questionBlockTile;
	public TileBase usedBlockTile;

	public TileData[] tileDatas;
	
	private Tilemap _tilemap;

	private void Start() { 
		_tilemap = GetComponent<Tilemap>();
	}
	
	/*
	 * TODO - Add Invisible brick which only triggers from below for 1UP powerups
	 * TODO - Add repetition to blocks so player can get coins by jumping in the block multiple times (Repetitions must be variable)
	 */
	
	// When player collisions from below the tile, it takes the collision point, transforms it to a cell in grid
	// position and takes the tile position into the powerup tile map. Then the tile in the powerup tilemap is compered
	// with the tiles in the TileData and return true when it finds a match. Then, the stored GameObject in the TileData
	// is instantiate by passing the spawn position and the target postion. Once loaded, the respective prefab has a
	// script attached to it with the behaviour that it should adopt
	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("Player")) return;
		
		// TODO Add Top Collision when player jumps and crouches in air.
		var contact = collision.GetContact(0);
		var isBottomCollision = contact.normal.Equals(Vector2.up);
	
		if (!isBottomCollision) return;
		
		var contactTileCell = _tilemap.WorldToCell(contact.point + Vector2.up * 0.01f);
		var contactTile = _tilemap.GetTile(contactTileCell);
		var isQuestionBlock = false;

		foreach (var tile in questionBlockTile) // Searches if the collision matches the Active QuestionBlock tile
			if (contactTile.Equals(tile))
				isQuestionBlock = true;
		
		//Debug.Log("Is QuestionBlock? " + isQuestionBlock);
		
		if (!isQuestionBlock) return;
		var powerUpTile = powerUpTilemap.GetTile(contactTileCell);
		
		TileData contactTileData = null;
		foreach (var tileData in tileDatas) {	// Searches for a match between tileData tiles and the tile in the powerup tilemap
			if (tileData.tile.Equals(powerUpTile)) {
				contactTileData = tileData;
			}
		}
		if (contactTileData == null) return; // Checks match was found
		var topTileCell = contactTileCell + Vector3Int.up;
		var topWorldPos = _tilemap.GetCellCenterWorld(topTileCell);
		var contactTileWorldPos = _tilemap.GetCellCenterWorld(contactTileCell);

		contactTileData.SpawnTile(contactTileWorldPos, topWorldPos);
		
		_tilemap.SetTile(contactTileCell, usedBlockTile);
		_tilemap.RefreshTile(contactTileCell);
	}
}
