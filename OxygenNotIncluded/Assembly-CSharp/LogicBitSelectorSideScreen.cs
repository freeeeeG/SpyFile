using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C29 RID: 3113
public class LogicBitSelectorSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600627B RID: 25211 RVA: 0x00245BF7 File Offset: 0x00243DF7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activeColor = GlobalAssets.Instance.colorSet.logicOnText;
		this.inactiveColor = GlobalAssets.Instance.colorSet.logicOffText;
	}

	// Token: 0x0600627C RID: 25212 RVA: 0x00245C33 File Offset: 0x00243E33
	public void SelectToggle(int bit)
	{
		this.target.SetBitSelection(bit);
		this.target.UpdateVisuals();
		this.RefreshToggles();
	}

	// Token: 0x0600627D RID: 25213 RVA: 0x00245C54 File Offset: 0x00243E54
	private void RefreshToggles()
	{
		for (int i = 0; i < this.target.GetBitDepth(); i++)
		{
			int n = i;
			if (!this.toggles_by_int.ContainsKey(i))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowPrefab.transform.parent.gameObject, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("bitName").SetText(string.Format(UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, i + 1));
				gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(i) ? this.activeColor : this.inactiveColor);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(i) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				this.toggles_by_int.Add(i, component);
			}
			this.toggles_by_int[i].onClick = delegate()
			{
				this.SelectToggle(n);
			};
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			if (this.target.GetBitSelection() == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x0600627E RID: 25214 RVA: 0x00245DF4 File Offset: 0x00243FF4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ILogicRibbonBitSelector>() != null;
	}

	// Token: 0x0600627F RID: 25215 RVA: 0x00245E00 File Offset: 0x00244000
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ILogicRibbonBitSelector>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an ILogicRibbonBitSelector");
			return;
		}
		this.titleKey = this.target.SideScreenTitle;
		this.readerDescriptionContainer.SetActive(this.target.SideScreenDisplayReaderDescription());
		this.writerDescriptionContainer.SetActive(this.target.SideScreenDisplayWriterDescription());
		this.RefreshToggles();
		this.UpdateInputOutputDisplay();
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
	}

	// Token: 0x06006280 RID: 25216 RVA: 0x00245ED8 File Offset: 0x002440D8
	public void RenderEveryTick(float dt)
	{
		if (this.target.Equals(null))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
		this.UpdateInputOutputDisplay();
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x00245F48 File Offset: 0x00244148
	private void UpdateInputOutputDisplay()
	{
		if (this.target.SideScreenDisplayReaderDescription())
		{
			this.outputDisplayIcon.color = ((this.target.GetOutputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
		if (this.target.SideScreenDisplayWriterDescription())
		{
			this.inputDisplayIcon.color = ((this.target.GetInputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
	}

	// Token: 0x06006282 RID: 25218 RVA: 0x00245FC0 File Offset: 0x002441C0
	private void UpdateStateVisuals(int bit)
	{
		MultiToggle multiToggle = this.toggles_by_int[bit];
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(bit) ? this.activeColor : this.inactiveColor);
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(bit) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
	}

	// Token: 0x04004325 RID: 17189
	private ILogicRibbonBitSelector target;

	// Token: 0x04004326 RID: 17190
	public GameObject rowPrefab;

	// Token: 0x04004327 RID: 17191
	public KImage inputDisplayIcon;

	// Token: 0x04004328 RID: 17192
	public KImage outputDisplayIcon;

	// Token: 0x04004329 RID: 17193
	public GameObject readerDescriptionContainer;

	// Token: 0x0400432A RID: 17194
	public GameObject writerDescriptionContainer;

	// Token: 0x0400432B RID: 17195
	[NonSerialized]
	public Dictionary<int, MultiToggle> toggles_by_int = new Dictionary<int, MultiToggle>();

	// Token: 0x0400432C RID: 17196
	private Color activeColor;

	// Token: 0x0400432D RID: 17197
	private Color inactiveColor;
}
