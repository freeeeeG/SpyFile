using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AB3 RID: 2739
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	// Token: 0x060053B7 RID: 21431 RVA: 0x001E2960 File Offset: 0x001E0B60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += base.NavigateBackward;
		int num = 0;
		using (Dictionary<string, ClusterLayout>.ValueCollection.Enumerator enumerator = SettingsCache.clusterLayouts.clusterCache.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.clusterCategory == 3)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.eventStyle.button.gameObject.SetActive(true);
			this.eventStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = this.eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.special);
			}));
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.classicStyle.button.gameObject.SetActive(true);
			this.classicStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = this.classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.spacedOutVanillaStyle);
			}));
			this.spacedOutStyle.button.gameObject.SetActive(true);
			this.spacedOutStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = this.spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.spacedOutStyle);
			}));
			this.panel.sizeDelta = ((num > 0) ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y));
			return;
		}
		this.vanillaStyle.button.gameObject.SetActive(true);
		this.vanillaStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
		MultiToggle button4 = this.vanillaStyle.button;
		button4.onClick = (System.Action)Delegate.Combine(button4.onClick, new System.Action(delegate()
		{
			this.OnClickOption(ClusterLayout.ClusterCategory.vanilla);
		}));
		this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
		this.eventStyle.kanim.Play("lab_asteroid_standard", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001E2C1C File Offset: 0x001E0E1C
	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		this.Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		base.NavigateForward();
	}

	// Token: 0x040037F1 RID: 14321
	public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;

	// Token: 0x040037F2 RID: 14322
	public ClusterCategorySelectionScreen.ButtonConfig classicStyle;

	// Token: 0x040037F3 RID: 14323
	public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;

	// Token: 0x040037F4 RID: 14324
	public ClusterCategorySelectionScreen.ButtonConfig eventStyle;

	// Token: 0x040037F5 RID: 14325
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x040037F6 RID: 14326
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040037F7 RID: 14327
	[SerializeField]
	private RectTransform panel;

	// Token: 0x020019C7 RID: 6599
	[Serializable]
	public class ButtonConfig
	{
		// Token: 0x06009545 RID: 38213 RVA: 0x0033976C File Offset: 0x0033796C
		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			this.headerLabel.SetText(headerText);
			MultiToggle multiToggle = this.button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnter));
			MultiToggle multiToggle2 = this.button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExit));
			HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
			this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		// Token: 0x06009546 RID: 38214 RVA: 0x0033981C File Offset: 0x00337A1C
		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(1f);
			this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
			this.descriptionArea.text = this.hoverDescriptionText;
		}

		// Token: 0x06009547 RID: 38215 RVA: 0x00339880 File Offset: 0x00337A80
		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(0f);
			this.headerImage.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
			this.descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}

		// Token: 0x04007751 RID: 30545
		public MultiToggle button;

		// Token: 0x04007752 RID: 30546
		public Image headerImage;

		// Token: 0x04007753 RID: 30547
		public LocText headerLabel;

		// Token: 0x04007754 RID: 30548
		public Image selectionFrame;

		// Token: 0x04007755 RID: 30549
		public KAnimControllerBase kanim;

		// Token: 0x04007756 RID: 30550
		private string hoverDescriptionText;

		// Token: 0x04007757 RID: 30551
		private LocText descriptionArea;
	}
}
