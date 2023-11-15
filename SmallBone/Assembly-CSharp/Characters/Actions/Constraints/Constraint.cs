using System;
using Characters.Actions.Constraints.Customs;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000971 RID: 2417
	public abstract class Constraint : MonoBehaviour
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x0009A2E7 File Offset: 0x000984E7
		protected virtual void OnDestroy()
		{
			this._action = null;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x0009A2F0 File Offset: 0x000984F0
		public virtual void Initilaize(Action action)
		{
			this._action = action;
		}

		// Token: 0x06003412 RID: 13330
		public abstract bool Pass();

		// Token: 0x06003413 RID: 13331 RVA: 0x00002191 File Offset: 0x00000391
		public virtual void Consume()
		{
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002A26 RID: 10790
		public static readonly Type[] types = new Type[]
		{
			typeof(ActionConstraint),
			typeof(IdleConstraint),
			typeof(AirAndGroundConstraint),
			typeof(DirectionConstraint),
			typeof(GoldConstraint),
			typeof(ReferenceConstraint),
			typeof(TimingConstraint),
			typeof(MotionConstraint),
			typeof(NeedAirJumpCountConstraint),
			typeof(CooldownConstraint),
			typeof(LimitedTimesOnAirConstraint),
			typeof(EnemyWithinRangeConstraint),
			typeof(EnemyStatusWithinRangeConstraint),
			typeof(GaugeConstraint),
			typeof(HealthConstraint),
			typeof(FighterRageReadyConstraint),
			typeof(GraveDiggerGraveConstraint),
			typeof(CanSummonGrassConstraint),
			typeof(MaxGaugeConstraint),
			typeof(DavyJonesCannonBallConstraint),
			typeof(DavyJonesEmptyMagazineConstraint)
		};

		// Token: 0x04002A27 RID: 10791
		protected Action _action;

		// Token: 0x02000972 RID: 2418
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06003417 RID: 13335 RVA: 0x0009A432 File Offset: 0x00098632
			public SubcomponentAttribute() : base(true, Constraint.types)
			{
			}
		}

		// Token: 0x02000973 RID: 2419
		[Serializable]
		public class Subcomponents : SubcomponentArray<Constraint>
		{
		}
	}
}
