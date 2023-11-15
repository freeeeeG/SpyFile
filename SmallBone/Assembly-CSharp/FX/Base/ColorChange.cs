using System;
using System.Collections;
using UnityEngine;

namespace FX.Base
{
	// Token: 0x02000292 RID: 658
	public class ColorChange : MonoBehaviour
	{
		// Token: 0x06000CC3 RID: 3267 RVA: 0x00029890 File Offset: 0x00027A90
		private void Awake()
		{
			this._runEase = new EasingFunction(this._runEaseMethod);
			this.defaultMaterial = new Material(Shader.Find("Sprites/Default"));
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000298B8 File Offset: 0x00027AB8
		public void Run(Chronometer chronometer)
		{
			this.ActiveChange = true;
			this.tempMaterial = new Material(Shader.Find("2DxFX/Standard/ColorRGB"));
			this.tempMaterial.hideFlags = HideFlags.None;
			this._renderer.sharedMaterial = this.tempMaterial;
			this._renderer.sharedMaterial.SetFloat("_Alpha", 1f - this._Alpha);
			this._renderer.sharedMaterial.SetFloat("_ColorR", this._ColorR);
			this._renderer.sharedMaterial.SetFloat("_ColorG", this._ColorG);
			this._renderer.sharedMaterial.SetFloat("_ColorB", this._ColorB);
			base.StartCoroutine(this.ChangeColor(chronometer));
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002997E File Offset: 0x00027B7E
		private IEnumerator ChangeColor(Chronometer chronometer)
		{
			float t = 0f;
			do
			{
				yield return null;
				this._renderer.sharedMaterial.SetFloat("_ColorR", this._runEase.function(this._ColorR, 0f, t / this.runTime));
				this._renderer.sharedMaterial.SetFloat("_ColorG", this._runEase.function(this._ColorG, 0f, t / this.runTime));
				this._renderer.sharedMaterial.SetFloat("_ColorB", this._runEase.function(this._ColorB, 0f, t / this.runTime));
				t += chronometer.deltaTime;
			}
			while (t <= this.runTime);
			this._renderer.sharedMaterial = this.defaultMaterial;
			this.ActiveChange = false;
			yield break;
		}

		// Token: 0x04000AEA RID: 2794
		private const string shader = "2DxFX/Standard/ColorRGB";

		// Token: 0x04000AEB RID: 2795
		[NonSerialized]
		public bool ActiveChange = true;

		// Token: 0x04000AEC RID: 2796
		[NonSerialized]
		public bool AddShadow = true;

		// Token: 0x04000AED RID: 2797
		[NonSerialized]
		public bool ReceivedShadow;

		// Token: 0x04000AEE RID: 2798
		[NonSerialized]
		public int BlendMode;

		// Token: 0x04000AEF RID: 2799
		[SerializeField]
		[Range(0f, 1f)]
		public float _Alpha = 1f;

		// Token: 0x04000AF0 RID: 2800
		[SerializeField]
		[Range(-1f, 1f)]
		public float _ColorR = 1f;

		// Token: 0x04000AF1 RID: 2801
		[SerializeField]
		[Range(-1f, 1f)]
		public float _ColorG = 1f;

		// Token: 0x04000AF2 RID: 2802
		[SerializeField]
		[Range(-1f, 1f)]
		public float _ColorB = 1f;

		// Token: 0x04000AF3 RID: 2803
		[SerializeField]
		public float runTime = 0.2f;

		// Token: 0x04000AF4 RID: 2804
		[SerializeField]
		private EasingFunction.Method _runEaseMethod = EasingFunction.Method.Linear;

		// Token: 0x04000AF5 RID: 2805
		[SerializeField]
		private Renderer _renderer;

		// Token: 0x04000AF6 RID: 2806
		private EasingFunction _runEase;

		// Token: 0x04000AF7 RID: 2807
		private float elapsedTime;

		// Token: 0x04000AF8 RID: 2808
		private Material tempMaterial;

		// Token: 0x04000AF9 RID: 2809
		private Material defaultMaterial;
	}
}
