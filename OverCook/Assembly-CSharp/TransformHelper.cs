using System;
using UnityEngine;

// Token: 0x0200026A RID: 618
public struct TransformHelper
{
	// Token: 0x06000B68 RID: 2920 RVA: 0x0003D0E4 File Offset: 0x0003B4E4
	public TransformHelper(Transform t)
	{
		this = new TransformHelper(t.rotation, t.position);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0003D0F8 File Offset: 0x0003B4F8
	public TransformHelper(Quaternion rot, Vector3 pos)
	{
		this.m_rotation = rot;
		this.m_position = pos;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0003D108 File Offset: 0x0003B508
	// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0003D110 File Offset: 0x0003B510
	public Quaternion Rotation
	{
		get
		{
			return this.m_rotation;
		}
		set
		{
			this.m_rotation = value;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0003D119 File Offset: 0x0003B519
	// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0003D121 File Offset: 0x0003B521
	public Vector3 Position
	{
		get
		{
			return this.m_position;
		}
		set
		{
			this.m_position = value;
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0003D12A File Offset: 0x0003B52A
	public Vector3 ToWorldDir(Vector3 _dir)
	{
		return this.m_rotation * _dir;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0003D138 File Offset: 0x0003B538
	public Vector3 F()
	{
		return this.ToWorldDir(new Vector3(0f, 0f, 1f));
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0003D154 File Offset: 0x0003B554
	public Vector3 U()
	{
		return this.ToWorldDir(new Vector3(0f, 1f, 0f));
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0003D170 File Offset: 0x0003B570
	public Vector3 R()
	{
		return this.ToWorldDir(new Vector3(1f, 0f, 0f));
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0003D18C File Offset: 0x0003B58C
	public Vector3 ToLocalDir(Vector3 _dir)
	{
		return TransformHelper.Inverted(this.m_rotation) * _dir;
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0003D19F File Offset: 0x0003B59F
	public Vector3 ToWorldPos(Vector3 _pos)
	{
		return this.ToWorldDir(_pos) + this.m_position;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0003D1B3 File Offset: 0x0003B5B3
	public Vector3 ToLocalPos(Vector3 _pos)
	{
		return this.ToLocalDir(_pos - this.m_position);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0003D1C7 File Offset: 0x0003B5C7
	public TransformHelper ToWorld(TransformHelper _trans)
	{
		return new TransformHelper(this.m_rotation * _trans.Rotation, this.m_position + this.m_rotation * _trans.Position);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0003D1FD File Offset: 0x0003B5FD
	public TransformHelper ToLocal(TransformHelper _trans)
	{
		return new TransformHelper(TransformHelper.Inverted(this.m_rotation) * _trans.Rotation, TransformHelper.Inverted(this.m_rotation) * (_trans.Position - this.m_position));
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0003D23D File Offset: 0x0003B63D
	public static Quaternion Inverted(Quaternion q)
	{
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	// Token: 0x040008DA RID: 2266
	private Quaternion m_rotation;

	// Token: 0x040008DB RID: 2267
	private Vector3 m_position;

	// Token: 0x040008DC RID: 2268
	public static TransformHelper Indentity = new TransformHelper(Quaternion.identity, Vector3.zero);
}
