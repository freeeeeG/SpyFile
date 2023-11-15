using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C71 RID: 3185
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
	// Token: 0x04004593 RID: 17811
	public Image image;

	// Token: 0x04004594 RID: 17812
	public LocText label;

	// Token: 0x04004595 RID: 17813
	public MultiToggle button;

	// Token: 0x04004596 RID: 17814
	public RectTransform selection;

	// Token: 0x04004597 RID: 17815
	public GameObject analysisSelection;

	// Token: 0x04004598 RID: 17816
	public Image unknownBG;

	// Token: 0x04004599 RID: 17817
	public GameObject rocketIconContainer;
}
