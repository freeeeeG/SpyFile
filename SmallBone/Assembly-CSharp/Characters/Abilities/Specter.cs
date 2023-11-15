using System;
using Characters.Gear.Quintessences;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A11 RID: 2577
	[Serializable]
	public class Specter : Ability
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x000A1B78 File Offset: 0x0009FD78
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Specter.Instance(owner, this, this._quintessence);
		}

		// Token: 0x04002BBC RID: 11196
		[SerializeField]
		private Quintessence _quintessence;

		// Token: 0x02000A12 RID: 2578
		public class Instance : AbilityInstance<Specter>
		{
			// Token: 0x060036AA RID: 13994 RVA: 0x000A1B87 File Offset: 0x0009FD87
			public Instance(Character owner, Specter ability, Quintessence quintessence) : base(owner, ability)
			{
				this._quintessence = quintessence;
			}

			// Token: 0x060036AB RID: 13995 RVA: 0x000A1B98 File Offset: 0x0009FD98
			protected override void OnAttach()
			{
				base.remainTime = this.ability.duration;
			}

			// Token: 0x060036AC RID: 13996 RVA: 0x000A1BAC File Offset: 0x0009FDAC
			protected override void OnDetach()
			{
				if (this.owner.health.dead)
				{
					Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence.items[0].cooldown.time.remainTime = 0f;
				}
			}

			// Token: 0x060036AD RID: 13997 RVA: 0x000A1B98 File Offset: 0x0009FD98
			public override void Refresh()
			{
				base.remainTime = this.ability.duration;
			}

			// Token: 0x04002BBD RID: 11197
			private Quintessence _quintessence;
		}
	}
}
