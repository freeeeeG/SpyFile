using System;
using System.Collections;
using flanne.Player;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x0200014F RID: 335
	public class GrowthRune : Rune
	{
		// Token: 0x060008A6 RID: 2214 RVA: 0x0002451E File Offset: 0x0002271E
		private void OnLevelChanged(int level)
		{
			if (level % this.levelPerHPDrop == 0)
			{
				base.StartCoroutine(this.WaitToDropHP());
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00024538 File Offset: 0x00022738
		protected override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.hpPrefab.name, this.hpPrefab, 100, true);
			this.playerXP = this.player.GetComponentInChildren<PlayerXP>();
			this.playerXP.xpMultiplier.AddMultiplierBonus(this.xpMultiplierPerLevel * (float)this.level);
			this.playerXP.OnLevelChanged.AddListener(new UnityAction<int>(this.OnLevelChanged));
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000245BA File Offset: 0x000227BA
		private void OnDestroy()
		{
			this.playerXP.OnLevelChanged.RemoveListener(new UnityAction<int>(this.OnLevelChanged));
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x000245D8 File Offset: 0x000227D8
		private IEnumerator WaitToDropHP()
		{
			yield return new WaitForSeconds(0.1f);
			GameObject pooledObject = this.OP.GetPooledObject(this.hpPrefab.name);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
			yield break;
		}

		// Token: 0x04000667 RID: 1639
		[SerializeField]
		private float xpMultiplierPerLevel;

		// Token: 0x04000668 RID: 1640
		[SerializeField]
		private int levelPerHPDrop;

		// Token: 0x04000669 RID: 1641
		[SerializeField]
		private GameObject hpPrefab;

		// Token: 0x0400066A RID: 1642
		private ObjectPooler OP;

		// Token: 0x0400066B RID: 1643
		private PlayerXP playerXP;
	}
}
