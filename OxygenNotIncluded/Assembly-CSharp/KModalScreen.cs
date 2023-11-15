using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B2E RID: 2862
public class KModalScreen : KScreen
{
	// Token: 0x0600583F RID: 22591 RVA: 0x0020585C File Offset: 0x00203A5C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backgroundRectTransform = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x00205870 File Offset: 0x00203A70
	public static RectTransform MakeScreenModal(KScreen screen)
	{
		screen.ConsumeMouseScroll = true;
		screen.activateOnSpawn = true;
		GameObject gameObject = new GameObject("background");
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		gameObject.AddComponent<CanvasRenderer>();
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color32(0, 0, 0, 160);
		image.raycastTarget = true;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(screen.transform);
		KModalScreen.ResizeBackground(component);
		return component;
	}

	// Token: 0x06005841 RID: 22593 RVA: 0x002058E4 File Offset: 0x00203AE4
	public static void ResizeBackground(RectTransform rectTransform)
	{
		rectTransform.SetAsFirstSibling();
		rectTransform.SetLocalPosition(Vector3.zero);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x06005842 RID: 22594 RVA: 0x00205950 File Offset: 0x00203B50
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = true;
		}
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06005843 RID: 22595 RVA: 0x002059B0 File Offset: 0x00203BB0
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = false;
		}
		base.Trigger(476357528, null);
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06005844 RID: 22596 RVA: 0x00205A1A File Offset: 0x00203C1A
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.backgroundRectTransform);
	}

	// Token: 0x06005845 RID: 22597 RVA: 0x00205A27 File Offset: 0x00203C27
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06005846 RID: 22598 RVA: 0x00205A2A File Offset: 0x00203C2A
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06005847 RID: 22599 RVA: 0x00205A31 File Offset: 0x00203C31
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06005848 RID: 22600 RVA: 0x00205A3A File Offset: 0x00203C3A
	protected override void OnDeactivate()
	{
		this.OnShow(false);
	}

	// Token: 0x06005849 RID: 22601 RVA: 0x00205A44 File Offset: 0x00203C44
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (this.pause && SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
	}

	// Token: 0x0600584A RID: 22602 RVA: 0x00205AA4 File Offset: 0x00203CA4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (Game.Instance != null && (e.TryConsume(global::Action.TogglePause) || e.TryConsume(global::Action.CycleSpeed)))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		if (!e.Consumed && (e.TryConsume(global::Action.Escape) || (e.TryConsume(global::Action.MouseRight) && this.canBackoutWithRightClick)))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x0600584B RID: 22603 RVA: 0x00205B21 File Offset: 0x00203D21
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x04003BB3 RID: 15283
	private bool shown;

	// Token: 0x04003BB4 RID: 15284
	public bool pause = true;

	// Token: 0x04003BB5 RID: 15285
	[Tooltip("Only used for main menu")]
	public bool canBackoutWithRightClick;

	// Token: 0x04003BB6 RID: 15286
	private RectTransform backgroundRectTransform;
}
