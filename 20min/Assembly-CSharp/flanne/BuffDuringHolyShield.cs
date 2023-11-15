using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000D2 RID: 210
	public class BuffDuringHolyShield : MonoBehaviour
	{
		// Token: 0x06000677 RID: 1655 RVA: 0x0001D671 File Offset: 0x0001B871
		private void OnDamagePrevented()
		{
			this.Deactivate();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001D679 File Offset: 0x0001B879
		private void OnCooldownDone()
		{
			this.Activate();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001D684 File Offset: 0x0001B884
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.holyShield = base.transform.root.GetComponentInChildren<PreventDamage>();
			if (this.holyShield.isActive)
			{
				this.Activate();
			}
			this.holyShield.OnDamagePrevented.AddListener(new UnityAction(this.OnDamagePrevented));
			this.holyShield.OnCooldownDone.AddListener(new UnityAction(this.OnCooldownDone));
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001D70A File Offset: 0x0001B90A
		private void OnDestroy()
		{
			this.holyShield.OnDamagePrevented.RemoveListener(new UnityAction(this.OnDamagePrevented));
			this.holyShield.OnCooldownDone.RemoveListener(new UnityAction(this.OnCooldownDone));
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001D744 File Offset: 0x0001B944
		private void Activate()
		{
			this.stats[StatType.ReloadRate].AddMultiplierBonus(this.reloadRateMulti);
			this.stats[StatType.MoveSpeed].AddMultiplierBonus(this.movespeedMulti);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001D775 File Offset: 0x0001B975
		private void Deactivate()
		{
			this.stats[StatType.ReloadRate].AddMultiplierBonus(-1f * this.reloadRateMulti);
			this.stats[StatType.MoveSpeed].AddMultiplierBonus(-1f * this.movespeedMulti);
		}

		// Token: 0x04000448 RID: 1096
		[SerializeField]
		private float reloadRateMulti;

		// Token: 0x04000449 RID: 1097
		[SerializeField]
		private float movespeedMulti;

		// Token: 0x0400044A RID: 1098
		private StatsHolder stats;

		// Token: 0x0400044B RID: 1099
		private PreventDamage holyShield;
	}
}
