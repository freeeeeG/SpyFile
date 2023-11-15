using System;
using TMPro;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class Obj_FireSource : MonoBehaviour, IPlayerStartPoint
{
	// Token: 0x06000481 RID: 1153 RVA: 0x00012224 File Offset: 0x00010424
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnHPChanged));
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterDealDamageToPlayer, new Action<AMonsterBase>(this.OnMonsterDealDamageToPlayer));
		this.lerpingLightIntensity = this.light.intensity;
		this.lerpingLightRange = this.light.range;
		this.lerpingFireParticleScale = this.particle_Fire.transform.localScale.x;
		this.lerpingFogOfWarRadius = this.fogOfWarCtrl.GetRadius();
		this.RegisterToMapManager();
		this.text_Debug_EnergyLevel.enabled = false;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x000122C1 File Offset: 0x000104C1
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnHPChanged));
		EventMgr.Remove<AMonsterBase>(eGameEvents.MonsterDealDamageToPlayer, new Action<AMonsterBase>(this.OnMonsterDealDamageToPlayer));
		this.UnregisterToMapManager();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000122FC File Offset: 0x000104FC
	private void OnMonsterDealDamageToPlayer(AMonsterBase monster)
	{
		eMonsterSize monsterSize = monster.MonsterData.GetMonsterSize();
		if (monsterSize <= eMonsterSize.MEDIUM || monsterSize != eMonsterSize.LARGE)
		{
			this.particle_TakeDamage_Medium.transform.position = monster.transform.position + Vector3.up;
			this.particle_TakeDamage_Medium.Play();
			Singleton<CameraManager>.Instance.ShakeCamera(0.15f, 0.005f, 0f);
			SoundManager.PlaySound("Monster", "DamagePlayer_Medium", -1f, -1f, -1f);
			return;
		}
		this.particle_TakeDamage_Medium.transform.position = monster.transform.position + Vector3.up;
		this.particle_TakeDamage_Large.Play();
		Singleton<CameraManager>.Instance.ShakeCamera(0.25f, 0.005f, 0f);
		SoundManager.PlaySound("Monster", "DamagePlayer_Large", -1f, -1f, -1f);
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000123EE File Offset: 0x000105EE
	private void OnHPChanged(int value)
	{
		this.text_Debug_EnergyLevel.text = value.ToString();
		this.SetEnergyLevel(value);
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00012409 File Offset: 0x00010609
	private void Reset()
	{
		if (this.fogOfWarCtrl == null)
		{
			this.fogOfWarCtrl = base.GetComponentInChildren<FogOfWarCtrl>();
		}
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00012428 File Offset: 0x00010628
	private void Update()
	{
		this.light.intensity = Mathf.Lerp(this.light.intensity, this.lerpingLightIntensity, 3f * Time.deltaTime);
		this.light.range = Mathf.Lerp(this.light.range, this.lerpingLightRange, 3f * Time.deltaTime);
		this.particle_Fire.transform.localScale = Vector3.one * Mathf.Lerp(this.particle_Fire.transform.localScale.x, this.lerpingFireParticleScale, 3f * Time.deltaTime);
		this.fogOfWarCtrl.SetRadius(Mathf.Lerp(this.fogOfWarCtrl.GetRadius(), this.lerpingFogOfWarRadius, 3f * Time.deltaTime));
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00012500 File Offset: 0x00010700
	public void SetEnergyLevel(int level)
	{
		this.energyLevel = (float)level;
		if (level <= 0)
		{
			this.lerpingLightIntensity = 0f;
			this.lerpingLightRange = 0f;
			this.particle_Fire.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			this.lerpingFogOfWarRadius = this.fogOfWarVisionRadiusRange.x;
			return;
		}
		float t = Mathf.InverseLerp(this.energyLevelRange.x, this.energyLevelRange.y, this.energyLevel);
		this.lerpingLightIntensity = Mathf.Lerp(this.lightIntensityValueRange.x, this.lightIntensityValueRange.y, t);
		this.lerpingLightRange = Mathf.Lerp(this.lightRangeValueRange.x, this.lightRangeValueRange.y, t);
		this.particle_Fire.Play();
		this.lerpingFireParticleScale = Mathf.Lerp(this.particleSizeValueRange.x, this.particleSizeValueRange.y, t);
		this.lerpingFogOfWarRadius = Mathf.Lerp(this.fogOfWarVisionRadiusRange.x, this.fogOfWarVisionRadiusRange.y, t);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00012603 File Offset: 0x00010803
	public void RegisterToMapManager()
	{
		Singleton<MapManager>.Instance.RegisterPlayerOrigin(this);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00012610 File Offset: 0x00010810
	public void UnregisterToMapManager()
	{
		if (Singleton<MapManager>.HasInstance())
		{
			Singleton<MapManager>.Instance.UnregisterPlayerOrigin(this);
		}
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00012624 File Offset: 0x00010824
	public Vector3 GetPosition()
	{
		return base.transform.position;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00012631 File Offset: 0x00010831
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400045C RID: 1116
	[SerializeField]
	private FogOfWarCtrl fogOfWarCtrl;

	// Token: 0x0400045D RID: 1117
	[SerializeField]
	private ParticleSystem particle_Fire;

	// Token: 0x0400045E RID: 1118
	[SerializeField]
	private ParticleSystem particle_TakeDamage_Small;

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private ParticleSystem particle_TakeDamage_Medium;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	private ParticleSystem particle_TakeDamage_Large;

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	private Light light;

	// Token: 0x04000462 RID: 1122
	[SerializeField]
	[Header("生命值Debug文字")]
	private TMP_Text text_Debug_EnergyLevel;

	// Token: 0x04000463 RID: 1123
	[SerializeField]
	[Header("能量的最大最小值 (用來轉換到其他數值)")]
	private Vector2 energyLevelRange;

	// Token: 0x04000464 RID: 1124
	[SerializeField]
	[Header("亮度最大最小值")]
	private Vector2 lightIntensityValueRange;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	[Header("照亮範圍最大最小值")]
	private Vector2 lightRangeValueRange;

	// Token: 0x04000466 RID: 1126
	[SerializeField]
	[Header("火焰尺寸最大最小值")]
	private Vector2 particleSizeValueRange;

	// Token: 0x04000467 RID: 1127
	[SerializeField]
	[Header("迷霧可視範圍最大最小值")]
	private Vector2 fogOfWarVisionRadiusRange;

	// Token: 0x04000468 RID: 1128
	private float lerpingLightIntensity;

	// Token: 0x04000469 RID: 1129
	private float lerpingLightRange;

	// Token: 0x0400046A RID: 1130
	private float lerpingFireParticleScale;

	// Token: 0x0400046B RID: 1131
	private float lerpingFogOfWarRadius;

	// Token: 0x0400046C RID: 1132
	private float energyLevel;
}
