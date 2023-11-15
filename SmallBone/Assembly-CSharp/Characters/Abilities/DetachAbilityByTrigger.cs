using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities.Triggers;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A18 RID: 2584
	[Serializable]
	public class DetachAbilityByTrigger : Ability
	{
		// Token: 0x060036C3 RID: 14019 RVA: 0x000A214C File Offset: 0x000A034C
		public override void Initialize()
		{
			base.Initialize();
			this._onAttach.Initialize();
			this._onDetach.Initialize();
			this._abilityComponent.Initialize();
			this.triggerComponents = new List<TriggerComponent>();
			foreach (DetachAbilityByTrigger.Triggers triggers2 in this._triggers)
			{
				this.triggerComponents.Add(triggers2.component);
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000A21B5 File Offset: 0x000A03B5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new DetachAbilityByTrigger.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x000A21CA File Offset: 0x000A03CA
		public void Destroy()
		{
			if (this._instance == null)
			{
				return;
			}
			this._instance.OnDestroy();
		}

		// Token: 0x04002BD2 RID: 11218
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002BD3 RID: 11219
		[SerializeField]
		private DetachAbilityByTrigger.Triggers[] _triggers;

		// Token: 0x04002BD4 RID: 11220
		[SerializeField]
		private bool _onOffFiilAmount;

		// Token: 0x04002BD5 RID: 11221
		[SerializeField]
		private float _detachTime;

		// Token: 0x04002BD6 RID: 11222
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onAttach;

		// Token: 0x04002BD7 RID: 11223
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onDetach;

		// Token: 0x04002BD8 RID: 11224
		private List<TriggerComponent> triggerComponents;

		// Token: 0x04002BD9 RID: 11225
		private DetachAbilityByTrigger.Instance _instance;

		// Token: 0x02000A19 RID: 2585
		public class Instance : AbilityInstance<DetachAbilityByTrigger>
		{
			// Token: 0x17000BA5 RID: 2981
			// (get) Token: 0x060036C6 RID: 14022 RVA: 0x000A21E0 File Offset: 0x000A03E0
			public override Sprite icon
			{
				get
				{
					if (!this._detached)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000BA6 RID: 2982
			// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000A21F4 File Offset: 0x000A03F4
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._onOffFiilAmount)
					{
						return (float)((this._remainDetachTime > 0f) ? 1 : 0);
					}
					if (this._remainDetachTime > 0f)
					{
						return 1f - this._remainDetachTime / this.ability._detachTime;
					}
					return base.iconFillAmount;
				}
			}

			// Token: 0x060036C8 RID: 14024 RVA: 0x000A224D File Offset: 0x000A044D
			public Instance(Character owner, DetachAbilityByTrigger ability) : base(owner, ability)
			{
			}

			// Token: 0x060036C9 RID: 14025 RVA: 0x000A2258 File Offset: 0x000A0458
			protected override void OnAttach()
			{
				this._index = 0;
				this.owner.ability.Add(this.ability._abilityComponent.ability);
				this.ability.triggerComponents[this._index].Attach(this.owner);
				this.ability.triggerComponents[this._index].onTriggered += this.OnTriggered;
			}

			// Token: 0x060036CA RID: 14026 RVA: 0x000A22D8 File Offset: 0x000A04D8
			protected override void OnDetach()
			{
				this.owner.StartCoroutine(this.CDelayDetach());
				this.ability.triggerComponents[this._index].Detach();
				this.ability.triggerComponents[this._index].onTriggered -= this.OnTriggered;
			}

			// Token: 0x060036CB RID: 14027 RVA: 0x000A233C File Offset: 0x000A053C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._detached)
				{
					this._remainDetachTime -= deltaTime;
					if (this._remainDetachTime < 0f)
					{
						this.owner.ability.Add(this.ability._abilityComponent.ability);
						this.owner.StartCoroutine(this.ability._onAttach.CRun(this.owner));
						this._detached = false;
					}
				}
				this.ability.triggerComponents[this._index].UpdateTime(deltaTime);
			}

			// Token: 0x060036CC RID: 14028 RVA: 0x000A23DC File Offset: 0x000A05DC
			private void OnTriggered()
			{
				if (this.ability.triggerComponents.Count - 1 == this._index && this._remainDetachTime > 0f)
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
				this.owner.ability.Remove(this.ability._abilityComponent.ability);
				this.owner.StartCoroutine(this.ability._onDetach.CRun(this.owner));
				this._remainDetachTime = this.ability._detachTime;
				this._detached = true;
			}

			// Token: 0x060036CD RID: 14029 RVA: 0x000A256E File Offset: 0x000A076E
			public void OnDestroy()
			{
				if (this.owner != null)
				{
					this.owner.ability.Remove(this.ability._abilityComponent.ability);
				}
			}

			// Token: 0x060036CE RID: 14030 RVA: 0x000A259F File Offset: 0x000A079F
			private IEnumerator CDelayDetach()
			{
				yield return null;
				if (this.owner == null)
				{
					yield break;
				}
				this.owner.ability.Remove(this.ability._abilityComponent.ability);
				yield break;
			}

			// Token: 0x04002BDA RID: 11226
			private float _remainDetachTime;

			// Token: 0x04002BDB RID: 11227
			private bool _detached;

			// Token: 0x04002BDC RID: 11228
			private int _index;
		}

		// Token: 0x02000A1B RID: 2587
		[Serializable]
		private class Triggers
		{
			// Token: 0x17000BA9 RID: 2985
			// (get) Token: 0x060036D5 RID: 14037 RVA: 0x000A263C File Offset: 0x000A083C
			public TriggerComponent component
			{
				get
				{
					return this._triggerComponent;
				}
			}

			// Token: 0x04002BE0 RID: 11232
			[SerializeField]
			[TriggerComponent.SubcomponentAttribute]
			private TriggerComponent _triggerComponent;
		}
	}
}
