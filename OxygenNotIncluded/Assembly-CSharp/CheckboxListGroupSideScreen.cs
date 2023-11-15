using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C04 RID: 3076
public class CheckboxListGroupSideScreen : SideScreenContent
{
	// Token: 0x06006155 RID: 24917 RVA: 0x0023EAA7 File Offset: 0x0023CCA7
	private CheckboxListGroupSideScreen.CheckboxContainer InstantiateCheckboxContainer()
	{
		return new CheckboxListGroupSideScreen.CheckboxContainer(Util.KInstantiateUI(this.checkboxGroupPrefab, this.groupParent.gameObject, true).GetComponent<HierarchyReferences>());
	}

	// Token: 0x06006156 RID: 24918 RVA: 0x0023EACA File Offset: 0x0023CCCA
	private GameObject InstantiateCheckbox()
	{
		return Util.KInstantiateUI(this.checkboxPrefab, this.checkboxParent.gameObject, false);
	}

	// Token: 0x06006157 RID: 24919 RVA: 0x0023EAE3 File Offset: 0x0023CCE3
	protected override void OnSpawn()
	{
		this.checkboxPrefab.SetActive(false);
		this.checkboxGroupPrefab.SetActive(false);
		base.OnSpawn();
	}

	// Token: 0x06006158 RID: 24920 RVA: 0x0023EB04 File Offset: 0x0023CD04
	public override bool IsValidForTarget(GameObject target)
	{
		ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
		if (components != null)
		{
			ICheckboxListGroupControl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].SidescreenEnabled())
				{
					return true;
				}
			}
		}
		using (List<ICheckboxListGroupControl>.Enumerator enumerator = target.GetAllSMI<ICheckboxListGroupControl>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SidescreenEnabled())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006159 RID: 24921 RVA: 0x0023EB88 File Offset: 0x0023CD88
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].CheckboxSideScreenSortOrder();
	}

	// Token: 0x0600615A RID: 24922 RVA: 0x0023EBA8 File Offset: 0x0023CDA8
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = target.GetAllSMI<ICheckboxListGroupControl>();
		this.targets.AddRange(target.GetComponents<ICheckboxListGroupControl>());
		this.Rebuild(target);
		this.uiRefreshSubHandle = this.currentBuildTarget.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x0600615B RID: 24923 RVA: 0x0023EC10 File Offset: 0x0023CE10
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.currentBuildTarget != null)
		{
			this.currentBuildTarget.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count);
	}

	// Token: 0x0600615C RID: 24924 RVA: 0x0023EC5D File Offset: 0x0023CE5D
	public override string GetTitle()
	{
		if (this.targets != null && this.targets.Count > 0 && this.targets[0] != null)
		{
			return this.targets[0].Title;
		}
		return base.GetTitle();
	}

	// Token: 0x0600615D RID: 24925 RVA: 0x0023EC9C File Offset: 0x0023CE9C
	private void Rebuild(GameObject buildTarget)
	{
		if (this.checkboxContainerPool == null)
		{
			this.checkboxContainerPool = new ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer>(new Func<CheckboxListGroupSideScreen.CheckboxContainer>(this.InstantiateCheckboxContainer), 0);
			this.checkboxPool = new GameObjectPool(new Func<GameObject>(this.InstantiateCheckbox), 0);
		}
		this.descriptionLabel.enabled = !this.targets[0].Description.IsNullOrWhiteSpace();
		if (!this.targets[0].Description.IsNullOrWhiteSpace())
		{
			this.descriptionLabel.SetText(this.targets[0].Description);
		}
		if (buildTarget == this.currentBuildTarget)
		{
			this.Refresh(null);
			return;
		}
		this.currentBuildTarget = buildTarget;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup group in checkboxListGroupControl.GetData())
			{
				CheckboxListGroupSideScreen.CheckboxContainer instance = this.checkboxContainerPool.GetInstance();
				this.InitContainer(checkboxListGroupControl, group, instance);
			}
		}
	}

	// Token: 0x0600615E RID: 24926 RVA: 0x0023EDCC File Offset: 0x0023CFCC
	[ContextMenu("Force refresh")]
	private void Test()
	{
		this.Refresh(null);
	}

	// Token: 0x0600615F RID: 24927 RVA: 0x0023EDD8 File Offset: 0x0023CFD8
	private void Refresh(object data = null)
	{
		int num = 0;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup listGroup in checkboxListGroupControl.GetData())
			{
				if (++num > this.activeChecklistGroups.Count)
				{
					this.InitContainer(checkboxListGroupControl, listGroup, this.checkboxContainerPool.GetInstance());
				}
				CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[num - 1];
				if (listGroup.resolveTitleCallback != null)
				{
					checkboxContainer.container.GetReference<LocText>("Text").SetText(listGroup.resolveTitleCallback(listGroup.title));
				}
				for (int j = 0; j < listGroup.checkboxItems.Length; j++)
				{
					ICheckboxListGroupControl.CheckboxItem data3 = listGroup.checkboxItems[j];
					if (checkboxContainer.checkboxUIItems.Count <= j)
					{
						this.CreateSingleCheckBoxForGroupUI(checkboxContainer);
					}
					HierarchyReferences checkboxUI = checkboxContainer.checkboxUIItems[j];
					this.SetCheckboxData(checkboxUI, data3, checkboxListGroupControl);
				}
				while (checkboxContainer.checkboxUIItems.Count > listGroup.checkboxItems.Length)
				{
					HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[checkboxContainer.checkboxUIItems.Count - 1];
					this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
				}
			}
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count - num);
	}

	// Token: 0x06006160 RID: 24928 RVA: 0x0023EF78 File Offset: 0x0023D178
	private void ReleaseContainers(int count)
	{
		int count2 = this.activeChecklistGroups.Count;
		for (int i = 1; i <= count; i++)
		{
			int index = count2 - i;
			CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[index];
			this.activeChecklistGroups.RemoveAt(index);
			for (int j = checkboxContainer.checkboxUIItems.Count - 1; j >= 0; j--)
			{
				HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[j];
				this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
			}
			checkboxContainer.container.gameObject.SetActive(false);
			this.checkboxContainerPool.ReleaseInstance(checkboxContainer);
		}
	}

	// Token: 0x06006161 RID: 24929 RVA: 0x0023F00C File Offset: 0x0023D20C
	private void InitContainer(ICheckboxListGroupControl target, ICheckboxListGroupControl.ListGroup group, CheckboxListGroupSideScreen.CheckboxContainer groupUI)
	{
		this.activeChecklistGroups.Add(groupUI);
		groupUI.container.gameObject.SetActive(true);
		string text = group.title;
		if (group.resolveTitleCallback != null)
		{
			text = group.resolveTitleCallback(text);
		}
		groupUI.container.GetReference<LocText>("Text").SetText(text);
		foreach (ICheckboxListGroupControl.CheckboxItem data in group.checkboxItems)
		{
			this.CreateSingleCheckBoxForGroupUI(data, target, groupUI);
		}
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x0023F08F File Offset: 0x0023D28F
	public void RemoveSingleCheckboxFromContainer(HierarchyReferences checkbox, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		container.checkboxUIItems.Remove(checkbox);
		checkbox.gameObject.SetActive(false);
		checkbox.transform.SetParent(this.checkboxParent);
		this.checkboxPool.ReleaseInstance(checkbox.gameObject);
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x0023F0CC File Offset: 0x0023D2CC
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences component = this.checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
		component.gameObject.SetActive(true);
		container.checkboxUIItems.Add(component);
		component.transform.SetParent(container.container.transform);
		return component;
	}

	// Token: 0x06006164 RID: 24932 RVA: 0x0023F11C File Offset: 0x0023D31C
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences hierarchyReferences = this.CreateSingleCheckBoxForGroupUI(container);
		this.SetCheckboxData(hierarchyReferences, data, target);
		return hierarchyReferences;
	}

	// Token: 0x06006165 RID: 24933 RVA: 0x0023F13C File Offset: 0x0023D33C
	public void SetCheckboxData(HierarchyReferences checkboxUI, ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target)
	{
		LocText reference = checkboxUI.GetReference<LocText>("Text");
		reference.SetText(data.text);
		reference.SetLinkOverrideAction(data.overrideLinkActions);
		checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
		ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
		reference2.SetSimpleTooltip(data.tooltip);
		reference2.refreshWhileHovering = (data.resolveTooltipCallback != null);
		reference2.OnToolTip = delegate()
		{
			if (data.resolveTooltipCallback == null)
			{
				return data.tooltip;
			}
			return data.resolveTooltipCallback(data.tooltip, target);
		};
	}

	// Token: 0x04004245 RID: 16965
	public const int DefaultCheckboxListSideScreenSortOrder = 20;

	// Token: 0x04004246 RID: 16966
	private ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer> checkboxContainerPool;

	// Token: 0x04004247 RID: 16967
	private GameObjectPool checkboxPool;

	// Token: 0x04004248 RID: 16968
	[SerializeField]
	private GameObject checkboxGroupPrefab;

	// Token: 0x04004249 RID: 16969
	[SerializeField]
	private GameObject checkboxPrefab;

	// Token: 0x0400424A RID: 16970
	[SerializeField]
	private RectTransform groupParent;

	// Token: 0x0400424B RID: 16971
	[SerializeField]
	private RectTransform checkboxParent;

	// Token: 0x0400424C RID: 16972
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x0400424D RID: 16973
	private List<ICheckboxListGroupControl> targets;

	// Token: 0x0400424E RID: 16974
	private GameObject currentBuildTarget;

	// Token: 0x0400424F RID: 16975
	private int uiRefreshSubHandle = -1;

	// Token: 0x04004250 RID: 16976
	private List<CheckboxListGroupSideScreen.CheckboxContainer> activeChecklistGroups = new List<CheckboxListGroupSideScreen.CheckboxContainer>();

	// Token: 0x02001B55 RID: 6997
	public class CheckboxContainer
	{
		// Token: 0x060099AA RID: 39338 RVA: 0x0034517A File Offset: 0x0034337A
		public CheckboxContainer(HierarchyReferences container)
		{
			this.container = container;
		}

		// Token: 0x04007C7D RID: 31869
		public HierarchyReferences container;

		// Token: 0x04007C7E RID: 31870
		public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();
	}
}
