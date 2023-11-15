using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
[CreateAssetMenu(fileName = "Keybingdings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
	// Token: 0x04000398 RID: 920
	public Keybindings.KeybingdingCheck[] keybindingChecks;

	// Token: 0x020002DE RID: 734
	[Serializable]
	public class KeybingdingCheck
	{
		// Token: 0x040009F9 RID: 2553
		public KeyBindingActions keybingdingAction;

		// Token: 0x040009FA RID: 2554
		public KeyCode keyCode;

		// Token: 0x040009FB RID: 2555
		public KeyCode defaultKeycode;
	}
}
