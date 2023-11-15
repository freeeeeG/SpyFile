using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000597 RID: 1431
[AddComponentMenu("KMonoBehaviour/scripts/AssignmentManager")]
public class AssignmentManager : KMonoBehaviour
{
	// Token: 0x060022CE RID: 8910 RVA: 0x000BED34 File Offset: 0x000BCF34
	public IEnumerator<Assignable> GetEnumerator()
	{
		return this.assignables.GetEnumerator();
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000BED46 File Offset: 0x000BCF46
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe<AssignmentManager>(586301400, AssignmentManager.MinionMigrationDelegate);
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000BED64 File Offset: 0x000BCF64
	protected void MinionMigration(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject() == minionMigrationEventArgs.minionId.gameObject)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000BEE10 File Offset: 0x000BD010
	public void Add(Assignable assignable)
	{
		this.assignables.Add(assignable);
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000BEE1E File Offset: 0x000BD01E
	public void Remove(Assignable assignable)
	{
		this.assignables.Remove(assignable);
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000BEE2D File Offset: 0x000BD02D
	public AssignmentGroup TryCreateAssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		if (this.assignment_groups.ContainsKey(id))
		{
			return this.assignment_groups[id];
		}
		return new AssignmentGroup(id, members, name);
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000BEE52 File Offset: 0x000BD052
	public void RemoveAssignmentGroup(string id)
	{
		if (!this.assignment_groups.ContainsKey(id))
		{
			global::Debug.LogError("Assignment group with id " + id + " doesn't exists");
			return;
		}
		this.assignment_groups.Remove(id);
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000BEE85 File Offset: 0x000BD085
	public void AddToAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].AddMember(member);
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000BEEAA File Offset: 0x000BD0AA
	public void RemoveFromAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].RemoveMember(member);
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000BEED0 File Offset: 0x000BD0D0
	public void RemoveFromAllGroups(IAssignableIdentity member)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee == member)
			{
				assignable.Unassign();
			}
		}
		foreach (KeyValuePair<string, AssignmentGroup> keyValuePair in this.assignment_groups)
		{
			if (keyValuePair.Value.HasMember(member))
			{
				keyValuePair.Value.RemoveMember(member);
			}
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000BEF84 File Offset: 0x000BD184
	public void RemoveFromWorld(IAssignableIdentity minionIdentity, int world_id)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null && assignable.assignee.GetOwners().Count == 1)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee == minionIdentity && assignable.GetMyWorldId() == world_id)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000BF028 File Offset: 0x000BD228
	public List<Assignable> GetPreferredAssignables(Assignables owner, AssignableSlot slot)
	{
		this.PreferredAssignableResults.Clear();
		int num = int.MaxValue;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == slot && assignable.assignee != null && assignable.assignee.HasOwner(owner))
			{
				Room room = assignable.assignee as Room;
				if (room != null && room.roomType.priority_building_use)
				{
					this.PreferredAssignableResults.Clear();
					this.PreferredAssignableResults.Add(assignable);
					return this.PreferredAssignableResults;
				}
				int num2 = assignable.assignee.NumOwners();
				if (num2 == num)
				{
					this.PreferredAssignableResults.Add(assignable);
				}
				else if (num2 < num)
				{
					num = num2;
					this.PreferredAssignableResults.Clear();
					this.PreferredAssignableResults.Add(assignable);
				}
			}
		}
		return this.PreferredAssignableResults;
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000BF138 File Offset: 0x000BD338
	public bool IsPreferredAssignable(Assignables owner, Assignable candidate)
	{
		IAssignableIdentity assignee = candidate.assignee;
		if (assignee == null || !assignee.HasOwner(owner))
		{
			return false;
		}
		int num = assignee.NumOwners();
		Room room = assignee as Room;
		if (room != null && room.roomType.priority_building_use)
		{
			return true;
		}
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == candidate.slot && assignable.assignee != assignee)
			{
				Room room2 = assignable.assignee as Room;
				if (room2 != null && room2.roomType.priority_building_use && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
				if (assignable.assignee.NumOwners() < num && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040013F2 RID: 5106
	private List<Assignable> assignables = new List<Assignable>();

	// Token: 0x040013F3 RID: 5107
	public Dictionary<string, AssignmentGroup> assignment_groups = new Dictionary<string, AssignmentGroup>
	{
		{
			"public",
			new AssignmentGroup("public", new IAssignableIdentity[0], UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.PUBLIC)
		}
	};

	// Token: 0x040013F4 RID: 5108
	private static readonly EventSystem.IntraObjectHandler<AssignmentManager> MinionMigrationDelegate = new EventSystem.IntraObjectHandler<AssignmentManager>(delegate(AssignmentManager component, object data)
	{
		component.MinionMigration(data);
	});

	// Token: 0x040013F5 RID: 5109
	private List<Assignable> PreferredAssignableResults = new List<Assignable>();
}
