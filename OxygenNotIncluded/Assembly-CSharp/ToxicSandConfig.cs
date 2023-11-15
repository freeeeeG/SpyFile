using System;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class ToxicSandConfig : IOreConfig
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0004E588 File Offset: 0x0004C788
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.ToxicSand;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0004E58F File Offset: 0x0004C78F
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0004E596 File Offset: 0x0004C796
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0004E5A0 File Offset: 0x0004C7A0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(2.0000001E-05f, 0.05f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
