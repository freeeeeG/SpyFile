using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x020004BA RID: 1210
public class AssignmentGroup : IAssignableIdentity
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06001B76 RID: 7030 RVA: 0x000933B6 File Offset: 0x000915B6
	// (set) Token: 0x06001B77 RID: 7031 RVA: 0x000933BE File Offset: 0x000915BE
	public string id { get; private set; }

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000933C7 File Offset: 0x000915C7
	// (set) Token: 0x06001B79 RID: 7033 RVA: 0x000933CF File Offset: 0x000915CF
	public string name { get; private set; }

	// Token: 0x06001B7A RID: 7034 RVA: 0x000933D8 File Offset: 0x000915D8
	public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		this.id = id;
		this.name = name;
		foreach (IAssignableIdentity item in members)
		{
			this.members.Add(item);
		}
		if (Game.Instance != null)
		{
			Game.Instance.assignmentManager.assignment_groups.Add(id, this);
			Game.Instance.Trigger(-1123234494, this);
		}
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x00093462 File Offset: 0x00091662
	public void AddMember(IAssignableIdentity member)
	{
		if (!this.members.Contains(member))
		{
			this.members.Add(member);
		}
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0009348E File Offset: 0x0009168E
	public void RemoveMember(IAssignableIdentity member)
	{
		this.members.Remove(member);
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000934AD File Offset: 0x000916AD
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x000934B5 File Offset: 0x000916B5
	public bool HasMember(IAssignableIdentity member)
	{
		return this.members.Contains(member);
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000934C3 File Offset: 0x000916C3
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000934C6 File Offset: 0x000916C6
	public ReadOnlyCollection<IAssignableIdentity> GetMembers()
	{
		return this.members.AsReadOnly();
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000934D4 File Offset: 0x000916D4
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			this.current_owners.AddRange(assignableIdentity.GetOwners());
		}
		return this.current_owners;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x00093544 File Offset: 0x00091744
	public Ownables GetSoleOwner()
	{
		if (this.members.Count == 1)
		{
			return this.members[0] as Ownables;
		}
		Debug.LogWarningFormat("GetSoleOwner called on AssignmentGroup with {0} members", new object[]
		{
			this.members.Count
		});
		return null;
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x00093598 File Offset: 0x00091798
	public bool HasOwner(Assignables owner)
	{
		using (List<IAssignableIdentity>.Enumerator enumerator = this.members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasOwner(owner))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x000935F4 File Offset: 0x000917F4
	public int NumOwners()
	{
		int num = 0;
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			num += assignableIdentity.NumOwners();
		}
		return num;
	}

	// Token: 0x04000F4D RID: 3917
	private List<IAssignableIdentity> members = new List<IAssignableIdentity>();

	// Token: 0x04000F4E RID: 3918
	public List<Ownables> current_owners = new List<Ownables>();
}
