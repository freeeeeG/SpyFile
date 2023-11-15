using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class StatusItemStackTraceWatcher : IDisposable
{
	// Token: 0x060021CE RID: 8654 RVA: 0x000B9916 File Offset: 0x000B7B16
	public bool GetShouldWatch()
	{
		return this.shouldWatch;
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000B991E File Offset: 0x000B7B1E
	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch == shouldWatch)
		{
			return;
		}
		this.shouldWatch = shouldWatch;
		this.Refresh();
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000B9937 File Offset: 0x000B7B37
	public Option<StatusItemGroup> GetTarget()
	{
		return this.currentTarget;
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000B9940 File Offset: 0x000B7B40
	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if (this.currentTarget.IsNone() && nextTarget.IsNone())
		{
			return;
		}
		if (this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
		{
			return;
		}
		this.currentTarget = nextTarget;
		this.Refresh();
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000B999C File Offset: 0x000B7B9C
	private void Refresh()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
		if (!this.shouldWatch)
		{
			return;
		}
		if (this.currentTarget.IsSome())
		{
			StatusItemGroup target = this.currentTarget.Unwrap();
			Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
			{
				this.entryIdToStackTraceMap[entry.id] = new StackTrace(true);
			};
			StatusItemGroup target3 = target;
			target3.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(target3.OnAddStatusItem, onAddStatusItem);
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				StatusItemGroup target2 = target;
				target2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(target2.OnAddStatusItem, onAddStatusItem);
			}));
			StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
			destroyListener.owner = this;
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				if (destroyListener.IsNullOrDestroyed())
				{
					return;
				}
				UnityEngine.Object.Destroy(destroyListener);
			}));
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				this.entryIdToStackTraceMap.Clear();
			}));
		}
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000B9AB9 File Offset: 0x000B7CB9
	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000B9ACD File Offset: 0x000B7CCD
	public void Dispose()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
	}

	// Token: 0x0400133B RID: 4923
	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	// Token: 0x0400133C RID: 4924
	private Option<StatusItemGroup> currentTarget;

	// Token: 0x0400133D RID: 4925
	private bool shouldWatch;

	// Token: 0x0400133E RID: 4926
	private System.Action onCleanup;

	// Token: 0x02001201 RID: 4609
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		// Token: 0x06007B7A RID: 31610 RVA: 0x002DDF54 File Offset: 0x002DC154
		private void OnDestroy()
		{
			bool flag = this.owner != null;
			bool flag2 = this.owner.currentTarget.IsSome() && this.owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (flag && flag2)
			{
				this.owner.SetTarget(Option.None);
			}
		}

		// Token: 0x04005E36 RID: 24118
		public StatusItemStackTraceWatcher owner;
	}
}
