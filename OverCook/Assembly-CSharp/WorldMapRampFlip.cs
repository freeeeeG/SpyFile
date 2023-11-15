using System;
using UnityEngine;

// Token: 0x02000BED RID: 3053
public class WorldMapRampFlip : WorldMapFlipperBase
{
	// Token: 0x06003E50 RID: 15952 RVA: 0x0012A724 File Offset: 0x00128B24
	protected override void Awake()
	{
		base.Awake();
		if (this.m_switchOwnerData.m_switchMapNode != null)
		{
			this.m_switchOwnerData.m_switchMapNode.AddToFlipSet(this.GetOrder(), this);
		}
		if (base.gameObject.RequestComponent<GridAutoParenting>() != null)
		{
			Transform parent = base.gameObject.transform.parent;
			WorldMapTileOptimizer component = parent.GetComponent<WorldMapTileOptimizer>();
			GameObject tile = component.Tile;
			if (tile != null)
			{
				base.transform.SetParent(tile.transform, true);
			}
			WorldMapTileFlip worldMapTileFlip = parent.gameObject.RequireComponent<WorldMapTileFlip>();
			MapNode mapNode = worldMapTileFlip.GetMapNode();
			if (mapNode != null)
			{
				mapNode.RegisterPreFlipCallback(new VoidGeneric<FlipDirection, FlipType>(this.OnPreFlipCallback));
			}
		}
	}

	// Token: 0x06003E51 RID: 15953 RVA: 0x0012A7EB File Offset: 0x00128BEB
	private int GetOrder()
	{
		return 0;
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0012A7F0 File Offset: 0x00128BF0
	public bool ShouldBeUnfolded()
	{
		SwitchMapNode switchMapNode = this.m_switchOwnerData.m_switchMapNode;
		return this.m_startFlipped || switchMapNode == null || switchMapNode.Unfolding || switchMapNode.Unfolded || switchMapNode.IsSwitchedDueToCompletion();
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x0012A83F File Offset: 0x00128C3F
	public override void StartUnfoldFlow()
	{
		if (this.ShouldBeUnfolded())
		{
			base.StartUnfoldFlow();
		}
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x0012A852 File Offset: 0x00128C52
	public override void StartInstantUnfold()
	{
		if (this.ShouldBeUnfolded())
		{
			base.StartInstantUnfold();
		}
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x0012A865 File Offset: 0x00128C65
	private void OnPreFlipCallback(FlipDirection _direction, FlipType _type)
	{
		if (base.IsFlipped() && _type == FlipType.Normal)
		{
			this.StartFoldFlow();
		}
	}

	// Token: 0x04003212 RID: 12818
	[SerializeField]
	private WorldMapRampFlip.SwitchOwnerData m_switchOwnerData = new WorldMapRampFlip.SwitchOwnerData();

	// Token: 0x02000BEE RID: 3054
	[Serializable]
	public class SwitchOwnerData
	{
		// Token: 0x04003213 RID: 12819
		public SwitchMapNode m_switchMapNode;
	}
}
