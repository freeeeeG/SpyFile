using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones
{
	// Token: 0x0200101E RID: 4126
	public sealed class PushCannonBall : CharacterOperation
	{
		// Token: 0x06004F97 RID: 20375 RVA: 0x000EF945 File Offset: 0x000EDB45
		public override void Run(Character owner)
		{
			this._passive.Push(this._type, this._count);
		}

		// Token: 0x04003FD9 RID: 16345
		[SerializeField]
		private DavyJonesPassiveComponent _passive;

		// Token: 0x04003FDA RID: 16346
		[SerializeField]
		private CannonBallType _type;

		// Token: 0x04003FDB RID: 16347
		[SerializeField]
		private int _count = 1;
	}
}
