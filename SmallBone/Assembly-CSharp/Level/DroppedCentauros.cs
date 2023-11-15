using System;
using System.Collections;
using Characters;
using Characters.Gear.Quintessences;
using Characters.Operations;
using Characters.Player;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x0200047A RID: 1146
	public class DroppedCentauros : MonoBehaviour
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x0004498C File Offset: 0x00042B8C
		public PoolObject Spawn(Vector2 position)
		{
			return this._poolObject.Spawn(position, true);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x000449A0 File Offset: 0x00042BA0
		private void Awake()
		{
			this._onLoot.Initialize();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000449B0 File Offset: 0x00042BB0
		private void OnEnable()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			base.StartCoroutine(this.CUpdate());
			QuintessenceInventory quintessence = this._player.playerComponents.inventory.quintessence;
			this._quintessence = quintessence.items[0];
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00044A07 File Offset: 0x00042C07
		private void OnTriggerEnter2D(Collider2D collision)
		{
			this._collisionStay = true;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00044A10 File Offset: 0x00042C10
		private void OnTriggerExit2D(Collider2D collision)
		{
			this._collisionStay = false;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00044A19 File Offset: 0x00042C19
		private IEnumerator CUpdate()
		{
			float time = 0f;
			yield return Chronometer.global.WaitForSeconds(0.5f);
			while (!(this._quintessence == null) && !this._quintessence.cooldown.time.canUse)
			{
				time += Chronometer.global.deltaTime;
				if (time >= 0.5f && this._collisionStay)
				{
					this._quintessence.cooldown.time.remainTime -= this._reduceAmount;
					Character player = Singleton<Service>.Instance.levelManager.player;
					this._onLoot.StopAll();
					CoroutineProxy.instance.StartCoroutine(this._onLoot.CRun(player));
					this._poolObject.Despawn();
				}
				yield return null;
			}
			this._poolObject.Despawn();
			yield break;
			yield break;
		}

		// Token: 0x0400131C RID: 4892
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x0400131D RID: 4893
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onLoot;

		// Token: 0x0400131E RID: 4894
		[SerializeField]
		private float _reduceAmount = 20f;

		// Token: 0x0400131F RID: 4895
		private Character _player;

		// Token: 0x04001320 RID: 4896
		private Quintessence _quintessence;

		// Token: 0x04001321 RID: 4897
		private bool _collisionStay;
	}
}
