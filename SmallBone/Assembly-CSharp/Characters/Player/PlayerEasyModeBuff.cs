using System;
using Characters.Abilities;
using Data;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007F9 RID: 2041
	public class PlayerEasyModeBuff : MonoBehaviour
	{
		// Token: 0x0600297C RID: 10620 RVA: 0x0007EA65 File Offset: 0x0007CC65
		private void Awake()
		{
			this._abilityAttacher.Initialize(this._character);
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x0007EA78 File Offset: 0x0007CC78
		private void Update()
		{
			if (GameData.Settings.easyMode && GameData.HardmodeProgress.hardmode)
			{
				GameData.Settings.easyMode = false;
			}
			if (GameData.Settings.easyMode)
			{
				if (!this._attached)
				{
					this._abilityAttacher.StartAttach();
					this._attached = true;
					return;
				}
			}
			else if (this._attached)
			{
				this._abilityAttacher.StopAttach();
				this._attached = false;
			}
		}

		// Token: 0x040023A7 RID: 9127
		[SerializeField]
		private Character _character;

		// Token: 0x040023A8 RID: 9128
		[SerializeField]
		private AbilityAttacher _abilityAttacher;

		// Token: 0x040023A9 RID: 9129
		private bool _attached;
	}
}
