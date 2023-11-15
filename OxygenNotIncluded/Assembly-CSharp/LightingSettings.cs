using System;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class LightingSettings : ScriptableObject
{
	// Token: 0x04002C41 RID: 11329
	[Header("Global")]
	public bool UpdateLightSettings;

	// Token: 0x04002C42 RID: 11330
	public float BloomScale;

	// Token: 0x04002C43 RID: 11331
	public Color32 LightColour = Color.white;

	// Token: 0x04002C44 RID: 11332
	[Header("Digging")]
	public float DigMapScale;

	// Token: 0x04002C45 RID: 11333
	public Color DigMapColour;

	// Token: 0x04002C46 RID: 11334
	public Texture2D DigDamageMap;

	// Token: 0x04002C47 RID: 11335
	[Header("State Transition")]
	public Texture2D StateTransitionMap;

	// Token: 0x04002C48 RID: 11336
	public Color StateTransitionColor;

	// Token: 0x04002C49 RID: 11337
	public float StateTransitionUVScale;

	// Token: 0x04002C4A RID: 11338
	public Vector2 StateTransitionUVOffsetRate;

	// Token: 0x04002C4B RID: 11339
	[Header("Falling Solids")]
	public Texture2D FallingSolidMap;

	// Token: 0x04002C4C RID: 11340
	public Color FallingSolidColor;

	// Token: 0x04002C4D RID: 11341
	public float FallingSolidUVScale;

	// Token: 0x04002C4E RID: 11342
	public Vector2 FallingSolidUVOffsetRate;

	// Token: 0x04002C4F RID: 11343
	[Header("Metal Shine")]
	public Vector2 ShineCenter;

	// Token: 0x04002C50 RID: 11344
	public Vector2 ShineRange;

	// Token: 0x04002C51 RID: 11345
	public float ShineZoomSpeed;

	// Token: 0x04002C52 RID: 11346
	[Header("Water")]
	public Color WaterTrimColor;

	// Token: 0x04002C53 RID: 11347
	public float WaterTrimSize;

	// Token: 0x04002C54 RID: 11348
	public float WaterAlphaTrimSize;

	// Token: 0x04002C55 RID: 11349
	public float WaterAlphaThreshold;

	// Token: 0x04002C56 RID: 11350
	public float WaterCubesAlphaThreshold;

	// Token: 0x04002C57 RID: 11351
	public float WaterWaveAmplitude;

	// Token: 0x04002C58 RID: 11352
	public float WaterWaveFrequency;

	// Token: 0x04002C59 RID: 11353
	public float WaterWaveSpeed;

	// Token: 0x04002C5A RID: 11354
	public float WaterDetailSpeed;

	// Token: 0x04002C5B RID: 11355
	public float WaterDetailTiling;

	// Token: 0x04002C5C RID: 11356
	public float WaterDetailTiling2;

	// Token: 0x04002C5D RID: 11357
	public Vector2 WaterDetailDirection;

	// Token: 0x04002C5E RID: 11358
	public float WaterWaveAmplitude2;

	// Token: 0x04002C5F RID: 11359
	public float WaterWaveFrequency2;

	// Token: 0x04002C60 RID: 11360
	public float WaterWaveSpeed2;

	// Token: 0x04002C61 RID: 11361
	public float WaterCubeMapScale;

	// Token: 0x04002C62 RID: 11362
	public float WaterColorScale;

	// Token: 0x04002C63 RID: 11363
	public float WaterDistortionScaleStart;

	// Token: 0x04002C64 RID: 11364
	public float WaterDistortionScaleEnd;

	// Token: 0x04002C65 RID: 11365
	public float WaterDepthColorOpacityStart;

	// Token: 0x04002C66 RID: 11366
	public float WaterDepthColorOpacityEnd;

	// Token: 0x04002C67 RID: 11367
	[Header("Liquid")]
	public float LiquidMin;

	// Token: 0x04002C68 RID: 11368
	public float LiquidMax;

	// Token: 0x04002C69 RID: 11369
	public float LiquidCutoff;

	// Token: 0x04002C6A RID: 11370
	public float LiquidTransparency;

	// Token: 0x04002C6B RID: 11371
	public float LiquidAmountOffset;

	// Token: 0x04002C6C RID: 11372
	public float LiquidMaxMass;

	// Token: 0x04002C6D RID: 11373
	[Header("Grid")]
	public float GridLineWidth;

	// Token: 0x04002C6E RID: 11374
	public float GridSize;

	// Token: 0x04002C6F RID: 11375
	public float GridMaxIntensity;

	// Token: 0x04002C70 RID: 11376
	public float GridMinIntensity;

	// Token: 0x04002C71 RID: 11377
	public Color GridColor;

	// Token: 0x04002C72 RID: 11378
	[Header("Terrain")]
	public float EdgeGlowCutoffStart;

	// Token: 0x04002C73 RID: 11379
	public float EdgeGlowCutoffEnd;

	// Token: 0x04002C74 RID: 11380
	public float EdgeGlowIntensity;

	// Token: 0x04002C75 RID: 11381
	public int BackgroundLayers;

	// Token: 0x04002C76 RID: 11382
	public float BackgroundBaseParallax;

	// Token: 0x04002C77 RID: 11383
	public float BackgroundLayerParallax;

	// Token: 0x04002C78 RID: 11384
	public float BackgroundDarkening;

	// Token: 0x04002C79 RID: 11385
	public float BackgroundClip;

	// Token: 0x04002C7A RID: 11386
	public float BackgroundUVScale;

	// Token: 0x04002C7B RID: 11387
	public global::LightingSettings.EdgeLighting substanceEdgeParameters;

	// Token: 0x04002C7C RID: 11388
	public global::LightingSettings.EdgeLighting tileEdgeParameters;

	// Token: 0x04002C7D RID: 11389
	public float AnimIntensity;

	// Token: 0x04002C7E RID: 11390
	public float GasMinOpacity;

	// Token: 0x04002C7F RID: 11391
	public float GasMaxOpacity;

	// Token: 0x04002C80 RID: 11392
	public Color[] DarkenTints;

	// Token: 0x04002C81 RID: 11393
	public global::LightingSettings.LightingColours characterLighting;

	// Token: 0x04002C82 RID: 11394
	public Color BrightenOverlayColour;

	// Token: 0x04002C83 RID: 11395
	public Color[] ColdColours;

	// Token: 0x04002C84 RID: 11396
	public Color[] HotColours;

	// Token: 0x04002C85 RID: 11397
	[Header("Temperature Overlay Effects")]
	public Vector4 TemperatureParallax;

	// Token: 0x04002C86 RID: 11398
	public Texture2D EmberTex;

	// Token: 0x04002C87 RID: 11399
	public Texture2D FrostTex;

	// Token: 0x04002C88 RID: 11400
	public Texture2D Thermal1Tex;

	// Token: 0x04002C89 RID: 11401
	public Texture2D Thermal2Tex;

	// Token: 0x04002C8A RID: 11402
	public Vector2 ColdFGUVOffset;

	// Token: 0x04002C8B RID: 11403
	public Vector2 ColdMGUVOffset;

	// Token: 0x04002C8C RID: 11404
	public Vector2 ColdBGUVOffset;

	// Token: 0x04002C8D RID: 11405
	public Vector2 HotFGUVOffset;

	// Token: 0x04002C8E RID: 11406
	public Vector2 HotMGUVOffset;

	// Token: 0x04002C8F RID: 11407
	public Vector2 HotBGUVOffset;

	// Token: 0x04002C90 RID: 11408
	public Texture2D DustTex;

	// Token: 0x04002C91 RID: 11409
	public Color DustColour;

	// Token: 0x04002C92 RID: 11410
	public float DustScale;

	// Token: 0x04002C93 RID: 11411
	public Vector3 DustMovement;

	// Token: 0x04002C94 RID: 11412
	public float ShowGas;

	// Token: 0x04002C95 RID: 11413
	public float ShowTemperature;

	// Token: 0x04002C96 RID: 11414
	public float ShowDust;

	// Token: 0x04002C97 RID: 11415
	public float ShowShadow;

	// Token: 0x04002C98 RID: 11416
	public Vector4 HeatHazeParameters;

	// Token: 0x04002C99 RID: 11417
	public Texture2D HeatHazeTexture;

	// Token: 0x04002C9A RID: 11418
	[Header("Biome")]
	public float WorldZoneGasBlend;

	// Token: 0x04002C9B RID: 11419
	public float WorldZoneLiquidBlend;

	// Token: 0x04002C9C RID: 11420
	public float WorldZoneForegroundBlend;

	// Token: 0x04002C9D RID: 11421
	public float WorldZoneSimpleAnimBlend;

	// Token: 0x04002C9E RID: 11422
	public float WorldZoneAnimBlend;

	// Token: 0x04002C9F RID: 11423
	[Header("FX")]
	public Color32 SmokeDamageTint;

	// Token: 0x04002CA0 RID: 11424
	[Header("Building Damage")]
	public Texture2D BuildingDamagedTex;

	// Token: 0x04002CA1 RID: 11425
	public Vector4 BuildingDamagedUVParameters;

	// Token: 0x04002CA2 RID: 11426
	[Header("Disease")]
	public Texture2D DiseaseOverlayTex;

	// Token: 0x04002CA3 RID: 11427
	public Vector4 DiseaseOverlayTexInfo;

	// Token: 0x04002CA4 RID: 11428
	[Header("Conduits")]
	public ConduitFlowVisualizer.Tuning GasConduit;

	// Token: 0x04002CA5 RID: 11429
	public ConduitFlowVisualizer.Tuning LiquidConduit;

	// Token: 0x04002CA6 RID: 11430
	public SolidConduitFlowVisualizer.Tuning SolidConduit;

	// Token: 0x04002CA7 RID: 11431
	[Header("Radiation Overlay")]
	public bool ShowRadiation;

	// Token: 0x04002CA8 RID: 11432
	public Texture2D Radiation1Tex;

	// Token: 0x04002CA9 RID: 11433
	public Texture2D Radiation2Tex;

	// Token: 0x04002CAA RID: 11434
	public Texture2D Radiation3Tex;

	// Token: 0x04002CAB RID: 11435
	public Texture2D Radiation4Tex;

	// Token: 0x04002CAC RID: 11436
	public Texture2D NoiseTex;

	// Token: 0x04002CAD RID: 11437
	public Color RadColor;

	// Token: 0x04002CAE RID: 11438
	public Vector2 Rad1UVOffset;

	// Token: 0x04002CAF RID: 11439
	public Vector2 Rad2UVOffset;

	// Token: 0x04002CB0 RID: 11440
	public Vector2 Rad3UVOffset;

	// Token: 0x04002CB1 RID: 11441
	public Vector2 Rad4UVOffset;

	// Token: 0x04002CB2 RID: 11442
	public Vector4 RadUVScales;

	// Token: 0x04002CB3 RID: 11443
	public Vector2 Rad1Range;

	// Token: 0x04002CB4 RID: 11444
	public Vector2 Rad2Range;

	// Token: 0x04002CB5 RID: 11445
	public Vector2 Rad3Range;

	// Token: 0x04002CB6 RID: 11446
	public Vector2 Rad4Range;

	// Token: 0x02001762 RID: 5986
	[Serializable]
	public struct EdgeLighting
	{
		// Token: 0x04006E8C RID: 28300
		public float intensity;

		// Token: 0x04006E8D RID: 28301
		public float edgeIntensity;

		// Token: 0x04006E8E RID: 28302
		public float directSunlightScale;

		// Token: 0x04006E8F RID: 28303
		public float power;
	}

	// Token: 0x02001763 RID: 5987
	public enum TintLayers
	{
		// Token: 0x04006E91 RID: 28305
		Background,
		// Token: 0x04006E92 RID: 28306
		Midground,
		// Token: 0x04006E93 RID: 28307
		Foreground,
		// Token: 0x04006E94 RID: 28308
		NumLayers
	}

	// Token: 0x02001764 RID: 5988
	[Serializable]
	public struct LightingColours
	{
		// Token: 0x04006E95 RID: 28309
		public Color32 litColour;

		// Token: 0x04006E96 RID: 28310
		public Color32 unlitColour;
	}
}
