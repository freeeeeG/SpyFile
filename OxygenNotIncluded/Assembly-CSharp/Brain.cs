using System;
using UnityEngine;

// Token: 0x0200039B RID: 923
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	// Token: 0x06001343 RID: 4931 RVA: 0x00065537 File Offset: 0x00063737
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0006553F File Offset: 0x0006373F
	protected override void OnSpawn()
	{
		this.prefabId = base.GetComponent<KPrefabID>();
		this.choreConsumer = base.GetComponent<ChoreConsumer>();
		this.running = true;
		Components.Brains.Add(this);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001345 RID: 4933 RVA: 0x0006556C File Offset: 0x0006376C
	// (remove) Token: 0x06001346 RID: 4934 RVA: 0x000655A4 File Offset: 0x000637A4
	public event System.Action onPreUpdate;

	// Token: 0x06001347 RID: 4935 RVA: 0x000655D9 File Offset: 0x000637D9
	public virtual void UpdateBrain()
	{
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (this.IsRunning())
		{
			this.UpdateChores();
		}
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x000655FC File Offset: 0x000637FC
	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return this.choreConsumer.FindNextChore(ref context);
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0006560C File Offset: 0x0006380C
	private void UpdateChores()
	{
		if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context chore = default(Chore.Precondition.Context);
		if (this.FindBetterChore(ref chore))
		{
			if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				base.Trigger(1485595942, null);
				return;
			}
			this.choreConsumer.choreDriver.SetChore(chore);
		}
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0006566E File Offset: 0x0006386E
	public bool IsRunning()
	{
		return this.running && !this.suspend;
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00065683 File Offset: 0x00063883
	public void Reset(string reason)
	{
		this.Stop("Reset");
		this.running = true;
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x00065697 File Offset: 0x00063897
	public void Stop(string reason)
	{
		base.GetComponent<ChoreDriver>().StopChore();
		this.running = false;
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x000656AB File Offset: 0x000638AB
	public void Resume(string caller)
	{
		this.suspend = false;
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x000656B4 File Offset: 0x000638B4
	public void Suspend(string caller)
	{
		this.suspend = true;
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x000656BD File Offset: 0x000638BD
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.Stop("OnCmpDisable");
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000656D0 File Offset: 0x000638D0
	protected override void OnCleanUp()
	{
		this.Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}

	// Token: 0x04000A57 RID: 2647
	private bool running;

	// Token: 0x04000A58 RID: 2648
	private bool suspend;

	// Token: 0x04000A59 RID: 2649
	protected KPrefabID prefabId;

	// Token: 0x04000A5A RID: 2650
	protected ChoreConsumer choreConsumer;
}
