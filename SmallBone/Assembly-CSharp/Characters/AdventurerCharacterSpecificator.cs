using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200069F RID: 1695
	public class AdventurerCharacterSpecificator : MonoBehaviour
	{
		// Token: 0x060021D9 RID: 8665 RVA: 0x00065A18 File Offset: 0x00063C18
		private void Awake()
		{
			this._statValue = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, (double)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.adventurerAttackDamageMultiplier),
				new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, (double)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.adventurerHealthMultiplier)
			});
			this._character.stat.AttachValues(this._statValue);
		}

		// Token: 0x04001CD3 RID: 7379
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001CD4 RID: 7380
		private Stat.Values _statValue;
	}
}
