using System;
using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200119D RID: 4509
	public class ThunderAttack : MonoBehaviour
	{
		// Token: 0x06005890 RID: 22672 RVA: 0x0010841C File Offset: 0x0010661C
		private void Initialize(Character character)
		{
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = bounds.min.x;
			float x2 = bounds.max.x;
			float x3 = this._attackRange.bounds.size.x;
			float x4 = this._attackRange.bounds.extents.x;
			for (float num = x + x4; num <= x2; num += x3 + this._distance)
			{
				this._count++;
			}
			this._sweepAttack.Initialize();
			this._initialized = true;
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001084C9 File Offset: 0x001066C9
		public IEnumerator CRun(AIController controller)
		{
			if (!this._initialized)
			{
				this.Initialize(controller.character);
			}
			Character character = controller.character;
			yield return Chronometer.global.WaitForSeconds(this._signDelay);
			Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			float startX = (character.lookingDirection == Character.LookingDirection.Left) ? platformBounds.max.x : platformBounds.min.x;
			float sizeX = this._attackRange.bounds.size.x;
			float extentsX = this._attackRange.bounds.extents.x;
			int sign = (character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			for (int j = 0; j < this._count; j++)
			{
				float x = startX + (sizeX * (float)j + extentsX) * (float)sign + (float)j * this._distance * (float)sign;
				this._attackRange.transform.position = new Vector3(x, platformBounds.max.y);
				this._spawnAttackSign.Run(character);
				this._playSignSound.Run(character);
			}
			yield return Chronometer.global.WaitForSeconds(1f);
			int num;
			for (int i = 0; i < this._count; i = num + 1)
			{
				float x2 = startX + (sizeX * (float)i + extentsX) * (float)sign + (float)i * this._distance * (float)sign;
				this._attackRange.transform.position = new Vector3(x2, platformBounds.max.y);
				Physics2D.SyncTransforms();
				base.StartCoroutine(this._operations.CRun(character));
				if (!Singleton<HardmodeManager>.Instance.hardmode)
				{
					yield return Chronometer.global.WaitForSeconds(this._term);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04004780 RID: 18304
		[SerializeField]
		private Collider2D _attackRange;

		// Token: 0x04004781 RID: 18305
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04004782 RID: 18306
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawnAttackSign;

		// Token: 0x04004783 RID: 18307
		[SerializeField]
		[Subcomponent(typeof(SweepAttack2))]
		private SweepAttack2 _sweepAttack;

		// Token: 0x04004784 RID: 18308
		[Subcomponent(typeof(SpawnEffect))]
		[SerializeField]
		private SpawnEffect _sweepAttackEffect;

		// Token: 0x04004785 RID: 18309
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _playSignSound;

		// Token: 0x04004786 RID: 18310
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _playAttackSound;

		// Token: 0x04004787 RID: 18311
		[SerializeField]
		private float _signDelay;

		// Token: 0x04004788 RID: 18312
		[SerializeField]
		private float _term = 0.15f;

		// Token: 0x04004789 RID: 18313
		[SerializeField]
		private float _distance;

		// Token: 0x0400478A RID: 18314
		private int _count;

		// Token: 0x0400478B RID: 18315
		private bool _initialized;
	}
}
