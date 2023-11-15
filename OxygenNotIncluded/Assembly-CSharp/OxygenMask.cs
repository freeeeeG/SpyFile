using System;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class OxygenMask : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004163 RID: 16739 RVA: 0x0016E1EB File Offset: 0x0016C3EB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
	}

	// Token: 0x06004164 RID: 16740 RVA: 0x0016E204 File Offset: 0x0016C404
	private void CheckOxygenLevels(object data)
	{
		if (this.suitTank.IsEmpty())
		{
			Equippable component = base.GetComponent<Equippable>();
			if (component.assignee != null)
			{
				Ownables soleOwner = component.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					soleOwner.GetComponent<Equipment>().Unequip(component);
				}
			}
		}
	}

	// Token: 0x06004165 RID: 16741 RVA: 0x0016E250 File Offset: 0x0016C450
	public void Sim200ms(float dt)
	{
		if (base.GetComponent<Equippable>().assignee == null)
		{
			float num = this.leakRate * dt;
			float massAvailable = this.storage.GetMassAvailable(this.suitTank.elementTag);
			num = Mathf.Min(num, massAvailable);
			this.storage.DropSome(this.suitTank.elementTag, num, true, true, default(Vector3), true, false);
		}
		if (this.suitTank.IsEmpty())
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04002A90 RID: 10896
	private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>(delegate(OxygenMask component, object data)
	{
		component.CheckOxygenLevels(data);
	});

	// Token: 0x04002A91 RID: 10897
	[MyCmpGet]
	private SuitTank suitTank;

	// Token: 0x04002A92 RID: 10898
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002A93 RID: 10899
	private float leakRate = 0.1f;
}
