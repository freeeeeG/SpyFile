using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class Ground : MonoBehaviour
{
	// Token: 0x060008DC RID: 2268 RVA: 0x00018369 File Offset: 0x00016569
	public void SetSize(Vector2Int size)
	{
		this._spriteRenderer.size = size;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0001837C File Offset: 0x0001657C
	public void Extend(Direction direction, int distance)
	{
		switch (direction)
		{
		case Direction.up:
			this._spriteRenderer.size += new Vector2Int(0, distance);
			base.transform.localPosition += new Vector3Int(0, distance / 2, 0);
			return;
		case Direction.right:
			this._spriteRenderer.size += new Vector2Int(distance, 0);
			base.transform.localPosition += new Vector3Int(distance / 2, 0, 0);
			return;
		case Direction.down:
			this._spriteRenderer.size += new Vector2Int(0, distance);
			base.transform.localPosition += new Vector3Int(0, -distance / 2, 0);
			return;
		case Direction.left:
			this._spriteRenderer.size += new Vector2Int(distance, 0);
			base.transform.localPosition += new Vector3Int(-distance / 2, 0, 0);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400049D RID: 1181
	[SerializeField]
	private SpriteRenderer _spriteRenderer;
}
