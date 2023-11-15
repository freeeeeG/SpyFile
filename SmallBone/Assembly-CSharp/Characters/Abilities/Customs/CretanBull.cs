using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D3C RID: 3388
	[Serializable]
	public class CretanBull : Ability
	{
		// Token: 0x06004450 RID: 17488 RVA: 0x000C68E2 File Offset: 0x000C4AE2
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x000C68F5 File Offset: 0x000C4AF5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CretanBull.Instance(owner, this);
		}

		// Token: 0x0400341A RID: 13338
		[SerializeField]
		private Weapon.Category _headCategory;

		// Token: 0x0400341B RID: 13339
		[SerializeField]
		private CretanBull.Timing _actionTiming;

		// Token: 0x0400341C RID: 13340
		[SerializeField]
		private ActionTypeBoolArray _actionTypeFilter;

		// Token: 0x0400341D RID: 13341
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x0400341E RID: 13342
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D3D RID: 3389
		public class Instance : AbilityInstance<CretanBull>
		{
			// Token: 0x17000E2B RID: 3627
			// (get) Token: 0x06004453 RID: 17491 RVA: 0x000C68FE File Offset: 0x000C4AFE
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

			// Token: 0x06004454 RID: 17492 RVA: 0x000C6931 File Offset: 0x000C4B31
			public Instance(Character owner, CretanBull ability) : base(owner, ability)
			{
			}

			// Token: 0x06004455 RID: 17493 RVA: 0x000C693C File Offset: 0x000C4B3C
			protected override void OnAttach()
			{
				if (this.ability._actionTiming == CretanBull.Timing.Start)
				{
					this.owner.onStartAction += this.OnCharacterAction;
					return;
				}
				if (this.ability._actionTiming == CretanBull.Timing.End)
				{
					this.owner.onCancelAction += this.OnCharacterAction;
					this.owner.onEndAction += this.OnCharacterAction;
				}
			}

			// Token: 0x06004456 RID: 17494 RVA: 0x000C69AC File Offset: 0x000C4BAC
			protected override void OnDetach()
			{
				if (this.ability._actionTiming == CretanBull.Timing.Start)
				{
					this.owner.onStartAction -= this.OnCharacterAction;
					return;
				}
				if (this.ability._actionTiming == CretanBull.Timing.End)
				{
					this.owner.onCancelAction -= this.OnCharacterAction;
					this.owner.onEndAction -= this.OnCharacterAction;
				}
			}

			// Token: 0x06004457 RID: 17495 RVA: 0x000C6A1C File Offset: 0x000C4C1C
			private void OnCharacterAction(Characters.Actions.Action action)
			{
				if (!this.ability._actionTypeFilter.GetOrDefault(action.type))
				{
					return;
				}
				if (action.type == Characters.Actions.Action.Type.Skill && action.cooldown.usedByStreak)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (this.owner.playerComponents.inventory.weapon.polymorphOrCurrent.category != this.ability._headCategory)
				{
					return;
				}
				this.ability._operations.Run(this.owner);
				this._remainCooldownTime = this.ability._cooldownTime;
			}

			// Token: 0x06004458 RID: 17496 RVA: 0x000C6ABB File Offset: 0x000C4CBB
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x0400341F RID: 13343
			private float _remainCooldownTime;
		}

		// Token: 0x02000D3E RID: 3390
		public enum Timing
		{
			// Token: 0x04003421 RID: 13345
			Start,
			// Token: 0x04003422 RID: 13346
			End
		}
	}
}
