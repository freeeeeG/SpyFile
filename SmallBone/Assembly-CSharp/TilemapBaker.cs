using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

// Token: 0x02000059 RID: 89
public class TilemapBaker : MonoBehaviour
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007C17 File Offset: 0x00005E17
	public Bounds bounds
	{
		get
		{
			return this._bounds;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00007C20 File Offset: 0x00005E20
	private void Bake(CustomColliderTile.ColliderFilter filter, int layer)
	{
		CustomColliderTile.colliderFilter = filter;
		this._tilemapCollider.usedByComposite = false;
		this._tilemap.RefreshAllTiles();
		if (this._tilemapCollider.bounds.size != Vector3.zero)
		{
			if (this._bounds.size == Vector3.zero)
			{
				this._bounds = this._tilemapCollider.bounds;
			}
			else
			{
				this._bounds.max = Vector3.Max(this._bounds.max, this._tilemapCollider.bounds.max);
				this._bounds.min = Vector3.Min(this._bounds.min, this._tilemapCollider.bounds.min);
			}
		}
		this._tilemapCollider.usedByComposite = true;
		this._compositeCollider.GenerateGeometry();
		GameObject gameObject = new GameObject(filter.ToString());
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.layer = layer;
		gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		for (int i = 0; i < this._compositeCollider.shapeCount; i++)
		{
			int path = this._compositeCollider.GetPath(i, TilemapBaker._pathesCache);
			new GameObject(filter.ToString())
			{
				transform = 
				{
					parent = gameObject.transform,
					position = Vector3.zero
				},
				layer = layer
			}.AddComponent<PolygonCollider2D>().points = TilemapBaker._pathesCache.Take(path).ToArray<Vector2>();
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00007DCC File Offset: 0x00005FCC
	private void FillPadding(int amount)
	{
		TilemapBaker.<>c__DisplayClass9_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.amount = amount;
		this._tilemap.CompressBounds();
		Bounds localBounds = this._tilemap.localBounds;
		Vector2Int vector2Int = new Vector2Int((int)localBounds.min.x, (int)localBounds.min.y);
		Vector2Int vector2Int2 = new Vector2Int((int)localBounds.max.x - 1, (int)localBounds.max.y - 1);
		for (int i = vector2Int.x; i <= vector2Int2.x; i++)
		{
			this.<FillPadding>g__Fill|9_0(new Vector3Int(i, vector2Int.y, 0), Vector3Int.down, (CustomColliderTile t) => t.verticallyOpened, ref CS$<>8__locals1);
			this.<FillPadding>g__Fill|9_0(new Vector3Int(i, vector2Int2.y, 0), Vector3Int.up, (CustomColliderTile t) => t.verticallyOpened, ref CS$<>8__locals1);
		}
		for (int j = vector2Int.y; j <= vector2Int2.y; j++)
		{
			this.<FillPadding>g__Fill|9_0(new Vector3Int(vector2Int.x, j, 0), Vector3Int.left, (CustomColliderTile t) => t.horizontallyOpened, ref CS$<>8__locals1);
			this.<FillPadding>g__Fill|9_0(new Vector3Int(vector2Int2.x, j, 0), Vector3Int.right, (CustomColliderTile t) => t.horizontallyOpened, ref CS$<>8__locals1);
		}
		this.<FillPadding>g__FillCorners|9_1(new Vector3Int(vector2Int.x, vector2Int.y, 0), -1, -1, ref CS$<>8__locals1);
		this.<FillPadding>g__FillCorners|9_1(new Vector3Int(vector2Int2.x, vector2Int.y, 0), 1, -1, ref CS$<>8__locals1);
		this.<FillPadding>g__FillCorners|9_1(new Vector3Int(vector2Int.x, vector2Int2.y, 0), -1, 1, ref CS$<>8__locals1);
		this.<FillPadding>g__FillCorners|9_1(new Vector3Int(vector2Int2.x, vector2Int2.y, 0), 1, 1, ref CS$<>8__locals1);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00007FE8 File Offset: 0x000061E8
	public void Bake()
	{
		this._rigidbody.bodyType = RigidbodyType2D.Static;
		this._compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
		this.Bake(CustomColliderTile.ColliderFilter.Terrain, 8);
		this.Bake(CustomColliderTile.ColliderFilter.TerrainFoothold, 18);
		this.Bake(CustomColliderTile.ColliderFilter.PlatformProjectileBlock, 19);
		this.Bake(CustomColliderTile.ColliderFilter.PlatformFoothold, 17);
		this.FillPadding(3);
		UnityEngine.Object.Destroy(this._compositeCollider);
		UnityEngine.Object.Destroy(this._tilemapCollider);
		UnityEngine.Object.Destroy(this._rigidbody);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000806C File Offset: 0x0000626C
	[CompilerGenerated]
	private void <FillPadding>g__Fill|9_0(Vector3Int point, Vector3Int direction, Func<CustomColliderTile, bool> tileFilter, ref TilemapBaker.<>c__DisplayClass9_0 A_4)
	{
		A_4.tile = this._tilemap.GetTile<CustomColliderTile>(point);
		if (A_4.tile != null && tileFilter(A_4.tile))
		{
			for (int i = 1; i <= A_4.amount; i++)
			{
				this._tilemap.SetTile(point + direction * i, A_4.tile);
			}
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000080DC File Offset: 0x000062DC
	[CompilerGenerated]
	private void <FillPadding>g__FillCorners|9_1(Vector3Int point, int xDirection, int yDirection, ref TilemapBaker.<>c__DisplayClass9_0 A_4)
	{
		A_4.tile = this._tilemap.GetTile<CustomColliderTile>(point);
		if (A_4.tile != null && A_4.tile.position == CustomColliderTile.Position.Inner)
		{
			for (int i = 1; i <= A_4.amount; i++)
			{
				for (int j = 1; j <= A_4.amount; j++)
				{
					this._tilemap.SetTile(new Vector3Int(point.x + xDirection * i, point.y + yDirection * j, 0), A_4.tile);
				}
			}
		}
	}

	// Token: 0x0400016D RID: 365
	private static readonly List<Vector2> _pathesCache = new List<Vector2>(32);

	// Token: 0x0400016E RID: 366
	[GetComponent]
	[SerializeField]
	private Tilemap _tilemap;

	// Token: 0x0400016F RID: 367
	[GetComponent]
	[SerializeField]
	private TilemapCollider2D _tilemapCollider;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	[GetComponent]
	private CompositeCollider2D _compositeCollider;

	// Token: 0x04000171 RID: 369
	[GetComponent]
	[SerializeField]
	private Rigidbody2D _rigidbody;

	// Token: 0x04000172 RID: 370
	private Bounds _bounds;
}
