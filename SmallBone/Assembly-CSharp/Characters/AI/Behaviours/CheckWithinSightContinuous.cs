using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012A1 RID: 4769
	public class CheckWithinSightContinuous : Behaviour
	{
		// Token: 0x06005E88 RID: 24200 RVA: 0x00115BA6 File Offset: 0x00113DA6
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				if (!(controller.target != null))
				{
					Character character = controller.FindClosestPlayerBody(this._sightCollider, controller.character.transform.position, this._blockedLayers);
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

		// Token: 0x04004BF6 RID: 19446
		[SerializeField]
		private Collider2D _sightCollider;

		// Token: 0x04004BF7 RID: 19447
		[SerializeField]
		private LayerMask _blockedLayers;
	}
}
