using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runnables
{
	// Token: 0x02000307 RID: 775
	public class ChangeLightSettings : CRunnable
	{
		// Token: 0x06000F2C RID: 3884 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		public override IEnumerator CRun()
		{
			Color startColor = this._light.color;
			float startIntensity = this._light.intensity;
			float elapsed = 0f;
			while (elapsed < this._curve.duration)
			{
				elapsed += Chronometer.global.deltaTime;
				float t = this._curve.Evaluate(elapsed);
				this._light.color = Color.Lerp(startColor, this._color, t);
				this._light.intensity = Mathf.Lerp(startIntensity, this._intensity, t);
				yield return null;
			}
			this._light.color = this._color;
			this._light.intensity = this._intensity;
			yield break;
		}

		// Token: 0x04000C7D RID: 3197
		[SerializeField]
		private Light2D _light;

		// Token: 0x04000C7E RID: 3198
		[SerializeField]
		private Curve _curve;

		// Token: 0x04000C7F RID: 3199
		[ColorUsage(false)]
		[SerializeField]
		private Color _color;

		// Token: 0x04000C80 RID: 3200
		[SerializeField]
		private float _intensity;
	}
}
