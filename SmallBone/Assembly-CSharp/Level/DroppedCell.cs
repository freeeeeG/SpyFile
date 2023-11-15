using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons.Gauges;
using FX;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004B2 RID: 1202
	public class DroppedCell : MonoBehaviour
	{
		// Token: 0x06001734 RID: 5940 RVA: 0x00049019 File Offset: 0x00047219
		private void Awake()
		{
			this._pickupTrigger.enabled = false;
			this._pickupTrigger.gameObject.AddComponent<DroppedCell.PickupProxy>().cell = this;
			Renderer spriteRenderer = this._spriteRenderer;
			short sortingOrder = DroppedCell._sortingOrder;
			DroppedCell._sortingOrder = sortingOrder + 1;
			spriteRenderer.sortingOrder = (int)sortingOrder;
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00049058 File Offset: 0x00047258
		private void OnEnable()
		{
			this._pickupTrigger.enabled = false;
			this._rigidbody.gravityScale = 3f;
			this._rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(5f, 15f));
			this._rigidbody.AddTorque((float)(UnityEngine.Random.Range(0, 15) * (MMMaths.RandomBool() ? 1 : -1)));
			base.StartCoroutine(this.CUpdatePickupable());
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000490DC File Offset: 0x000472DC
		private IEnumerator CUpdatePickupable()
		{
			this._pickupTrigger.enabled = false;
			yield return Chronometer.global.WaitForSeconds(0.5f);
			this._pickupTrigger.enabled = true;
			yield break;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000490EC File Offset: 0x000472EC
		public void PickedUpBy(Character character)
		{
			this._prisonerGauge.Add(1f);
			this._effect.Spawn(base.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._poolObject.Despawn();
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00049151 File Offset: 0x00047351
		public void Spawn(Vector3 postion, ValueGauge gauge)
		{
			this._poolObject.Spawn(postion, true).GetComponent<DroppedCell>()._prisonerGauge = gauge;
		}

		// Token: 0x04001452 RID: 5202
		private static short _sortingOrder = short.MinValue;

		// Token: 0x04001453 RID: 5203
		[SerializeField]
		private Collider2D _pickupTrigger;

		// Token: 0x04001454 RID: 5204
		[Space]
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x04001455 RID: 5205
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x04001456 RID: 5206
		[GetComponent]
		[Space]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x04001457 RID: 5207
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001458 RID: 5208
		[SerializeField]
		[GetComponent]
		private Rigidbody2D _rigidbody;

		// Token: 0x04001459 RID: 5209
		private ValueGauge _prisonerGauge;

		// Token: 0x020004B3 RID: 1203
		private class PickupProxy : MonoBehaviour, IPickupable
		{
			// Token: 0x0600173B RID: 5947 RVA: 0x00049177 File Offset: 0x00047377
			public void PickedUpBy(Character character)
			{
				this.cell.PickedUpBy(character);
			}

			// Token: 0x0400145A RID: 5210
			public DroppedCell cell;
		}
	}
}
