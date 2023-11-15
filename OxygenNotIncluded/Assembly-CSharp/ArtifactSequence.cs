﻿using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020006DC RID: 1756
public static class ArtifactSequence
{
	// Token: 0x06003032 RID: 12338 RVA: 0x000FEAC8 File Offset: 0x000FCCC8
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(ArtifactSequence.Sequence());
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x000FEAD6 File Offset: 0x000FCCD6
	private static IEnumerator Sequence()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.CollectedArtifacts.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		GameObject cameraTaget = null;
		foreach (object obj in Components.Telepads)
		{
			Telepad telepad = (Telepad)obj;
			if (telepad != null)
			{
				cameraTaget = telepad.gameObject;
			}
		}
		CameraController.Instance.FadeOut(1f, 2f, null);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 10f, false);
		CameraController.Instance.SetOverrideZoomSpeed(10f);
		yield return SequenceUtil.WaitForSecondsRealtime(0.6f);
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
				new EmoteChore(minionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 4, null);
			}
		}
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		yield return SequenceUtil.WaitForSecondsRealtime(3f);
		cameraTaget = null;
		List<SpaceArtifact> list = new List<SpaceArtifact>();
		foreach (object obj3 in Components.SpaceArtifacts)
		{
			SpaceArtifact spaceArtifact = (SpaceArtifact)obj3;
			if (spaceArtifact != null && spaceArtifact.HasTag(GameTags.Stored) && !spaceArtifact.HasTag(GameTags.CharmedArtifact))
			{
				bool flag = true;
				foreach (SpaceArtifact spaceArtifact2 in list)
				{
					if (!(spaceArtifact2 == spaceArtifact) && (spaceArtifact.GetMyWorld() == spaceArtifact2.GetMyWorld() || Grid.GetCellDistance(Grid.PosToCell(spaceArtifact), Grid.PosToCell(spaceArtifact2)) < 10))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(spaceArtifact);
				}
			}
			if (list.Count >= 3)
			{
				break;
			}
		}
		if (list.Count < 3)
		{
			foreach (object obj4 in Components.SpaceArtifacts)
			{
				SpaceArtifact spaceArtifact3 = (SpaceArtifact)obj4;
				if (!list.Contains(spaceArtifact3))
				{
					if (spaceArtifact3 != null && !spaceArtifact3.HasTag(GameTags.CharmedArtifact))
					{
						if (list.Count == 0)
						{
							list.Add(spaceArtifact3);
						}
						else
						{
							bool flag2 = true;
							foreach (SpaceArtifact spaceArtifact4 in list)
							{
								if (!(spaceArtifact4 == spaceArtifact3) && Grid.GetCellDistance(Grid.PosToCell(spaceArtifact3), Grid.PosToCell(spaceArtifact4)) < 10)
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								list.Add(spaceArtifact3);
							}
						}
					}
					if (list.Count >= 3)
					{
						break;
					}
				}
			}
		}
		foreach (SpaceArtifact spaceArtifact5 in list)
		{
			cameraTaget = spaceArtifact5.gameObject;
			CameraController.Instance.FadeOut(1f, 2f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 4f, false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.FadeIn(0f, 2f, null);
			foreach (object obj5 in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity2 = (MinionIdentity)obj5;
				if (minionIdentity2 != null)
				{
					minionIdentity2.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db2 = Db.Get();
					new EmoteChore(minionIdentity2.GetComponent<ChoreProvider>(), db2.ChoreTypes.EmoteHighPriority, db2.Emotes.Minion.Cheer, 4, null);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.SetOverrideZoomSpeed(0.04f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 8f, false);
			yield return SequenceUtil.WaitForSecondsRealtime(3f);
			cameraTaget = null;
		}
		List<SpaceArtifact>.Enumerator enumerator3 = default(List<SpaceArtifact>.Enumerator);
		CameraController.Instance.FadeOut(1f, 1f, null);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.CollectedArtifacts.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.CollectedArtifacts.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot, false);
		component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.CollectedArtifacts.messageBody, Db.Get().ColonyAchievements.CollectedArtifacts.Id, Db.Get().ColonyAchievements.CollectedArtifacts.loopVideoName);
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
		yield break;
	}
}
