using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012F4 RID: 4852
	public class Idle : Behaviour
	{
		// Token: 0x06005FF4 RID: 24564 RVA: 0x00118EAA File Offset: 0x001170AA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			float duration = UnityEngine.Random.Range(this._duration.x, this._duration.y);
			float elapsed = 0f;
			if (duration > 0f)
			{
				while (base.result == Behaviour.Result.Doing)
				{
					yield return null;
					elapsed += controller.character.chronometer.master.deltaTime;
					if (elapsed > duration)
					{
						break;
					}
				}
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004D32 RID: 19762
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _duration;
	}
}
