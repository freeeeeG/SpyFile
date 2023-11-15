using System;
using Characters.Gear;
using Characters.Gear.Upgrades;
using Data;
using Hardmode.Darktech;
using Platforms;
using UnityEngine;

namespace Hardmode
{
	// Token: 0x02000151 RID: 337
	public sealed class UnlockUpgrade : MonoBehaviour
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0001352C File Offset: 0x0001172C
		private void OnEnable()
		{
			string typeName = Gear.Type.Upgrade.ToString();
			if (GameData.HardmodeProgress.unlocked.GetData(this._type))
			{
				foreach (UpgradeObject upgradeObject in this._upgradeObjects)
				{
					GameData.Gear.SetUnlocked(typeName, upgradeObject.name, true);
				}
				if (this._unlockAcheivementOnActivate)
				{
					Achievement.Type.DarkAbility.Set();
				}
			}
		}

		// Token: 0x040004E7 RID: 1255
		[SerializeField]
		private DarktechData.Type _type;

		// Token: 0x040004E8 RID: 1256
		[SerializeField]
		private UpgradeObject[] _upgradeObjects;

		// Token: 0x040004E9 RID: 1257
		[SerializeField]
		private bool _unlockAcheivementOnActivate;
	}
}
