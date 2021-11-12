using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// Makes bricks bounce if player is in normal mode and breaks blocks if in Super mode
public class BrickController : MonoBehaviour {
	
	public float bounceHeight = 0.5f;
	public float bounceSpeed = 4f;

	public GameObject brick;

	private Tilemap _tilemap;
	
	private Vector2 _originalPosition;
	private TileBase _tile;
	private bool _canBounce = true;

	private void Start() {
		_tilemap = GetComponent<Tilemap>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("Player")) return;

		_canBounce = !PlayerController.IsSuperMode;

		var contact = collision.GetContact(0);
		var contactTileCell = _tilemap.WorldToCell(contact.point + Vector2.up * 0.01f);
		var contactTileWorldPositionCenter = _tilemap.GetCellCenterWorld(contactTileCell);
		
		//Debug.Log("Contact normal: " + contact.normal);
		
		var contactBottom = contact.normal.Equals(Vector2.up);
		var contactTop = contact.normal.Equals(Vector2.down);
		
		//_canBounce = false;  //Debug process
		if (!contactBottom) return;
		if (!_canBounce) {
			//if (!contactBottom && !contactTop) return; // Needs crouching detction to ensure that it is not a simple collision but a breaking action
			StartCoroutine(DestroyBrick(contactTileCell));
			return;
		}
		_originalPosition = contactTileWorldPositionCenter;
		
		_tile = _tilemap.GetTile(contactTileCell);
		
		_tilemap.SetTile(contactTileCell, null);
		_tilemap.RefreshTile(contactTileCell);
		
		GameObject brickClone = Instantiate(brick, contactTileWorldPositionCenter, Quaternion.identity);
		brickClone.transform.parent = transform;
		
		StartCoroutine(BounceUp(brickClone.transform));
	}
	// TODO Add BounceDown corutine when player jumps and crouches and Player is not in SuperMode
	
	// Small bounce mechanic that makes the block move upwards for a small height and downwards to spawn position. Then,
	// the GameObject is destroyed and the tile is placed again in the tilemap.
	IEnumerator BounceUp (Transform brickCloneTransform) {
		var localPosition = brickCloneTransform.localPosition;
		while (true) {
			localPosition = new Vector2(localPosition.x,
			                            localPosition.y +
			                            bounceSpeed * Time.deltaTime);
			
			brickCloneTransform.localPosition = localPosition;
			if (brickCloneTransform.localPosition.y >= _originalPosition.y + bounceHeight)
				break;
			yield return null;
		}

		while (true) {
			localPosition = new Vector2(localPosition.x,
			                            localPosition.y -
			                            bounceSpeed * Time.deltaTime);
			brickCloneTransform.localPosition = localPosition;
			if (brickCloneTransform.localPosition.y <= _originalPosition.y)
				break;
			yield return null;
		}
		Destroy(brickCloneTransform.gameObject);
		var worldToCell = _tilemap.WorldToCell(_originalPosition);
		_tilemap.SetTile(worldToCell, _tile);
		_tilemap.RefreshTile(worldToCell);
		_canBounce = true;
	}

	IEnumerator DestroyBrick(Vector3Int cellPos) {
		_tilemap.SetTile(cellPos, null);
		_tilemap.RefreshTile(cellPos);
		// TODO Add animation when Brick breaks.
		yield return null;
	}
}
