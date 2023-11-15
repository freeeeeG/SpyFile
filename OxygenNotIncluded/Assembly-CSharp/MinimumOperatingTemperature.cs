using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
	// Token: 0x06001C3D RID: 7229 RVA: 0x000970AF File Offset: 0x000952AF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.TestTemperature(true);
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000970BE File Offset: 0x000952BE
	public void Sim200ms(float dt)
	{
		this.TestTemperature(false);
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x000970C8 File Offset: 0x000952C8
	private void TestTemperature(bool force)
	{
		bool flag;
		if (this.primaryElement.Temperature < this.minimumTemperature)
		{
			flag = false;
		}
		else
		{
			flag = true;
			for (int i = 0; i < this.building.PlacementCells.Length; i++)
			{
				int i2 = this.building.PlacementCells[i];
				float num = Grid.Temperature[i2];
				float num2 = Grid.Mass[i2];
				if ((num != 0f || num2 != 0f) && num < this.minimumTemperature)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			this.lastOffTime = Time.time;
		}
		if ((flag != this.isWarm && !flag) || (flag != this.isWarm && flag && Time.time > this.lastOffTime + 5f) || force)
		{
			this.isWarm = flag;
			this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, this);
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x000971D2 File Offset: 0x000953D2
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000971EC File Offset: 0x000953EC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x04000F8E RID: 3982
	[MyCmpReq]
	private Building building;

	// Token: 0x04000F8F RID: 3983
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04000F90 RID: 3984
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04000F91 RID: 3985
	public float minimumTemperature = 275.15f;

	// Token: 0x04000F92 RID: 3986
	private const float TURN_ON_DELAY = 5f;

	// Token: 0x04000F93 RID: 3987
	private float lastOffTime;

	// Token: 0x04000F94 RID: 3988
	public static readonly Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

	// Token: 0x04000F95 RID: 3989
	private bool isWarm;

	// Token: 0x04000F96 RID: 3990
	private HandleVector<int>.Handle partitionerEntry;
}
