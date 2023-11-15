using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class KBoxCollider2D : KCollider2D
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00093F56 File Offset: 0x00092156
	// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00093F5E File Offset: 0x0009215E
	public Vector2 size
	{
		get
		{
			return this._size;
		}
		set
		{
			this._size = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x00093F70 File Offset: 0x00092170
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = this.size * 0.9999f;
		Vector2 vector3 = new Vector2(vector.x - vector2.x * 0.5f, vector.y - vector2.y * 0.5f);
		Vector2 vector4 = new Vector2(vector.x + vector2.x * 0.5f, vector.y + vector2.y * 0.5f);
		Vector2I vector2I = new Vector2I((int)vector3.x, (int)vector3.y);
		Vector2I vector2I2 = new Vector2I((int)vector4.x, (int)vector4.y);
		int width = vector2I2.x - vector2I.x + 1;
		int height = vector2I2.y - vector2I.y + 1;
		return new Extents(vector2I.x, vector2I.y, width, height);
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x0009407C File Offset: 0x0009227C
	public override bool Intersects(Vector2 intersect_pos)
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.size.x * 0.5f, vector.y - this.size.y * 0.5f);
		Vector2 vector3 = new Vector2(vector.x + this.size.x * 0.5f, vector.y + this.size.y * 0.5f);
		return intersect_pos.x >= vector2.x && intersect_pos.x <= vector3.x && intersect_pos.y >= vector2.y && intersect_pos.y <= vector3.y;
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00094168 File Offset: 0x00092368
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._size.x, this._size.y, 0f));
		}
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x000941CC File Offset: 0x000923CC
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0f));
	}

	// Token: 0x04000F61 RID: 3937
	[SerializeField]
	private Vector2 _size;
}
