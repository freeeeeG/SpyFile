﻿using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020006DF RID: 1759
public static class EnterTemporalTearSequence
{
	// Token: 0x0600304F RID: 12367 RVA: 0x000FFA84 File Offset: 0x000FDC84
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(EnterTemporalTearSequence.Sequence());
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x000FFA92 File Offset: 0x000FDC92
	private static IEnumerator Sequence()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		CameraController.Instance.FadeOut(1f, 1f, null);
		yield return SequenceUtil.WaitForSecondsRealtime(3f);
		ManagementMenu.Instance.CloseAll();
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		Vector3 cameraBiasUp = Vector3.up * 5f;
		GameObject cameraTaget = EnterTemporalTearSequence.tearOpenerGameObject;
		if (cameraTaget != null)
		{
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 10f, false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			SpeedControlScreen.Instance.SetSpeed(1);
			CameraController.Instance.SetOverrideZoomSpeed(0.1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 20f, false);
			CameraController.Instance.FadeIn(0f, 2f, null);
			foreach (object obj in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj;
				if (minionIdentity != null)
				{
					minionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db = Db.Get();
					new EmoteChore(minionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2, null);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			yield return SequenceUtil.WaitForSecondsRealtime(1.5f);
			CameraController.Instance.FadeOut(1f, 1f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1.5f);
		}
		cameraTaget = null;
		cameraTaget = null;
		foreach (object obj2 in Components.Telepads)
		{
			Telepad telepad = (Telepad)obj2;
			if (telepad != null)
			{
				cameraTaget = telepad.gameObject;
				CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 10f, false);
				CameraController.Instance.SetOverrideZoomSpeed(10f);
				yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
				if (SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Unpause(false);
				}
				SpeedControlScreen.Instance.SetSpeed(1);
				CameraController.Instance.SetOverrideZoomSpeed(0.05f);
				CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 20f, false);
				CameraController.Instance.FadeIn(0f, 2f, null);
				foreach (object obj3 in Components.LiveMinionIdentities)
				{
					MinionIdentity minionIdentity2 = (MinionIdentity)obj3;
					if (minionIdentity2 != null)
					{
						minionIdentity2.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
						Db db2 = Db.Get();
						new EmoteChore(minionIdentity2.GetComponent<ChoreProvider>(), db2.ChoreTypes.EmoteHighPriority, db2.Emotes.Minion.Cheer, 2, null);
					}
				}
				yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
				yield return SequenceUtil.WaitForSecondsRealtime(1.5f);
				CameraController.Instance.FadeOut(1f, 1f, null);
				yield return SequenceUtil.WaitForSecondsRealtime(1.5f);
			}
		}
		IEnumerator enumerator2 = null;
		cameraTaget = null;
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ReachedDistantPlanet.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot, false);
		component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.ReachedDistantPlanet.messageBody, Db.Get().ColonyAchievements.ReachedDistantPlanet.Id, Db.Get().ColonyAchievements.ReachedDistantPlanet.loopVideoName);
		component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
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
		yield break;
	}

	// Token: 0x04001C7E RID: 7294
	public static GameObject tearOpenerGameObject;
}
