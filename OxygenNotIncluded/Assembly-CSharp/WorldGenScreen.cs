using System;
using System.IO;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000CA0 RID: 3232
public class WorldGenScreen : NewGameFlowScreen
{
	// Token: 0x060066DD RID: 26333 RVA: 0x00265E1E File Offset: 0x0026401E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldGenScreen.Instance = this;
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x00265E2C File Offset: 0x0026402C
	protected override void OnForcedCleanUp()
	{
		WorldGenScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x00265E3C File Offset: 0x0026403C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (MainMenu.Instance != null)
		{
			MainMenu.Instance.StopAmbience();
		}
		this.TriggerLoadingMusic();
		UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
		SaveLoader.SetActiveSaveFilePath(null);
		try
		{
			int num = 0;
			while (File.Exists(WorldGen.GetSIMSaveFilename(num)))
			{
				File.Delete(WorldGen.GetSIMSaveFilename(num));
				num++;
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex.ToString()
			});
		}
		this.offlineWorldGen.Generate();
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x00265ED8 File Offset: 0x002640D8
	private void TriggerLoadingMusic()
	{
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MainMenu.Instance.StopMainMenuMusic();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
		}
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x00265F4B File Offset: 0x0026414B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.Escape);
		}
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.MouseRight);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04004728 RID: 18216
	[MyCmpReq]
	private OfflineWorldGen offlineWorldGen;

	// Token: 0x04004729 RID: 18217
	public static WorldGenScreen Instance;
}
