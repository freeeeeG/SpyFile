using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200050B RID: 1291
public class StatusItemGroup
{
	// Token: 0x06001E62 RID: 7778 RVA: 0x000A236B File Offset: 0x000A056B
	public IEnumerator<StatusItemGroup.Entry> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06001E63 RID: 7779 RVA: 0x000A237D File Offset: 0x000A057D
	// (set) Token: 0x06001E64 RID: 7780 RVA: 0x000A2385 File Offset: 0x000A0585
	public GameObject gameObject { get; private set; }

	// Token: 0x06001E65 RID: 7781 RVA: 0x000A238E File Offset: 0x000A058E
	public StatusItemGroup(GameObject go)
	{
		this.gameObject = go;
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x000A23C2 File Offset: 0x000A05C2
	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
		Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x000A23E4 File Offset: 0x000A05E4
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				return this.items[i];
			}
		}
		return StatusItemGroup.Entry.EmptyEntry;
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000A2430 File Offset: 0x000A0630
	public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
	{
		if (item != null && item.allowMultiples)
		{
			throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
		}
		if (category == null)
		{
			throw new ArgumentException("SetStatusItem requires a category.");
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				if (this.items[i].item == item)
				{
					this.Log("Set (exists in category)", item, this.items[i].id, category);
					return this.items[i].id;
				}
				this.Log("Set->Remove existing in category", item, this.items[i].id, category);
				this.RemoveStatusItem(this.items[i].id, false);
			}
		}
		if (item != null)
		{
			Guid guid = this.AddStatusItem(item, data, category);
			this.Log("Set (new)", item, guid, category);
			return guid;
		}
		this.Log("Set (failed)", item, Guid.Empty, category);
		return Guid.Empty;
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x000A254B File Offset: 0x000A074B
	public void SetStatusItem(Guid guid, StatusItemCategory category, StatusItem new_item, object data = null)
	{
		this.RemoveStatusItem(guid, false);
		if (new_item != null)
		{
			this.AddStatusItem(new_item, data, category);
		}
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000A2564 File Offset: 0x000A0764
	public bool HasStatusItem(StatusItem status_item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x000A25B0 File Offset: 0x000A07B0
	public bool HasStatusItemID(string status_item_id)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item_id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x000A25F4 File Offset: 0x000A07F4
	public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
	{
		if (this.gameObject == null || (!item.allowMultiples && this.HasStatusItem(item)))
		{
			return Guid.Empty;
		}
		if (!item.allowMultiples)
		{
			using (List<StatusItemGroup.Entry>.Enumerator enumerator = this.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item.Id == item.Id)
					{
						throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
					}
				}
			}
		}
		StatusItemGroup.Entry entry = new StatusItemGroup.Entry(item, category, data);
		if (item.shouldNotify)
		{
			entry.notification = new Notification(item.notificationText, item.notificationType, new Func<List<Notification>, object, string>(StatusItemGroup.OnToolTip), item, false, 0f, item.notificationClickCallback, data, null, true, false, false);
			this.gameObject.AddOrGet<Notifier>().Add(entry.notification, "");
		}
		if (item.ShouldShowIcon())
		{
			Game.Instance.AddStatusItem(this.gameObject.transform, item);
			Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
		}
		this.items.Add(entry);
		if (this.OnAddStatusItem != null)
		{
			this.OnAddStatusItem(entry, category);
		}
		return entry.id;
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x000A2764 File Offset: 0x000A0964
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (status_item.allowMultiples)
		{
			throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
		}
		int i = 0;
		while (i < this.items.Count)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				Guid id = this.items[i].id;
				if (id == Guid.Empty)
				{
					return id;
				}
				this.RemoveStatusItemInternal(id, i, immediate);
				return id;
			}
			else
			{
				i++;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000A27FC File Offset: 0x000A09FC
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (guid == Guid.Empty)
		{
			return guid;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == guid)
			{
				this.RemoveStatusItemInternal(guid, i, immediate);
				return guid;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x000A2858 File Offset: 0x000A0A58
	private void RemoveStatusItemInternal(Guid guid, int itemIdx, bool immediate)
	{
		StatusItemGroup.Entry entry = this.items[itemIdx];
		this.items.RemoveAt(itemIdx);
		if (entry.notification != null)
		{
			this.gameObject.GetComponent<Notifier>().Remove(entry.notification);
		}
		if (entry.item.ShouldShowIcon() && Game.Instance != null)
		{
			Game.Instance.RemoveStatusItem(this.gameObject.transform, entry.item);
		}
		if (this.OnRemoveStatusItem != null)
		{
			this.OnRemoveStatusItem(entry, immediate);
		}
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x000A28E6 File Offset: 0x000A0AE6
	private static string OnToolTip(List<Notification> notifications, object data)
	{
		return ((StatusItem)data).notificationTooltipText + notifications.ReduceMessages(true);
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x000A28FF File Offset: 0x000A0AFF
	public void Destroy()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		while (this.items.Count > 0)
		{
			this.RemoveStatusItem(this.items[0].id, false);
		}
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000A2930 File Offset: 0x000A0B30
	[Conditional("ENABLE_LOGGER")]
	private void Log(string action, StatusItem item, Guid guid)
	{
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000A2932 File Offset: 0x000A0B32
	private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
	{
	}

	// Token: 0x04001118 RID: 4376
	private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();

	// Token: 0x04001119 RID: 4377
	public Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;

	// Token: 0x0400111A RID: 4378
	public Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;

	// Token: 0x0400111C RID: 4380
	private Vector3 offset = new Vector3(0f, 0f, 0f);

	// Token: 0x020011B2 RID: 4530
	public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
	{
		// Token: 0x06007A7A RID: 31354 RVA: 0x002DC099 File Offset: 0x002DA299
		public Entry(StatusItem item, StatusItemCategory category, object data)
		{
			this.id = Guid.NewGuid();
			this.item = item;
			this.data = data;
			this.category = category;
			this.notification = null;
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x002DC0C2 File Offset: 0x002DA2C2
		public string GetName()
		{
			return this.item.GetName(this.data);
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x002DC0D5 File Offset: 0x002DA2D5
		public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style)
		{
			this.item.ShowToolTip(tooltip_widget, this.data, property_style);
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x002DC0EA File Offset: 0x002DA2EA
		public void SetIcon(Image image)
		{
			this.item.SetIcon(image, this.data);
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x002DC0FE File Offset: 0x002DA2FE
		public int CompareTo(StatusItemGroup.Entry other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x002DC111 File Offset: 0x002DA311
		public bool Equals(StatusItemGroup.Entry other)
		{
			return this.id == other.id;
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x002DC124 File Offset: 0x002DA324
		public void OnClick()
		{
			this.item.OnClick(this.data);
		}

		// Token: 0x04005D3F RID: 23871
		public static StatusItemGroup.Entry EmptyEntry = new StatusItemGroup.Entry
		{
			id = Guid.Empty
		};

		// Token: 0x04005D40 RID: 23872
		public Guid id;

		// Token: 0x04005D41 RID: 23873
		public StatusItem item;

		// Token: 0x04005D42 RID: 23874
		public object data;

		// Token: 0x04005D43 RID: 23875
		public Notification notification;

		// Token: 0x04005D44 RID: 23876
		public StatusItemCategory category;
	}
}
