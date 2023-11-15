using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009DC RID: 2524
	public class AlwaysAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035A2 RID: 13730 RVA: 0x0009F515 File Offset: 0x0009D715
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0009F522 File Offset: 0x0009D722
		public override void StartAttach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x0009F540 File Offset: 0x0009D740
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B2A RID: 11050
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
