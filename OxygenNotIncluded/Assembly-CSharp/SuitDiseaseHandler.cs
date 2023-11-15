using System;
using UnityEngine;

// Token: 0x020009FA RID: 2554
[AddComponentMenu("KMonoBehaviour/scripts/SuitDiseaseHandler")]
public class SuitDiseaseHandler : KMonoBehaviour
{
	// Token: 0x06004C61 RID: 19553 RVA: 0x001AC74D File Offset: 0x001AA94D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitDiseaseHandler>(-1617557748, SuitDiseaseHandler.OnEquippedDelegate);
		base.Subscribe<SuitDiseaseHandler>(-170173755, SuitDiseaseHandler.OnUnequippedDelegate);
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x001AC778 File Offset: 0x001AA978
	private PrimaryElement GetPrimaryElement(object data)
	{
		GameObject targetGameObject = ((Equipment)data).GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject)
		{
			return targetGameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x001AC7A8 File Offset: 0x001AA9A8
	private void OnEquipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(true);
			primaryElement.RedirectDisease(base.gameObject);
		}
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x001AC7DC File Offset: 0x001AA9DC
	private void OnUnequipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(false);
			primaryElement.RedirectDisease(null);
		}
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x001AC808 File Offset: 0x001AAA08
	private void OnModifyDiseaseCount(int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().ModifyDiseaseCount(delta, reason);
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x001AC817 File Offset: 0x001AAA17
	private void OnAddDisease(byte disease_idx, int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().AddDisease(disease_idx, delta, reason);
	}

	// Token: 0x040031D6 RID: 12758
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040031D7 RID: 12759
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnUnequipped(data);
	});
}
