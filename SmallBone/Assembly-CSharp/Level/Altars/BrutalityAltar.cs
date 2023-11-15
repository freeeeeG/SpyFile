using System;
using Characters;
using Characters.Abilities.CharacterStat;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x0200060E RID: 1550
	public class BrutalityAltar : MonoBehaviour
	{
		// Token: 0x06001F08 RID: 7944 RVA: 0x0005E0E3 File Offset: 0x0005C2E3
		private void Awake()
		{
			this._altar.onDestroyed += this.OnAltarDestroyed;
			this._floatingText = Localization.GetLocalizedString("floating/altar/brutality");
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0005E10C File Offset: 0x0005C30C
		private void OnAltarDestroyed()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Bounds bounds = player.collider.bounds;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(this._floatingText, new Vector2(bounds.center.x, bounds.max.y + 1f), "#F2F2F2");
			player.ability.Add(this._statBonus.ability);
		}

		// Token: 0x04001A35 RID: 6709
		[GetComponent]
		[SerializeField]
		private Altar _altar;

		// Token: 0x04001A36 RID: 6710
		[SerializeField]
		private StatBonusComponent _statBonus;

		// Token: 0x04001A37 RID: 6711
		private string _floatingText;
	}
}
