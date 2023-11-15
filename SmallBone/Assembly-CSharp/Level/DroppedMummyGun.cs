using System;
using System.Collections;
using Characters;
using Characters.Abilities.Customs;
using FX;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004C1 RID: 1217
	public class DroppedMummyGun : MonoBehaviour
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x0600178F RID: 6031 RVA: 0x0004A000 File Offset: 0x00048200
		// (remove) Token: 0x06001790 RID: 6032 RVA: 0x0004A038 File Offset: 0x00048238
		public event Action onPickedUp;

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0004A06D File Offset: 0x0004826D
		public Rigidbody2D rigidbody
		{
			get
			{
				return this._rigidbody;
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0004A075 File Offset: 0x00048275
		private void Awake()
		{
			this._pickupTrigger.enabled = false;
			if (this._pickupTrigger.GetComponent<DroppedMummyGun.PickupProxy>() == null)
			{
				this._pickupTrigger.gameObject.AddComponent<DroppedMummyGun.PickupProxy>().droppedMummyGun = this;
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0004A0AC File Offset: 0x000482AC
		private void OnEnable()
		{
			this._pickupTrigger.enabled = false;
			this._rigidbody.gravityScale = 3f;
			this._rigidbody.velocity = new Vector2(0f, this._startYVelocity);
			base.StartCoroutine(this.CUpdatePickupable());
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0004A0FD File Offset: 0x000482FD
		private IEnumerator CUpdatePickupable()
		{
			this._pickupTrigger.enabled = false;
			yield return Chronometer.global.WaitForSeconds(this._pickupDelay);
			this._pickupTrigger.enabled = true;
			yield break;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0004A10C File Offset: 0x0004830C
		public DroppedMummyGun Spawn(Vector3 position, MummyPassive mummyPassive)
		{
			DroppedMummyGun component = this._poolObject.Spawn(position, true).GetComponent<DroppedMummyGun>();
			component._mummyPassive = mummyPassive;
			return component;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0004A128 File Offset: 0x00048328
		public void PickedUpBy(Character character)
		{
			if (this._mummyPassive != null)
			{
				this._mummyPassive.PickUpWeapon(this._key);
			}
			this._effect.Spawn(base.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._poolObject.Despawn();
			Action action = this.onPickedUp;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0004A1A6 File Offset: 0x000483A6
		public void Despawn()
		{
			this._poolObject.Despawn();
		}

		// Token: 0x04001499 RID: 5273
		[SerializeField]
		private string _key;

		// Token: 0x0400149A RID: 5274
		[Space]
		[SerializeField]
		private Collider2D _pickupTrigger;

		// Token: 0x0400149B RID: 5275
		[SerializeField]
		private float _pickupDelay;

		// Token: 0x0400149C RID: 5276
		[SerializeField]
		private float _startYVelocity;

		// Token: 0x0400149D RID: 5277
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x0400149E RID: 5278
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x0400149F RID: 5279
		[Space]
		[SerializeField]
		[GetComponent]
		private PoolObject _poolObject;

		// Token: 0x040014A0 RID: 5280
		[SerializeField]
		[GetComponent]
		private Rigidbody2D _rigidbody;

		// Token: 0x040014A1 RID: 5281
		private MummyPassive _mummyPassive;

		// Token: 0x020004C2 RID: 1218
		private class PickupProxy : MonoBehaviour, IPickupable
		{
			// Token: 0x06001799 RID: 6041 RVA: 0x0004A1B3 File Offset: 0x000483B3
			public void PickedUpBy(Character character)
			{
				this.droppedMummyGun.PickedUpBy(character);
			}

			// Token: 0x040014A2 RID: 5282
			public DroppedMummyGun droppedMummyGun;
		}
	}
}
