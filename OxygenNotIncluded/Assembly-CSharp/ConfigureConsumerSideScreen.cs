using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0D RID: 3085
public class ConfigureConsumerSideScreen : SideScreenContent
{
	// Token: 0x060061B7 RID: 25015 RVA: 0x00241BB3 File Offset: 0x0023FDB3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x00241BBE File Offset: 0x0023FDBE
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (this.settings == null)
		{
			this.settings = this.targetProducer.GetSettingOptions();
		}
		this.PopulateOptions();
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x00241BF4 File Offset: 0x0023FDF4
	private void ClearOldOptions()
	{
		if (this.descriptor != null)
		{
			this.descriptor.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			this.settingToggles[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x00241C50 File Offset: 0x0023FE50
	private void PopulateOptions()
	{
		this.ClearOldOptions();
		for (int i = this.settingToggles.Count; i < this.settings.Length; i++)
		{
			IConfigurableConsumerOption setting = this.settings[i];
			HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
			this.settingToggles.Add(component);
			component.GetReference<LocText>("Label").text = setting.GetName();
			component.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = component.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				this.SelectOption(setting);
			}));
		}
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x00241D3A File Offset: 0x0023FF3A
	private void SelectOption(IConfigurableConsumerOption option)
	{
		this.targetProducer.SetSelectedOption(option);
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x00241D54 File Offset: 0x0023FF54
	private void RefreshToggles()
	{
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			MultiToggle reference = this.settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((this.settings[i] == this.targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(true);
		}
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x00241DB8 File Offset: 0x0023FFB8
	private void RefreshDetails()
	{
		if (this.descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true);
			this.descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			this.descriptor.text = selectedOption.GetDetailedDescription();
			this.selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			this.descriptor.gameObject.SetActive(true);
		}
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x00241E48 File Offset: 0x00240048
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x04004299 RID: 17049
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	// Token: 0x0400429A RID: 17050
	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	// Token: 0x0400429B RID: 17051
	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	// Token: 0x0400429C RID: 17052
	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	// Token: 0x0400429D RID: 17053
	[SerializeField]
	private LocText selectedOptionNameLabel;

	// Token: 0x0400429E RID: 17054
	[SerializeField]
	private GameObject settingDescriptorPrefab;

	// Token: 0x0400429F RID: 17055
	private IConfigurableConsumer targetProducer;

	// Token: 0x040042A0 RID: 17056
	private IConfigurableConsumerOption[] settings;

	// Token: 0x040042A1 RID: 17057
	private LocText descriptor;

	// Token: 0x040042A2 RID: 17058
	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	// Token: 0x040042A3 RID: 17059
	private List<GameObject> requirementRows = new List<GameObject>();
}
