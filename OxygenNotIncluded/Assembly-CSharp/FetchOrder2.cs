using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
public class FetchOrder2
{
	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x0600371F RID: 14111 RVA: 0x0012A88F File Offset: 0x00128A8F
	// (set) Token: 0x06003720 RID: 14112 RVA: 0x0012A897 File Offset: 0x00128A97
	public float TotalAmount { get; set; }

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06003721 RID: 14113 RVA: 0x0012A8A0 File Offset: 0x00128AA0
	// (set) Token: 0x06003722 RID: 14114 RVA: 0x0012A8A8 File Offset: 0x00128AA8
	public int PriorityMod { get; set; }

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06003723 RID: 14115 RVA: 0x0012A8B1 File Offset: 0x00128AB1
	// (set) Token: 0x06003724 RID: 14116 RVA: 0x0012A8B9 File Offset: 0x00128AB9
	public HashSet<Tag> Tags { get; protected set; }

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06003725 RID: 14117 RVA: 0x0012A8C2 File Offset: 0x00128AC2
	// (set) Token: 0x06003726 RID: 14118 RVA: 0x0012A8CA File Offset: 0x00128ACA
	public FetchChore.MatchCriteria Criteria { get; protected set; }

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06003727 RID: 14119 RVA: 0x0012A8D3 File Offset: 0x00128AD3
	// (set) Token: 0x06003728 RID: 14120 RVA: 0x0012A8DB File Offset: 0x00128ADB
	public Tag RequiredTag { get; protected set; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06003729 RID: 14121 RVA: 0x0012A8E4 File Offset: 0x00128AE4
	// (set) Token: 0x0600372A RID: 14122 RVA: 0x0012A8EC File Offset: 0x00128AEC
	public Tag[] ForbiddenTags { get; protected set; }

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x0600372B RID: 14123 RVA: 0x0012A8F5 File Offset: 0x00128AF5
	// (set) Token: 0x0600372C RID: 14124 RVA: 0x0012A8FD File Offset: 0x00128AFD
	public Storage Destination { get; set; }

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x0600372D RID: 14125 RVA: 0x0012A906 File Offset: 0x00128B06
	// (set) Token: 0x0600372E RID: 14126 RVA: 0x0012A90E File Offset: 0x00128B0E
	private float UnfetchedAmount
	{
		get
		{
			return this._UnfetchedAmount;
		}
		set
		{
			this._UnfetchedAmount = value;
			this.Assert(this._UnfetchedAmount <= this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
			this.Assert(this._UnfetchedAmount >= 0f, "_UnfetchedAmount >= 0");
		}
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x0012A950 File Offset: 0x00128B50
	public FetchOrder2(ChoreType chore_type, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags, Storage destination, float amount, Operational.State operationalRequirementDEPRECATED = Operational.State.None, int priorityMod = 0)
	{
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", new object[]
				{
					chore_type.Id,
					tags,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		this.choreType = chore_type;
		this.Tags = tags;
		this.Criteria = criteria;
		this.RequiredTag = required_tag;
		this.ForbiddenTags = forbidden_tags;
		this.Destination = destination;
		this.TotalAmount = amount;
		this.UnfetchedAmount = amount;
		this.PriorityMod = priorityMod;
		this.operationalRequirement = operationalRequirementDEPRECATED;
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06003730 RID: 14128 RVA: 0x0012AA1C File Offset: 0x00128C1C
	public bool InProgress
	{
		get
		{
			bool result = false;
			using (List<FetchChore>.Enumerator enumerator = this.Chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x0012AA78 File Offset: 0x00128C78
	private void IssueTask()
	{
		if (this.UnfetchedAmount > 0f)
		{
			this.SetFetchTask(this.UnfetchedAmount);
			this.UnfetchedAmount = 0f;
		}
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x0012AAA0 File Offset: 0x00128CA0
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.Chores.Count; i++)
		{
			this.Chores[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x0012AAE4 File Offset: 0x00128CE4
	private void SetFetchTask(float amount)
	{
		FetchChore fetchChore = new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, null, true, new Action<Chore>(this.OnFetchChoreComplete), new Action<Chore>(this.OnFetchChoreBegin), new Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod);
		fetchChore.validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange;
		this.Chores.Add(fetchChore);
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x0012AB68 File Offset: 0x00128D68
	private void OnFetchChoreEnd(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		if (this.Chores.Contains(fetchChore))
		{
			this.UnfetchedAmount += fetchChore.amount;
			fetchChore.Cancel("FetchChore Redistribution");
			this.Chores.Remove(fetchChore);
			this.IssueTask();
		}
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x0012ABBC File Offset: 0x00128DBC
	private void OnFetchChoreComplete(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.Chores.Remove(fetchChore);
		if (this.Chores.Count == 0 && this.OnComplete != null)
		{
			this.OnComplete(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x0012AC04 File Offset: 0x00128E04
	private void OnFetchChoreBegin(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
		this.IssueTask();
		if (this.OnBegin != null)
		{
			this.OnBegin(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x0012AC54 File Offset: 0x00128E54
	public void Cancel(string reason)
	{
		while (this.Chores.Count > 0)
		{
			FetchChore fetchChore = this.Chores[0];
			fetchChore.Cancel(reason);
			this.Chores.Remove(fetchChore);
		}
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x0012AC92 File Offset: 0x00128E92
	public void Suspend(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x0012AC9E File Offset: 0x00128E9E
	public void Resume(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x0012ACAC File Offset: 0x00128EAC
	public void Submit(Action<FetchOrder2, Pickupable> on_complete, bool check_storage_contents, Action<FetchOrder2, Pickupable> on_begin = null)
	{
		this.OnComplete = on_complete;
		this.OnBegin = on_begin;
		this.checkStorageContents = check_storage_contents;
		if (check_storage_contents)
		{
			Pickupable arg = null;
			this.UnfetchedAmount = this.GetRemaining(out arg);
			if (this.UnfetchedAmount > this.Destination.storageFullMargin)
			{
				this.IssueTask();
				return;
			}
			if (this.OnComplete != null)
			{
				this.OnComplete(this, arg);
				return;
			}
		}
		else
		{
			this.IssueTask();
		}
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x0012AD18 File Offset: 0x00128F18
	public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
	{
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					foreach (Tag tag in this.Tags)
					{
						if (kprefabID.HasTag(tag))
						{
							amount = component.TotalAmount;
							out_item = component;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x0012ADE4 File Offset: 0x00128FE4
	public float AmountWaitingToFetch()
	{
		if (!this.checkStorageContents)
		{
			float num = this.UnfetchedAmount;
			for (int i = 0; i < this.Chores.Count; i++)
			{
				num += this.Chores[i].AmountWaitingToFetch();
			}
			return num;
		}
		Pickupable pickupable;
		return this.GetRemaining(out pickupable);
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x0012AE34 File Offset: 0x00129034
	public float GetRemaining(out Pickupable out_item)
	{
		float num = this.TotalAmount;
		float num2 = 0f;
		out_item = null;
		if (this.IsMaterialOnStorage(this.Destination, ref num2, ref out_item))
		{
			num = Math.Max(num - num2, 0f);
		}
		return num;
	}

	// Token: 0x0600373E RID: 14142 RVA: 0x0012AE74 File Offset: 0x00129074
	public bool IsComplete()
	{
		for (int i = 0; i < this.Chores.Count; i++)
		{
			if (!this.Chores[i].isComplete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x0012AEB0 File Offset: 0x001290B0
	private void Assert(bool condition, string message)
	{
		if (condition)
		{
			return;
		}
		string text = "FetchOrder error: " + message;
		if (this.Destination == null)
		{
			text += "\nDestination: None";
		}
		else
		{
			text = text + "\nDestination: " + this.Destination.name;
		}
		text = text + "\nTotal Amount: " + this.TotalAmount.ToString();
		text = text + "\nUnfetched Amount: " + this._UnfetchedAmount.ToString();
		global::Debug.LogError(text);
	}

	// Token: 0x040021F1 RID: 8689
	public Action<FetchOrder2, Pickupable> OnComplete;

	// Token: 0x040021F2 RID: 8690
	public Action<FetchOrder2, Pickupable> OnBegin;

	// Token: 0x040021F7 RID: 8695
	public bool validateRequiredTagOnTagChange;

	// Token: 0x040021FB RID: 8699
	public List<FetchChore> Chores = new List<FetchChore>();

	// Token: 0x040021FC RID: 8700
	private ChoreType choreType;

	// Token: 0x040021FD RID: 8701
	private float _UnfetchedAmount;

	// Token: 0x040021FE RID: 8702
	private bool checkStorageContents;

	// Token: 0x040021FF RID: 8703
	private Operational.State operationalRequirement = Operational.State.None;
}
