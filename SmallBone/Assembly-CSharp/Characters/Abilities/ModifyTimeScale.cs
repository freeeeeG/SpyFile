using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A88 RID: 2696
	[Serializable]
	public class ModifyTimeScale : Ability
	{
		// Token: 0x060037F1 RID: 14321 RVA: 0x000A523D File Offset: 0x000A343D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyTimeScale.Instance(owner, this);
		}

		// Token: 0x04002CA2 RID: 11426
		[SerializeField]
		private ModifyTimeScale.ChronometerType _chronometerType;

		// Token: 0x04002CA3 RID: 11427
		[SerializeField]
		private float _timeScale = 1f;

		// Token: 0x04002CA4 RID: 11428
		[SerializeField]
		private float _globalTimeScale = 1f;

		// Token: 0x02000A89 RID: 2697
		public enum ChronometerType
		{
			// Token: 0x04002CA6 RID: 11430
			Master,
			// Token: 0x04002CA7 RID: 11431
			Animation,
			// Token: 0x04002CA8 RID: 11432
			Effect,
			// Token: 0x04002CA9 RID: 11433
			Projectile
		}

		// Token: 0x02000A8A RID: 2698
		public class Instance : AbilityInstance<ModifyTimeScale>
		{
			// Token: 0x060037F2 RID: 14322 RVA: 0x000A5246 File Offset: 0x000A3446
			public Instance(Character owner, ModifyTimeScale ability) : base(owner, ability)
			{
			}

			// Token: 0x060037F3 RID: 14323 RVA: 0x000A5250 File Offset: 0x000A3450
			private Chronometer GetChronometer()
			{
				switch (this.ability._chronometerType)
				{
				case ModifyTimeScale.ChronometerType.Animation:
					return this.owner.chronometer.animation;
				case ModifyTimeScale.ChronometerType.Effect:
					return this.owner.chronometer.effect;
				case ModifyTimeScale.ChronometerType.Projectile:
					return this.owner.chronometer.projectile;
				default:
					return this.owner.chronometer.master;
				}
			}

			// Token: 0x060037F4 RID: 14324 RVA: 0x000A52C2 File Offset: 0x000A34C2
			protected override void OnAttach()
			{
				Chronometer.global.AttachTimeScale(this, this.ability._globalTimeScale);
				this.GetChronometer().AttachTimeScale(this, this.ability._timeScale);
			}

			// Token: 0x060037F5 RID: 14325 RVA: 0x000A52F1 File Offset: 0x000A34F1
			protected override void OnDetach()
			{
				Chronometer.global.DetachTimeScale(this);
				this.GetChronometer().DetachTimeScale(this);
			}
		}
	}
}
