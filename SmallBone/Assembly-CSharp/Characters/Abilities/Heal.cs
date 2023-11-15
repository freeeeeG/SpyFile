using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A44 RID: 2628
	[Serializable]
	public class Heal : Ability
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x000A2E46 File Offset: 0x000A1046
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Heal.Instance(owner, this);
		}

		// Token: 0x04002BFA RID: 11258
		[SerializeField]
		private int _totalPercent;

		// Token: 0x04002BFB RID: 11259
		[SerializeField]
		private int _count = 3;

		// Token: 0x02000A45 RID: 2629
		public class Instance : AbilityInstance<Heal>
		{
			// Token: 0x06003730 RID: 14128 RVA: 0x000A2E4F File Offset: 0x000A104F
			public Instance(Character owner, Heal ability) : base(owner, ability)
			{
			}

			// Token: 0x06003731 RID: 14129 RVA: 0x000A2E59 File Offset: 0x000A1059
			public override void Refresh()
			{
				base.Refresh();
				this.OnAttach();
			}

			// Token: 0x06003732 RID: 14130 RVA: 0x000A2E67 File Offset: 0x000A1067
			protected override void OnAttach()
			{
				this._cHealReference.Stop();
				this._cHealReference = this.owner.StartCoroutineWithReference(this.CHeal());
			}

			// Token: 0x06003733 RID: 14131 RVA: 0x000A2E8B File Offset: 0x000A108B
			protected override void OnDetach()
			{
				this._cHealReference.Stop();
			}

			// Token: 0x06003734 RID: 14132 RVA: 0x000A2E98 File Offset: 0x000A1098
			private IEnumerator CHeal()
			{
				int num;
				for (int i = 0; i < this.ability._count; i = num + 1)
				{
					this.owner.health.PercentHeal((float)(this.ability._totalPercent / this.ability._count) * 0.01f);
					yield return this.owner.chronometer.master.WaitForSeconds(this.ability.duration / (float)this.ability._count);
					num = i;
				}
				yield break;
			}

			// Token: 0x04002BFC RID: 11260
			private CoroutineReference _cHealReference;
		}
	}
}
