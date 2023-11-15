using System;
using UnityEngine;

// Token: 0x02000607 RID: 1543
[AddComponentMenu("KMonoBehaviour/Workable/GasBottler")]
public class GasBottler : Workable
{
	// Token: 0x060026BB RID: 9915 RVA: 0x000D23DA File Offset: 0x000D05DA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new GasBottler.Controller.Instance(this);
		this.smi.StartSM();
		this.UpdateStoredItemState();
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000D23FF File Offset: 0x000D05FF
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000D2420 File Offset: 0x000D0620
	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	// Token: 0x04001635 RID: 5685
	public Storage storage;

	// Token: 0x04001636 RID: 5686
	private GasBottler.Controller.Instance smi;

	// Token: 0x020012B1 RID: 4785
	private class Controller : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler>
	{
		// Token: 0x06007E37 RID: 32311 RVA: 0x002E7960 File Offset: 0x002E5B60
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.filling, (GasBottler.Controller.Instance smi) => smi.master.storage.IsFull());
			this.filling.PlayAnim("working").OnAnimQueueComplete(this.ready);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.pickup, (GasBottler.Controller.Instance smi) => !smi.master.storage.IsFull()).Enter(delegate(GasBottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					gameObject.GetComponent<KPrefabID>().AddTag(GameTags.GasSource, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(GasBottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject go in smi.master.storage.items)
				{
					go.Trigger(-778359855, smi.master.storage);
				}
			});
			this.pickup.PlayAnim("pick_up").OnAnimQueueComplete(this.empty);
		}

		// Token: 0x04006063 RID: 24675
		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State empty;

		// Token: 0x04006064 RID: 24676
		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State filling;

		// Token: 0x04006065 RID: 24677
		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State ready;

		// Token: 0x04006066 RID: 24678
		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State pickup;

		// Token: 0x020020D3 RID: 8403
		public new class Instance : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.GameInstance
		{
			// Token: 0x0600A772 RID: 42866 RVA: 0x0036F579 File Offset: 0x0036D779
			public Instance(GasBottler master) : base(master)
			{
			}
		}
	}
}
