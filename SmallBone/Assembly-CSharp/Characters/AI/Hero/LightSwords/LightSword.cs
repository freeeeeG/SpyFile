using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x0200127C RID: 4732
	public class LightSword : MonoBehaviour
	{
		// Token: 0x06005DCD RID: 24013 RVA: 0x00114019 File Offset: 0x00112219
		private void Awake()
		{
			this._onDraw.Initialize();
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x00114026 File Offset: 0x00112226
		private void OnDestroy()
		{
			this._stuck = null;
			this._projectile = null;
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x00114036 File Offset: 0x00112236
		public void Initialzie(Character owner)
		{
			this._owner = owner;
			if (this._owner == null)
			{
				return;
			}
			this._owner.health.onDiedTryCatch += this.Hide;
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06005DD0 RID: 24016 RVA: 0x0011406A File Offset: 0x0011226A
		// (set) Token: 0x06005DD1 RID: 24017 RVA: 0x00114072 File Offset: 0x00112272
		public bool active { get; private set; }

		// Token: 0x06005DD2 RID: 24018 RVA: 0x0011407B File Offset: 0x0011227B
		public void Draw(Character owner)
		{
			this._onDraw.gameObject.SetActive(true);
			this._onDraw.Run(owner);
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x0011409C File Offset: 0x0011229C
		public void Fire(Character owner, Vector2 source, Vector2 destination)
		{
			float degree = this.AngleBetween(source, destination);
			this._moveCoroutine = this.StartCoroutineWithReference(this.CMove(source, destination, degree));
			this.active = true;
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x001140D0 File Offset: 0x001122D0
		private float AngleBetween(Vector2 from, Vector2 to)
		{
			Vector2 vector = to - from;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (num >= 0f)
			{
				return num;
			}
			return num + 360f;
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x0011410E File Offset: 0x0011230E
		public Vector3 GetStuckPosition()
		{
			return this._stuck.transform.position;
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x00114120 File Offset: 0x00112320
		public void Sign()
		{
			this._stuck.Sign();
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x0011412D File Offset: 0x0011232D
		public void Despawn()
		{
			this.active = false;
			this._stuck.Despawn();
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x000075E7 File Offset: 0x000057E7
		private void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x00114141 File Offset: 0x00112341
		private IEnumerator CMove(Vector2 firePosition, Vector2 destination, float degree)
		{
			yield return this._projectile.CFire(firePosition, destination, degree);
			this._stuck.OnStuck(this._owner, destination, degree);
			yield break;
		}

		// Token: 0x04004B61 RID: 19297
		[SerializeField]
		private LightSwordStuck _stuck;

		// Token: 0x04004B62 RID: 19298
		[SerializeField]
		private LightSwordProjectile _projectile;

		// Token: 0x04004B63 RID: 19299
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onDraw;

		// Token: 0x04004B64 RID: 19300
		private Character _owner;

		// Token: 0x04004B65 RID: 19301
		private CoroutineReference _moveCoroutine;
	}
}
