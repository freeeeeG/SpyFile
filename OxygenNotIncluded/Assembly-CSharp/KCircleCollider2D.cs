using System;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class KCircleCollider2D : KCollider2D
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06001BAE RID: 7086 RVA: 0x0009421E File Offset: 0x0009241E
	// (set) Token: 0x06001BAF RID: 7087 RVA: 0x00094226 File Offset: 0x00092426
	public float radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00094238 File Offset: 0x00092438
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.radius, vector.y - this.radius);
		Vector2 vector3 = new Vector2(vector.x + this.radius, vector.y + this.radius);
		int width = (int)vector3.x - (int)vector2.x + 1;
		int height = (int)vector3.y - (int)vector2.y + 1;
		return new Extents((int)(vector.x - this._radius), (int)(vector.y - this._radius), width, height);
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x000942FC File Offset: 0x000924FC
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._radius * 2f, this._radius * 2f, 0f));
		}
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x00094360 File Offset: 0x00092560
	public override bool Intersects(Vector2 pos)
	{
		Vector3 position = base.transform.GetPosition();
		Vector2 b = new Vector2(position.x, position.y) + base.offset;
		return (pos - b).sqrMagnitude <= this._radius * this._radius;
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x000943B8 File Offset: 0x000925B8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.bounds.center, this.radius);
	}

	// Token: 0x04000F62 RID: 3938
	[SerializeField]
	private float _radius;
}
