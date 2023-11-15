using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084A RID: 2122
	public sealed class AttachAbility : UpgradeAbility
	{
		// Token: 0x06002C31 RID: 11313 RVA: 0x000875DB File Offset: 0x000857DB
		public override void Attach(Character target)
		{
			if (target == null)
			{
				Debug.LogError("Player is null");
				return;
			}
			this._target = target;
			this._abilityAttacher.Initialize(this._target);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x00087614 File Offset: 0x00085814
		public override void Detach()
		{
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x0400255C RID: 9564
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher.Subcomponents _abilityAttacher;

		// Token: 0x0400255D RID: 9565
		private Character _target;
	}
}
