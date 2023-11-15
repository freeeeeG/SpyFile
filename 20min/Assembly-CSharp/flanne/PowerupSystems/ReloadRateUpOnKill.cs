using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystems
{
	// Token: 0x020001DE RID: 478
	public class ReloadRateUpOnKill : MonoBehaviour
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x00028CE4 File Offset: 0x00026EE4
		private void OnDeath(object sender, object args)
		{
			if ((sender as Health).gameObject.tag == "Enemy")
			{
				this.stats[StatType.ReloadRate].AddMultiplierBonus(this.bonusPerStack);
				this._stacks++;
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00028D32 File Offset: 0x00026F32
		private void OnReload()
		{
			this.stats[StatType.ReloadRate].AddMultiplierBonus((float)(-1 * this._stacks) * this.bonusPerStack);
			this._stacks = 0;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00028D5C File Offset: 0x00026F5C
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.ammo = componentInParent.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00028DC0 File Offset: 0x00026FC0
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x04000781 RID: 1921
		[SerializeField]
		private float bonusPerStack;

		// Token: 0x04000782 RID: 1922
		private StatsHolder stats;

		// Token: 0x04000783 RID: 1923
		private Ammo ammo;

		// Token: 0x04000784 RID: 1924
		private int _stacks;
	}
}
