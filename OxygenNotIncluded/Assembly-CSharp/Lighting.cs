using System;
using UnityEngine;

// Token: 0x02000915 RID: 2325
[ExecuteInEditMode]
public class Lighting : MonoBehaviour
{
	// Token: 0x06004376 RID: 17270 RVA: 0x001795A0 File Offset: 0x001777A0
	private void Awake()
	{
		Lighting.Instance = this;
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x001795A8 File Offset: 0x001777A8
	private void OnDestroy()
	{
		Lighting.Instance = null;
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x001795B0 File Offset: 0x001777B0
	private Color PremultiplyAlpha(Color c)
	{
		return c * c.a;
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x001795BE File Offset: 0x001777BE
	private void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x001795C6 File Offset: 0x001777C6
	private void Update()
	{
		this.UpdateLighting();
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x001795D0 File Offset: 0x001777D0
	private void UpdateLighting()
	{
		Shader.SetGlobalInt(Lighting._liquidZ, -28);
		Shader.SetGlobalVector(Lighting._DigMapMapParameters, new Vector4(this.Settings.DigMapColour.r, this.Settings.DigMapColour.g, this.Settings.DigMapColour.b, this.Settings.DigMapScale));
		Shader.SetGlobalTexture(Lighting._DigDamageMap, this.Settings.DigDamageMap);
		Shader.SetGlobalTexture(Lighting._StateTransitionMap, this.Settings.StateTransitionMap);
		Shader.SetGlobalColor(Lighting._StateTransitionColor, this.Settings.StateTransitionColor);
		Shader.SetGlobalVector(Lighting._StateTransitionParameters, new Vector4(1f / this.Settings.StateTransitionUVScale, this.Settings.StateTransitionUVOffsetRate.x, this.Settings.StateTransitionUVOffsetRate.y, 0f));
		Shader.SetGlobalTexture(Lighting._FallingSolidMap, this.Settings.FallingSolidMap);
		Shader.SetGlobalColor(Lighting._FallingSolidColor, this.Settings.FallingSolidColor);
		Shader.SetGlobalVector(Lighting._FallingSolidParameters, new Vector4(1f / this.Settings.FallingSolidUVScale, this.Settings.FallingSolidUVOffsetRate.x, this.Settings.FallingSolidUVOffsetRate.y, 0f));
		Shader.SetGlobalColor(Lighting._WaterTrimColor, this.Settings.WaterTrimColor);
		Shader.SetGlobalVector(Lighting._WaterParameters2, new Vector4(this.Settings.WaterTrimSize, this.Settings.WaterAlphaTrimSize, 0f, this.Settings.WaterAlphaThreshold));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters, new Vector4(this.Settings.WaterWaveAmplitude, this.Settings.WaterWaveFrequency, this.Settings.WaterWaveSpeed, 0f));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters2, new Vector4(this.Settings.WaterWaveAmplitude2, this.Settings.WaterWaveFrequency2, this.Settings.WaterWaveSpeed2, 0f));
		Shader.SetGlobalVector(Lighting._WaterDetailParameters, new Vector4(this.Settings.WaterCubeMapScale, this.Settings.WaterDetailTiling, this.Settings.WaterColorScale, this.Settings.WaterDetailTiling2));
		Shader.SetGlobalVector(Lighting._WaterDistortionParameters, new Vector4(this.Settings.WaterDistortionScaleStart, this.Settings.WaterDistortionScaleEnd, this.Settings.WaterDepthColorOpacityStart, this.Settings.WaterDepthColorOpacityEnd));
		Shader.SetGlobalVector(Lighting._BloomParameters, new Vector4(this.Settings.BloomScale, 0f, 0f, 0f));
		Shader.SetGlobalVector(Lighting._LiquidParameters2, new Vector4(this.Settings.LiquidMin, this.Settings.LiquidMax, this.Settings.LiquidCutoff, this.Settings.LiquidTransparency));
		Shader.SetGlobalVector(Lighting._GridParameters, new Vector4(this.Settings.GridLineWidth, this.Settings.GridSize, this.Settings.GridMinIntensity, this.Settings.GridMaxIntensity));
		Shader.SetGlobalColor(Lighting._GridColor, this.Settings.GridColor);
		Shader.SetGlobalVector(Lighting._EdgeGlowParameters, new Vector4(this.Settings.EdgeGlowCutoffStart, this.Settings.EdgeGlowCutoffEnd, this.Settings.EdgeGlowIntensity, 0f));
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, Vector4.one);
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, Vector4.one);
		}
		else
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, new Vector4(this.Settings.substanceEdgeParameters.intensity, this.Settings.substanceEdgeParameters.edgeIntensity, this.Settings.substanceEdgeParameters.directSunlightScale, this.Settings.substanceEdgeParameters.power));
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, new Vector4(this.Settings.tileEdgeParameters.intensity, this.Settings.tileEdgeParameters.edgeIntensity, this.Settings.tileEdgeParameters.directSunlightScale, this.Settings.tileEdgeParameters.power));
		}
		float w = (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() == OverlayModes.Disease.ID) ? 1f : 0f;
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(1f, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		else
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(this.Settings.AnimIntensity, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		Shader.SetGlobalVector(Lighting._GasOpacity, new Vector4(this.Settings.GasMinOpacity, this.Settings.GasMaxOpacity, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DarkenTintBackground, this.Settings.DarkenTints[0]);
		Shader.SetGlobalColor(Lighting._DarkenTintMidground, this.Settings.DarkenTints[1]);
		Shader.SetGlobalColor(Lighting._DarkenTintForeground, this.Settings.DarkenTints[2]);
		Shader.SetGlobalColor(Lighting._BrightenOverlay, this.Settings.BrightenOverlayColour);
		Shader.SetGlobalColor(Lighting._ColdFG, this.PremultiplyAlpha(this.Settings.ColdColours[2]));
		Shader.SetGlobalColor(Lighting._ColdMG, this.PremultiplyAlpha(this.Settings.ColdColours[1]));
		Shader.SetGlobalColor(Lighting._ColdBG, this.PremultiplyAlpha(this.Settings.ColdColours[0]));
		Shader.SetGlobalColor(Lighting._HotFG, this.PremultiplyAlpha(this.Settings.HotColours[2]));
		Shader.SetGlobalColor(Lighting._HotMG, this.PremultiplyAlpha(this.Settings.HotColours[1]));
		Shader.SetGlobalColor(Lighting._HotBG, this.PremultiplyAlpha(this.Settings.HotColours[0]));
		Shader.SetGlobalVector(Lighting._TemperatureParallax, this.Settings.TemperatureParallax);
		Shader.SetGlobalVector(Lighting._ColdUVOffset1, new Vector4(this.Settings.ColdBGUVOffset.x, this.Settings.ColdBGUVOffset.y, this.Settings.ColdMGUVOffset.x, this.Settings.ColdMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._ColdUVOffset2, new Vector4(this.Settings.ColdFGUVOffset.x, this.Settings.ColdFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalVector(Lighting._HotUVOffset1, new Vector4(this.Settings.HotBGUVOffset.x, this.Settings.HotBGUVOffset.y, this.Settings.HotMGUVOffset.x, this.Settings.HotMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._HotUVOffset2, new Vector4(this.Settings.HotFGUVOffset.x, this.Settings.HotFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DustColour, this.PremultiplyAlpha(this.Settings.DustColour));
		Shader.SetGlobalVector(Lighting._DustInfo, new Vector4(this.Settings.DustScale, this.Settings.DustMovement.x, this.Settings.DustMovement.y, this.Settings.DustMovement.z));
		Shader.SetGlobalTexture(Lighting._DustTex, this.Settings.DustTex);
		Shader.SetGlobalVector(Lighting._DebugShowInfo, new Vector4(this.Settings.ShowDust, this.Settings.ShowGas, this.Settings.ShowShadow, this.Settings.ShowTemperature));
		Shader.SetGlobalVector(Lighting._HeatHazeParameters, this.Settings.HeatHazeParameters);
		Shader.SetGlobalTexture(Lighting._HeatHazeTexture, this.Settings.HeatHazeTexture);
		Shader.SetGlobalVector(Lighting._ShineParams, new Vector4(this.Settings.ShineCenter.x, this.Settings.ShineCenter.y, this.Settings.ShineRange.x, this.Settings.ShineRange.y));
		Shader.SetGlobalVector(Lighting._ShineParams2, new Vector4(this.Settings.ShineZoomSpeed, 0f, 0f, 0f));
		Shader.SetGlobalFloat(Lighting._WorldZoneGasBlend, this.Settings.WorldZoneGasBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneLiquidBlend, this.Settings.WorldZoneLiquidBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneForegroundBlend, this.Settings.WorldZoneForegroundBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneSimpleAnimBlend, this.Settings.WorldZoneSimpleAnimBlend);
		Shader.SetGlobalColor(Lighting._CharacterLitColour, this.Settings.characterLighting.litColour);
		Shader.SetGlobalColor(Lighting._CharacterUnlitColour, this.Settings.characterLighting.unlitColour);
		Shader.SetGlobalTexture(Lighting._BuildingDamagedTex, this.Settings.BuildingDamagedTex);
		Shader.SetGlobalVector(Lighting._BuildingDamagedUVParameters, this.Settings.BuildingDamagedUVParameters);
		Shader.SetGlobalTexture(Lighting._DiseaseOverlayTex, this.Settings.DiseaseOverlayTex);
		Shader.SetGlobalVector(Lighting._DiseaseOverlayTexInfo, this.Settings.DiseaseOverlayTexInfo);
		if (this.Settings.ShowRadiation)
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, this.PremultiplyAlpha(this.Settings.RadColor));
		}
		else
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, Color.clear);
		}
		Shader.SetGlobalVector(Lighting._RadUVOffset1, new Vector4(this.Settings.Rad1UVOffset.x, this.Settings.Rad1UVOffset.y, this.Settings.Rad2UVOffset.x, this.Settings.Rad2UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVOffset2, new Vector4(this.Settings.Rad3UVOffset.x, this.Settings.Rad3UVOffset.y, this.Settings.Rad4UVOffset.x, this.Settings.Rad4UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVScales, new Vector4(1f / this.Settings.RadUVScales.x, 1f / this.Settings.RadUVScales.y, 1f / this.Settings.RadUVScales.z, 1f / this.Settings.RadUVScales.w));
		Shader.SetGlobalVector(Lighting._RadRange1, new Vector4(this.Settings.Rad1Range.x, this.Settings.Rad1Range.y, this.Settings.Rad2Range.x, this.Settings.Rad2Range.y));
		Shader.SetGlobalVector(Lighting._RadRange2, new Vector4(this.Settings.Rad3Range.x, this.Settings.Rad3Range.y, this.Settings.Rad4Range.x, this.Settings.Rad4Range.y));
		if (LightBuffer.Instance != null && LightBuffer.Instance.Texture != null)
		{
			Shader.SetGlobalTexture(Lighting._LightBufferTex, LightBuffer.Instance.Texture);
		}
	}

	// Token: 0x04002BF7 RID: 11255
	public global::LightingSettings Settings;

	// Token: 0x04002BF8 RID: 11256
	public static Lighting Instance;

	// Token: 0x04002BF9 RID: 11257
	[NonSerialized]
	public bool disableLighting;

	// Token: 0x04002BFA RID: 11258
	private static int _liquidZ = Shader.PropertyToID("_LiquidZ");

	// Token: 0x04002BFB RID: 11259
	private static int _DigMapMapParameters = Shader.PropertyToID("_DigMapMapParameters");

	// Token: 0x04002BFC RID: 11260
	private static int _DigDamageMap = Shader.PropertyToID("_DigDamageMap");

	// Token: 0x04002BFD RID: 11261
	private static int _StateTransitionMap = Shader.PropertyToID("_StateTransitionMap");

	// Token: 0x04002BFE RID: 11262
	private static int _StateTransitionColor = Shader.PropertyToID("_StateTransitionColor");

	// Token: 0x04002BFF RID: 11263
	private static int _StateTransitionParameters = Shader.PropertyToID("_StateTransitionParameters");

	// Token: 0x04002C00 RID: 11264
	private static int _FallingSolidMap = Shader.PropertyToID("_FallingSolidMap");

	// Token: 0x04002C01 RID: 11265
	private static int _FallingSolidColor = Shader.PropertyToID("_FallingSolidColor");

	// Token: 0x04002C02 RID: 11266
	private static int _FallingSolidParameters = Shader.PropertyToID("_FallingSolidParameters");

	// Token: 0x04002C03 RID: 11267
	private static int _WaterTrimColor = Shader.PropertyToID("_WaterTrimColor");

	// Token: 0x04002C04 RID: 11268
	private static int _WaterParameters2 = Shader.PropertyToID("_WaterParameters2");

	// Token: 0x04002C05 RID: 11269
	private static int _WaterWaveParameters = Shader.PropertyToID("_WaterWaveParameters");

	// Token: 0x04002C06 RID: 11270
	private static int _WaterWaveParameters2 = Shader.PropertyToID("_WaterWaveParameters2");

	// Token: 0x04002C07 RID: 11271
	private static int _WaterDetailParameters = Shader.PropertyToID("_WaterDetailParameters");

	// Token: 0x04002C08 RID: 11272
	private static int _WaterDistortionParameters = Shader.PropertyToID("_WaterDistortionParameters");

	// Token: 0x04002C09 RID: 11273
	private static int _BloomParameters = Shader.PropertyToID("_BloomParameters");

	// Token: 0x04002C0A RID: 11274
	private static int _LiquidParameters2 = Shader.PropertyToID("_LiquidParameters2");

	// Token: 0x04002C0B RID: 11275
	private static int _GridParameters = Shader.PropertyToID("_GridParameters");

	// Token: 0x04002C0C RID: 11276
	private static int _GridColor = Shader.PropertyToID("_GridColor");

	// Token: 0x04002C0D RID: 11277
	private static int _EdgeGlowParameters = Shader.PropertyToID("_EdgeGlowParameters");

	// Token: 0x04002C0E RID: 11278
	private static int _SubstanceParameters = Shader.PropertyToID("_SubstanceParameters");

	// Token: 0x04002C0F RID: 11279
	private static int _TileEdgeParameters = Shader.PropertyToID("_TileEdgeParameters");

	// Token: 0x04002C10 RID: 11280
	private static int _AnimParameters = Shader.PropertyToID("_AnimParameters");

	// Token: 0x04002C11 RID: 11281
	private static int _GasOpacity = Shader.PropertyToID("_GasOpacity");

	// Token: 0x04002C12 RID: 11282
	private static int _DarkenTintBackground = Shader.PropertyToID("_DarkenTintBackground");

	// Token: 0x04002C13 RID: 11283
	private static int _DarkenTintMidground = Shader.PropertyToID("_DarkenTintMidground");

	// Token: 0x04002C14 RID: 11284
	private static int _DarkenTintForeground = Shader.PropertyToID("_DarkenTintForeground");

	// Token: 0x04002C15 RID: 11285
	private static int _BrightenOverlay = Shader.PropertyToID("_BrightenOverlay");

	// Token: 0x04002C16 RID: 11286
	private static int _ColdFG = Shader.PropertyToID("_ColdFG");

	// Token: 0x04002C17 RID: 11287
	private static int _ColdMG = Shader.PropertyToID("_ColdMG");

	// Token: 0x04002C18 RID: 11288
	private static int _ColdBG = Shader.PropertyToID("_ColdBG");

	// Token: 0x04002C19 RID: 11289
	private static int _HotFG = Shader.PropertyToID("_HotFG");

	// Token: 0x04002C1A RID: 11290
	private static int _HotMG = Shader.PropertyToID("_HotMG");

	// Token: 0x04002C1B RID: 11291
	private static int _HotBG = Shader.PropertyToID("_HotBG");

	// Token: 0x04002C1C RID: 11292
	private static int _TemperatureParallax = Shader.PropertyToID("_TemperatureParallax");

	// Token: 0x04002C1D RID: 11293
	private static int _ColdUVOffset1 = Shader.PropertyToID("_ColdUVOffset1");

	// Token: 0x04002C1E RID: 11294
	private static int _ColdUVOffset2 = Shader.PropertyToID("_ColdUVOffset2");

	// Token: 0x04002C1F RID: 11295
	private static int _HotUVOffset1 = Shader.PropertyToID("_HotUVOffset1");

	// Token: 0x04002C20 RID: 11296
	private static int _HotUVOffset2 = Shader.PropertyToID("_HotUVOffset2");

	// Token: 0x04002C21 RID: 11297
	private static int _DustColour = Shader.PropertyToID("_DustColour");

	// Token: 0x04002C22 RID: 11298
	private static int _DustInfo = Shader.PropertyToID("_DustInfo");

	// Token: 0x04002C23 RID: 11299
	private static int _DustTex = Shader.PropertyToID("_DustTex");

	// Token: 0x04002C24 RID: 11300
	private static int _DebugShowInfo = Shader.PropertyToID("_DebugShowInfo");

	// Token: 0x04002C25 RID: 11301
	private static int _HeatHazeParameters = Shader.PropertyToID("_HeatHazeParameters");

	// Token: 0x04002C26 RID: 11302
	private static int _HeatHazeTexture = Shader.PropertyToID("_HeatHazeTexture");

	// Token: 0x04002C27 RID: 11303
	private static int _ShineParams = Shader.PropertyToID("_ShineParams");

	// Token: 0x04002C28 RID: 11304
	private static int _ShineParams2 = Shader.PropertyToID("_ShineParams2");

	// Token: 0x04002C29 RID: 11305
	private static int _WorldZoneGasBlend = Shader.PropertyToID("_WorldZoneGasBlend");

	// Token: 0x04002C2A RID: 11306
	private static int _WorldZoneLiquidBlend = Shader.PropertyToID("_WorldZoneLiquidBlend");

	// Token: 0x04002C2B RID: 11307
	private static int _WorldZoneForegroundBlend = Shader.PropertyToID("_WorldZoneForegroundBlend");

	// Token: 0x04002C2C RID: 11308
	private static int _WorldZoneSimpleAnimBlend = Shader.PropertyToID("_WorldZoneSimpleAnimBlend");

	// Token: 0x04002C2D RID: 11309
	private static int _CharacterLitColour = Shader.PropertyToID("_CharacterLitColour");

	// Token: 0x04002C2E RID: 11310
	private static int _CharacterUnlitColour = Shader.PropertyToID("_CharacterUnlitColour");

	// Token: 0x04002C2F RID: 11311
	private static int _BuildingDamagedTex = Shader.PropertyToID("_BuildingDamagedTex");

	// Token: 0x04002C30 RID: 11312
	private static int _BuildingDamagedUVParameters = Shader.PropertyToID("_BuildingDamagedUVParameters");

	// Token: 0x04002C31 RID: 11313
	private static int _DiseaseOverlayTex = Shader.PropertyToID("_DiseaseOverlayTex");

	// Token: 0x04002C32 RID: 11314
	private static int _DiseaseOverlayTexInfo = Shader.PropertyToID("_DiseaseOverlayTexInfo");

	// Token: 0x04002C33 RID: 11315
	private static int _RadHazeColor = Shader.PropertyToID("_RadHazeColor");

	// Token: 0x04002C34 RID: 11316
	private static int _RadUVOffset1 = Shader.PropertyToID("_RadUVOffset1");

	// Token: 0x04002C35 RID: 11317
	private static int _RadUVOffset2 = Shader.PropertyToID("_RadUVOffset2");

	// Token: 0x04002C36 RID: 11318
	private static int _RadUVScales = Shader.PropertyToID("_RadUVScales");

	// Token: 0x04002C37 RID: 11319
	private static int _RadRange1 = Shader.PropertyToID("_RadRange1");

	// Token: 0x04002C38 RID: 11320
	private static int _RadRange2 = Shader.PropertyToID("_RadRange2");

	// Token: 0x04002C39 RID: 11321
	private static int _LightBufferTex = Shader.PropertyToID("_LightBufferTex");
}
