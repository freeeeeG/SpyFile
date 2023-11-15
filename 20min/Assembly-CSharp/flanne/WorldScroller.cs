using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000142 RID: 322
	public class WorldScroller : MonoBehaviour
	{
		// Token: 0x06000864 RID: 2148 RVA: 0x00023543 File Offset: 0x00021743
		private void Start()
		{
			this.player = PlayerController.Instance.transform;
			this._currentTile = this.GetCurrentTile();
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00023564 File Offset: 0x00021764
		private void FixedUpdate()
		{
			if (!this.IsPlayerOnTile(this._currentTile))
			{
				this._currentTile = this.GetCurrentTile();
			}
			WorldScroller.Quadrant quadrant = this.GetQuadrant(this._currentTile);
			if (this._currentQuadrant != quadrant)
			{
				this._currentQuadrant = quadrant;
				List<Transform> list = new List<Transform>(this.tiles);
				list.Remove(this._currentTile);
				switch (quadrant)
				{
				case WorldScroller.Quadrant.TopLeft:
					list[0].position = new Vector3(this._currentTile.position.x - this.tileSize, this._currentTile.position.y, 0f);
					list[1].position = new Vector3(this._currentTile.position.x, this._currentTile.position.y + this.tileSize, 0f);
					list[2].position = new Vector3(this._currentTile.position.x - this.tileSize, this._currentTile.position.y + this.tileSize, 0f);
					return;
				case WorldScroller.Quadrant.TopRight:
					list[0].position = new Vector3(this._currentTile.position.x + this.tileSize, this._currentTile.position.y, 0f);
					list[1].position = new Vector3(this._currentTile.position.x, this._currentTile.position.y + this.tileSize, 0f);
					list[2].position = new Vector3(this._currentTile.position.x + this.tileSize, this._currentTile.position.y + this.tileSize, 0f);
					return;
				case WorldScroller.Quadrant.BotLeft:
					list[0].position = new Vector3(this._currentTile.position.x - this.tileSize, this._currentTile.position.y, 0f);
					list[1].position = new Vector3(this._currentTile.position.x, this._currentTile.position.y - this.tileSize, 0f);
					list[2].position = new Vector3(this._currentTile.position.x - this.tileSize, this._currentTile.position.y - this.tileSize, 0f);
					return;
				case WorldScroller.Quadrant.BotRight:
					list[0].position = new Vector3(this._currentTile.position.x + this.tileSize, this._currentTile.position.y, 0f);
					list[1].position = new Vector3(this._currentTile.position.x, this._currentTile.position.y - this.tileSize, 0f);
					list[2].position = new Vector3(this._currentTile.position.x + this.tileSize, this._currentTile.position.y - this.tileSize, 0f);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000238D8 File Offset: 0x00021AD8
		private WorldScroller.Quadrant GetQuadrant(Transform tile)
		{
			Vector3 position = this.player.position;
			Vector3 position2 = tile.position;
			if (position.x < position2.x && position.y < position2.y)
			{
				return WorldScroller.Quadrant.BotLeft;
			}
			if (position.x < position2.x && position.y >= position2.y)
			{
				return WorldScroller.Quadrant.TopLeft;
			}
			if (position.x >= position2.x && position.y < position2.y)
			{
				return WorldScroller.Quadrant.BotRight;
			}
			return WorldScroller.Quadrant.TopRight;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00023954 File Offset: 0x00021B54
		private bool IsPlayerOnTile(Transform tile)
		{
			Vector3 position = this.player.position;
			Vector3 position2 = tile.position;
			return position2.x - this.tileSize / 2f < position.x && position2.x + this.tileSize / 2f > position.x && position2.y - this.tileSize / 2f < position.y && position2.y + this.tileSize / 2f > position.y;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000239E4 File Offset: 0x00021BE4
		private Transform GetCurrentTile()
		{
			for (int i = 0; i < this.tiles.Count; i++)
			{
				if (this.IsPlayerOnTile(this.tiles[i]))
				{
					return this.tiles[i];
				}
			}
			Debug.LogError("Player is not on any current tile");
			return this.tiles[0];
		}

		// Token: 0x04000632 RID: 1586
		[SerializeField]
		private float tileSize = 32f;

		// Token: 0x04000633 RID: 1587
		[SerializeField]
		private List<Transform> tiles = new List<Transform>(4);

		// Token: 0x04000634 RID: 1588
		private Transform player;

		// Token: 0x04000635 RID: 1589
		private WorldScroller.Quadrant _currentQuadrant;

		// Token: 0x04000636 RID: 1590
		private Transform _currentTile;

		// Token: 0x020002CE RID: 718
		private enum Quadrant
		{
			// Token: 0x04000B10 RID: 2832
			TopLeft,
			// Token: 0x04000B11 RID: 2833
			TopRight,
			// Token: 0x04000B12 RID: 2834
			BotLeft,
			// Token: 0x04000B13 RID: 2835
			BotRight
		}
	}
}
