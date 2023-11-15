using System;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CA RID: 714
	public class DemoRandomColorSwap : MonoBehaviour
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x00031D00 File Offset: 0x0002FF00
		private void Start()
		{
			if (base.GetComponent<SpriteRenderer>() != null)
			{
				this.mat = base.GetComponent<Renderer>().material;
				if (this.mat != null)
				{
					base.InvokeRepeating("NewColor", 0f, 0.6f);
					return;
				}
				Debug.LogError("No material found");
				Object.Destroy(this);
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00031D60 File Offset: 0x0002FF60
		private void NewColor()
		{
			this.mat.SetColor("_ColorSwapRed", this.GenerateColor());
			this.mat.SetColor("_ColorSwapGreen", this.GenerateColor());
			this.mat.SetColor("_ColorSwapBlue", this.GenerateColor());
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00031DAF File Offset: 0x0002FFAF
		private Color GenerateColor()
		{
			return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		}

		// Token: 0x040009B3 RID: 2483
		private Material mat;
	}
}
