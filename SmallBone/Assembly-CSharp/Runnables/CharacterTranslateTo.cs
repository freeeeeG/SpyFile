using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002DC RID: 732
	public sealed class CharacterTranslateTo : CRunnable
	{
		// Token: 0x06000E9D RID: 3741 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		public override IEnumerator CRun()
		{
			Character character = this._target.character;
			Vector3 start = character.transform.position;
			Vector3 end = this._destination.position;
			for (float elapsed = 0f; elapsed < this.curve.duration; elapsed += Chronometer.global.deltaTime)
			{
				yield return null;
				character.transform.position = Vector2.Lerp(start, end, this.curve.Evaluate(elapsed));
			}
			character.transform.position = end;
			yield break;
		}

		// Token: 0x04000C1A RID: 3098
		[SerializeField]
		private Target _target;

		// Token: 0x04000C1B RID: 3099
		[SerializeField]
		private Transform _destination;

		// Token: 0x04000C1C RID: 3100
		[SerializeField]
		private Curve curve;
	}
}
