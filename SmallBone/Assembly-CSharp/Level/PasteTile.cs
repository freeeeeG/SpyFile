using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
	// Token: 0x02000504 RID: 1284
	public class PasteTile : MonoBehaviour
	{
		// Token: 0x06001951 RID: 6481 RVA: 0x0004F6C4 File Offset: 0x0004D8C4
		public void Paste(Tilemap to)
		{
			TileBase[] tilesBlock = this._original.GetTilesBlock(this._original.cellBounds);
			for (int i = 0; i < tilesBlock.Length; i++)
			{
				TileBase tileBase = tilesBlock[i];
				if (!(tileBase == null))
				{
					int x = this._original.cellBounds.size.x;
					Vector3Int origin = this._original.origin;
					origin.x += i % x;
					origin.y += i / x;
					to.SetTile(origin, tileBase);
				}
			}
		}

		// Token: 0x04001613 RID: 5651
		[SerializeField]
		private Tilemap _original;
	}
}
