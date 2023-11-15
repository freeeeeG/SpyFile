using System;
using System.Collections;
using Characters.AI.Hero;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A2 RID: 5026
	public sealed class SaintFieldLoop : Decorator
	{
		// Token: 0x06006325 RID: 25381 RVA: 0x001208E9 File Offset: 0x0011EAE9
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			while (this._saintField.isStuck)
			{
				yield return this._behaviour.CRun(controller);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004FF5 RID: 20469
		[SerializeField]
		private SaintField _saintField;

		// Token: 0x04004FF6 RID: 20470
		[SerializeField]
		[Behaviour.SubcomponentAttribute(true)]
		private Behaviour _behaviour;
	}
}
