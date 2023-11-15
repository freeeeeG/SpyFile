using System;
using Characters.Abilities.Items;
using Characters.Operations;
using FX.SpriteEffects;
using GameResources;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AAA RID: 2730
	[Serializable]
	public class Revive : Ability
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x000A5FD5 File Offset: 0x000A41D5
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
			if (this._ability != null)
			{
				this._ability.Initialize();
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000A6001 File Offset: 0x000A4201
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Revive.Instance(owner, this);
		}

		// Token: 0x04002CD7 RID: 11479
		[SerializeField]
		private int _heal = 30;

		// Token: 0x04002CD8 RID: 11480
		[SerializeField]
		private bool _percentHeal;

		// Token: 0x04002CD9 RID: 11481
		[Range(0f, 100f)]
		[SerializeField]
		private int _percentHealAmount;

		// Token: 0x04002CDA RID: 11482
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _ability;

		// Token: 0x04002CDB RID: 11483
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000AAB RID: 2731
		public class Instance : AbilityInstance<Revive>
		{
			// Token: 0x0600384F RID: 14415 RVA: 0x000A600A File Offset: 0x000A420A
			public Instance(Character owner, Revive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003850 RID: 14416 RVA: 0x000A6014 File Offset: 0x000A4214
			protected override void OnAttach()
			{
				this.owner.health.onDie += this.ReviveOwner;
			}

			// Token: 0x06003851 RID: 14417 RVA: 0x000A6032 File Offset: 0x000A4232
			protected override void OnDetach()
			{
				this.owner.health.onDie -= this.ReviveOwner;
			}

			// Token: 0x06003852 RID: 14418 RVA: 0x000A6050 File Offset: 0x000A4250
			private void ReviveOwner()
			{
				if (this.owner.health.currentHealth > 0.0)
				{
					return;
				}
				ChosenHerosCirclet.Instance instanceByInstanceType = this.owner.ability.GetInstanceByInstanceType<ChosenHerosCirclet.Instance>();
				if (instanceByInstanceType != null && instanceByInstanceType.canUse)
				{
					return;
				}
				this.owner.health.onDie -= this.ReviveOwner;
				Chronometer.global.AttachTimeScale(this, 0.2f, 0.5f);
				if (this.ability._percentHeal)
				{
					this.owner.health.PercentHeal((float)this.ability._percentHealAmount * 0.01f);
				}
				else
				{
					this.owner.health.Heal((double)this.ability._heal, true);
				}
				CommonResource.instance.reassembleParticle.Emit(this.owner.transform.position, this.owner.collider.bounds, this.owner.movement.push);
				this.owner.CancelAction();
				this.owner.chronometer.master.AttachTimeScale(this, 0.01f, 0.5f);
				this.owner.spriteEffectStack.Add(new ColorBlend(int.MaxValue, Color.clear, 0.5f));
				GetInvulnerable getInvulnerable = new GetInvulnerable();
				getInvulnerable.duration = 3f;
				this.owner.spriteEffectStack.Add(new Invulnerable(0, 0.2f, getInvulnerable.duration));
				this.owner.ability.Add(getInvulnerable);
				if (this.ability._ability != null)
				{
					this.owner.ability.Add(this.ability._ability.ability);
				}
				this.ability._operations.Run(this.owner);
				this.owner.ability.Remove(this);
			}
		}
	}
}
