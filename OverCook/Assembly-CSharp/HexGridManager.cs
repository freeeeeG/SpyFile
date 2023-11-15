using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class HexGridManager : GridManager
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x0600180F RID: 6159 RVA: 0x0007A67C File Offset: 0x00078A7C
	public float HexRadius
	{
		get
		{
			return this.m_hexRadius;
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x0007A684 File Offset: 0x00078A84
	public Vector3 Origin()
	{
		return this.m_origin;
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x0007A68C File Offset: 0x00078A8C
	public override Vector3 GetPosFromGridLocation(GridIndex _index)
	{
		return base.transform.TransformPoint(this.m_origin + new Vector3(this.m_hexRadius * (float)_index.X * 3f / 2f, (float)_index.Y * this.m_gridSizeY, this.m_hexRadius * ((float)_index.X * 1.7320508f / 2f + 1.7320508f * (float)_index.Z)));
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x0007A708 File Offset: 0x00078B08
	protected Vector3 GetLocalPosFromGridXYZ(float x, float y, float z)
	{
		return new Vector3(this.m_hexRadius * x * 3f / 2f, y * this.m_gridSizeY, this.m_hexRadius * (x * 1.7320508f / 2f + 1.7320508f * z));
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x0007A748 File Offset: 0x00078B48
	public override GridIndex GetUnclampedGridLocationFromPos(Vector3 _pos)
	{
		Vector3 b = base.transform.InverseTransformPoint(_pos) - this.m_origin;
		float num = Mathf.Round(b.y / this.m_gridSizeY);
		float f = 2f * b.x / (3f * this.m_hexRadius);
		float f2 = (b.z - b.x / 1.7320508f) / (1.7320508f * this.m_hexRadius);
		float num2 = Mathf.Floor(f);
		float num3 = Mathf.Ceil(f);
		float num4 = Mathf.Floor(f2);
		float num5 = Mathf.Ceil(f2);
		Vector3 localPosFromGridXYZ = this.GetLocalPosFromGridXYZ(num2, num, num4);
		Vector3 localPosFromGridXYZ2 = this.GetLocalPosFromGridXYZ(num3, num, num4);
		Vector3 localPosFromGridXYZ3 = this.GetLocalPosFromGridXYZ(num2, num, num5);
		Vector3 localPosFromGridXYZ4 = this.GetLocalPosFromGridXYZ(num3, num, num5);
		float sqrMagnitude = (localPosFromGridXYZ - b).sqrMagnitude;
		float sqrMagnitude2 = (localPosFromGridXYZ2 - b).sqrMagnitude;
		float sqrMagnitude3 = (localPosFromGridXYZ3 - b).sqrMagnitude;
		float sqrMagnitude4 = (localPosFromGridXYZ4 - b).sqrMagnitude;
		float num6 = (Mathf.Min(sqrMagnitude, sqrMagnitude3) >= Mathf.Min(sqrMagnitude2, sqrMagnitude4)) ? num3 : num2;
		float num7 = (Mathf.Min(sqrMagnitude, sqrMagnitude2) >= Mathf.Min(sqrMagnitude3, sqrMagnitude4)) ? num5 : num4;
		GridIndex result = new GridIndex((int)num6, (int)num, (int)num7);
		return result;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x0007A8B4 File Offset: 0x00078CB4
	public static int ComputeDistanceHexGrid(GridIndex A, GridIndex B)
	{
		Point2 point = new Point2(0, 0);
		point.X = A.Z - B.Z;
		point.Y = -(A.X - B.X);
		Point2 point2 = new Point2(0, 0);
		int num = (Mathf.Abs(point.X) >= Mathf.Abs(point.Y)) ? Mathf.Abs(point.Y) : Mathf.Abs(point.X);
		point2.X = ((point.X >= 0) ? num : (-num));
		point2.Y = ((point.Y >= 0) ? num : (-num));
		Point2 point3 = new Point2(0, 0);
		point3.X = point.X - point2.X;
		point3.Y = point.Y - point2.Y;
		int num2 = Mathf.Abs(point3.X) + Mathf.Abs(point3.Y);
		int num3 = Mathf.Abs(point2.X);
		if ((point2.X < 0 && point2.Y > 0) || (point2.X > 0 && point2.Y < 0))
		{
			num3 *= 2;
		}
		return num2 + num3;
	}

	// Token: 0x0400135F RID: 4959
	[SerializeField]
	private Vector3 m_origin;

	// Token: 0x04001360 RID: 4960
	[SerializeField]
	private float m_hexRadius = 1f;

	// Token: 0x04001361 RID: 4961
	[SerializeField]
	private float m_gridSizeY = 1f;

	// Token: 0x04001362 RID: 4962
	private const float c_root3 = 1.7320508f;
}
