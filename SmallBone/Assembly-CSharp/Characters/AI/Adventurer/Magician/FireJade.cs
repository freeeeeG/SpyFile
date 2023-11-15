using System;
using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013EE RID: 5102
	public class FireJade : MonoBehaviour
	{
		// Token: 0x06006472 RID: 25714 RVA: 0x00123831 File Offset: 0x00121A31
		private void OnEnable()
		{
			this._spawnReadyEffect.Initialize();
			this._spawnFireEffect.Initialize();
			this._fireProjectile.Initialize();
			base.StartCoroutine(this.CAttack());
			base.StartCoroutine(this.CHide());
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x0012386E File Offset: 0x00121A6E
		private IEnumerator CAttack()
		{
			this._spawnReadyEffect.Run(this._character);
			yield return Chronometer.global.WaitForSeconds(this._startTiming * this._interval);
			for (;;)
			{
				this._takeAim.Run(this._character);
				this._fireProjectile.Run(this._character);
				this._spawnFireEffect.Run(this._character);
				this._spawnSound.Run(this._character);
				yield return Chronometer.global.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x0012387D File Offset: 0x00121A7D
		private IEnumerator CHide()
		{
			yield return Chronometer.global.WaitForSeconds(this._lifeTime);
			this._spawnReadyEffect.Run(this._character);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04005106 RID: 20742
		[SerializeField]
		private Character _character;

		// Token: 0x04005107 RID: 20743
		[Range(0f, 1f)]
		[SerializeField]
		private float _startTiming;

		// Token: 0x04005108 RID: 20744
		[SerializeField]
		private float _interval = 0.5f;

		// Token: 0x04005109 RID: 20745
		[SerializeField]
		private float _lifeTime = 1.5f;

		// Token: 0x0400510A RID: 20746
		[SerializeField]
		private TakeAim _takeAim;

		// Token: 0x0400510B RID: 20747
		[SerializeField]
		private FireProjectile _fireProjectile;

		// Token: 0x0400510C RID: 20748
		[Subcomponent(typeof(SpawnEffect))]
		[SerializeField]
		private SpawnEffect _spawnReadyEffect;

		// Token: 0x0400510D RID: 20749
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawnFireEffect;

		// Token: 0x0400510E RID: 20750
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _spawnSound;
	}
}
