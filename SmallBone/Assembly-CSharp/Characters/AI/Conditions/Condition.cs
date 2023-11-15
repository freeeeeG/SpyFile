using System;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C7 RID: 4551
	public abstract class Condition : MonoBehaviour
	{
		// Token: 0x0600597F RID: 22911
		protected abstract bool Check(AIController controller);

		// Token: 0x06005980 RID: 22912 RVA: 0x0010A611 File Offset: 0x00108811
		public bool IsSatisfied(AIController controller)
		{
			return this._inverter ^ this.Check(controller);
		}

		// Token: 0x04004848 RID: 18504
		[SerializeField]
		private bool _inverter;

		// Token: 0x020011C8 RID: 4552
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005982 RID: 22914 RVA: 0x0010A621 File Offset: 0x00108821
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Condition.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004849 RID: 18505
			public new static readonly Type[] types = new Type[]
			{
				typeof(BehaviourCoolTime),
				typeof(BehaviourResult),
				typeof(BetweenTargetAndWall),
				typeof(CanStartAction),
				typeof(CompareDistanceFromWall),
				typeof(CoolDown),
				typeof(CheckCollision),
				typeof(EnterTrigger),
				typeof(HealthCondition),
				typeof(MonsterCount),
				typeof(TargetIsGrounded)
			};
		}
	}
}
