using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000256 RID: 598
	public class ThunderDuringHolyShield : MonoBehaviour
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x0002F5A8 File Offset: 0x0002D7A8
		private void Start()
		{
			this.TGen = ThunderGenerator.SharedInstance;
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerTransform = componentInParent.transform;
			this.holyShield = componentInParent.GetComponentInChildren<PreventDamage>();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002F5E0 File Offset: 0x0002D7E0
		private void Update()
		{
			if (this.holyShield.isActive)
			{
				this._timer += Time.deltaTime;
			}
			if (this._timer > this.cooldown)
			{
				this._timer -= this.cooldown;
				for (int i = 0; i < this.thundersPerWave; i++)
				{
					GameObject randomEnemy = EnemyFinder.GetRandomEnemy(this.playerTransform.position, this.range);
					if (randomEnemy != null)
					{
						this.TGen.GenerateAt(randomEnemy, this.baseDamage);
					}
				}
			}
		}

		// Token: 0x04000946 RID: 2374
		[SerializeField]
		private int baseDamage;

		// Token: 0x04000947 RID: 2375
		[SerializeField]
		private float cooldown;

		// Token: 0x04000948 RID: 2376
		[SerializeField]
		private int thundersPerWave;

		// Token: 0x04000949 RID: 2377
		[SerializeField]
		private Vector2 range;

		// Token: 0x0400094A RID: 2378
		private ThunderGenerator TGen;

		// Token: 0x0400094B RID: 2379
		private Transform playerTransform;

		// Token: 0x0400094C RID: 2380
		private PreventDamage holyShield;

		// Token: 0x0400094D RID: 2381
		private float _timer;
	}
}
