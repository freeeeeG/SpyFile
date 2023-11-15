using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class TurretBaseContent : GameTileContent
{
	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06000C60 RID: 3168 RVA: 0x000203DB File Offset: 0x0001E5DB
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.TurretBase;
		}
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x000203DE File Offset: 0x0001E5DE
	public override void ContentLanded()
	{
		base.ContentLanded();
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x000203E6 File Offset: 0x0001E5E6
	public override void OnContentSelected(bool value)
	{
		base.OnContentSelected(value);
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x000203EF File Offset: 0x0001E5EF
	public override void CorretRotation()
	{
		base.CorretRotation();
	}

	// Token: 0x0400062D RID: 1581
	[HideInInspector]
	public TurretBaseAttribute m_TurretBaseAttribute;

	// Token: 0x0400062E RID: 1582
	public bool needReset;
}
