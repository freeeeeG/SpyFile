using System;
using UnityEngine;

// Token: 0x02000C38 RID: 3128
public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x060062FA RID: 25338 RVA: 0x002497A4 File Offset: 0x002479A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggleButton.onClick += this.ClickToggle;
		this.togglePendingStatusItem = new StatusItem("PlayerControlledToggleSideScreen", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x002497F7 File Offset: 0x002479F7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IPlayerControlledToggle>() != null;
	}

	// Token: 0x060062FC RID: 25340 RVA: 0x00249804 File Offset: 0x00247A04
	public void RenderEveryTick(float dt)
	{
		if (base.isActiveAndEnabled)
		{
			if (!this.keyDown && (Input.GetKeyDown(KeyCode.Return) & Time.unscaledTime - this.lastKeyboardShortcutTime > 0.1f))
			{
				if (SpeedControlScreen.Instance.IsPaused)
				{
					this.RequestToggle();
				}
				else
				{
					this.Toggle();
				}
				this.lastKeyboardShortcutTime = Time.unscaledTime;
				this.keyDown = true;
			}
			if (this.keyDown && Input.GetKeyUp(KeyCode.Return))
			{
				this.keyDown = false;
			}
		}
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x00249882 File Offset: 0x00247A82
	private void ClickToggle()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			this.RequestToggle();
			return;
		}
		this.Toggle();
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x002498A0 File Offset: 0x00247AA0
	private void RequestToggle()
	{
		this.target.ToggleRequested = !this.target.ToggleRequested;
		if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, this);
		}
		else
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), true);
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x0024995C File Offset: 0x00247B5C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IPlayerControlledToggle>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an IPlayerControlledToggle");
			return;
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), false);
		this.titleKey = this.target.SideScreenTitleKey;
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x002499DC File Offset: 0x00247BDC
	private void Toggle()
	{
		this.target.ToggledByPlayer();
		this.UpdateVisuals(this.target.ToggledOn(), true);
		this.target.ToggleRequested = false;
		this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem, false);
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x00249A2C File Offset: 0x00247C2C
	private void UpdateVisuals(bool state, bool smooth)
	{
		if (state != this.currentState)
		{
			if (smooth)
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS, KAnim.PlayMode.Once);
			}
			else
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1], KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		this.currentState = state;
	}

	// Token: 0x0400437F RID: 17279
	public IPlayerControlledToggle target;

	// Token: 0x04004380 RID: 17280
	public KButton toggleButton;

	// Token: 0x04004381 RID: 17281
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04004382 RID: 17282
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x04004383 RID: 17283
	public float animScaleBase = 0.25f;

	// Token: 0x04004384 RID: 17284
	private StatusItem togglePendingStatusItem;

	// Token: 0x04004385 RID: 17285
	[SerializeField]
	private KBatchedAnimController kbac;

	// Token: 0x04004386 RID: 17286
	private float lastKeyboardShortcutTime;

	// Token: 0x04004387 RID: 17287
	private const float KEYBOARD_COOLDOWN = 0.1f;

	// Token: 0x04004388 RID: 17288
	private bool keyDown;

	// Token: 0x04004389 RID: 17289
	private bool currentState;
}
