using System;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class OxyRockConfig : IOreConfig
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0004E4A3 File Offset: 0x0004C6A3
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.OxyRock;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0004E4AA File Offset: 0x0004C6AA
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.Oxygen;
		}
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0004E4B1 File Offset: 0x0004C6B1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0004E4B8 File Offset: 0x0004C6B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.010000001f, 0.0050000004f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
