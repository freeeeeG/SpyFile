using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C44 RID: 3140
	[Serializable]
	public class StatBonusByAirTime : Ability
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x000BAF41 File Offset: 0x000B9141
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByAirTime.Instance(owner, this);
		}

		// Token: 0x04003177 RID: 12663
		[SerializeField]
		private float _timeToMaxStat;

		// Token: 0x04003178 RID: 12664
		[SerializeField]
		[Tooltip("바닥에 착지할 경우 이 시간 후에 버프가 사라짐")]
		private float _remainTimeOnGround = 1f;

		// Token: 0x04003179 RID: 12665
		[SerializeField]
		[Space]
		private Stat.Values _maxStat;

		// Token: 0x02000C45 RID: 3141
		public class Instance : AbilityInstance<StatBonusByAirTime>
		{
			// Token: 0x17000D97 RID: 3479
			// (get) Token: 0x06004063 RID: 16483 RVA: 0x000BAF5D File Offset: 0x000B915D
			public override Sprite icon
			{
				get
				{
					if (this._remainBuffTime <= 0f)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000D98 RID: 3480
			// (get) Token: 0x06004064 RID: 16484 RVA: 0x000BAF79 File Offset: 0x000B9179
			public override float iconFillAmount
			{
				get
				{
					return 1f - this._airTime / this.ability._timeToMaxStat;
				}
			}

			// Token: 0x06004065 RID: 16485 RVA: 0x000BAF93 File Offset: 0x000B9193
			public Instance(Character owner, StatBonusByAirTime ability) : base(owner, ability)
			{
				this._stat = ability._maxStat.Clone();
			}

			// Token: 0x06004066 RID: 16486 RVA: 0x000BAFB0 File Offset: 0x000B91B0
			protected override void OnAttach()
			{
				this._wasGrounded = this.owner.movement.controller.isGrounded;
				this.SetStat(0f);
				this.owner.stat.AttachValues(this._stat);
				this.owner.movement.onJump += this.OnJump;
				this.owner.movement.onGrounded += this.OnGrounded;
			}

			// Token: 0x06004067 RID: 16487 RVA: 0x000BB034 File Offset: 0x000B9234
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.movement.onJump -= this.OnJump;
				this.owner.movement.onGrounded -= this.OnGrounded;
			}

			// Token: 0x06004068 RID: 16488 RVA: 0x000BB090 File Offset: 0x000B9290
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._wasGrounded)
				{
					this._remainBuffTime -= deltaTime;
				}
				else
				{
					this._airTime += deltaTime;
				}
				if (this._airTime > this.ability._timeToMaxStat)
				{
					this._airTime = this.ability._timeToMaxStat;
				}
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.25f;
					this.UpdateStat();
				}
			}

			// Token: 0x06004069 RID: 16489 RVA: 0x000BB11C File Offset: 0x000B931C
			public void UpdateStat()
			{
				Stat.Value[] values = this._stat.values;
				float num = 0f;
				if (this._remainBuffTime > 0f)
				{
					num = this._airTime / this.ability._timeToMaxStat;
				}
				if (num == this._cachedMultiplier)
				{
					return;
				}
				this._cachedMultiplier = num;
				this.SetStat(num);
			}

			// Token: 0x0600406A RID: 16490 RVA: 0x000BB174 File Offset: 0x000B9374
			private void SetStat(float multiplier)
			{
				Stat.Value[] values = this._stat.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._maxStat.values[i].GetMultipliedValue(multiplier);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600406B RID: 16491 RVA: 0x000BB1CB File Offset: 0x000B93CB
			private void OnJump(Movement.JumpType jumpType, float jumpHeight)
			{
				if (this._wasGrounded)
				{
					this._wasGrounded = false;
					this._airTime = 0f;
				}
				this._remainBuffTime = float.PositiveInfinity;
				this.UpdateStat();
			}

			// Token: 0x0600406C RID: 16492 RVA: 0x000BB1F8 File Offset: 0x000B93F8
			private void OnGrounded()
			{
				this._wasGrounded = true;
				this._remainBuffTime = this.ability._remainTimeOnGround;
			}

			// Token: 0x0400317A RID: 12666
			private const float _updateInterval = 0.25f;

			// Token: 0x0400317B RID: 12667
			private float _remainUpdateTime;

			// Token: 0x0400317C RID: 12668
			private Stat.Values _stat;

			// Token: 0x0400317D RID: 12669
			private float _remainBuffTime;

			// Token: 0x0400317E RID: 12670
			private bool _wasGrounded;

			// Token: 0x0400317F RID: 12671
			private float _airTime;

			// Token: 0x04003180 RID: 12672
			private float _cachedMultiplier;
		}
	}
}
