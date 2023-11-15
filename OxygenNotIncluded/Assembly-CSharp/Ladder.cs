using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200061D RID: 1565
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Ladder")]
public class Ladder : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06002782 RID: 10114 RVA: 0x000D66A0 File Offset: 0x000D48A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int i2 = Grid.OffsetCell(Grid.PosToCell(this), offset);
			Grid.HasPole[i2] = this.isPole;
			Grid.HasLadder[i2] = !this.isPole;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Ladders, false);
		Components.Ladders.Add(this);
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000D673E File Offset: 0x000D493E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000D6774 File Offset: 0x000D4974
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int num = Grid.OffsetCell(Grid.PosToCell(this), offset);
			if (Grid.Objects[num, 24] == null)
			{
				Grid.HasPole[num] = false;
				Grid.HasLadder[num] = false;
			}
		}
		Components.Ladders.Remove(this);
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000D680C File Offset: 0x000D4A0C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.upwardsMovementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x040016D5 RID: 5845
	public float upwardsMovementSpeedMultiplier = 1f;

	// Token: 0x040016D6 RID: 5846
	public float downwardsMovementSpeedMultiplier = 1f;

	// Token: 0x040016D7 RID: 5847
	public bool isPole;

	// Token: 0x040016D8 RID: 5848
	public CellOffset[] offsets = new CellOffset[]
	{
		CellOffset.none
	};
}
