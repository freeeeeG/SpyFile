using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009E7 RID: 2535
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Spawner")]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004BB9 RID: 19385 RVA: 0x001A9304 File Offset: 0x001A7504
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell(this));
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0400316A RID: 12650
	[Serialize]
	public Tag prefabTag;

	// Token: 0x0400316B RID: 12651
	[Serialize]
	public int units = 1;
}
