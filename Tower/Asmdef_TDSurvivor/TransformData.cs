using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class TransformData
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0002C946 File Offset: 0x0002AB46
	public Vector3 Position
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0002C94E File Offset: 0x0002AB4E
	public Quaternion Rotation
	{
		get
		{
			return this.rotation;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0002C956 File Offset: 0x0002AB56
	public Vector3 Scale
	{
		get
		{
			return this.scale;
		}
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0002C95E File Offset: 0x0002AB5E
	public TransformData(Transform transform)
	{
		this.SaveData(transform);
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0002C96D File Offset: 0x0002AB6D
	public void SaveData(Transform from)
	{
		this.position = from.localPosition;
		this.rotation = from.localRotation;
		this.scale = from.localScale;
	}

	// Token: 0x0400091A RID: 2330
	private Vector3 position;

	// Token: 0x0400091B RID: 2331
	private Quaternion rotation;

	// Token: 0x0400091C RID: 2332
	private Vector3 scale;
}
