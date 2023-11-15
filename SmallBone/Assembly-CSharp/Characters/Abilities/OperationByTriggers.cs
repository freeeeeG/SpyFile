using System;
using System.Collections.Generic;
using Characters.Abilities.Triggers;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A95 RID: 2709
	[Serializable]
	public class OperationByTriggers : Ability
	{
		// Token: 0x06003814 RID: 14356 RVA: 0x000A56C8 File Offset: 0x000A38C8
		public override void Initialize()
		{
			base.Initialize();
			for (int i = 0; i < this.operations.Length; i++)
			{
				this.operations[i].Initialize();
			}
			this.triggerComponents = new List<TriggerComponent>();
			foreach (OperationByTriggers.Triggers triggers2 in this._triggers)
			{
				this.triggerComponents.Add(triggers2.component);
			}
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000A5730 File Offset: 0x000A3930
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OperationByTriggers.Instance(owner, this);
		}

		// Token: 0x04002CB7 RID: 11447
		[SerializeField]
		private int _iconActiveTriggerIndex;

		// Token: 0x04002CB8 RID: 11448
		[SerializeField]
		private OperationByTriggers.Triggers[] _triggers;

		// Token: 0x04002CB9 RID: 11449
		private List<TriggerComponent> triggerComponents;

		// Token: 0x04002CBA RID: 11450
		[NonSerialized]
		public CharacterOperation[] operations;

		// Token: 0x04002CBB RID: 11451
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002CBC RID: 11452
		[SerializeField]
		private bool _stopOperationOnRun;

		// Token: 0x02000A96 RID: 2710
		public class Instance : AbilityInstance<OperationByTriggers>
		{
			// Token: 0x17000BC0 RID: 3008
			// (get) Token: 0x06003816 RID: 14358 RVA: 0x000A5739 File Offset: 0x000A3939
			public override Sprite icon
			{
				get
				{
					if (this._index < this.ability._iconActiveTriggerIndex)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000BC1 RID: 3009
			// (get) Token: 0x06003817 RID: 14359 RVA: 0x000A5756 File Offset: 0x000A3956
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

			// Token: 0x06003818 RID: 14360 RVA: 0x000A5789 File Offset: 0x000A3989
			public Instance(Character owner, OperationByTriggers ability) : base(owner, ability)
			{
			}

			// Token: 0x06003819 RID: 14361 RVA: 0x000A5794 File Offset: 0x000A3994
			protected override void OnAttach()
			{
				this._index = 0;
				this.ability.triggerComponents[this._index].Attach(this.owner);
				this.ability.triggerComponents[this._index].onTriggered += this.OnTriggered;
			}

			// Token: 0x0600381A RID: 14362 RVA: 0x000A57F0 File Offset: 0x000A39F0
			protected override void OnDetach()
			{
				this.ability.triggerComponents[this._index].Detach();
				this.ability.triggerComponents[this._index].onTriggered -= this.OnTriggered;
			}

			// Token: 0x0600381B RID: 14363 RVA: 0x000A583F File Offset: 0x000A3A3F
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
				this.ability.triggerComponents[this._index].UpdateTime(deltaTime);
			}

			// Token: 0x0600381C RID: 14364 RVA: 0x000A5874 File Offset: 0x000A3A74
			private void OnTriggered()
			{
				if (this.ability.triggerComponents.Count - 1 == this._index && this._remainCooldownTime > 0f)
				{
					return;
				}
				this.ability.triggerComponents[this._index].Detach();
				this.ability.triggerComponents[this._index].onTriggered -= this.OnTriggered;
				if (this._index < this.ability.triggerComponents.Count - 1)
				{
					this._index++;
					this.ability.triggerComponents[this._index].Attach(this.owner);
					this.ability.triggerComponents[this._index].onTriggered += this.OnTriggered;
					return;
				}
				this._index = 0;
				this.ability.triggerComponents[this._index].Attach(this.owner);
				this.ability.triggerComponents[this._index].onTriggered += this.OnTriggered;
				for (int i = 0; i < this.ability.operations.Length; i++)
				{
					if (this.ability._stopOperationOnRun)
					{
						this.ability.operations[i].Stop();
					}
					this.ability.operations[i].Run(this.owner);
				}
				this._remainCooldownTime = this.ability._cooldownTime;
			}

			// Token: 0x04002CBD RID: 11453
			private float _remainCooldownTime;

			// Token: 0x04002CBE RID: 11454
			private int _index;
		}

		// Token: 0x02000A97 RID: 2711
		[Serializable]
		private class Triggers
		{
			// Token: 0x17000BC2 RID: 3010
			// (get) Token: 0x0600381D RID: 14365 RVA: 0x000A5A0B File Offset: 0x000A3C0B
			public TriggerComponent component
			{
				get
				{
					return this._triggerComponent;
				}
			}

			// Token: 0x04002CBF RID: 11455
			[SerializeField]
			[TriggerComponent.SubcomponentAttribute]
			private TriggerComponent _triggerComponent;
		}
	}
}
