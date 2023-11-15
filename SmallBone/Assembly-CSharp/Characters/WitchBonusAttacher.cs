using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000741 RID: 1857
	internal class WitchBonusAttacher : MonoBehaviour
	{
		// Token: 0x060025D2 RID: 9682 RVA: 0x000721D5 File Offset: 0x000703D5
		private void Awake()
		{
			WitchBonus.instance.Apply(this._character);
		}

		// Token: 0x04001FFC RID: 8188
		[SerializeField]
		private Character _character;
	}
}
