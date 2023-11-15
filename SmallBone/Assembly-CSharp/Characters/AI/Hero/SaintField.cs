using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x0200126C RID: 4716
	public class SaintField : MonoBehaviour
	{
		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06005D80 RID: 23936 RVA: 0x00113225 File Offset: 0x00111425
		public bool isStuck
		{
			get
			{
				return this._sword.isStuck;
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x00113234 File Offset: 0x00111434
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._sword.OnStuck += this.ActivePillarOfLight;
			this._sword.OnStuck += delegate()
			{
				base.StartCoroutine(this.CExpire());
			};
			this._owner.health.onDiedTryCatch += this.DeactivePillarOfLight;
		}

		// Token: 0x06005D82 RID: 23938 RVA: 0x001132A0 File Offset: 0x001114A0
		public void DropGiganticSaintSword()
		{
			float y = this._player.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
			this._sword.Fire(this._fireTransform.position, y);
		}

		// Token: 0x06005D83 RID: 23939 RVA: 0x001132F8 File Offset: 0x001114F8
		private void ActivePillarOfLight()
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			this._checkPlayerOutReference = this.StartCoroutineWithReference(this.CCheckPlayerOut());
			this._leftPillarOfLight.SetActive(true);
			this._rightPillarOfLight.SetActive(true);
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x00113347 File Offset: 0x00111547
		public void DeactivePillarOfLight()
		{
			this._leftPillarOfLight.SetActive(false);
			this._rightPillarOfLight.SetActive(false);
			this._sword.Despawn();
			this._checkPlayerOutReference.Stop();
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x00113377 File Offset: 0x00111577
		private IEnumerator CCheckPlayerOut()
		{
			SaintField.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.player = Singleton<Service>.Instance.levelManager.player;
			while (!this._owner.health.dead)
			{
				if (!this.<CCheckPlayerOut>g__Contains|15_0(ref CS$<>8__locals1))
				{
					CS$<>8__locals1.player.transform.position = this._owner.transform.position;
					yield return Chronometer.global.WaitForSeconds(1f);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x00113386 File Offset: 0x00111586
		private IEnumerator CExpire()
		{
			float elapsed = 0f;
			while (elapsed < this._duration)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
				if (!this.ContainsPlayer())
				{
					break;
				}
			}
			this.DeactivePillarOfLight();
			yield break;
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x00113398 File Offset: 0x00111598
		private bool ContainsPlayer()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			return this._leftPillarOfLight.transform.position.x <= player.transform.position.x && this._rightPillarOfLight.transform.position.x >= player.transform.position.x;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x0011342A File Offset: 0x0011162A
		[CompilerGenerated]
		private bool <CCheckPlayerOut>g__Contains|15_0(ref SaintField.<>c__DisplayClass15_0 A_1)
		{
			return !(A_1.player.movement.controller.collisionState.lastStandingCollider != this._owner.movement.controller.collisionState.lastStandingCollider);
		}

		// Token: 0x04004B0C RID: 19212
		[SerializeField]
		private Character _owner;

		// Token: 0x04004B0D RID: 19213
		[SerializeField]
		private float _duration = 10f;

		// Token: 0x04004B0E RID: 19214
		[SerializeField]
		private GameObject _leftPillarOfLight;

		// Token: 0x04004B0F RID: 19215
		[SerializeField]
		private GameObject _rightPillarOfLight;

		// Token: 0x04004B10 RID: 19216
		[SerializeField]
		private Transform _fireTransform;

		// Token: 0x04004B11 RID: 19217
		[SerializeField]
		private GiganticSaintSword _sword;

		// Token: 0x04004B12 RID: 19218
		[SerializeField]
		private float _height;

		// Token: 0x04004B13 RID: 19219
		private Character _player;

		// Token: 0x04004B14 RID: 19220
		private CoroutineReference _checkPlayerOutReference;
	}
}
