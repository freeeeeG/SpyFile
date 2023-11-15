using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class AlgaeConfig : IOreConfig
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0004E0AB File Offset: 0x0004C2AB
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Algae;
		}
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x0004E0B2 File Offset: 0x0004C2B2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0004E0B9 File Offset: 0x0004C2B9
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>
		{
			GameTags.Life
		});
	}
}
