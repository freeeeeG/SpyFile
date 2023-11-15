using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005FA RID: 1530
public class ButtonHoverIcon : MonoBehaviour
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000643DA File Offset: 0x000627DA
	public HoverIconUIController HoverIconController
	{
		get
		{
			return this.m_hoverIconController;
		}
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000643E2 File Offset: 0x000627E2
	public void SetVisibility(bool _vis)
	{
		if (this.m_visible != _vis)
		{
			this.m_visible = _vis;
			if (base.enabled && this.m_hoverIconController != null)
			{
				this.m_hoverIconController.SetVisibility(_vis);
			}
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0006441F File Offset: 0x0006281F
	protected virtual void OnEnable()
	{
		if (this.m_visible && this.m_hoverIconController != null)
		{
			this.m_hoverIconController.SetVisibility(true);
		}
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x00064449 File Offset: 0x00062849
	protected virtual void OnDisable()
	{
		if (this.m_visible && this.m_hoverIconController != null)
		{
			this.m_hoverIconController.SetVisibility(false);
		}
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x00064474 File Offset: 0x00062874
	protected virtual void Awake()
	{
		this.m_semanticIconLookup = GameUtils.RequireManager<SemanticIconLookup>();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_promptUIObject = GameUtils.InstantiateHoverIconUIController(this.m_iconPrefab, base.transform, "HoverIconCanvas", this.m_offset);
		this.m_hoverIconController = this.m_promptUIObject.RequireComponent<HoverIconUIController>();
		GameObject gameObject = this.m_promptUIObject.transform.Find("Icon").gameObject;
		this.m_icon = gameObject.RequireComponent<Image>();
		this.m_icon.sprite = this.m_semanticIconLookup.GetIcon(this.m_semantic, PlayerInputLookup.Player.One, ControllerIconLookup.IconContext.Bordered);
		this.m_hoverIconController.SetVisibility(this.m_visible);
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.OnRegenerateControls));
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x00064558 File Offset: 0x00062958
	protected virtual void OnDestroy()
	{
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.OnRegenerateControls));
		UnityEngine.Object.Destroy(this.m_promptUIObject);
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000645A7 File Offset: 0x000629A7
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _b, GamepadUser _a)
	{
		this.RefreshIcon();
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000645AF File Offset: 0x000629AF
	private void OnRegenerateControls()
	{
		this.RefreshIcon();
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000645B7 File Offset: 0x000629B7
	private void RefreshIcon()
	{
		if (this.m_icon != null)
		{
			this.m_icon.sprite = this.m_semanticIconLookup.GetIcon(this.m_semantic, PlayerInputLookup.Player.One, ControllerIconLookup.IconContext.Bordered);
		}
	}

	// Token: 0x040016A3 RID: 5795
	[SerializeField]
	private Vector3 m_offset;

	// Token: 0x040016A4 RID: 5796
	[SerializeField]
	private GameObject m_iconPrefab;

	// Token: 0x040016A5 RID: 5797
	[SerializeField]
	private SemanticIconLookup.Semantic m_semantic = SemanticIconLookup.Semantic.Generic;

	// Token: 0x040016A6 RID: 5798
	private ControllerIconLookup m_controlIconLookup;

	// Token: 0x040016A7 RID: 5799
	private GameObject m_promptUIObject;

	// Token: 0x040016A8 RID: 5800
	private Image m_icon;

	// Token: 0x040016A9 RID: 5801
	private SemanticIconLookup m_semanticIconLookup;

	// Token: 0x040016AA RID: 5802
	private PlayerManager m_playerManager;

	// Token: 0x040016AB RID: 5803
	private bool m_visible;

	// Token: 0x040016AC RID: 5804
	private HoverIconUIController m_hoverIconController;
}
