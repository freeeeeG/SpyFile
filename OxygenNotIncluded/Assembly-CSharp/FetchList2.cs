using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
public class FetchList2 : IFetchList
{
	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x060036EF RID: 14063 RVA: 0x0012944C File Offset: 0x0012764C
	// (set) Token: 0x060036F0 RID: 14064 RVA: 0x00129454 File Offset: 0x00127654
	public bool ShowStatusItem
	{
		get
		{
			return this.bShowStatusItem;
		}
		set
		{
			this.bShowStatusItem = value;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x060036F1 RID: 14065 RVA: 0x0012945D File Offset: 0x0012765D
	public bool IsComplete
	{
		get
		{
			return this.FetchOrders.Count == 0;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x060036F2 RID: 14066 RVA: 0x00129470 File Offset: 0x00127670
	public bool InProgress
	{
		get
		{
			if (this.FetchOrders.Count < 0)
			{
				return false;
			}
			bool result = false;
			using (List<FetchOrder2>.Enumerator enumerator = this.FetchOrders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060036F3 RID: 14067 RVA: 0x001294DC File Offset: 0x001276DC
	// (set) Token: 0x060036F4 RID: 14068 RVA: 0x001294E4 File Offset: 0x001276E4
	public Storage Destination { get; private set; }

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060036F5 RID: 14069 RVA: 0x001294ED File Offset: 0x001276ED
	// (set) Token: 0x060036F6 RID: 14070 RVA: 0x001294F5 File Offset: 0x001276F5
	public int PriorityMod { get; private set; }

	// Token: 0x060036F7 RID: 14071 RVA: 0x00129500 File Offset: 0x00127700
	public FetchList2(Storage destination, ChoreType chore_type)
	{
		this.Destination = destination;
		this.choreType = chore_type;
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x0012956C File Offset: 0x0012776C
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			this.FetchOrders[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x001295B0 File Offset: 0x001277B0
	public void Add(HashSet<Tag> tags, Tag requiredTag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, requiredTag, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x00129640 File Offset: 0x00127840
	public void Add(HashSet<Tag> tags, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x001296D4 File Offset: 0x001278D4
	public void Add(Tag tag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		if (!this.MinimumAmount.ContainsKey(tag))
		{
			this.MinimumAmount[tag] = amount;
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, new HashSet<Tag>
		{
			tag
		}, FetchChore.MatchCriteria.MatchTags, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x00129738 File Offset: 0x00127938
	public float GetMinimumAmount(Tag tag)
	{
		float result = 0f;
		this.MinimumAmount.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x0012975B File Offset: 0x0012795B
	private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
	{
		this.FetchOrders.Remove(fetch_order);
		if (this.FetchOrders.Count == 0)
		{
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
			FetchListStatusItemUpdater.instance.RemoveFetchList(this);
			this.ClearStatus();
		}
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x0012979C File Offset: 0x0012799C
	public void Cancel(string reason)
	{
		FetchListStatusItemUpdater.instance.RemoveFetchList(this);
		this.ClearStatus();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Cancel(reason);
		}
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x00129800 File Offset: 0x00127A00
	public void UpdateRemaining()
	{
		this.Remaining.Clear();
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			FetchOrder2 fetchOrder = this.FetchOrders[i];
			foreach (Tag key in fetchOrder.Tags)
			{
				float num = 0f;
				this.Remaining.TryGetValue(key, out num);
				this.Remaining[key] = num + fetchOrder.AmountWaitingToFetch();
			}
		}
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x001298A8 File Offset: 0x00127AA8
	public Dictionary<Tag, float> GetRemaining()
	{
		return this.Remaining;
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x001298B0 File Offset: 0x00127AB0
	public Dictionary<Tag, float> GetRemainingMinimum()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			foreach (Tag key in fetchOrder.Tags)
			{
				dictionary[key] = this.MinimumAmount[key];
			}
		}
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					if (dictionary.ContainsKey(kprefabID.PrefabTag))
					{
						dictionary[kprefabID.PrefabTag] = Math.Max(dictionary[kprefabID.PrefabTag] - component.TotalAmount, 0f);
					}
					foreach (Tag key2 in kprefabID.Tags)
					{
						if (dictionary.ContainsKey(key2))
						{
							dictionary[key2] = Math.Max(dictionary[key2] - component.TotalAmount, 0f);
						}
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x00129A68 File Offset: 0x00127C68
	public void Suspend(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Suspend(reason);
		}
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x00129ABC File Offset: 0x00127CBC
	public void Resume(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Resume(reason);
		}
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x00129B10 File Offset: 0x00127D10
	public void Submit(System.Action on_complete, bool check_storage_contents)
	{
		this.OnComplete = on_complete;
		foreach (FetchOrder2 fetchOrder in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
		{
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, null);
		}
		if (!this.IsComplete && this.ShowStatusItem)
		{
			FetchListStatusItemUpdater.instance.AddFetchList(this);
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x00129BA4 File Offset: 0x00127DA4
	private void ClearStatus()
	{
		if (this.Destination != null)
		{
			KSelectable component = this.Destination.GetComponent<KSelectable>();
			if (component != null)
			{
				this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
				this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
				this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
			}
		}
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x00129C10 File Offset: 0x00127E10
	public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
	{
		bool flag = handle != Guid.Empty;
		if (should_add != flag)
		{
			if (should_add)
			{
				KSelectable component = this.Destination.GetComponent<KSelectable>();
				if (component != null)
				{
					handle = component.AddStatusItem(status_item, this);
					GameScheduler.Instance.Schedule("Digging Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
					}, null, null);
					return;
				}
			}
			else
			{
				KSelectable component2 = this.Destination.GetComponent<KSelectable>();
				if (component2 != null)
				{
					handle = component2.RemoveStatusItem(handle, false);
				}
			}
		}
	}

	// Token: 0x040021DD RID: 8669
	private System.Action OnComplete;

	// Token: 0x040021E0 RID: 8672
	private ChoreType choreType;

	// Token: 0x040021E1 RID: 8673
	public Guid waitingForMaterialsHandle = Guid.Empty;

	// Token: 0x040021E2 RID: 8674
	public Guid materialsUnavailableForRefillHandle = Guid.Empty;

	// Token: 0x040021E3 RID: 8675
	public Guid materialsUnavailableHandle = Guid.Empty;

	// Token: 0x040021E4 RID: 8676
	public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();

	// Token: 0x040021E5 RID: 8677
	public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();

	// Token: 0x040021E6 RID: 8678
	private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();

	// Token: 0x040021E7 RID: 8679
	private bool bShowStatusItem = true;
}
