using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E4 RID: 2532
	public class DuringCombatAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035D8 RID: 13784 RVA: 0x0009FD89 File Offset: 0x0009DF89
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x0009FD98 File Offset: 0x0009DF98
		public override void StartAttach()
		{
			base.owner.playerComponents.combatDetector.onBeginCombat += this.Attach;
			base.owner.playerComponents.combatDetector.onFinishCombat += this.Detach;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0009FDE8 File Offset: 0x0009DFE8
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.playerComponents.combatDetector.onBeginCombat -= this.Attach;
			base.owner.playerComponents.combatDetector.onFinishCombat -= this.Detach;
			this.Detach();
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0009FE4C File Offset: 0x0009E04C
		private void Attach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x0009FE6A File Offset: 0x0009E06A
		private void Detach()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B47 RID: 11079
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
