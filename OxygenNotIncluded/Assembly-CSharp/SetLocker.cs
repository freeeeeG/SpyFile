using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200096B RID: 2411
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x060046AF RID: 18095 RVA: 0x0018F473 File Offset: 0x0018D673
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x0018F47B File Offset: 0x0018D67B
	public void ChooseContents()
	{
		this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x0018F49C File Offset: 0x0018D69C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x0018F4B0 File Offset: 0x0018D6B0
	public void DropContents()
	{
		if (this.contents == null)
		{
			return;
		}
		for (int i = 0; i < this.contents.Length; i++)
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[i], Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[i].ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
		}
		if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
		{
			int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
			for (int j = 0; j <= num; j++)
			{
				Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		base.gameObject.Trigger(-372600542, this);
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x0018F611 File Offset: 0x0018D811
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x0018F61A File Offset: 0x0018D81A
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x0018F624 File Offset: 0x0018D824
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x0018F690 File Offset: 0x0018D890
	public void CancelChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x0018F6B2 File Offset: 0x0018D8B2
	private void CompleteChore()
	{
		this.used = true;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x060046B8 RID: 18104 RVA: 0x0018F6F2 File Offset: 0x0018D8F2
	public string SidescreenButtonText
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.NAME : UI.USERMENUACTIONS.OPENPOI.NAME_OFF;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x060046B9 RID: 18105 RVA: 0x0018F70D File Offset: 0x0018D90D
	public string SidescreenButtonTooltip
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.TOOLTIP : UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;
		}
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x0018F728 File Offset: 0x0018D928
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x0018F72B File Offset: 0x0018D92B
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x0018F72E File Offset: 0x0018D92E
	public void OnSidescreenButtonPressed()
	{
		if (this.chore == null)
		{
			this.OnClickOpen();
			return;
		}
		this.OnClickCancel();
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x0018F745 File Offset: 0x0018D945
	public bool SidescreenButtonInteractable()
	{
		return !this.used;
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x0018F750 File Offset: 0x0018D950
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x0018F754 File Offset: 0x0018D954
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04002EE2 RID: 12002
	public string[][] possible_contents_ids;

	// Token: 0x04002EE3 RID: 12003
	public string machineSound;

	// Token: 0x04002EE4 RID: 12004
	public string overrideAnim;

	// Token: 0x04002EE5 RID: 12005
	public Vector2I dropOffset = Vector2I.zero;

	// Token: 0x04002EE6 RID: 12006
	public int[] numDataBanks;

	// Token: 0x04002EE7 RID: 12007
	[Serialize]
	private string[] contents;

	// Token: 0x04002EE8 RID: 12008
	[Serialize]
	private bool used;

	// Token: 0x04002EE9 RID: 12009
	private Chore chore;

	// Token: 0x020017CB RID: 6091
	public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
	{
		// Token: 0x06008F7D RID: 36733 RVA: 0x00322A4E File Offset: 0x00320C4E
		public StatesInstance(SetLocker master) : base(master)
		{
		}
	}

	// Token: 0x020017CC RID: 6092
	public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
	{
		// Token: 0x06008F7E RID: 36734 RVA: 0x00322A58 File Offset: 0x00320C58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.PlayAnim("working").OnAnimQueueComplete(this.off).Exit(delegate(SetLocker.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.off.PlayAnim("off").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x04006FED RID: 28653
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;

		// Token: 0x04006FEE RID: 28654
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;

		// Token: 0x04006FEF RID: 28655
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;
	}
}
