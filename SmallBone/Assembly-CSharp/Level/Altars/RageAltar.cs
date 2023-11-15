using System;
using Characters;
using Characters.Abilities.CharacterStat;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x02000611 RID: 1553
	public class RageAltar : MonoBehaviour
	{
		// Token: 0x06001F15 RID: 7957 RVA: 0x0005E358 File Offset: 0x0005C558
		private void Awake()
		{
			this._altar.onDestroyed += this.OnAltarDestroyed;
			this._floatingText = Localization.GetLocalizedString("floating/altar/rage");
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0005E384 File Offset: 0x0005C584
		private void OnAltarDestroyed()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.ability.Add(this._statBonus.ability);
			Bounds bounds = player.collider.bounds;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(this._floatingText, new Vector2(bounds.center.x, bounds.max.y + 1f), "#F2F2F2");
		}

		// Token: 0x04001A3F RID: 6719
		[SerializeField]
		[GetComponent]
		private Altar _altar;

		// Token: 0x04001A40 RID: 6720
		[SerializeField]
		private StatBonusComponent _statBonus;

		// Token: 0x04001A41 RID: 6721
		private string _floatingText;
	}
}
