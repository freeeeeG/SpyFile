using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000748 RID: 1864
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/DecorProvider")]
public class DecorProvider : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003372 RID: 13170 RVA: 0x0011252C File Offset: 0x0011072C
	private void AddDecor()
	{
		this.currDecor = 0f;
		if (this.decor != null)
		{
			this.currDecor = this.decor.GetTotalValue();
		}
		if (base.gameObject.HasTag(GameTags.Stored))
		{
			this.currDecor = 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Transparent[num] && Grid.Solid[num] && this.simCellOccupier == null)
		{
			this.currDecor = 0f;
		}
		if (this.currDecor == 0f)
		{
			return;
		}
		this.cellCount = 0;
		int num2 = 5;
		if (this.decorRadius != null)
		{
			num2 = (int)this.decorRadius.GetTotalValue();
		}
		Extents extents = this.occupyArea.GetExtents();
		extents.x = Mathf.Max(extents.x - num2, 0);
		extents.y = Mathf.Max(extents.y - num2, 0);
		extents.width = Mathf.Min(extents.width + num2 * 2, Grid.WidthInCells - 1);
		extents.height = Mathf.Min(extents.height + num2 * 2, Grid.HeightInCells - 1);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatCollectDecorProviders", base.gameObject, extents, GameScenePartitioner.Instance.decorProviderLayer, this.onCollectDecorProvidersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatSolidCheck", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.refreshPartionerCallback);
		int num3 = extents.x + extents.width;
		int num4 = extents.y + extents.height;
		int num5 = extents.x;
		int num6 = extents.y;
		int x;
		int y;
		Grid.CellToXY(num, out x, out y);
		num3 = Math.Min(num3, Grid.WidthInCells);
		num4 = Math.Min(num4, Grid.HeightInCells);
		num5 = Math.Max(0, num5);
		num6 = Math.Max(0, num6);
		int num7 = (num3 - num5) * (num4 - num6);
		if (this.cells == null || this.cells.Length != num7)
		{
			this.cells = new int[num7];
		}
		for (int i = num5; i < num3; i++)
		{
			for (int j = num6; j < num4; j++)
			{
				if (Grid.VisibilityTest(x, y, i, j, false))
				{
					int num8 = Grid.XYToCell(i, j);
					if (Grid.IsValidCell(num8))
					{
						Grid.Decor[num8] += this.currDecor;
						int[] array = this.cells;
						int num9 = this.cellCount;
						this.cellCount = num9 + 1;
						array[num9] = num8;
					}
				}
			}
		}
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x001127C2 File Offset: 0x001109C2
	public void Clear()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		this.RemoveDecor();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x001127F8 File Offset: 0x001109F8
	private void RemoveDecor()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		for (int i = 0; i < this.cellCount; i++)
		{
			int num = this.cells[i];
			if (Grid.IsValidCell(num))
			{
				Grid.Decor[num] -= this.currDecor;
			}
		}
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x0011284C File Offset: 0x00110A4C
	public void Refresh()
	{
		this.Clear();
		this.AddDecor();
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = component.HasTag(RoomConstraints.ConstraintTags.Decor20);
		bool flag2 = this.decor.GetTotalValue() >= 20f;
		if (flag != flag2)
		{
			if (flag2)
			{
				component.AddTag(RoomConstraints.ConstraintTags.Decor20, false);
			}
			else
			{
				component.RemoveTag(RoomConstraints.ConstraintTags.Decor20);
			}
			int cell = Grid.PosToCell(this);
			if (Grid.IsValidCell(cell))
			{
				Game.Instance.roomProber.SolidChangedEvent(cell, true);
			}
		}
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x001128CC File Offset: 0x00110ACC
	public float GetDecorForCell(int cell)
	{
		for (int i = 0; i < this.cellCount; i++)
		{
			if (this.cells[i] == cell)
			{
				return this.currDecor;
			}
		}
		return 0f;
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x00112901 File Offset: 0x00110B01
	public void SetValues(EffectorValues values)
	{
		this.baseDecor = (float)values.amount;
		this.baseRadius = (float)values.radius;
		if (base.IsInitialized())
		{
			this.UpdateBaseDecorModifiers();
		}
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0011292C File Offset: 0x00110B2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decor = this.GetAttributes().Add(Db.Get().BuildingAttributes.Decor);
		this.decorRadius = this.GetAttributes().Add(Db.Get().BuildingAttributes.DecorRadius);
		this.UpdateBaseDecorModifiers();
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x00112988 File Offset: 0x00110B88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectDecorProvidersCallback = new Action<object>(this.OnCollectDecorProviders);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "DecorProvider.OnSpawn");
		AttributeInstance attributeInstance = this.decor;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.decorRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		this.Refresh();
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x00112A3C File Offset: 0x00110C3C
	private void UpdateBaseDecorModifiers()
	{
		Attributes attributes = this.GetAttributes();
		if (this.baseDecorModifier != null)
		{
			attributes.Remove(this.baseDecorModifier);
			attributes.Remove(this.baseDecorRadiusModifier);
			this.baseDecorModifier = null;
			this.baseDecorRadiusModifier = null;
		}
		if (this.baseDecor != 0f)
		{
			this.baseDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, this.baseDecor, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			this.baseDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, this.baseRadius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(this.baseDecorModifier);
			attributes.Add(this.baseDecorRadiusModifier);
		}
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x00112B07 File Offset: 0x00110D07
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x00112B0F File Offset: 0x00110D0F
	private void OnCollectDecorProviders(object data)
	{
		((List<DecorProvider>)data).Add(this);
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x00112B1D File Offset: 0x00110D1D
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.overrideName))
		{
			return base.GetComponent<KSelectable>().GetName();
		}
		return this.overrideName;
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x00112B40 File Offset: 0x00110D40
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			AttributeInstance attributeInstance = this.decor;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
			AttributeInstance attributeInstance2 = this.decorRadius;
			attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}
		this.Clear();
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x00112BC0 File Offset: 0x00110DC0
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.decor != null && this.decorRadius != null)
		{
			float totalValue = this.decor.GetTotalValue();
			float totalValue2 = this.decorRadius.GetTotalValue();
			string arg = (this.baseDecor > 0f) ? "produced" : "consumed";
			string text = (this.baseDecor > 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			text = text + "\n\n" + this.decor.GetAttributeValueTooltip();
			string text2 = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg, text2, totalValue2), string.Format(text, text2, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseDecor != 0f)
		{
			string arg2 = (this.baseDecor >= 0f) ? "produced" : "consumed";
			string format = (this.baseDecor >= 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			string text3 = GameUtil.AddPositiveSign(this.baseDecor.ToString(), this.baseDecor > 0f);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg2, text3, this.baseRadius), string.Format(format, text3, this.baseRadius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x00112D55 File Offset: 0x00110F55
	public static int GetLightDecorBonus(int cell)
	{
		if (Grid.LightIntensity[cell] > 0)
		{
			return DECOR.LIT_BONUS;
		}
		return 0;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00112D6C File Offset: 0x00110F6C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04001EE7 RID: 7911
	public const string ID = "DecorProvider";

	// Token: 0x04001EE8 RID: 7912
	public float baseRadius;

	// Token: 0x04001EE9 RID: 7913
	public float baseDecor;

	// Token: 0x04001EEA RID: 7914
	public string overrideName;

	// Token: 0x04001EEB RID: 7915
	public System.Action refreshCallback;

	// Token: 0x04001EEC RID: 7916
	public Action<object> refreshPartionerCallback;

	// Token: 0x04001EED RID: 7917
	public Action<object> onCollectDecorProvidersCallback;

	// Token: 0x04001EEE RID: 7918
	public AttributeInstance decor;

	// Token: 0x04001EEF RID: 7919
	public AttributeInstance decorRadius;

	// Token: 0x04001EF0 RID: 7920
	private AttributeModifier baseDecorModifier;

	// Token: 0x04001EF1 RID: 7921
	private AttributeModifier baseDecorRadiusModifier;

	// Token: 0x04001EF2 RID: 7922
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04001EF3 RID: 7923
	[MyCmpGet]
	public SimCellOccupier simCellOccupier;

	// Token: 0x04001EF4 RID: 7924
	private int[] cells;

	// Token: 0x04001EF5 RID: 7925
	private int cellCount;

	// Token: 0x04001EF6 RID: 7926
	public float currDecor;

	// Token: 0x04001EF7 RID: 7927
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001EF8 RID: 7928
	private HandleVector<int>.Handle solidChangedPartitionerEntry;
}
