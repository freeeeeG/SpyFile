using System;
using System.Collections;
using Characters;
using Characters.Actions;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000683 RID: 1667
	public class ThornTrap2 : MonoBehaviour
	{
		// Token: 0x06002156 RID: 8534 RVA: 0x0006446E File Offset: 0x0006266E
		private void Awake()
		{
			this._attackAction.Initialize(this._character);
			this._attackWithReadyAction.Initialize(this._character);
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x0006449F File Offset: 0x0006269F
		private void OnDestroy()
		{
			this._idleClip2 = null;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000644A8 File Offset: 0x000626A8
		private IEnumerator CAttack()
		{
			do
			{
				yield return null;
				this.FindPlayer();
			}
			while (this._overlapper.results.Count == 0);
			this._animation.SetIdle(this._idleClip2);
			this._attackWithReadyAction.TryStart();
			yield return this._attackWithReadyAction.CWaitForEndOfRunning();
			yield return Chronometer.global.WaitForSeconds(this._interval);
			for (;;)
			{
				yield return null;
				this.FindPlayer();
				if (this._overlapper.results.Count != 0)
				{
					this._attackAction.TryStart();
					yield return this._attackAction.CWaitForEndOfRunning();
					yield return Chronometer.global.WaitForSeconds(this._interval);
				}
			}
			yield break;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000644B8 File Offset: 0x000626B8
		private void FindPlayer()
		{
			this._range.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(512);
			this._overlapper.OverlapCollider(this._range);
			this._range.enabled = false;
		}

		// Token: 0x04001C67 RID: 7271
		[SerializeField]
		private Character _character;

		// Token: 0x04001C68 RID: 7272
		[SerializeField]
		private CharacterAnimation _animation;

		// Token: 0x04001C69 RID: 7273
		[SerializeField]
		private AnimationClip _idleClip2;

		// Token: 0x04001C6A RID: 7274
		[SerializeField]
		private float _interval = 4f;

		// Token: 0x04001C6B RID: 7275
		[SerializeField]
		private Characters.Actions.Action _attackWithReadyAction;

		// Token: 0x04001C6C RID: 7276
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04001C6D RID: 7277
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04001C6E RID: 7278
		[SerializeField]
		private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);
	}
}
