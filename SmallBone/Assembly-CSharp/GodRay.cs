using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Token: 0x02000044 RID: 68
public class GodRay : MonoBehaviour
{
	// Token: 0x06000133 RID: 307 RVA: 0x00006C8C File Offset: 0x00004E8C
	private void Awake()
	{
		this._showRange = Mathf.Max(this._showRange, 0.01f);
		base.StartCoroutine(this.CModifyIntensity());
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00006CB1 File Offset: 0x00004EB1
	private IEnumerator CModifyIntensity()
	{
		float defaultIntensity = this._light.intensity;
		float noiseBlend = this._noiseBlend;
		float noiseBlend2 = this._noiseBlend;
		float noiseShift = UnityEngine.Random.value;
		for (;;)
		{
			float num = Camera.main.transform.position.x - base.transform.position.x;
			float time = 1f - Mathf.Clamp01(Mathf.Abs(num) / this._showRange);
			float num2 = this._intensityCurve.Evaluate(time);
			float num3 = Mathf.PerlinNoise(num / this._showRange * 0.5f * this._noisePower, noiseShift * this._noisePower);
			num3 = 1f - this._noiseBlend + num3 * this._noiseBlend;
			this._light.intensity = this._minIntensity + (defaultIntensity - this._minIntensity) * num2 * num3;
			noiseShift += Chronometer.global.deltaTime * this._noiseShiftSpeed;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000105 RID: 261
	[SerializeField]
	[GetComponent]
	private Light2D _light;

	// Token: 0x04000106 RID: 262
	[SerializeField]
	private float _showRange = 10f;

	// Token: 0x04000107 RID: 263
	[SerializeField]
	private float _minIntensity;

	// Token: 0x04000108 RID: 264
	[SerializeField]
	private Curve _intensityCurve;

	// Token: 0x04000109 RID: 265
	[SerializeField]
	[Range(0f, 1f)]
	private float _noiseBlend = 1f;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private float _noisePower = 1f;

	// Token: 0x0400010B RID: 267
	[SerializeField]
	private float _noiseShiftSpeed = 0.1f;
}
