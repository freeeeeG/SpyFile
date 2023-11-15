using System;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using UnityEngine;

namespace Characters.Gear.Synergy
{
	// Token: 0x02000862 RID: 2146
	public class Synergy : MonoBehaviour
	{
		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06002CC5 RID: 11461 RVA: 0x00088928 File Offset: 0x00086B28
		// (remove) Token: 0x06002CC6 RID: 11462 RVA: 0x00088960 File Offset: 0x00086B60
		public event Action onChanged;

		// Token: 0x06002CC7 RID: 11463 RVA: 0x00088998 File Offset: 0x00086B98
		public void Initialize(Character character)
		{
			for (int i = 0; i < this.inscriptions.Count; i++)
			{
				this.inscriptions.Array[i] = new Inscription();
				ref InscriptionSettings ptr = ref this._synergySettings.settings.Array[i];
				this.inscriptions.Array[i].Initialize(this.inscriptions.Keys[i], ptr, character);
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x00088A0C File Offset: 0x00086C0C
		public void UpdateBonus()
		{
			for (int i = 0; i < this.inscriptions.Count; i++)
			{
				this.inscriptions.Array[i].Update();
			}
			Action action = this.onChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x00088A54 File Offset: 0x00086C54
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			foreach (Inscription inscription in this.inscriptions)
			{
				if (inscription != null)
				{
					inscription.Clear();
				}
			}
		}

		// Token: 0x040025A5 RID: 9637
		public readonly EnumArray<Inscription.Key, Inscription> inscriptions = new EnumArray<Inscription.Key, Inscription>();

		// Token: 0x040025A6 RID: 9638
		[SerializeField]
		private SynergySettings _synergySettings;

		// Token: 0x040025A7 RID: 9639
		[SerializeField]
		private GameObject _container;
	}
}
