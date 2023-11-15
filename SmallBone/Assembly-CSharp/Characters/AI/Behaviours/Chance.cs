using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012A8 RID: 4776
	public class Chance : Decorator
	{
		// Token: 0x06005EA3 RID: 24227 RVA: 0x00115E11 File Offset: 0x00114011
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (MMMaths.Chance(this._successChance))
			{
				yield return this._behaviour.CRun(controller);
				base.result = Behaviour.Result.Success;
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x04004C08 RID: 19464
		[Range(0f, 1f)]
		[SerializeField]
		private float _successChance;

		// Token: 0x04004C09 RID: 19465
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _behaviour;
	}
}
