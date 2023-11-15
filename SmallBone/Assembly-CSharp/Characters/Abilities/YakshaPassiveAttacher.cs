using System;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A00 RID: 2560
	public class YakshaPassiveAttacher : AbilityAttacher
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x000A1264 File Offset: 0x0009F464
		public override void OnIntialize()
		{
			this._yakshaPassive.owner = base.owner;
			this._yakshaPassive.Initialize();
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000A1282 File Offset: 0x0009F482
		public override void StartAttach()
		{
			base.owner.ability.Add(this._yakshaPassive);
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000A129B File Offset: 0x0009F49B
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.ability.Remove(this._yakshaPassive);
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000A12C3 File Offset: 0x0009F4C3
		public void AddStack()
		{
			this._yakshaPassive.AddStack();
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002BA4 RID: 11172
		[SerializeField]
		private YakshaPassive _yakshaPassive;
	}
}
