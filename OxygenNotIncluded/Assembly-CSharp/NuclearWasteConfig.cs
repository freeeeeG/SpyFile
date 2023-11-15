using System;
using UnityEngine;

// Token: 0x020002B6 RID: 694
public class NuclearWasteConfig : IOreConfig
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0004E431 File Offset: 0x0004C631
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.NuclearWaste;
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0004E438 File Offset: 0x0004C638
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0004E440 File Offset: 0x0004C640
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.decayStorage = true;
		sublimates.spawnFXHash = SpawnFXHashes.NuclearWasteDrip;
		sublimates.info = new Sublimates.Info(0.066f, 6.6f, 1000f, 0f, this.ElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
