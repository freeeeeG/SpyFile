using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200129F RID: 4767
	public class CheckWithinSight : Behaviour
	{
		// Token: 0x06005E80 RID: 24192 RVA: 0x00115ACA File Offset: 0x00113CCA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				if (!(controller.target != null))
				{
					Character character = controller.FindClosestPlayerBody(this._sightCollider);
					if (character != null && !character.stealth.value)
					{
						controller.target = character;
						controller.FoundEnemy();
						base.result = Behaviour.Result.Done;
					}
				}
			}
			yield break;
		}

		// Token: 0x04004BF1 RID: 19441
		[SerializeField]
		private Collider2D _sightCollider;
	}
}
