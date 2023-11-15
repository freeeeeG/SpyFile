using System;
using Scenes;
using UI;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006A3 RID: 1699
	public class BossHealthbarAttacher : MonoBehaviour
	{
		// Token: 0x060021EB RID: 8683 RVA: 0x00065D38 File Offset: 0x00063F38
		private void Start()
		{
			if (this._character != null)
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(this._type, this._character);
			}
		}

		// Token: 0x04001CE3 RID: 7395
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001CE4 RID: 7396
		[SerializeField]
		private BossHealthbarController.Type _type;
	}
}
