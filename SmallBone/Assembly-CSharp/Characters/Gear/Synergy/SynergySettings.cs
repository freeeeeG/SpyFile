using System;
using UnityEngine;

namespace Characters.Gear.Synergy
{
	// Token: 0x02000863 RID: 2147
	[CreateAssetMenu]
	public class SynergySettings : ScriptableObject
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06002CCB RID: 11467 RVA: 0x00088ABF File Offset: 0x00086CBF
		public InscriptionSettingsByKey settings
		{
			get
			{
				return this._settings;
			}
		}

		// Token: 0x040025A9 RID: 9641
		[SerializeField]
		private InscriptionSettingsByKey _settings;
	}
}
