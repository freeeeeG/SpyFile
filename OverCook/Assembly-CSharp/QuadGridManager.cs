using System;
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class QuadGridManager : GridManager
{
	// Token: 0x0600185E RID: 6238 RVA: 0x0007BE79 File Offset: 0x0007A279
	public Vector3 Origin()
	{
		return this.m_origin;
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x0600185F RID: 6239 RVA: 0x0007BE81 File Offset: 0x0007A281
	// (set) Token: 0x06001860 RID: 6240 RVA: 0x0007BE89 File Offset: 0x0007A289
	public Vector3 AccessOrigin
	{
		get
		{
			return this.m_origin;
		}
		set
		{
			this.m_origin = value;
		}
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x0007BE94 File Offset: 0x0007A294
	public override Vector3 GetPosFromGridLocation(GridIndex _index)
	{
		return base.transform.TransformPoint(this.m_origin + new Vector3((float)_index.X * this.m_size.x, (float)_index.Y * this.m_size.y, (float)_index.Z * this.m_size.z));
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x0007BEF8 File Offset: 0x0007A2F8
	public override GridIndex GetUnclampedGridLocationFromPos(Vector3 _pos)
	{
		Vector3 vector = base.transform.InverseTransformPoint(_pos) - this.m_origin;
		Vector3 vector2 = new Vector3(Mathf.Round(vector.x / this.m_size.x), Mathf.Round(vector.y / this.m_size.y), Mathf.Round(vector.z / this.m_size.z));
		GridIndex result = new GridIndex((int)vector2.x, (int)vector2.y, (int)vector2.z);
		return result;
	}

	// Token: 0x0400139F RID: 5023
	[SerializeField]
	private Vector3 m_origin;

	// Token: 0x040013A0 RID: 5024
	[SerializeField]
	private Vector3 m_size = Vector3.one;
}
