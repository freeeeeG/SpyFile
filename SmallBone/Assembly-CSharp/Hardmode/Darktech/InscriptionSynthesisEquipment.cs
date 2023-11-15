using System;
using Characters.Gear.Synergy.Inscriptions;
using Data;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000169 RID: 361
	public sealed class InscriptionSynthesisEquipment : MonoBehaviour
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00014FBC File Offset: 0x000131BC
		private void Start()
		{
			int num = 0;
			InscriptionSynthesisEquipmentSlot[] slots = this._slots;
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].Initialize(this, num);
				if (num >= GameData.HardmodeProgress.InscriptionSynthesisEquipment.count)
				{
					break;
				}
				num++;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00014FF8 File Offset: 0x000131F8
		public bool IsSelectable(InscriptionSynthesisEquipmentSlot from, Inscription.Key key)
		{
			if (key == Inscription.Key.SunAndMoon || key == Inscription.Key.Masterpiece || key == Inscription.Key.Sin || key == Inscription.Key.Omen)
			{
				return false;
			}
			foreach (InscriptionSynthesisEquipmentSlot inscriptionSynthesisEquipmentSlot in this._slots)
			{
				if (!(inscriptionSynthesisEquipmentSlot == from) && inscriptionSynthesisEquipmentSlot.selected != null && inscriptionSynthesisEquipmentSlot.selected.Value == key)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000563 RID: 1379
		public static readonly int increasement = 1;

		// Token: 0x04000564 RID: 1380
		[SerializeField]
		private InscriptionSynthesisEquipmentSlot[] _slots;
	}
}
