using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BEC RID: 3052
public class WorldMapPathFlip : WorldMapFlipperBase
{
	// Token: 0x06003E4D RID: 15949 RVA: 0x0012A674 File Offset: 0x00128A74
	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < this.m_levelPortalMapNode.Length; i++)
		{
			this.m_levelPortalMapNode[i].RegisterFlipCallback(new VoidGeneric<FlipDirection, FlipType>(this.OnFlipCallback));
		}
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x0012A6BC File Offset: 0x00128ABC
	private void OnFlipCallback(FlipDirection _direction, FlipType _type)
	{
		this.m_collectedFlipTypes.Add(_type);
		if (this.m_collectedFlipTypes.Count == this.m_levelPortalMapNode.Length)
		{
			if (this.m_collectedFlipTypes.Contains(FlipType.Normal))
			{
				this.StartUnfoldFlow();
			}
			else
			{
				this.StartInstantUnfold();
			}
		}
	}

	// Token: 0x04003210 RID: 12816
	[SerializeField]
	private MapNode[] m_levelPortalMapNode = new MapNode[0];

	// Token: 0x04003211 RID: 12817
	private List<FlipType> m_collectedFlipTypes = new List<FlipType>();
}
