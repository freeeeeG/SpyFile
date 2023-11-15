using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000775 RID: 1909
[AddComponentMenu("KMonoBehaviour/Workable/DropToUserCapacity")]
public class DropToUserCapacity : Workable
{
	// Token: 0x060034C9 RID: 13513 RVA: 0x0011B77D File Offset: 0x0011997D
	protected DropToUserCapacity()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x0011B790 File Offset: 0x00119990
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		base.Subscribe<DropToUserCapacity>(-945020481, DropToUserCapacity.OnStorageCapacityChangedHandler);
		base.Subscribe<DropToUserCapacity>(-1697596308, DropToUserCapacity.OnStorageChangedHandler);
		this.synchronizeAnims = false;
		base.SetWorkTime(0.1f);
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x0011B7EC File Offset: 0x001199EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateChore();
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x0011B7FA File Offset: 0x001199FA
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x0011B816 File Offset: 0x00119A16
	private void OnStorageChanged(object data)
	{
		this.UpdateChore();
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x0011B820 File Offset: 0x00119A20
	public void UpdateChore()
	{
		IUserControlledCapacity component = base.GetComponent<IUserControlledCapacity>();
		if (component != null && component.AmountStored > component.UserMaxCapacity)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<DropToUserCapacity>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Cancelled emptying");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x0011B8B4 File Offset: 0x00119AB4
	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = base.GetComponent<Storage>();
		IUserControlledCapacity component2 = base.GetComponent<IUserControlledCapacity>();
		float num = Mathf.Max(0f, component2.AmountStored - component2.UserMaxCapacity);
		List<GameObject> list = new List<GameObject>(component.items);
		for (int i = 0; i < list.Count; i++)
		{
			Pickupable component3 = list[i].GetComponent<Pickupable>();
			if (component3.PrimaryElement.Mass > num)
			{
				component3.Take(num).transform.SetPosition(base.transform.GetPosition());
				return;
			}
			num -= component3.PrimaryElement.Mass;
			component.Drop(component3.gameObject, true);
		}
		this.chore = null;
	}

	// Token: 0x04001FE9 RID: 8169
	private Chore chore;

	// Token: 0x04001FEA RID: 8170
	private bool showCmd;

	// Token: 0x04001FEB RID: 8171
	private Storage[] storages;

	// Token: 0x04001FEC RID: 8172
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageCapacityChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x04001FED RID: 8173
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});
}
