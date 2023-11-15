using System;
using Data;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001120 RID: 4384
	[RequireComponent(typeof(Character))]
	public sealed class EnemyHardmodeStat : MonoBehaviour
	{
		// Token: 0x06005544 RID: 21828 RVA: 0x000FE92F File Offset: 0x000FCB2F
		private void Start()
		{
			if (!GameData.HardmodeProgress.hardmode)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			new EnemyBonusStatInHardmode().AttachTo(this._character);
		}

		// Token: 0x04004453 RID: 17491
		[SerializeField]
		[GetComponent]
		private Character _character;
	}
}
