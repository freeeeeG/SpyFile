using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
public class AssignableSideScreen : SideScreenContent
{
	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x060060F2 RID: 24818 RVA: 0x0023D10F File Offset: 0x0023B30F
	// (set) Token: 0x060060F3 RID: 24819 RVA: 0x0023D117 File Offset: 0x0023B317
	public Assignable targetAssignable { get; private set; }

	// Token: 0x060060F4 RID: 24820 RVA: 0x0023D120 File Offset: 0x0023B320
	public override string GetTitle()
	{
		if (this.targetAssignable != null)
		{
			return string.Format(base.GetTitle(), this.targetAssignable.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x060060F5 RID: 24821 RVA: 0x0023D150 File Offset: 0x0023B350
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortByName(true);
		}));
		MultiToggle multiToggle2 = this.generalSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortByAssignment(true);
		}));
		base.Subscribe(Game.Instance.gameObject, 875045922, new Action<object>(this.OnRefreshData));
	}

	// Token: 0x060060F6 RID: 24822 RVA: 0x0023D1D3 File Offset: 0x0023B3D3
	private void OnRefreshData(object obj)
	{
		this.SetTarget(this.targetAssignable.gameObject);
	}

	// Token: 0x060060F7 RID: 24823 RVA: 0x0023D1E8 File Offset: 0x0023B3E8
	public override void ClearTarget()
	{
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
			this.targetAssignableSubscriptionHandle = -1;
		}
		this.targetAssignable = null;
		Components.LiveMinionIdentities.OnAdd -= this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnMinionIdentitiesChanged;
		base.ClearTarget();
	}

	// Token: 0x060060F8 RID: 24824 RVA: 0x0023D25D File Offset: 0x0023B45D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Assignable>() != null && target.GetComponent<Assignable>().CanBeAssigned && target.GetComponent<AssignmentGroupController>() == null;
	}

	// Token: 0x060060F9 RID: 24825 RVA: 0x0023D288 File Offset: 0x0023B488
	public override void SetTarget(GameObject target)
	{
		Components.LiveMinionIdentities.OnAdd += this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnMinionIdentitiesChanged;
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
		}
		this.targetAssignable = target.GetComponent<Assignable>();
		if (this.targetAssignable == null)
		{
			global::Debug.LogError(string.Format("{0} selected has no Assignable component.", target.GetProperName()));
			return;
		}
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AssignableSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		this.activeSortToggle = null;
		this.activeSortFunction = null;
		if (!this.targetAssignable.CanBeAssigned)
		{
			this.HideScreen(true);
		}
		else
		{
			this.HideScreen(false);
		}
		this.targetAssignableSubscriptionHandle = this.targetAssignable.Subscribe(684616645, new Action<object>(this.OnAssigneeChanged));
		this.Refresh(this.identityList);
		this.SortByAssignment(false);
	}

	// Token: 0x060060FA RID: 24826 RVA: 0x0023D3CB File Offset: 0x0023B5CB
	private void OnMinionIdentitiesChanged(MinionIdentity change)
	{
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList);
	}

	// Token: 0x060060FB RID: 24827 RVA: 0x0023D3F0 File Offset: 0x0023B5F0
	private void OnAssigneeChanged(object data = null)
	{
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.Refresh(null);
		}
	}

	// Token: 0x060060FC RID: 24828 RVA: 0x0023D44C File Offset: 0x0023B64C
	private void Refresh(List<MinionAssignablesProxy> identities)
	{
		this.ClearContent();
		this.currentOwnerText.text = string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED, Array.Empty<object>());
		if (this.targetAssignable == null)
		{
			return;
		}
		if (this.targetAssignable.GetComponent<Equippable>() == null && !this.targetAssignable.HasTag(GameTags.NotRoomAssignable))
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.targetAssignable.gameObject);
			if (roomOfGameObject != null)
			{
				RoomType roomType = roomOfGameObject.roomType;
				if (roomType.primary_constraint != null && !roomType.primary_constraint.building_criteria(this.targetAssignable.GetComponent<KPrefabID>()))
				{
					AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
					freeElement.sideScreen = this;
					this.identityRowMap.Add(roomOfGameObject, freeElement);
					freeElement.SetContent(roomOfGameObject, new Action<IAssignableIdentity>(this.OnRowClicked), this);
					return;
				}
			}
		}
		if (this.targetAssignable.canBePublic)
		{
			AssignableSideScreenRow freeElement2 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement2.sideScreen = this;
			freeElement2.transform.SetAsFirstSibling();
			this.identityRowMap.Add(Game.Instance.assignmentManager.assignment_groups["public"], freeElement2);
			freeElement2.SetContent(Game.Instance.assignmentManager.assignment_groups["public"], new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AssignableSideScreenRow freeElement3 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement3.sideScreen = this;
			this.identityRowMap.Add(minionAssignablesProxy, freeElement3);
			freeElement3.SetContent(minionAssignablesProxy, new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		this.ExecuteSort(this.activeSortFunction);
	}

	// Token: 0x060060FD RID: 24829 RVA: 0x0023D64C File Offset: 0x0023B84C
	private void SortByName(bool reselect)
	{
		this.SelectSortToggle(this.dupeSortingToggle, reselect);
		this.ExecuteSort((IAssignableIdentity i1, IAssignableIdentity i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (this.sortReversed ? -1 : 1));
	}

	// Token: 0x060060FE RID: 24830 RVA: 0x0023D670 File Offset: 0x0023B870
	private void SortByAssignment(bool reselect)
	{
		this.SelectSortToggle(this.generalSortingToggle, reselect);
		Comparison<IAssignableIdentity> sortFunction = delegate(IAssignableIdentity i1, IAssignableIdentity i2)
		{
			int num = this.targetAssignable.CanAssignTo(i1).CompareTo(this.targetAssignable.CanAssignTo(i2));
			if (num != 0)
			{
				return num * -1;
			}
			num = this.identityRowMap[i1].currentState.CompareTo(this.identityRowMap[i2].currentState);
			if (num != 0)
			{
				return num * (this.sortReversed ? -1 : 1);
			}
			return i1.GetProperName().CompareTo(i2.GetProperName());
		};
		this.ExecuteSort(sortFunction);
	}

	// Token: 0x060060FF RID: 24831 RVA: 0x0023D6A0 File Offset: 0x0023B8A0
	private void SelectSortToggle(MultiToggle toggle, bool reselect)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (reselect && this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x06006100 RID: 24832 RVA: 0x0023D70C File Offset: 0x0023B90C
	private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction)
	{
		if (sortFunction != null)
		{
			List<IAssignableIdentity> list = new List<IAssignableIdentity>(this.identityRowMap.Keys);
			list.Sort(sortFunction);
			for (int i = 0; i < list.Count; i++)
			{
				this.identityRowMap[list[i]].transform.SetSiblingIndex(i);
			}
			this.activeSortFunction = sortFunction;
		}
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x0023D76C File Offset: 0x0023B96C
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.DestroyAll();
		}
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.targetIdentity = null;
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x06006102 RID: 24834 RVA: 0x0023D7E4 File Offset: 0x0023B9E4
	private void HideScreen(bool hide)
	{
		if (hide)
		{
			base.transform.localScale = Vector3.zero;
			return;
		}
		if (base.transform.localScale != Vector3.one)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x0023D821 File Offset: 0x0023BA21
	private void OnRowClicked(IAssignableIdentity identity)
	{
		if (this.targetAssignable.assignee != identity)
		{
			this.ChangeAssignment(identity);
			return;
		}
		if (this.CanDeselect(identity))
		{
			this.ChangeAssignment(null);
		}
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x0023D849 File Offset: 0x0023BA49
	private bool CanDeselect(IAssignableIdentity identity)
	{
		return identity is MinionAssignablesProxy;
	}

	// Token: 0x06006105 RID: 24837 RVA: 0x0023D854 File Offset: 0x0023BA54
	private void ChangeAssignment(IAssignableIdentity new_identity)
	{
		this.targetAssignable.Unassign();
		if (!new_identity.IsNullOrDestroyed())
		{
			this.targetAssignable.Assign(new_identity);
		}
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x0023D875 File Offset: 0x0023BA75
	private void OnValidStateChanged(bool state)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Refresh(this.identityList);
		}
	}

	// Token: 0x0400420D RID: 16909
	[SerializeField]
	private AssignableSideScreenRow rowPrefab;

	// Token: 0x0400420E RID: 16910
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x0400420F RID: 16911
	[SerializeField]
	private LocText currentOwnerText;

	// Token: 0x04004210 RID: 16912
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x04004211 RID: 16913
	[SerializeField]
	private MultiToggle generalSortingToggle;

	// Token: 0x04004212 RID: 16914
	private MultiToggle activeSortToggle;

	// Token: 0x04004213 RID: 16915
	private Comparison<IAssignableIdentity> activeSortFunction;

	// Token: 0x04004214 RID: 16916
	private bool sortReversed;

	// Token: 0x04004215 RID: 16917
	private int targetAssignableSubscriptionHandle = -1;

	// Token: 0x04004217 RID: 16919
	private UIPool<AssignableSideScreenRow> rowPool;

	// Token: 0x04004218 RID: 16920
	private Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();

	// Token: 0x04004219 RID: 16921
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
}
