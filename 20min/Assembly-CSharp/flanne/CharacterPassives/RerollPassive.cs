using System;
using UnityEngine;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025E RID: 606
	public class RerollPassive : MonoBehaviour
	{
		// Token: 0x06000D22 RID: 3362 RVA: 0x0002FFA4 File Offset: 0x0002E1A4
		private void Start()
		{
			PowerupGenerator.CanReroll = true;
		}
	}
}
