using System;
using Characters;
using Characters.Abilities;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x02000612 RID: 1554
	public class SteelAltar : MonoBehaviour
	{
		// Token: 0x06001F18 RID: 7960 RVA: 0x0005E404 File Offset: 0x0005C604
		private void Awake()
		{
			this._altar.onDestroyed += this.OnAltarDestroyed;
			this._floatingText = Localization.GetLocalizedString("floating/altar/steel");
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0005E430 File Offset: 0x0005C630
		private void OnAltarDestroyed()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.ability.Add(this._shieldComponent.ability);
			Bounds bounds = player.collider.bounds;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(this._floatingText, new Vector2(bounds.center.x, bounds.max.y + 1f), "#F2F2F2");
		}

		// Token: 0x04001A42 RID: 6722
		[GetComponent]
		[SerializeField]
		private Altar _altar;

		// Token: 0x04001A43 RID: 6723
		[SerializeField]
		private ShieldComponent _shieldComponent;

		// Token: 0x04001A44 RID: 6724
		private string _floatingText;
	}
}
