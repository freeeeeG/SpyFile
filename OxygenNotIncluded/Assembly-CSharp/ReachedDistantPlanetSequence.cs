﻿using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
public static class ReachedDistantPlanetSequence
{
	// Token: 0x06003051 RID: 12369 RVA: 0x000FFA9A File Offset: 0x000FDC9A
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(ReachedDistantPlanetSequence.Sequence());
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x000FFAA8 File Offset: 0x000FDCA8
	private static IEnumerator Sequence()
	{
		Vector3 cameraTagetMid = Vector3.zero;
		Vector3 cameraTargetTop = Vector3.zero;
		Spacecraft spacecraft = null;
		foreach (Spacecraft spacecraft2 in SpacecraftManager.instance.GetSpacecraft())
		{
			if (spacecraft2.state != Spacecraft.MissionState.Grounded && SpacecraftManager.instance.GetSpacecraftDestination(spacecraft2.id).GetDestinationType().Id == Db.Get().SpaceDestinationTypes.Wormhole.Id)
			{
				spacecraft = spacecraft2;
				foreach (RocketModule rocketModule in spacecraft2.launchConditions.rocketModules)
				{
					if (rocketModule.GetComponent<RocketEngine>() != null)
					{
						cameraTagetMid = rocketModule.gameObject.transform.position + Vector3.up * 7f;
						break;
					}
				}
				cameraTargetTop = cameraTagetMid + Vector3.up * 20f;
			}
		}
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		CameraController.Instance.FadeOut(1f, 1f, null);
		yield return SequenceUtil.WaitForSecondsRealtime(3f);
		CameraController.Instance.SetTargetPos(cameraTagetMid, 15f, false);
		CameraController.Instance.SetOverrideZoomSpeed(5f);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		CameraController.Instance.FadeIn(0f, 1f, null);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		foreach (object obj in Components.LiveMinionIdentities)
		{
			MinionIdentity minionIdentity = (MinionIdentity)obj;
			if (minionIdentity != null)
			{
				minionIdentity.GetComponent<Facing>().Face(cameraTagetMid.x);
				Db db = Db.Get();
				ChoreProvider component = minionIdentity.GetComponent<ChoreProvider>();
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2, null);
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2, null);
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2, null);
			}
		}
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		SpeedControlScreen.Instance.SetSpeed(1);
		CameraController.Instance.SetOverrideZoomSpeed(0.01f);
		CameraController.Instance.SetTargetPos(cameraTargetTop, 35f, false);
		float baseZoomSpeed = 0.03f;
		int num;
		for (int i = 0; i < 10; i = num + 1)
		{
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.SetOverrideZoomSpeed(baseZoomSpeed + (float)i * 0.006f);
			num = i;
		}
		yield return SequenceUtil.WaitForSecondsRealtime(6f);
		CameraController.Instance.FadeOut(1f, 1f, null);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		spacecraft.TemporallyTear();
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		VideoScreen component2 = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		component2.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ReachedDistantPlanet.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot, false);
		component2.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.ReachedDistantPlanet.messageBody, Db.Get().ColonyAchievements.ReachedDistantPlanet.Id, Db.Get().ColonyAchievements.ReachedDistantPlanet.loopVideoName);
		component2.OnStop = (System.Action)Delegate.Combine(component2.OnStop, new System.Action(delegate()
		{
			StoryMessageScreen.HideInterface(false);
			CameraController.Instance.FadeIn(0f, 1f, null);
			CameraController.Instance.SetWorldInteractive(true);
			HoverTextScreen.Instance.Show(true);
			CameraController.Instance.SetOverrideZoomSpeed(1f);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot, STOP_MODE.ALLOWFADEOUT);
			RootMenu.Instance.canTogglePauseScreen = true;
		}));
		yield break;
	}
}
