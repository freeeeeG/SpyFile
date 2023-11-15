using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class EmptyContent : GameTileContent
{
	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0001F4FC File Offset: 0x0001D6FC
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.Empty;
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x0001F500 File Offset: 0x0001D700
	public override void ContentLanded()
	{
		base.ContentLanded();
		Collider2D col = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.ConcreteTileMask
		}));
		this.ContentLandedCheck(col);
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x0001F548 File Offset: 0x0001D748
	public override void SaveContent(out ContentStruct contentStruct)
	{
		base.SaveContent(out contentStruct);
		contentStruct = this.m_ContentStruct;
		this.m_ContentStruct.ContentName = "Empty";
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x0001F569 File Offset: 0x0001D769
	protected override void ContentLandedCheck(Collider2D col)
	{
		if (col != null)
		{
			Singleton<ObjectPool>.Instance.UnSpawn(base.m_GameTile);
			return;
		}
		StaticData.SetNodeWalkable(base.m_GameTile, true, true);
	}
}
