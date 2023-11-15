using System;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x02000922 RID: 2338
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060043BB RID: 17339 RVA: 0x0017BD28 File Offset: 0x00179F28
	public static void DestroyInstance()
	{
		TimeOfDay.Instance = null;
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x0017BD30 File Offset: 0x00179F30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		TimeOfDay.Instance = this;
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x0017BD3E File Offset: 0x00179F3E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		TimeOfDay.Instance = null;
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x0017BD4C File Offset: 0x00179F4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeRegion = this.GetCurrentTimeRegion();
		this.UpdateSunlightIntensity();
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x0017BD67 File Offset: 0x00179F67
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.UpdateVisuals();
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x0017BD6F File Offset: 0x00179F6F
	public TimeOfDay.TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeOfDay.TimeRegion.Night;
		}
		return TimeOfDay.TimeRegion.Day;
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x0017BD80 File Offset: 0x00179F80
	private void Update()
	{
		this.UpdateVisuals();
		this.UpdateAudio();
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x0017BD90 File Offset: 0x00179F90
	private void UpdateVisuals()
	{
		float num = 0.875f;
		float num2 = 0.2f;
		float num3 = 1f;
		float b = 0f;
		if (GameClock.Instance.GetCurrentCycleAsPercentage() >= num)
		{
			b = num3;
		}
		this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
		float y = this.UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, y, 0f, 0f));
	}

	// Token: 0x060043C3 RID: 17347 RVA: 0x0017BE08 File Offset: 0x0017A008
	private void UpdateAudio()
	{
		TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
		if (currentTimeRegion != this.timeRegion)
		{
			this.TriggerSoundChange(currentTimeRegion);
			this.timeRegion = currentTimeRegion;
			base.Trigger(1791086652, null);
		}
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x0017BE3F File Offset: 0x0017A03F
	public void Sim4000ms(float dt)
	{
		this.UpdateSunlightIntensity();
	}

	// Token: 0x060043C5 RID: 17349 RVA: 0x0017BE48 File Offset: 0x0017A048
	public void SetEclipse(bool eclipse)
	{
		this.isEclipse = eclipse;
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x0017BE54 File Offset: 0x0017A054
	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || this.isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * 3.1415927f);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = (float)worldContainer.cosmicRadiation;
		}
		return num2;
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x0017BF0C File Offset: 0x0017A10C
	private void TriggerSoundChange(TimeOfDay.TimeRegion new_region)
	{
		if (new_region == TimeOfDay.TimeRegion.Day)
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Stinger_Loop_Night"))
			{
				MusicManager.instance.StopSong("Stinger_Loop_Night", true, STOP_MODE.ALLOWFADEOUT);
			}
			MusicManager.instance.PlaySong("Stinger_Day", false);
			MusicManager.instance.PlayDynamicMusic();
			return;
		}
		if (new_region != TimeOfDay.TimeRegion.Night)
		{
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
		MusicManager.instance.PlaySong("Stinger_Loop_Night", false);
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x0017BF9A File Offset: 0x0017A19A
	public void SetScale(float new_scale)
	{
		this.scale = new_scale;
	}

	// Token: 0x04002CEB RID: 11499
	[Serialize]
	private float scale;

	// Token: 0x04002CEC RID: 11500
	private TimeOfDay.TimeRegion timeRegion;

	// Token: 0x04002CED RID: 11501
	private EventInstance nightLPEvent;

	// Token: 0x04002CEE RID: 11502
	public static TimeOfDay Instance;

	// Token: 0x04002CEF RID: 11503
	private bool isEclipse;

	// Token: 0x02001766 RID: 5990
	public enum TimeRegion
	{
		// Token: 0x04006E9B RID: 28315
		Invalid,
		// Token: 0x04006E9C RID: 28316
		Day,
		// Token: 0x04006E9D RID: 28317
		Night
	}
}
