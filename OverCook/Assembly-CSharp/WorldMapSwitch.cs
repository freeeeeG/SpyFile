using System;
using UnityEngine;

// Token: 0x02000BF3 RID: 3059
[RequireComponent(typeof(WorldMapFlipperBase))]
public class WorldMapSwitch : MonoBehaviour
{
	// Token: 0x06003E73 RID: 15987 RVA: 0x0012B07C File Offset: 0x0012947C
	public bool IsFlipped()
	{
		WorldMapFlipperBase worldMapFlipperBase = base.gameObject.RequireComponent<WorldMapFlipperBase>();
		return worldMapFlipperBase.IsFlipped();
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0012B09B File Offset: 0x0012949B
	public bool CanBePressed()
	{
		return this.m_switchOwnerData.m_switchMapNode == null || this.m_switchOwnerData.m_switchMapNode.CanProcessSwitch();
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0012B0C6 File Offset: 0x001294C6
	public bool IsSwitchActivated()
	{
		return this.m_switchOwnerData.m_switchMapNode == null || this.m_switchOwnerData.m_switchMapNode.IsSwitchPressed();
	}

	// Token: 0x0400322A RID: 12842
	[SerializeField]
	public WorldMapSwitch.SwitchOwnerData m_switchOwnerData = new WorldMapSwitch.SwitchOwnerData();

	// Token: 0x0400322B RID: 12843
	[SerializeField]
	public MeshRenderer m_PadMesh;

	// Token: 0x0400322C RID: 12844
	[SerializeField]
	public MeshRenderer m_PadGlowMesh;

	// Token: 0x0400322D RID: 12845
	[SerializeField]
	public float m_PadPressDistance = 0.02f;

	// Token: 0x02000BF4 RID: 3060
	[Serializable]
	public class SwitchOwnerData
	{
		// Token: 0x0400322E RID: 12846
		public SwitchMapNode m_switchMapNode;
	}
}
