using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BBD RID: 3005
	[Serializable]
	public sealed class Protection : Ability
	{
		// Token: 0x06003DEB RID: 15851 RVA: 0x000B412D File Offset: 0x000B232D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Protection.Instance(owner, this);
		}

		// Token: 0x04002FDA RID: 12250
		[SerializeField]
		private Guard _guard;

		// Token: 0x04002FDB RID: 12251
		[SerializeField]
		private float _guardDurabilityMultiplier;

		// Token: 0x04002FDC RID: 12252
		[SerializeField]
		private DarkAbilityGauge _gauge;

		// Token: 0x04002FDD RID: 12253
		[SerializeField]
		private float _guardCooldownTime;

		// Token: 0x02000BBE RID: 3006
		public sealed class Instance : AbilityInstance<Protection>
		{
			// Token: 0x06003DED RID: 15853 RVA: 0x000B4138 File Offset: 0x000B2338
			public Instance(Character owner, Protection ability) : base(owner, ability)
			{
				ability._guard.Initialize(owner);
				float num = (float)owner.health.maximumHealth * ability._guardDurabilityMultiplier;
				ability._guard.durability = num;
				ability._gauge.Set(num, num);
			}

			// Token: 0x06003DEE RID: 15854 RVA: 0x000B4186 File Offset: 0x000B2386
			protected override void OnAttach()
			{
				this.owner.StartCoroutine(this.CGuardUp());
			}

			// Token: 0x06003DEF RID: 15855 RVA: 0x000B419C File Offset: 0x000B239C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				float num = this.ability._guard.currentDurability;
				if (!Mathf.Approximately(num, this._lastGuardDurability))
				{
					this._lastGuardDurability = num;
					this.ability._gauge.Set(num);
				}
				if (!this.ability._guard.active && !this._raising)
				{
					this._remainCooldownTime -= deltaTime;
					if (this._remainCooldownTime <= 0f)
					{
						this.owner.StartCoroutine(this.CGuardUp());
					}
				}
			}

			// Token: 0x06003DF0 RID: 15856 RVA: 0x000B422F File Offset: 0x000B242F
			private IEnumerator CGuardUp()
			{
				this._raising = true;
				float elapsed = 0f;
				float length = 0.5f;
				while (elapsed <= length)
				{
					this.ability._guard.currentDurability = Mathf.Lerp(0f, this.ability._guard.durability, elapsed / length);
					elapsed += this.owner.chronometer.master.deltaTime;
					yield return null;
				}
				this._raising = false;
				this.GuardUp();
				yield break;
			}

			// Token: 0x06003DF1 RID: 15857 RVA: 0x000B423E File Offset: 0x000B243E
			private void GuardUp()
			{
				this.ability._guard.gameObject.SetActive(true);
				this.ability._guard.GuardUp();
				this._remainCooldownTime = this.ability._guardCooldownTime;
			}

			// Token: 0x06003DF2 RID: 15858 RVA: 0x000B4277 File Offset: 0x000B2477
			private void GuardDown()
			{
				this.ability._guard.gameObject.SetActive(false);
				this.ability._guard.GuardDown();
			}

			// Token: 0x06003DF3 RID: 15859 RVA: 0x000B429F File Offset: 0x000B249F
			protected override void OnDetach()
			{
				this.GuardDown();
			}

			// Token: 0x04002FDE RID: 12254
			private float _remainCooldownTime;

			// Token: 0x04002FDF RID: 12255
			private float _lastGuardDurability;

			// Token: 0x04002FE0 RID: 12256
			private bool _raising;
		}
	}
}
