using System;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F1 RID: 1777
	public class EffectOnEvade : MonoBehaviour
	{
		// Token: 0x060023E7 RID: 9191 RVA: 0x0006BDD7 File Offset: 0x00069FD7
		public void Awake()
		{
			this._character.onEvade += this.SpawnFloatingText;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0006BDF0 File Offset: 0x00069FF0
		private void SpawnFloatingText(ref Damage damage)
		{
			Vector2 v = MMMaths.RandomPointWithinBounds(this._character.collider.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnEvade(Localization.GetLocalizedString("floating/evade"), v, "#a3a3a3");
		}

		// Token: 0x04001E9B RID: 7835
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001E9C RID: 7836
		private const string _floatingTextKey = "floating/evade";

		// Token: 0x04001E9D RID: 7837
		private const string _floatingTextColor = "#a3a3a3";
	}
}
