using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000DA2 RID: 3490
	public class DetachInvulnerableComponent : AbilityComponent<DetachInvulnerable>
	{
		// Token: 0x06004641 RID: 17985 RVA: 0x000CB1ED File Offset: 0x000C93ED
		private void Awake()
		{
			this._owner.invulnerable.Attach(this._key);
		}

		// Token: 0x0400353A RID: 13626
		[SerializeField]
		private Character _owner;

		// Token: 0x0400353B RID: 13627
		[SerializeField]
		private Transform _key;
	}
}
