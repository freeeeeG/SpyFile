using System;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E6A RID: 3690
	public class Smash : TargetedCharacterOperation
	{
		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x000D586C File Offset: 0x000D3A6C
		public PushInfo pushInfo
		{
			get
			{
				return this._pushInfo;
			}
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x000D5874 File Offset: 0x000D3A74
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onCollide.Initialize();
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x000D5890 File Offset: 0x000D3A90
		private void OnEnd(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction)
		{
			if (endType != Push.SmashEndType.Collide)
			{
				return;
			}
			base.StartCoroutine(this._onCollide.CRun(from, to));
			Damage damage = from.stat.GetDamage((double)this._attackDamage.amount, raycastHit.Value.point, this._hitInfo);
			TargetStruct targetStruct = new TargetStruct(to);
			from.TryAttackCharacter(targetStruct, ref damage);
			this._effect.Spawn(to, push, raycastHit.Value, direction, damage, targetStruct);
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x000D5918 File Offset: 0x000D3B18
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			if (this._transfromOverride == null)
			{
				target.movement.push.ApplySmash(owner, this._pushInfo, new Push.OnSmashEndDelegate(this.OnEnd));
				return;
			}
			target.movement.push.ApplySmash(owner, this._transfromOverride, this._pushInfo, new Push.OnSmashEndDelegate(this.OnEnd));
		}

		// Token: 0x0400385B RID: 14427
		[Information("이 값을 지정해주면 오퍼레이션 소유 캐릭터 대신 해당 트랜스폼을 기준으로 넉백합니다.", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Transform _transfromOverride;

		// Token: 0x0400385C RID: 14428
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(true, false);

		// Token: 0x0400385D RID: 14429
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x0400385E RID: 14430
		[SerializeField]
		[SmashAttackVisualEffect.SubcomponentAttribute]
		private SmashAttackVisualEffect.Subcomponents _effect;

		// Token: 0x0400385F RID: 14431
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onCollide;

		// Token: 0x04003860 RID: 14432
		private IAttackDamage _attackDamage;
	}
}
