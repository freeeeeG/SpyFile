using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x0200060F RID: 1551
	public class EnergyAltar : MonoBehaviour
	{
		// Token: 0x06001F0B RID: 7947 RVA: 0x0005E18C File Offset: 0x0005C38C
		private void Awake()
		{
			base.StartCoroutine(this.CHeal());
			this._altar.onDestroyed += this.OnAltarDestroyed;
			this._floatingText = Localization.GetLocalizedString("floating/altar/energy");
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		private void OnAltarDestroyed()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.ability.Add(this._healComponent.ability);
			Bounds bounds = player.collider.bounds;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(this._floatingText, new Vector2(bounds.center.x, bounds.max.y + 1f), "#F2F2F2");
			base.StopCoroutine("CHeal");
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0005E24F File Offset: 0x0005C44F
		private IEnumerator CHeal()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(3f);
				using (List<Character>.Enumerator enumerator = this._altar.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Character character = enumerator.Current;
						if (!character.health.dead)
						{
							character.health.Heal(Math.Max(5.0, character.health.maximumHealth * 0.0666), true);
						}
					}
					continue;
				}
				yield break;
			}
		}

		// Token: 0x04001A38 RID: 6712
		[SerializeField]
		[GetComponent]
		private Altar _altar;

		// Token: 0x04001A39 RID: 6713
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x04001A3A RID: 6714
		[SerializeField]
		private HealComponent _healComponent;

		// Token: 0x04001A3B RID: 6715
		private string _floatingText;
	}
}
