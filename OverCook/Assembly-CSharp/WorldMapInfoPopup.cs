using System;
using UnityEngine;

// Token: 0x02000BE7 RID: 3047
public class WorldMapInfoPopup : MonoBehaviour
{
	// Token: 0x04003200 RID: 12800
	[SerializeField]
	public WorldMapInfoPopup.Type m_type;

	// Token: 0x04003201 RID: 12801
	[SerializeField]
	public PlayerInputLookup.LogicalButtonID m_button;

	// Token: 0x04003202 RID: 12802
	[SerializeField]
	public T17Image m_buttonImage;

	// Token: 0x04003203 RID: 12803
	[SerializeField]
	public bool m_autoCancel;

	// Token: 0x04003204 RID: 12804
	[SerializeField]
	public float m_autoCancelTime = 5f;

	// Token: 0x02000BE8 RID: 3048
	public enum Type
	{
		// Token: 0x04003206 RID: 12806
		Switch,
		// Token: 0x04003207 RID: 12807
		HiddenLevel,
		// Token: 0x04003208 RID: 12808
		NewGamePlus,
		// Token: 0x04003209 RID: 12809
		PracticeMode,
		// Token: 0x0400320A RID: 12810
		HordeMode
	}
}
