using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001306 RID: 4870
	public class SkipableIdle : Behaviour
	{
		// Token: 0x06006047 RID: 24647 RVA: 0x00119B96 File Offset: 0x00117D96
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (MMMaths.Chance(this._chance))
			{
				base.result = Behaviour.Result.Done;
				yield break;
			}
			float duration = UnityEngine.Random.Range(this._duration.x, this._duration.y);
			float elapsed = 0f;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				elapsed += controller.character.chronometer.master.deltaTime;
				if (elapsed > duration)
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x04004D84 RID: 19844
		[SerializeField]
		[Range(0f, 1f)]
		private float _chance;

		// Token: 0x04004D85 RID: 19845
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _duration;
	}
}
