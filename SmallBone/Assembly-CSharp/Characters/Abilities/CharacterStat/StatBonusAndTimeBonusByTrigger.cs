using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C41 RID: 3137
	[Serializable]
	public class StatBonusAndTimeBonusByTrigger : Ability
	{
		// Token: 0x06004057 RID: 16471 RVA: 0x00089C49 File Offset: 0x00087E49
		public StatBonusAndTimeBonusByTrigger()
		{
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x000BADC3 File Offset: 0x000B8FC3
		public StatBonusAndTimeBonusByTrigger(Stat.Values stat)
		{
			this._stat = stat;
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x000BADD2 File Offset: 0x000B8FD2
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusAndTimeBonusByTrigger.Instance(owner, this);
		}

		// Token: 0x04003171 RID: 12657
		[SerializeField]
		private SoundInfo _effectAudioClipInfo;

		// Token: 0x04003172 RID: 12658
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04003173 RID: 12659
		[SerializeField]
		private int _bounsTime;

		// Token: 0x04003174 RID: 12660
		[SerializeField]
		private bool _clampToAbilityDuration;

		// Token: 0x04003175 RID: 12661
		public ITrigger trigger;

		// Token: 0x02000C42 RID: 3138
		public class Instance : AbilityInstance<StatBonusAndTimeBonusByTrigger>
		{
			// Token: 0x0600405A RID: 16474 RVA: 0x000BADDB File Offset: 0x000B8FDB
			public Instance(Character owner, StatBonusAndTimeBonusByTrigger ability) : base(owner, ability)
			{
			}

			// Token: 0x0600405B RID: 16475 RVA: 0x000BADE8 File Offset: 0x000B8FE8
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this.ability._stat);
				this.ability.trigger.Attach(this.owner);
				this.ability.trigger.onTriggered += this.OnTriggered;
				base.remainTime = this.ability.duration;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._effectAudioClipInfo, this.owner.transform.position);
			}

			// Token: 0x0600405C RID: 16476 RVA: 0x000BAE7C File Offset: 0x000B907C
			private void OnTriggered()
			{
				base.remainTime += (float)this.ability._bounsTime;
				if (this.ability._clampToAbilityDuration)
				{
					base.remainTime = Mathf.Min(base.remainTime, this.ability.duration);
				}
			}

			// Token: 0x0600405D RID: 16477 RVA: 0x0009AFE4 File Offset: 0x000991E4
			public override void UpdateTime(float deltaTime)
			{
				base.remainTime -= deltaTime;
			}

			// Token: 0x0600405E RID: 16478 RVA: 0x000BAECC File Offset: 0x000B90CC
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
				this.ability.trigger.Detach();
				this.ability.trigger.onTriggered += this.OnTriggered;
			}
		}
	}
}
