using System;
using System.Collections;
using Level;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runnables
{
	// Token: 0x02000305 RID: 773
	public class ChangeGlobalLightIntensity : CRunnable
	{
		// Token: 0x06000F24 RID: 3876 RVA: 0x0002E5F2 File Offset: 0x0002C7F2
		public override IEnumerator CRun()
		{
			Light2D globalLight = Map.Instance.globalLight;
			float startIntensity = globalLight.intensity;
			float elapsed = 0f;
			while (elapsed < this._curve.duration)
			{
				elapsed += Chronometer.global.deltaTime;
				globalLight.intensity = Mathf.Lerp(startIntensity, this._intensity, this._curve.Evaluate(elapsed));
				yield return null;
			}
			globalLight.intensity = this._intensity;
			yield break;
		}

		// Token: 0x04000C75 RID: 3189
		[SerializeField]
		private Curve _curve;

		// Token: 0x04000C76 RID: 3190
		[SerializeField]
		private float _intensity;
	}
}
