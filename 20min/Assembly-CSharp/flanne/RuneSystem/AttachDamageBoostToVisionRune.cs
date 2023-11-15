using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000145 RID: 325
	public class AttachDamageBoostToVisionRune : Rune
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x00023BE4 File Offset: 0x00021DE4
		protected override void Init()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerVision");
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.damageBoostPrefab.gameObject);
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.SetActive(true);
			gameObject2.GetComponent<DamageBoostInRange>().damageBoost = this.damageBoostPerLevel * (float)this.level;
		}

		// Token: 0x0400063F RID: 1599
		[SerializeField]
		private DamageBoostInRange damageBoostPrefab;

		// Token: 0x04000640 RID: 1600
		[SerializeField]
		private float damageBoostPerLevel;
	}
}
