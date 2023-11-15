using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000A0 RID: 160
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000245FC File Offset: 0x000229FC
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00024680 File Offset: 0x00022A80
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00024708 File Offset: 0x00022B08
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(position, settings.lut);
		}

		// Token: 0x020000A1 RID: 161
		private static class Uniforms
		{
			// Token: 0x040002E1 RID: 737
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x040002E2 RID: 738
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
