using System;
using GameModes;
using UnityEngine;

// Token: 0x02000BA5 RID: 2981
[ExecutionDependency(typeof(BootstrapManager))]
public class HordeLevelPortalMapNode : LevelPortalMapNode
{
	// Token: 0x06003D0B RID: 15627 RVA: 0x00124464 File Offset: 0x00122864
	public override WorldMapLevelIconUI GetUIPrefab(Kind _kind)
	{
		return this.m_uiPrefab;
	}

	// Token: 0x04003123 RID: 12579
	[AssignResource("horde_world_map_level_preview", Editorbility.Editable)]
	[SerializeField]
	private WorldMapLevelIconUI m_uiPrefab;
}
