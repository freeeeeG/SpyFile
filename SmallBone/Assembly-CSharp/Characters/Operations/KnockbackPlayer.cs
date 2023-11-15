using System;
using Characters.Movements;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DE8 RID: 3560
	public class KnockbackPlayer : CharacterOperation
	{
		// Token: 0x06004751 RID: 18257 RVA: 0x000CF4A0 File Offset: 0x000CD6A0
		public override void Run(Character owner)
		{
			Singleton<Service>.Instance.levelManager.player.movement.push.ApplyKnockback(owner, this._pushInfo);
		}

		// Token: 0x0400365F RID: 13919
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);
	}
}
