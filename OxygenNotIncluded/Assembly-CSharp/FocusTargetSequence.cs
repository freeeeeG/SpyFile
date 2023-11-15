using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000384 RID: 900
public static class FocusTargetSequence
{
	// Token: 0x06001289 RID: 4745 RVA: 0x000638E3 File Offset: 0x00061AE3
	public static void Start(MonoBehaviour coroutineRunner, FocusTargetSequence.Data sequenceData)
	{
		FocusTargetSequence.sequenceCoroutine = coroutineRunner.StartCoroutine(FocusTargetSequence.RunSequence(sequenceData));
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x000638F8 File Offset: 0x00061AF8
	public static void Cancel(MonoBehaviour coroutineRunner)
	{
		if (FocusTargetSequence.sequenceCoroutine == null)
		{
			return;
		}
		coroutineRunner.StopCoroutine(FocusTargetSequence.sequenceCoroutine);
		FocusTargetSequence.sequenceCoroutine = null;
		if (FocusTargetSequence.prevSpeed >= 0)
		{
			SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		}
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (!SpeedControlScreen.Instance.IsPaused && FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		FocusTargetSequence.SetUIVisible(true);
		CameraController.Instance.SetWorldInteractive(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		FocusTargetSequence.prevSelected = null;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSpeed = -1;
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x000639A5 File Offset: 0x00061BA5
	public static IEnumerator RunSequence(FocusTargetSequence.Data sequenceData)
	{
		SaveGame.Instance.GetComponent<UserNavigation>();
		CameraController.Instance.FadeOut(1f, 1f, null);
		FocusTargetSequence.prevSpeed = SpeedControlScreen.Instance.GetSpeed();
		SpeedControlScreen.Instance.SetSpeed(0);
		FocusTargetSequence.wasPaused = SpeedControlScreen.Instance.IsPaused;
		if (!FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		PlayerController.Instance.CancelDragging();
		CameraController.Instance.SetWorldInteractive(false);
		yield return CameraController.Instance.activeFadeRoutine;
		FocusTargetSequence.prevSelected = SelectTool.Instance.selected;
		SelectTool.Instance.Select(null, true);
		FocusTargetSequence.SetUIVisible(false);
		ClusterManager.Instance.SetActiveWorld(sequenceData.WorldId);
		ManagementMenu.Instance.CloseAll();
		CameraController.Instance.SnapTo(sequenceData.Target, sequenceData.OrthographicSize);
		if (sequenceData.PopupData != null)
		{
			EventInfoScreen.ShowPopup(sequenceData.PopupData);
		}
		CameraController.Instance.FadeIn(0f, 2f, null);
		if (sequenceData.TargetSize - sequenceData.OrthographicSize > Mathf.Epsilon)
		{
			CameraController.Instance.StartCoroutine(CameraController.Instance.DoCinematicZoom(sequenceData.TargetSize));
		}
		if (sequenceData.CanCompleteCB != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
			while (!sequenceData.CanCompleteCB())
			{
				yield return SequenceUtil.WaitForNextFrame;
			}
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(true);
		SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (sequenceData.CompleteCB != null)
		{
			sequenceData.CompleteCB();
		}
		FocusTargetSequence.SetUIVisible(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		sequenceData.Clear();
		FocusTargetSequence.sequenceCoroutine = null;
		FocusTargetSequence.prevSpeed = -1;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSelected = null;
		yield break;
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x000639B4 File Offset: 0x00061BB4
	private static void SetUIVisible(bool visible)
	{
		NotificationScreen.Instance.Show(visible);
		OverlayMenu.Instance.Show(visible);
		ManagementMenu.Instance.Show(visible);
		ToolMenu.Instance.Show(visible);
		ToolMenu.Instance.PriorityScreen.Show(visible);
		PinnedResourcesPanel.Instance.Show(visible);
		TopLeftControlScreen.Instance.Show(visible);
		global::DateTime.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		ColonyDiagnosticScreen.Instance.Show(visible);
		RootMenu.Instance.Show(visible);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(visible);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(visible);
		}
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(visible);
		}
	}

	// Token: 0x04000A0A RID: 2570
	private static Coroutine sequenceCoroutine = null;

	// Token: 0x04000A0B RID: 2571
	private static KSelectable prevSelected = null;

	// Token: 0x04000A0C RID: 2572
	private static bool wasPaused = false;

	// Token: 0x04000A0D RID: 2573
	private static int prevSpeed = -1;

	// Token: 0x02000FAB RID: 4011
	public struct Data
	{
		// Token: 0x060072CC RID: 29388 RVA: 0x002C0FCF File Offset: 0x002BF1CF
		public void Clear()
		{
			this.PopupData = null;
			this.CompleteCB = null;
			this.CanCompleteCB = null;
		}

		// Token: 0x04005682 RID: 22146
		public int WorldId;

		// Token: 0x04005683 RID: 22147
		public float OrthographicSize;

		// Token: 0x04005684 RID: 22148
		public float TargetSize;

		// Token: 0x04005685 RID: 22149
		public Vector3 Target;

		// Token: 0x04005686 RID: 22150
		public EventInfoData PopupData;

		// Token: 0x04005687 RID: 22151
		public System.Action CompleteCB;

		// Token: 0x04005688 RID: 22152
		public Func<bool> CanCompleteCB;
	}
}
