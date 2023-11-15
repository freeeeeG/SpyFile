using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class EnvironmentController : MonoBehaviour
{
	// Token: 0x06000288 RID: 648 RVA: 0x0000AC1B File Offset: 0x00008E1B
	private void Awake()
	{
		this.isCorruptedStage = GameDataManager.instance.IntermediateData.isCorrupted;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000AC32 File Offset: 0x00008E32
	private void Start()
	{
		this.curSceneTimeType = (this.isCorruptedStage ? eSceneTimeType.NIGHT : eSceneTimeType.DAYTIME);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000AC48 File Offset: 0x00008E48
	private void OnEnable()
	{
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Register<float>(eGameEvents.RoundTimeFastForwardToNight, new Action<float>(this.OnRoundTimeFastForwardToNight));
		EventMgr.Register<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
		EventMgr.Register<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000ACCC File Offset: 0x00008ECC
	private void OnDisable()
	{
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Remove<float>(eGameEvents.RoundTimeFastForwardToNight, new Action<float>(this.OnRoundTimeFastForwardToNight));
		EventMgr.Remove<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
		EventMgr.Remove<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000AD50 File Offset: 0x00008F50
	private void OnShowStageAnnounce(int index, float duration)
	{
		base.StartCoroutine(this.CR_IntroFogClearEffect(2.5f));
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000AD64 File Offset: 0x00008F64
	private IEnumerator CR_IntroFogClearEffect(float duration)
	{
		if (!this.isInitialized)
		{
			Debug.LogError("環境控制器尚未初始化, 無法執行煙霧消散效果");
		}
		float time = 0f;
		Vector2 fogEndDist = this.mat_RenderFeatureSceneFog.GetVector("_FogDistance");
		Vector2 v = Vector2.zero;
		while (time <= duration)
		{
			float t = time / duration;
			v = Vector2.Lerp(Vector2.zero, fogEndDist, t);
			this.mat_RenderFeatureSceneFog.SetVector("_FogDistance", v);
			yield return null;
			time += Time.deltaTime;
		}
		yield break;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000AD7A File Offset: 0x00008F7A
	private void OnRoundStart(int roundIndex, int totalRound)
	{
		this.curRoundIndex = roundIndex;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000AD84 File Offset: 0x00008F84
	private void OnUpdateEnvSceneBindings(GameSceneReferenceHandler refHandler)
	{
		this.light_SceneDirectional = refHandler.DirectionLight;
		this.settingData_Day = refHandler.EnvSceneSettingData_Day;
		this.settingData_Night = refHandler.EnvSceneSettingData_Corrupted;
		this.mat_RenderFeatureSceneFog.CopyPropertiesFromMaterial(refHandler.Mat_RenderFeatureSceneFog);
		if (this.isCorruptedStage)
		{
			this.mat_RenderFeatureSceneFog.SetColor("_FogColor_A", Color.black);
		}
		if (this.curSceneTimeType == eSceneTimeType.DAYTIME)
		{
			this.SetEnvironmentValue(this.settingData_Day, 0f, true, false);
		}
		else if (this.curSceneTimeType == eSceneTimeType.NIGHT)
		{
			this.SetEnvironmentValue(this.settingData_Night, 0f, false, false);
		}
		this.isInitialized = true;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000AE24 File Offset: 0x00009024
	private void OnUpdateRoundTimer(float time, float percentage)
	{
		if (this.curSceneTimeType == eSceneTimeType.DAYTIME)
		{
			this.SetEnvironmentValue(this.settingData_Day, percentage, false, this.curRoundIndex == 1);
			return;
		}
		if (this.curSceneTimeType == eSceneTimeType.NIGHT)
		{
			this.SetEnvironmentValue(this.settingData_Night, 0f, false, false);
		}
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000AE63 File Offset: 0x00009063
	private void OnRoundTimeFastForwardToNight(float curPercentage)
	{
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000AE65 File Offset: 0x00009065
	private IEnumerator CR_FastForwardToNight(float curPercentage)
	{
		float duration = 3f;
		for (float time = curPercentage * duration; time <= duration; time += Time.deltaTime)
		{
			float t = time / duration;
			this.SetEnvironmentValue(this.settingData_Day, t, true, false);
			yield return null;
		}
		this.SetEnvironmentValue(this.settingData_Day, 1f, true, false);
		yield break;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000AE7C File Offset: 0x0000907C
	private void SetEnvironmentValue(EnvSceneSettingData data, float t, bool isFastForward = false, bool isFirstDay = false)
	{
		this.light_SceneDirectional.transform.rotation = Quaternion.Euler(Vector3.Lerp(data.LightOrientation_Start, data.LightOrientation_End, t));
		if (isFirstDay)
		{
			this.light_SceneDirectional.color = data.Gradient_LightColor_FirstDay.Evaluate(t);
			this.light_SceneDirectional.intensity = data.Curve_LightIntensity.Evaluate(0.5f);
			this.light_SceneDirectional.shadowStrength = data.Curve_ShadowStrength.Evaluate(0.5f);
			return;
		}
		if (isFastForward)
		{
			this.light_SceneDirectional.color = data.Gradient_LightColor_FastForward.Evaluate(t);
			this.light_SceneDirectional.intensity = data.Curve_LightIntensity.Evaluate(t);
			this.light_SceneDirectional.shadowStrength = data.Curve_ShadowStrength.Evaluate(t);
			return;
		}
		this.light_SceneDirectional.color = data.Gradient_LightColor.Evaluate(t);
		this.light_SceneDirectional.intensity = data.Curve_LightIntensity.Evaluate(t);
		this.light_SceneDirectional.shadowStrength = data.Curve_ShadowStrength.Evaluate(t);
	}

	// Token: 0x04000304 RID: 772
	[SerializeField]
	private Light light_SceneDirectional;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private Material mat_RenderFeatureSceneFog;

	// Token: 0x04000306 RID: 774
	[SerializeField]
	private EnvSceneSettingData settingData_Day;

	// Token: 0x04000307 RID: 775
	[SerializeField]
	private EnvSceneSettingData settingData_Night;

	// Token: 0x04000308 RID: 776
	[SerializeField]
	private bool debug_AlwaysDayTime;

	// Token: 0x04000309 RID: 777
	private bool isCorruptedStage;

	// Token: 0x0400030A RID: 778
	private int curRoundIndex;

	// Token: 0x0400030B RID: 779
	private eSceneTimeType curSceneTimeType;

	// Token: 0x0400030C RID: 780
	private bool isInitialized;
}
