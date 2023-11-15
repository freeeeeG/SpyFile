using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000089 RID: 137
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0002154E File Offset: 0x0001F94E
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00021571 File Offset: 0x0001F971
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0002157C File Offset: 0x0001F97C
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000215C8 File Offset: 0x0001F9C8
		public override void Prepare(Material uberMaterial)
		{
			if (++this.textureIndex >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x04000244 RID: 580
		private Texture2D[] noiseTextures;

		// Token: 0x04000245 RID: 581
		private int textureIndex;

		// Token: 0x04000246 RID: 582
		private const int k_TextureCount = 64;

		// Token: 0x0200008A RID: 138
		private static class Uniforms
		{
			// Token: 0x04000247 RID: 583
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04000248 RID: 584
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
