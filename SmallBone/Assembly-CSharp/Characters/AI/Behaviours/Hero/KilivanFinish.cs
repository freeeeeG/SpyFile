using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C6 RID: 5062
	public sealed class KilivanFinish : Behaviour
	{
		// Token: 0x060063C9 RID: 25545 RVA: 0x00121D96 File Offset: 0x0011FF96
		public override IEnumerator CRun(AIController controller)
		{
			this._ready.TryStart();
			while (this._ready.running)
			{
				yield return null;
			}
			this._throw.TryStart();
			yield return this._kilivanProjectile.CFire();
			this._attack.TryStart();
			while (this._attack.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005071 RID: 20593
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x04005072 RID: 20594
		[SerializeField]
		private Characters.Actions.Action _throw;

		// Token: 0x04005073 RID: 20595
		[SerializeField]
		private Characters.Actions.Action _attack;

		// Token: 0x04005074 RID: 20596
		[SerializeField]
		private KilivanFinish _kilivanProjectile;
	}
}
