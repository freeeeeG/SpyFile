using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones
{
	// Token: 0x0200101D RID: 4125
	public sealed class PopCannonBall : CharacterOperation
	{
		// Token: 0x06004F95 RID: 20373 RVA: 0x000EF938 File Offset: 0x000EDB38
		public override void Run(Character owner)
		{
			this._passive.Pop();
		}

		// Token: 0x04003FD8 RID: 16344
		[SerializeField]
		private DavyJonesPassiveComponent _passive;
	}
}
