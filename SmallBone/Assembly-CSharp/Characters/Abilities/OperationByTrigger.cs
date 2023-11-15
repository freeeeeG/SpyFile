using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A92 RID: 2706
	[Serializable]
	public class OperationByTrigger : Ability
	{
		// Token: 0x06003804 RID: 14340 RVA: 0x000A540B File Offset: 0x000A360B
		public OperationByTrigger()
		{
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000A541A File Offset: 0x000A361A
		public OperationByTrigger(ITrigger trigger, CharacterOperation[] operations)
		{
			this.trigger = trigger;
			this.operations = operations;
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000A5438 File Offset: 0x000A3638
		public override void Initialize()
		{
			base.Initialize();
			for (int i = 0; i < this.operations.Length; i++)
			{
				this.operations[i].Initialize();
			}
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000A546B File Offset: 0x000A366B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OperationByTrigger.Instance(owner, this);
		}

		// Token: 0x04002CAD RID: 11437
		public ITrigger trigger;

		// Token: 0x04002CAE RID: 11438
		[NonSerialized]
		public CharacterOperation[] operations;

		// Token: 0x04002CAF RID: 11439
		[SerializeField]
		private bool _onAwake = true;

		// Token: 0x04002CB0 RID: 11440
		[SerializeField]
		private int _triggerCount;

		// Token: 0x04002CB1 RID: 11441
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002CB2 RID: 11442
		[SerializeField]
		private bool _stopOperationOnRun;

		// Token: 0x02000A93 RID: 2707
		public class Instance : AbilityInstance<OperationByTrigger>
		{
			// Token: 0x17000BBD RID: 3005
			// (get) Token: 0x06003808 RID: 14344 RVA: 0x000A5474 File Offset: 0x000A3674
			public override Sprite icon
			{
				get
				{
					Sprite result = base.icon;
					if (this.ability._triggerCount > 0)
					{
						if (this._count == 0)
						{
							result = null;
						}
					}
					else
					{
						if (this.ability._cooldownTime <= 0f)
						{
							return result;
						}
						if (this._remainCooldownTime <= 0f)
						{
							result = null;
						}
					}
					return result;
				}
			}

			// Token: 0x17000BBE RID: 3006
			// (get) Token: 0x06003809 RID: 14345 RVA: 0x000A54C8 File Offset: 0x000A36C8
			public override int iconStacks
			{
				get
				{
					if (this._count <= 0)
					{
						return base.iconStacks;
					}
					return this._count;
				}
			}

			// Token: 0x17000BBF RID: 3007
			// (get) Token: 0x0600380A RID: 14346 RVA: 0x000A54E0 File Offset: 0x000A36E0
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._cooldownTime > 0f)
					{
						return 1f - this._remainCooldownTime / this.ability._cooldownTime;
					}
					return base.iconFillAmount;
				}
			}

			// Token: 0x0600380B RID: 14347 RVA: 0x000A5513 File Offset: 0x000A3713
			public Instance(Character owner, OperationByTrigger ability) : base(owner, ability)
			{
			}

			// Token: 0x0600380C RID: 14348 RVA: 0x000A5520 File Offset: 0x000A3720
			protected override void OnAttach()
			{
				if (!this.ability._onAwake)
				{
					this._remainCooldownTime = this.ability._cooldownTime;
				}
				this.ability.trigger.Attach(this.owner);
				this.ability.trigger.onTriggered += this.OnTriggered;
			}

			// Token: 0x0600380D RID: 14349 RVA: 0x000A557D File Offset: 0x000A377D
			protected override void OnDetach()
			{
				this.ability.trigger.Detach();
				this.ability.trigger.onTriggered -= this.OnTriggered;
			}

			// Token: 0x0600380E RID: 14350 RVA: 0x000A55AB File Offset: 0x000A37AB
			public override void Refresh()
			{
				base.Refresh();
				this.ability.trigger.Refresh();
			}

			// Token: 0x0600380F RID: 14351 RVA: 0x000A55C3 File Offset: 0x000A37C3
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
				this.ability.trigger.UpdateTime(deltaTime);
			}

			// Token: 0x06003810 RID: 14352 RVA: 0x000A55EC File Offset: 0x000A37EC
			private void OnTriggered()
			{
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (this._count < this.ability._triggerCount)
				{
					this._count++;
					return;
				}
				this._count = 0;
				this._remainCooldownTime = this.ability._cooldownTime;
				for (int i = 0; i < this.ability.operations.Length; i++)
				{
					if (this.ability._stopOperationOnRun)
					{
						this.ability.operations[i].Stop();
					}
					this.ability.operations[i].Run(this.owner);
				}
			}

			// Token: 0x04002CB3 RID: 11443
			private int _count;

			// Token: 0x04002CB4 RID: 11444
			private float _remainCooldownTime;
		}
	}
}
