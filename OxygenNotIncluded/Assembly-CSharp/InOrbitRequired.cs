using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004BC RID: 1212
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06001B86 RID: 7046 RVA: 0x0009364C File Offset: 0x0009184C
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
		base.OnSpawn();
		bool newInOrbit = this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround);
		this.UpdateFlag(newInOrbit);
		this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000936A7 File Offset: 0x000918A7
	protected override void OnCleanUp()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000936D4 File Offset: 0x000918D4
	private void OnTagsChanged(object data)
	{
		TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
		if (tagChangedEventData.tag == GameTags.RocketNotOnGround)
		{
			this.UpdateFlag(tagChangedEventData.added);
		}
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x00093706 File Offset: 0x00091906
	private void UpdateFlag(bool newInOrbit)
	{
		this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, this);
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x00093739 File Offset: 0x00091939
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04000F4F RID: 3919
	[MyCmpReq]
	private Building building;

	// Token: 0x04000F50 RID: 3920
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04000F51 RID: 3921
	public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);

	// Token: 0x04000F52 RID: 3922
	private CraftModuleInterface craftModuleInterface;
}
