using System;
using UnityEngine;

// Token: 0x020002B3 RID: 691
public class DirtyWaterConfig : IOreConfig
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0004E150 File Offset: 0x0004C350
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.DirtyWater;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0004E157 File Offset: 0x0004C357
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0004E15E File Offset: 0x0004C35E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0004E168 File Offset: 0x0004C368
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubbleWater;
		sublimates.info = new Sublimates.Info(4.0000006E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
