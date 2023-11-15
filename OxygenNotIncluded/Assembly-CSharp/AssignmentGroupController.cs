using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x0200098B RID: 2443
public class AssignmentGroupController : KMonoBehaviour
{
	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x0600480E RID: 18446 RVA: 0x00196919 File Offset: 0x00194B19
	// (set) Token: 0x0600480F RID: 18447 RVA: 0x00196921 File Offset: 0x00194B21
	public string AssignmentGroupID
	{
		get
		{
			return this._assignmentGroupID;
		}
		private set
		{
			this._assignmentGroupID = value;
		}
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x0019692A File Offset: 0x00194B2A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x00196932 File Offset: 0x00194B32
	[OnDeserialized]
	protected void CreateOrRestoreGroupID()
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
			return;
		}
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x0019696F File Offset: 0x00194B6F
	public void SetGroupID(string id)
	{
		DebugUtil.DevAssert(!string.IsNullOrEmpty(id), "Trying to set Assignment group on " + base.gameObject.name + " to null or empty.", null);
		if (!string.IsNullOrEmpty(id))
		{
			this.AssignmentGroupID = id;
		}
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x001969A9 File Offset: 0x00194BA9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreGroupAssignees();
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x001969B8 File Offset: 0x00194BB8
	private void GenerateGroupID()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			return;
		}
		this.SetGroupID(base.GetComponent<KPrefabID>().PrefabID().ToString() + "_" + base.GetComponent<KPrefabID>().InstanceID.ToString() + "_assignmentGroup");
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x00196A44 File Offset: 0x00194C44
	private void RestoreGroupAssignees()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		this.CreateOrRestoreGroupID();
		if (this.minionsInGroupAtLoad == null)
		{
			this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[0];
		}
		for (int i = 0; i < this.minionsInGroupAtLoad.Length; i++)
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, this.minionsInGroupAtLoad[i].Get());
		}
		Ownable component = base.GetComponent<Ownable>();
		if (component != null)
		{
			component.Assign(Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID]);
			component.SetCanBeAssigned(false);
		}
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x00196AE0 File Offset: 0x00194CE0
	public bool CheckMinionIsMember(MinionAssignablesProxy minion)
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
		}
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].HasMember(minion);
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x00196B18 File Offset: 0x00194D18
	public void SetMember(MinionAssignablesProxy minion, bool isAllowed)
	{
		Debug.Assert(DlcManager.IsExpansion1Active());
		if (!isAllowed)
		{
			Game.Instance.assignmentManager.RemoveFromAssignmentGroup(this.AssignmentGroupID, minion);
			return;
		}
		if (!this.CheckMinionIsMember(minion))
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, minion);
		}
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x00196B68 File Offset: 0x00194D68
	protected override void OnCleanUp()
	{
		if (this.generateGroupOnStart)
		{
			Game.Instance.assignmentManager.RemoveAssignmentGroup(this.AssignmentGroupID);
		}
		base.OnCleanUp();
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x00196B90 File Offset: 0x00194D90
	[OnSerializing]
	private void OnSerialize()
	{
		Debug.Assert(!string.IsNullOrEmpty(this.AssignmentGroupID), "Assignment group on " + base.gameObject.name + " has null or empty ID");
		ReadOnlyCollection<IAssignableIdentity> members = Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
		this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[members.Count];
		for (int i = 0; i < members.Count; i++)
		{
			this.minionsInGroupAtLoad[i] = new Ref<MinionAssignablesProxy>((MinionAssignablesProxy)members[i]);
		}
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x00196C25 File Offset: 0x00194E25
	public ReadOnlyCollection<IAssignableIdentity> GetMembers()
	{
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
	}

	// Token: 0x04002FC6 RID: 12230
	public bool generateGroupOnStart;

	// Token: 0x04002FC7 RID: 12231
	[Serialize]
	private string _assignmentGroupID;

	// Token: 0x04002FC8 RID: 12232
	[Serialize]
	private Ref<MinionAssignablesProxy>[] minionsInGroupAtLoad;
}
