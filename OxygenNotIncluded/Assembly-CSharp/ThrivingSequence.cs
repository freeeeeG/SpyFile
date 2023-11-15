﻿using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
public static class ThrivingSequence
{
	// Token: 0x06003053 RID: 12371 RVA: 0x000FFAB0 File Offset: 0x000FDCB0
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(ThrivingSequence.Sequence());
	}

	// Token: 0x06003054 RID: 12372 RVA: 0x000FFABE File Offset: 0x000FDCBE
	private static IEnumerator Sequence()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		Vector3 cameraBiasUp = Vector3.up * 5f;
		GameObject cameraTaget = null;
		foreach (object obj in Components.Telepads)
		{
			Telepad telepad = (Telepad)obj;
			if (telepad != null)
			{
				cameraTaget = telepad.gameObject;
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
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
			foreach (object obj2 in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj2;
				if (minionIdentity != null)
				{
					minionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db = Db.Get();
					new EmoteChore(minionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2, null);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			yield return SequenceUtil.WaitForSecondsRealtime(3f);
		}
		cameraTaget = null;
		cameraTaget = null;
		foreach (object obj3 in Components.ComplexFabricators)
		{
			ComplexFabricator complexFabricator = (ComplexFabricator)obj3;
			if (complexFabricator != null)
			{
				cameraTaget = complexFabricator.gameObject;
			}
		}
		if (cameraTaget == null)
		{
			foreach (object obj4 in Components.Generators)
			{
				Generator generator = (Generator)obj4;
				if (generator != null)
				{
					cameraTaget = generator.gameObject;
				}
			}
		}
		if (cameraTaget == null)
		{
			foreach (object obj5 in Components.Fabricators)
			{
				Fabricator fabricator = (Fabricator)obj5;
				if (fabricator != null)
				{
					cameraTaget = fabricator.gameObject;
				}
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 10f, false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			CameraController.Instance.SetOverrideZoomSpeed(0.1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 20f, false);
			CameraController.Instance.FadeIn(0f, 2f, null);
			foreach (object obj6 in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity2 = (MinionIdentity)obj6;
				if (minionIdentity2 != null)
				{
					minionIdentity2.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db2 = Db.Get();
					new EmoteChore(minionIdentity2.GetComponent<ChoreProvider>(), db2.ChoreTypes.EmoteHighPriority, db2.Emotes.Minion.Cheer, 2, null);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			yield return SequenceUtil.WaitForSecondsRealtime(3f);
		}
		cameraTaget = null;
		cameraTaget = null;
		foreach (object obj7 in Components.MonumentParts)
		{
			MonumentPart monumentPart = (MonumentPart)obj7;
			if (monumentPart.IsMonumentCompleted())
			{
				cameraTaget = monumentPart.gameObject;
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 15f, false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			CameraController.Instance.FadeIn(0f, 2f, null);
			foreach (object obj8 in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity3 = (MinionIdentity)obj8;
				if (minionIdentity3 != null)
				{
					minionIdentity3.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db3 = Db.Get();
					new EmoteChore(minionIdentity3.GetComponent<ChoreProvider>(), db3.ChoreTypes.EmoteHighPriority, db3.Emotes.Minion.Cheer, 2, null);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.SetOverrideZoomSpeed(0.075f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 25f, false);
			yield return SequenceUtil.WaitForSecondsRealtime(5f);
		}
		cameraTaget = null;
		CameraController.Instance.FadeOut(1f, 1f, null);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.Thriving.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot, false);
		component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.Thriving.messageBody, Db.Get().ColonyAchievements.Thriving.Id, Db.Get().ColonyAchievements.Thriving.loopVideoName);
		component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
		{
			StoryMessageScreen.HideInterface(false);
			CameraController.Instance.FadeIn(0f, 1f, null);
			CameraController.Instance.SetWorldInteractive(true);
			CameraController.Instance.SetOverrideZoomSpeed(1f);
			HoverTextScreen.Instance.Show(true);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot, STOP_MODE.ALLOWFADEOUT);
			RootMenu.Instance.canTogglePauseScreen = true;
		}));
		yield break;
	}
}
