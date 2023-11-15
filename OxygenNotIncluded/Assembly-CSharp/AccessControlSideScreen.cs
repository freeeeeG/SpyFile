using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BF1 RID: 3057
public class AccessControlSideScreen : SideScreenContent
{
	// Token: 0x0600609C RID: 24732 RVA: 0x0023B55E File Offset: 0x0023975E
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return string.Format(base.GetTitle(), this.target.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x0023B58C File Offset: 0x0023978C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sortByNameToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName));
		});
		this.sortByRoleToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole));
		});
		this.sortByPermissionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SortByPermission));
	}

	// Token: 0x0600609E RID: 24734 RVA: 0x0023B5F3 File Offset: 0x002397F3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AccessControl>() != null && target.GetComponent<AccessControl>().controlEnabled;
	}

	// Token: 0x0600609F RID: 24735 RVA: 0x0023B610 File Offset: 0x00239810
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		this.target = target.GetComponent<AccessControl>();
		this.doorTarget = target.GetComponent<Door>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AccessControlSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList, true);
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x0023B6D0 File Offset: 0x002398D0
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x0023B72C File Offset: 0x0023992C
	private void Refresh(List<MinionAssignablesProxy> identities, bool rebuild)
	{
		Rotatable component = this.target.GetComponent<Rotatable>();
		bool rotated = component != null && component.IsRotated;
		this.defaultsRow.SetRotated(rotated);
		this.defaultsRow.SetContent(this.target.DefaultPermission, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnDefaultPermissionChanged));
		if (rebuild)
		{
			this.ClearContent();
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AccessControlSideScreenRow accessControlSideScreenRow;
			if (rebuild)
			{
				accessControlSideScreenRow = this.rowPool.GetFreeElement(this.rowGroup, true);
				this.identityRowMap.Add(minionAssignablesProxy, accessControlSideScreenRow);
			}
			else
			{
				accessControlSideScreenRow = this.identityRowMap[minionAssignablesProxy];
			}
			AccessControl.Permission setPermission = this.target.GetSetPermission(minionAssignablesProxy);
			bool isDefault = this.target.IsDefaultPermission(minionAssignablesProxy);
			accessControlSideScreenRow.SetRotated(rotated);
			accessControlSideScreenRow.SetMinionContent(minionAssignablesProxy, setPermission, isDefault, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnPermissionChanged), new Action<MinionAssignablesProxy, bool>(this.OnPermissionDefault));
		}
		this.RefreshOnline();
		this.ContentContainer.SetActive(this.target.controlEnabled);
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x0023B868 File Offset: 0x00239A68
	private void RefreshOnline()
	{
		bool flag = this.target.Online && (this.doorTarget == null || this.doorTarget.CurrentState == Door.ControlState.Auto);
		this.disabledOverlay.SetActive(!flag);
		this.headerBG.ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Inactive);
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x0023B8C6 File Offset: 0x00239AC6
	private void SortByPermission(bool state)
	{
		this.ExecuteSort<int>(this.sortByPermissionToggle, state, delegate(MinionAssignablesProxy identity)
		{
			if (!this.target.IsDefaultPermission(identity))
			{
				return (int)this.target.GetSetPermission(identity);
			}
			return -1;
		}, false);
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x0023B8E4 File Offset: 0x00239AE4
	private void ExecuteSort<T>(Toggle toggle, bool state, Func<MinionAssignablesProxy, T> sortFunction, bool refresh = false)
	{
		toggle.GetComponent<ImageToggleState>().SetActiveState(state);
		if (!state)
		{
			return;
		}
		this.identityList = (state ? this.identityList.OrderBy(sortFunction).ToList<MinionAssignablesProxy>() : this.identityList.OrderByDescending(sortFunction).ToList<MinionAssignablesProxy>());
		if (refresh)
		{
			this.Refresh(this.identityList, false);
			return;
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x0023B994 File Offset: 0x00239B94
	private void SortEntries(bool reverse_sort, Comparison<MinionAssignablesProxy> compare)
	{
		this.identityList.Sort(compare);
		if (reverse_sort)
		{
			this.identityList.Reverse();
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x0023BA0C File Offset: 0x00239C0C
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.ClearAll();
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x0023BA2C File Offset: 0x00239C2C
	private void OnDefaultPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.DefaultPermission = permission;
		this.Refresh(this.identityList, false);
		foreach (MinionAssignablesProxy key in this.identityList)
		{
			if (this.target.IsDefaultPermission(key))
			{
				this.target.ClearPermission(key);
			}
		}
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x0023BAAC File Offset: 0x00239CAC
	private void OnPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.SetPermission(identity, permission);
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x0023BABB File Offset: 0x00239CBB
	private void OnPermissionDefault(MinionAssignablesProxy identity, bool isDefault)
	{
		if (isDefault)
		{
			this.target.ClearPermission(identity);
		}
		else
		{
			this.target.SetPermission(identity, this.target.DefaultPermission);
		}
		this.Refresh(this.identityList, false);
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x0023BAF2 File Offset: 0x00239CF2
	private void OnAccessControlChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x0023BAFA File Offset: 0x00239CFA
	private void OnDoorStateChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x060060AC RID: 24748 RVA: 0x0023BB04 File Offset: 0x00239D04
	private void OnSelectSortFunc(IListableOption role, object data)
	{
		if (role != null)
		{
			foreach (AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo in AccessControlSideScreen.MinionIdentitySort.SortInfos)
			{
				if (sortInfo.name == role.GetProperName())
				{
					this.sortInfo = sortInfo;
					this.identityList.Sort(this.sortInfo.compare);
					for (int j = 0; j < this.identityList.Count; j++)
					{
						if (this.identityRowMap.ContainsKey(this.identityList[j]))
						{
							this.identityRowMap[this.identityList[j]].transform.SetSiblingIndex(j);
						}
					}
					return;
				}
			}
		}
	}

	// Token: 0x040041D4 RID: 16852
	[SerializeField]
	private AccessControlSideScreenRow rowPrefab;

	// Token: 0x040041D5 RID: 16853
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x040041D6 RID: 16854
	[SerializeField]
	private AccessControlSideScreenDoor defaultsRow;

	// Token: 0x040041D7 RID: 16855
	[SerializeField]
	private Toggle sortByNameToggle;

	// Token: 0x040041D8 RID: 16856
	[SerializeField]
	private Toggle sortByPermissionToggle;

	// Token: 0x040041D9 RID: 16857
	[SerializeField]
	private Toggle sortByRoleToggle;

	// Token: 0x040041DA RID: 16858
	[SerializeField]
	private GameObject disabledOverlay;

	// Token: 0x040041DB RID: 16859
	[SerializeField]
	private KImage headerBG;

	// Token: 0x040041DC RID: 16860
	private AccessControl target;

	// Token: 0x040041DD RID: 16861
	private Door doorTarget;

	// Token: 0x040041DE RID: 16862
	private UIPool<AccessControlSideScreenRow> rowPool;

	// Token: 0x040041DF RID: 16863
	private AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo = AccessControlSideScreen.MinionIdentitySort.SortInfos[0];

	// Token: 0x040041E0 RID: 16864
	private Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow> identityRowMap = new Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow>();

	// Token: 0x040041E1 RID: 16865
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();

	// Token: 0x02001B49 RID: 6985
	private static class MinionIdentitySort
	{
		// Token: 0x06009996 RID: 39318 RVA: 0x00344E8E File Offset: 0x0034308E
		public static int CompareByName(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			return a.GetProperName().CompareTo(b.GetProperName());
		}

		// Token: 0x06009997 RID: 39319 RVA: 0x00344EA4 File Offset: 0x003430A4
		public static int CompareByRole(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			global::Debug.Assert(a, "a was null");
			global::Debug.Assert(b, "b was null");
			GameObject targetGameObject = a.GetTargetGameObject();
			GameObject targetGameObject2 = b.GetTargetGameObject();
			MinionResume minionResume = targetGameObject ? targetGameObject.GetComponent<MinionResume>() : null;
			MinionResume minionResume2 = targetGameObject2 ? targetGameObject2.GetComponent<MinionResume>() : null;
			if (minionResume2 == null)
			{
				return 1;
			}
			if (minionResume == null)
			{
				return -1;
			}
			int num = minionResume.CurrentRole.CompareTo(minionResume2.CurrentRole);
			if (num != 0)
			{
				return num;
			}
			return AccessControlSideScreen.MinionIdentitySort.CompareByName(a, b);
		}

		// Token: 0x04007C5C RID: 31836
		public static readonly AccessControlSideScreen.MinionIdentitySort.SortInfo[] SortInfos = new AccessControlSideScreen.MinionIdentitySort.SortInfo[]
		{
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.NAME,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName)
			},
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.ROLE,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole)
			}
		};

		// Token: 0x02002238 RID: 8760
		public class SortInfo : IListableOption
		{
			// Token: 0x0600AD30 RID: 44336 RVA: 0x0037922C File Offset: 0x0037742C
			public string GetProperName()
			{
				return this.name;
			}

			// Token: 0x040098ED RID: 39149
			public LocString name;

			// Token: 0x040098EE RID: 39150
			public Comparison<MinionAssignablesProxy> compare;
		}
	}
}
