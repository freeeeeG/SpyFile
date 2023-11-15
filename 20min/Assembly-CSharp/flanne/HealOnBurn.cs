using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E6 RID: 230
	public class HealOnBurn : MonoBehaviour
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x0001E544 File Offset: 0x0001C744
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerHealth = componentInParent.playerHealth;
			this.AddObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001E57B File Offset: 0x0001C77B
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001E594 File Offset: 0x0001C794
		private void OnInflictBurn(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHeal)
			{
				this.playerHealth.Heal(1);
			}
		}

		// Token: 0x04000494 RID: 1172
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToHeal;

		// Token: 0x04000495 RID: 1173
		private PlayerHealth playerHealth;
	}
}
