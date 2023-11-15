using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000082 RID: 130
public class GameSceneReferenceHandler : MonoBehaviour
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000B455 File Offset: 0x00009655
	public MeshRenderer Renderer_Ground
	{
		get
		{
			return this.renderer_Ground;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000B45D File Offset: 0x0000965D
	public Light DirectionLight
	{
		get
		{
			return this.directionLight;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000B465 File Offset: 0x00009665
	public EnvSceneSettingData EnvSceneSettingData_Day
	{
		get
		{
			return this.envSceneSettingData_Day;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000B46D File Offset: 0x0000966D
	public EnvSceneSettingData EnvSceneSettingData_Corrupted
	{
		get
		{
			return this.envSceneSettingData_Corrupted;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000B475 File Offset: 0x00009675
	public Material Mat_RenderFeatureSceneFog
	{
		get
		{
			return this.mat_RenderFeatureSceneFog;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000B47D File Offset: 0x0000967D
	public List<MonsterSpawner> List_MonsterSpawners
	{
		get
		{
			return this.list_MonsterSpawners;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000B485 File Offset: 0x00009685
	public Vector3 InitialCameraOffset
	{
		get
		{
			return this.initialCameraOffset;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000B48D File Offset: 0x0000968D
	public float OverrideCameraFOV
	{
		get
		{
			return this.overrideCameraFOV;
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0000B495 File Offset: 0x00009695
	private void Start()
	{
		EventMgr.SendEvent<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, this);
		this.RollWeatherEffect();
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000B4AC File Offset: 0x000096AC
	private void RollWeatherEffect()
	{
		if (this.list_WeatherEffects.Count == 0)
		{
			return;
		}
		bool isCorrupted = GameDataManager.instance.IntermediateData.isCorrupted;
		foreach (GameSceneReferenceHandler.WeatherEffect weatherEffect in this.list_WeatherEffects)
		{
			if (weatherEffect.obj_Effect != null)
			{
				weatherEffect.obj_Effect.SetActive(false);
			}
		}
		List<GameSceneReferenceHandler.WeatherEffect> list;
		if (isCorrupted)
		{
			list = this.list_WeatherEffects.FindAll((GameSceneReferenceHandler.WeatherEffect x) => x.enableType == GameSceneReferenceHandler.eEnableRule.CORRUPTED_ONLY || x.enableType == GameSceneReferenceHandler.eEnableRule.NO_LIMIT);
		}
		else
		{
			list = this.list_WeatherEffects.FindAll((GameSceneReferenceHandler.WeatherEffect x) => x.enableType == GameSceneReferenceHandler.eEnableRule.NORMAL_ONLY || x.enableType == GameSceneReferenceHandler.eEnableRule.NO_LIMIT);
		}
		WeightedRandom<GameSceneReferenceHandler.WeatherEffect> weightedRandom = new WeightedRandom<GameSceneReferenceHandler.WeatherEffect>();
		foreach (GameSceneReferenceHandler.WeatherEffect weatherEffect2 in list)
		{
			weightedRandom.AddItem(weatherEffect2, weatherEffect2.weight);
		}
		GameSceneReferenceHandler.WeatherEffect randomResult = weightedRandom.GetRandomResult();
		if (randomResult != null && randomResult.obj_Effect != null)
		{
			randomResult.obj_Effect.SetActive(true);
		}
	}

	// Token: 0x0400032A RID: 810
	[SerializeField]
	private MeshRenderer renderer_Ground;

	// Token: 0x0400032B RID: 811
	[SerializeField]
	[Header("變色範圍")]
	private Vector2 groundTintRange = new Vector2(5f, 15f);

	// Token: 0x0400032C RID: 812
	[SerializeField]
	[Header("顏色")]
	private Color groundTintColor = Color.white;

	// Token: 0x0400032D RID: 813
	[SerializeField]
	private Light directionLight;

	// Token: 0x0400032E RID: 814
	[SerializeField]
	[Header("一般場景")]
	private EnvSceneSettingData envSceneSettingData_Day;

	// Token: 0x0400032F RID: 815
	[SerializeField]
	[Header("腐化場景")]
	[FormerlySerializedAs("envSceneSettingData_Night")]
	private EnvSceneSettingData envSceneSettingData_Corrupted;

	// Token: 0x04000330 RID: 816
	[SerializeField]
	[Header("一般煙霧")]
	private Material mat_RenderFeatureSceneFog;

	// Token: 0x04000331 RID: 817
	[SerializeField]
	private List<MonsterSpawner> list_MonsterSpawners;

	// Token: 0x04000332 RID: 818
	[SerializeField]
	private Vector3 initialCameraOffset;

	// Token: 0x04000333 RID: 819
	[SerializeField]
	private float overrideCameraFOV = 20f;

	// Token: 0x04000334 RID: 820
	[SerializeField]
	private List<GameSceneReferenceHandler.WeatherEffect> list_WeatherEffects;

	// Token: 0x020001F0 RID: 496
	public enum eEnableRule
	{
		// Token: 0x040009F7 RID: 2551
		[InspectorName("永遠啟用")]
		NO_LIMIT,
		// Token: 0x040009F8 RID: 2552
		[InspectorName("正常關限定")]
		NORMAL_ONLY,
		// Token: 0x040009F9 RID: 2553
		[InspectorName("腐化關限定")]
		CORRUPTED_ONLY
	}

	// Token: 0x020001F1 RID: 497
	[Serializable]
	public class WeatherEffect
	{
		// Token: 0x040009FA RID: 2554
		public GameObject obj_Effect;

		// Token: 0x040009FB RID: 2555
		public GameSceneReferenceHandler.eEnableRule enableType;

		// Token: 0x040009FC RID: 2556
		public int weight = 1;
	}
}
