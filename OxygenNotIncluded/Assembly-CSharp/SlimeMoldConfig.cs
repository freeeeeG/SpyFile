using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class SlimeMoldConfig : IOreConfig
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000E2F RID: 3631 RVA: 0x0004E514 File Offset: 0x0004C714
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.SlimeMold;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0004E51B File Offset: 0x0004C71B
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0004E522 File Offset: 0x0004C722
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0004E52C File Offset: 0x0004C72C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
