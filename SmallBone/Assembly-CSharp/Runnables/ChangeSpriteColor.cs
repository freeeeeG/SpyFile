using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002EC RID: 748
	public class ChangeSpriteColor : CRunnable
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002DCEA File Offset: 0x0002BEEA
		public override IEnumerator CRun()
		{
			Color startColor = this._sprite.color;
			float elapsed = 0f;
			while (elapsed < this._curve.duration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._sprite.color = Color.Lerp(startColor, this._color, this._curve.Evaluate(elapsed));
				yield return null;
			}
			this._sprite.color = this._color;
			yield break;
		}

		// Token: 0x04000C33 RID: 3123
		[SerializeField]
		private SpriteRenderer _sprite;

		// Token: 0x04000C34 RID: 3124
		[SerializeField]
		private Color _color;

		// Token: 0x04000C35 RID: 3125
		[SerializeField]
		private Curve _curve;
	}
}
