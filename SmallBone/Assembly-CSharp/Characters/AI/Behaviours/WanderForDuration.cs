using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001338 RID: 4920
	public class WanderForDuration : Wander
	{
		// Token: 0x0600610E RID: 24846 RVA: 0x0011C67D File Offset: 0x0011A87D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._move.direction = (MMMaths.RandomBool() ? Vector2.left : Vector2.right);
			yield return this._move.CRun(controller);
			yield break;
		}
	}
}
