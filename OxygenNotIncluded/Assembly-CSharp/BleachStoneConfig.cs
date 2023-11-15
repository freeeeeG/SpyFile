using System;
using UnityEngine;

// Token: 0x020002B2 RID: 690
public class BleachStoneConfig : IOreConfig
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0004E0DE File Offset: 0x0004C2DE
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.BleachStone;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0004E0E5 File Offset: 0x0004C2E5
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ChlorineGas;
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0004E0EC File Offset: 0x0004C2EC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0004E0F4 File Offset: 0x0004C2F4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.00020000001f, 0.0025000002f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
