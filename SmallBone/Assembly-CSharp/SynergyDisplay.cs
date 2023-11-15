using System;
using System.Collections;
using System.Text;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class SynergyDisplay : MonoBehaviour
{
	// Token: 0x0600034B RID: 843 RVA: 0x0000C70E File Offset: 0x0000A90E
	private IEnumerator Start()
	{
		while (Singleton<Service>.Instance.levelManager.player == null)
		{
			yield return null;
		}
		this._inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
		this._inventory.onUpdated += this.UpdateText;
		this.UpdateText();
		yield break;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0000C720 File Offset: 0x0000A920
	private void UpdateText()
	{
		EnumArray<Inscription.Key, Inscription> inscriptions = this._inventory.synergy.inscriptions;
		this._stringBuilder.Clear();
		foreach (Inscription inscription in inscriptions)
		{
			if (inscription.count != 0)
			{
				this._stringBuilder.AppendFormat("{0} ({1})\n", inscription.name, inscription.count);
				if (inscription.step != 0)
				{
					this._stringBuilder.AppendLine();
				}
			}
		}
		this._text.text = this._stringBuilder.ToString();
	}

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private TMP_Text _text;

	// Token: 0x040002B1 RID: 689
	private Inventory _inventory;

	// Token: 0x040002B2 RID: 690
	private readonly StringBuilder _stringBuilder = new StringBuilder();
}
