using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F2 RID: 242
	public class PerkTest : MonoBehaviour
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0001EBDD File Offset: 0x0001CDDD
		private void AddPerk()
		{
			this.perk.Apply(this.player);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		private void SerializePerk()
		{
			Debug.Log(JsonUtility.ToJson(this.perk));
		}

		// Token: 0x040004C3 RID: 1219
		[SerializeField]
		private PlayerController player;

		// Token: 0x040004C4 RID: 1220
		[SerializeField]
		private Powerup perk;
	}
}
