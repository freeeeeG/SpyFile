using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000776 RID: 1910
[AddComponentMenu("KMonoBehaviour/Workable/Dumpable")]
public class Dumpable : Workable
{
	// Token: 0x060034D1 RID: 13521 RVA: 0x0011B9A2 File Offset: 0x00119BA2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x0011B9D0 File Offset: 0x00119BD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDumping)
		{
			this.CreateChore();
		}
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_dumpable_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"working"
		};
		this.synchronizeAnims = false;
		base.SetWorkTime(1f);
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x0011BA40 File Offset: 0x00119C40
	public void ToggleDumping()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.isMarkedForDumping)
		{
			this.isMarkedForDumping = false;
			this.chore.Cancel("Cancel Dumping!");
			Prioritizable.RemoveRef(base.gameObject);
			this.chore = null;
			base.ShowProgressBar(false);
			return;
		}
		this.isMarkedForDumping = true;
		this.CreateChore();
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x0011BAA4 File Offset: 0x00119CA4
	private void CreateChore()
	{
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x0011BAF0 File Offset: 0x00119CF0
	protected override void OnCompleteWork(Worker worker)
	{
		this.isMarkedForDumping = false;
		this.chore = null;
		this.Dump();
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x0011BB11 File Offset: 0x00119D11
	public void Dump()
	{
		this.Dump(base.transform.GetPosition());
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x0011BB24 File Offset: 0x00119D24
	public void Dump(Vector3 pos)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			if (component.Element.IsLiquid)
			{
				FallingWater.instance.AddParticle(Grid.PosToCell(pos), component.Element.idx, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, false, false, false);
			}
			else
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
			}
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x0011BBCC File Offset: 0x00119DCC
	private void OnRefreshUserMenu(object data)
	{
		if (this.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001FEE RID: 8174
	private Chore chore;

	// Token: 0x04001FEF RID: 8175
	[Serialize]
	private bool isMarkedForDumping;

	// Token: 0x04001FF0 RID: 8176
	private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>(delegate(Dumpable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
