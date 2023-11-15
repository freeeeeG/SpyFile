using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations.SetPosition;
using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DDF RID: 3551
	public class AbyssMeteor : CharacterOperation
	{
		// Token: 0x06004735 RID: 18229 RVA: 0x000CEC8E File Offset: 0x000CCE8E
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x000CEC9C File Offset: 0x000CCE9C
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CFire(owner));
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x000CECAC File Offset: 0x000CCEAC
		private IEnumerator CFire(Character owner)
		{
			int count = (int)((float)(this._projectileCountMax - this._projectileCountMin) * this._chargeAction.chargedPercent) + this._projectileCountMin;
			Character.LookingDirection lookingDirection = owner.lookingDirection;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				this.Fire(owner, lookingDirection);
				yield return owner.chronometer.animation.WaitForSeconds(this._fireInterval.value);
				num = i;
			}
			yield break;
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x000CECC4 File Offset: 0x000CCEC4
		private void Fire(Character owner, Character.LookingDirection lookingDirection)
		{
			Vector2 vector = Vector2.right * this._distance;
			float num = MMMaths.RandomBool() ? this._angle.value : (180f - this._angle.value);
			float f = num * 0.017453292f;
			float num2 = Mathf.Cos(f);
			float num3 = Mathf.Sin(f);
			Vector2 b = new Vector2(vector.x * num2 - vector.y * num3, vector.x * num3 + vector.y * num2);
			float num4 = MMMaths.RandomBool() ? UnityEngine.Random.Range(0f, this._horizontalNoise) : UnityEngine.Random.Range(-this._horizontalNoise, 0f);
			Vector2 position = this._policy.GetPosition(owner);
			position = new Vector2(position.x + num4, position.y);
			float num5 = num + 180f;
			Vector2 v = MMMaths.Vector3ToVector2(position) + b;
			if (this._spawnEffect != null)
			{
				this._spawnEffect.Spawn(v, owner, num5, 1f);
			}
			this._projectile.reusable.Spawn(v, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, num5, false, false, 1f, null, 0f);
		}

		// Token: 0x0400361D RID: 13853
		[SerializeField]
		private ChargeAction _chargeAction;

		// Token: 0x0400361E RID: 13854
		[SerializeField]
		[Space]
		[Tooltip("차지를 하나도 안 했을 때 프로젝타일 개수")]
		private int _projectileCountMin = 1;

		// Token: 0x0400361F RID: 13855
		[Tooltip("풀차지 했을 때 프로젝타일 개수")]
		[SerializeField]
		private int _projectileCountMax = 5;

		// Token: 0x04003620 RID: 13856
		[SerializeField]
		[Space]
		[Tooltip("프로젝타일 발사 간격")]
		private CustomFloat _fireInterval;

		// Token: 0x04003621 RID: 13857
		[SerializeField]
		[Space]
		private EffectInfo _spawnEffect;

		// Token: 0x04003622 RID: 13858
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04003623 RID: 13859
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003624 RID: 13860
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003625 RID: 13861
		private IAttackDamage _attackDamage;

		// Token: 0x04003626 RID: 13862
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003627 RID: 13863
		[SerializeField]
		private float _distance;

		// Token: 0x04003628 RID: 13864
		[SerializeField]
		private float _horizontalNoise;

		// Token: 0x04003629 RID: 13865
		[Policy.SubcomponentAttribute(true)]
		[SerializeField]
		private Policy _policy;

		// Token: 0x02000DE0 RID: 3552
		public enum DirectionType
		{
			// Token: 0x0400362B RID: 13867
			OwnerDirection,
			// Token: 0x0400362C RID: 13868
			Constant
		}
	}
}
