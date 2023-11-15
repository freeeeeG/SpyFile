using System;
using GameModes;
using UnityEngine;

// Token: 0x02000BB3 RID: 2995
public class MiniLevelPortalMapNode : PortalMapNode
{
	// Token: 0x06003D42 RID: 15682 RVA: 0x00124F01 File Offset: 0x00123301
	public override WorldMapLevelIconUI GetUIPrefab(Kind _kind)
	{
		return this.m_uiPrefab;
	}

	// Token: 0x0400313E RID: 12606
	[SerializeField]
	[AssignResource("WorldMapMiniLevelIconUI", Editorbility.Editable)]
	public WorldMapLevelIconUI m_uiPrefab;
}
