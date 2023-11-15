using System;
using System.Collections;
using Characters;
using Characters.Abilities.Customs;
using Characters.Abilities.Weapons.Ghoul;
using FX;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004BA RID: 1210
	public class DroppedGhoulFlesh : MonoBehaviour
	{
		// Token: 0x0600176C RID: 5996 RVA: 0x00049AE7 File Offset: 0x00047CE7
		private void Awake()
		{
			this._pickupTrigger.enabled = false;
			this._pickupTrigger.gameObject.AddComponent<DroppedGhoulFlesh.PickupProxy>().ghoulFlesh = this;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00049B0C File Offset: 0x00047D0C
		private void OnEnable()
		{
			this._pickupTrigger.enabled = false;
			this._rigidbody.gravityScale = 3f;
			this._rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(5f, 15f));
			this._rigidbody.AddTorque((float)(UnityEngine.Random.Range(0, 15) * (MMMaths.RandomBool() ? 1 : -1)));
			base.StartCoroutine(this.CUpdatePickupable());
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00049B90 File Offset: 0x00047D90
		private IEnumerator CUpdatePickupable()
		{
			this._pickupTrigger.enabled = false;
			yield return Chronometer.global.WaitForSeconds(0.5f);
			this._pickupTrigger.enabled = true;
			yield break;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00049B9F File Offset: 0x00047D9F
		public void Spawn(Vector3 postion, GhoulPassive ghoulPassive)
		{
			this._poolObject.Spawn(postion, true).GetComponent<DroppedGhoulFlesh>()._ghoulPassive = ghoulPassive;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00049BB9 File Offset: 0x00047DB9
		public void Spawn(Vector3 postion, GhoulPassive2 ghoulHealthPassive)
		{
			this._poolObject.Spawn(postion, true).GetComponent<DroppedGhoulFlesh>()._ghoulHealthPassive = ghoulHealthPassive;
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00049BD4 File Offset: 0x00047DD4
		public void PickedUpBy(Character character)
		{
			this._ghoulHealthPassive.AddStack();
			this._effect.Spawn(base.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._poolObject.Despawn();
		}

		// Token: 0x0400147B RID: 5243
		[SerializeField]
		private Collider2D _pickupTrigger;

		// Token: 0x0400147C RID: 5244
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x0400147D RID: 5245
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x0400147E RID: 5246
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x0400147F RID: 5247
		[SerializeField]
		[GetComponent]
		private Rigidbody2D _rigidbody;

		// Token: 0x04001480 RID: 5248
		private GhoulPassive _ghoulPassive;

		// Token: 0x04001481 RID: 5249
		private GhoulPassive2 _ghoulHealthPassive;

		// Token: 0x020004BB RID: 1211
		private class PickupProxy : MonoBehaviour, IPickupable
		{
			// Token: 0x06001773 RID: 6003 RVA: 0x00049C34 File Offset: 0x00047E34
			public void PickedUpBy(Character character)
			{
				this.ghoulFlesh.PickedUpBy(character);
			}

			// Token: 0x04001482 RID: 5250
			public DroppedGhoulFlesh ghoulFlesh;
		}
	}
}
