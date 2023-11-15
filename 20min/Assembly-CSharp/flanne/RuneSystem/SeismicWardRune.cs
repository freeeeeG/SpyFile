using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000158 RID: 344
	public class SeismicWardRune : Rune
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x00024E3C File Offset: 0x0002303C
		protected override void Init()
		{
			AttackingSummon attackingSummon = this.AttachSeismicWard(new Vector3(0f, 1f, 0f));
			AttackingSummon attackingSummon2 = this.AttachSeismicWard(new Vector3(0f, -1f, 0f));
			attackingSummon.attackSpeedMod.AddFlatBonus(this.cdrPerLevel * this.level);
			attackingSummon2.attackSpeedMod.AddFlatBonus(this.cdrPerLevel * this.level);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00024EAD File Offset: 0x000230AD
		private AttackingSummon AttachSeismicWard(Vector3 localPosition)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.seismicWardPrefab);
			gameObject.transform.SetParent(this.player.transform);
			gameObject.transform.localPosition = localPosition;
			gameObject.SetActive(true);
			return gameObject.GetComponent<AttackingSummon>();
		}

		// Token: 0x0400068E RID: 1678
		[SerializeField]
		private GameObject seismicWardPrefab;

		// Token: 0x0400068F RID: 1679
		[SerializeField]
		private int cdrPerLevel;
	}
}
