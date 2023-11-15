using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A47 RID: 2631
public class AudioMixerSnapshots : ScriptableObject
{
	// Token: 0x06004F2D RID: 20269 RVA: 0x001C0208 File Offset: 0x001BE408
	[ContextMenu("Reload")]
	public void ReloadSnapshots()
	{
		this.snapshotMap.Clear();
		EventReference[] array = this.snapshots;
		for (int i = 0; i < array.Length; i++)
		{
			string eventReferencePath = KFMOD.GetEventReferencePath(array[i]);
			if (!eventReferencePath.IsNullOrWhiteSpace())
			{
				this.snapshotMap.Add(eventReferencePath);
			}
		}
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001C0256 File Offset: 0x001BE456
	public static AudioMixerSnapshots Get()
	{
		if (AudioMixerSnapshots.instance == null)
		{
			AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>("AudioMixerSnapshots");
		}
		return AudioMixerSnapshots.instance;
	}

	// Token: 0x04003388 RID: 13192
	public EventReference TechFilterOnMigrated;

	// Token: 0x04003389 RID: 13193
	public EventReference TechFilterLogicOn;

	// Token: 0x0400338A RID: 13194
	public EventReference NightStartedMigrated;

	// Token: 0x0400338B RID: 13195
	public EventReference MenuOpenMigrated;

	// Token: 0x0400338C RID: 13196
	public EventReference MenuOpenHalfEffect;

	// Token: 0x0400338D RID: 13197
	public EventReference SpeedPausedMigrated;

	// Token: 0x0400338E RID: 13198
	public EventReference DuplicantCountAttenuatorMigrated;

	// Token: 0x0400338F RID: 13199
	public EventReference NewBaseSetupSnapshot;

	// Token: 0x04003390 RID: 13200
	public EventReference FrontEndSnapshot;

	// Token: 0x04003391 RID: 13201
	public EventReference FrontEndWelcomeScreenSnapshot;

	// Token: 0x04003392 RID: 13202
	public EventReference FrontEndWorldGenerationSnapshot;

	// Token: 0x04003393 RID: 13203
	public EventReference IntroNIS;

	// Token: 0x04003394 RID: 13204
	public EventReference PulseSnapshot;

	// Token: 0x04003395 RID: 13205
	public EventReference ESCPauseSnapshot;

	// Token: 0x04003396 RID: 13206
	public EventReference MENUNewDuplicantSnapshot;

	// Token: 0x04003397 RID: 13207
	public EventReference UserVolumeSettingsSnapshot;

	// Token: 0x04003398 RID: 13208
	public EventReference DuplicantCountMovingSnapshot;

	// Token: 0x04003399 RID: 13209
	public EventReference DuplicantCountSleepingSnapshot;

	// Token: 0x0400339A RID: 13210
	public EventReference PortalLPDimmedSnapshot;

	// Token: 0x0400339B RID: 13211
	public EventReference DynamicMusicPlayingSnapshot;

	// Token: 0x0400339C RID: 13212
	public EventReference FabricatorSideScreenOpenSnapshot;

	// Token: 0x0400339D RID: 13213
	public EventReference SpaceVisibleSnapshot;

	// Token: 0x0400339E RID: 13214
	public EventReference MENUStarmapSnapshot;

	// Token: 0x0400339F RID: 13215
	public EventReference MENUStarmapNotPausedSnapshot;

	// Token: 0x040033A0 RID: 13216
	public EventReference GameNotFocusedSnapshot;

	// Token: 0x040033A1 RID: 13217
	public EventReference FacilityVisibleSnapshot;

	// Token: 0x040033A2 RID: 13218
	public EventReference TutorialVideoPlayingSnapshot;

	// Token: 0x040033A3 RID: 13219
	public EventReference VictoryMessageSnapshot;

	// Token: 0x040033A4 RID: 13220
	public EventReference VictoryNISGenericSnapshot;

	// Token: 0x040033A5 RID: 13221
	public EventReference VictoryNISRocketSnapshot;

	// Token: 0x040033A6 RID: 13222
	public EventReference VictoryCinematicSnapshot;

	// Token: 0x040033A7 RID: 13223
	public EventReference VictoryFadeToBlackSnapshot;

	// Token: 0x040033A8 RID: 13224
	public EventReference MuteDynamicMusicSnapshot;

	// Token: 0x040033A9 RID: 13225
	public EventReference ActiveBaseChangeSnapshot;

	// Token: 0x040033AA RID: 13226
	public EventReference EventPopupSnapshot;

	// Token: 0x040033AB RID: 13227
	public EventReference SmallRocketInteriorReverbSnapshot;

	// Token: 0x040033AC RID: 13228
	public EventReference MediumRocketInteriorReverbSnapshot;

	// Token: 0x040033AD RID: 13229
	public EventReference MainMenuVideoPlayingSnapshot;

	// Token: 0x040033AE RID: 13230
	public EventReference TechFilterRadiationOn;

	// Token: 0x040033AF RID: 13231
	public EventReference FrontEndSupplyClosetSnapshot;

	// Token: 0x040033B0 RID: 13232
	public EventReference FrontEndItemDropScreenSnapshot;

	// Token: 0x040033B1 RID: 13233
	[SerializeField]
	private EventReference[] snapshots;

	// Token: 0x040033B2 RID: 13234
	[NonSerialized]
	public List<string> snapshotMap = new List<string>();

	// Token: 0x040033B3 RID: 13235
	private static AudioMixerSnapshots instance;
}
