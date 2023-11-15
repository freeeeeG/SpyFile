using System;
using System.Collections;
using Data;
using FX;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A6 RID: 1190
	[RequireComponent(typeof(PoolObject), typeof(Rigidbody2D))]
	public class CurrencyParticle : MonoBehaviour
	{
		// Token: 0x060016DA RID: 5850 RVA: 0x00048048 File Offset: 0x00046248
		private void OnEnable()
		{
			this._collider.isTrigger = false;
			this._rigidbody.gravityScale = 3f;
			this._rigidbody.velocity = MMMaths.RandomVector2(CurrencyParticle._minVelocity, CurrencyParticle._maxVelocity);
			this._rigidbody.AddTorque(UnityEngine.Random.Range(-10f, 10f));
			base.StartCoroutine(this.CUpdate());
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x000480B2 File Offset: 0x000462B2
		private void OnDisable()
		{
			GameData.Currency.currencies[this.currencyType].Earn(this.currencyAmount);
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x000480D0 File Offset: 0x000462D0
		public void SetForce(Vector2 force)
		{
			if (Mathf.Abs(force.x) < 1f || Mathf.Abs(force.y) < 1f)
			{
				return;
			}
			force.y = Mathf.Abs(force.y);
			force *= 4f;
			force = Quaternion.AngleAxis(UnityEngine.Random.Range(-15f, 15f), Vector3.forward) * force * UnityEngine.Random.Range(0.8f, 1.2f);
			force.y += 10f;
			this._rigidbody.velocity = Vector2.zero;
			this._rigidbody.angularVelocity = 0f;
			this._rigidbody.AddForce(force * UnityEngine.Random.Range(0.5f, 1f), ForceMode2D.Impulse);
			this._rigidbody.AddTorque(UnityEngine.Random.Range(-0.5f, 0.5f), ForceMode2D.Impulse);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x000481CC File Offset: 0x000463CC
		private IEnumerator CUpdate()
		{
			yield return Chronometer.global.WaitForSeconds(UnityEngine.Random.Range(0.9f, 1.1f));
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._effect.Spawn(base.transform.position, 0f, 1f);
			this._poolObject.Despawn();
			yield break;
		}

		// Token: 0x04001408 RID: 5128
		private static readonly Vector2 _minVelocity = new Vector2(-4f, 7f);

		// Token: 0x04001409 RID: 5129
		private static readonly Vector2 _maxVelocity = new Vector2(4f, 17f);

		// Token: 0x0400140A RID: 5130
		private const float _minTorque = -10f;

		// Token: 0x0400140B RID: 5131
		private const float _maxTorque = 10f;

		// Token: 0x0400140C RID: 5132
		[SerializeField]
		[Header("Required")]
		[GetComponent]
		private PoolObject _poolObject;

		// Token: 0x0400140D RID: 5133
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x0400140E RID: 5134
		[SerializeField]
		[GetComponent]
		private Rigidbody2D _rigidbody;

		// Token: 0x0400140F RID: 5135
		[Header("FX")]
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x04001410 RID: 5136
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x04001411 RID: 5137
		[NonSerialized]
		public GameData.Currency.Type currencyType;

		// Token: 0x04001412 RID: 5138
		[NonSerialized]
		public int currencyAmount;
	}
}
