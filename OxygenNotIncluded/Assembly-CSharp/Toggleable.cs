using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000517 RID: 1303
[AddComponentMenu("KMonoBehaviour/Workable/Toggleable")]
public class Toggleable : Workable
{
	// Token: 0x06001F44 RID: 8004 RVA: 0x000A6E17 File Offset: 0x000A5017
	private Toggleable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000A6E2C File Offset: 0x000A502C
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.targets = new List<KeyValuePair<IToggleHandler, Chore>>();
		base.SetWorkTime(3f);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Toggling;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x000A6E96 File Offset: 0x000A5096
	public int SetTarget(IToggleHandler handler)
	{
		this.targets.Add(new KeyValuePair<IToggleHandler, Chore>(handler, null));
		return this.targets.Count - 1;
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x000A6EB8 File Offset: 0x000A50B8
	public IToggleHandler GetToggleHandlerForWorker(Worker worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1)
		{
			return this.targets[targetForWorker].Key;
		}
		return null;
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x000A6EE8 File Offset: 0x000A50E8
	private int GetTargetForWorker(Worker worker)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Value != null && this.targets[i].Value.driver != null && this.targets[i].Value.driver.gameObject == worker.gameObject)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x000A6F70 File Offset: 0x000A5170
	protected override void OnCompleteWork(Worker worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1 && this.targets[targetForWorker].Key != null)
		{
			this.targets[targetForWorker] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetForWorker].Key, null);
			this.targets[targetForWorker].Key.HandleToggle();
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000A6FFC File Offset: 0x000A51FC
	private void QueueToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value == null)
		{
			if (DebugHandler.InstantBuildMode)
			{
				this.targets[targetIdx].Key.HandleToggle();
				return;
			}
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, new WorkChore<Toggleable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true));
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000A70AC File Offset: 0x000A52AC
	public void Toggle(int targetIdx)
	{
		if (targetIdx >= this.targets.Count)
		{
			return;
		}
		if (this.targets[targetIdx].Value == null)
		{
			this.QueueToggle(targetIdx);
			return;
		}
		this.CancelToggle(targetIdx);
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x000A70F0 File Offset: 0x000A52F0
	private void CancelToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value != null)
		{
			this.targets[targetIdx].Value.Cancel("Toggle cancelled");
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x000A7174 File Offset: 0x000A5374
	public bool IsToggleQueued(int targetIdx)
	{
		return this.targets[targetIdx].Value != null;
	}

	// Token: 0x04001198 RID: 4504
	private List<KeyValuePair<IToggleHandler, Chore>> targets;
}
