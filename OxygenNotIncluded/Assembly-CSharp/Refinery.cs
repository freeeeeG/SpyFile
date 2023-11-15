using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200067C RID: 1660
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
	// Token: 0x06002C3B RID: 11323 RVA: 0x000EAD40 File Offset: 0x000E8F40
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x02001382 RID: 4994
	[Serializable]
	public struct OrderSaveData
	{
		// Token: 0x0600815D RID: 33117 RVA: 0x002F51F0 File Offset: 0x002F33F0
		public OrderSaveData(string id, bool infinite)
		{
			this.id = id;
			this.infinite = infinite;
		}

		// Token: 0x040062AD RID: 25261
		public string id;

		// Token: 0x040062AE RID: 25262
		public bool infinite;
	}
}
