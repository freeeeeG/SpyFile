using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001311 RID: 4881
	public abstract class Move : Behaviour
	{
		// Token: 0x06006075 RID: 24693 RVA: 0x0011A690 File Offset: 0x00118890
		protected void Start()
		{
			this._childs = new List<Behaviour>
			{
				this.idle
			};
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x0011A6AC File Offset: 0x001188AC
		protected bool LookAround(AIController controller)
		{
			Character character = controller.FindClosestPlayerBody(controller.stopTrigger);
			if (character != null)
			{
				controller.target = character;
				return true;
			}
			return false;
		}

		// Token: 0x04004DB0 RID: 19888
		[HideInInspector]
		public Vector2 direction;

		// Token: 0x04004DB1 RID: 19889
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Idle))]
		protected Behaviour idle;

		// Token: 0x04004DB2 RID: 19890
		[SerializeField]
		protected bool checkWithinSight;

		// Token: 0x04004DB3 RID: 19891
		[SerializeField]
		protected bool wander;

		// Token: 0x02001312 RID: 4882
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06006078 RID: 24696 RVA: 0x0011A6D9 File Offset: 0x001188D9
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Move.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004DB4 RID: 19892
			public new static readonly Type[] types = new Type[]
			{
				typeof(MoveForDuration),
				typeof(MoveToDestination),
				typeof(MoveToTarget),
				typeof(MoveToBehindWithFly),
				typeof(MoveForDurationWithFly),
				typeof(Fly)
			};
		}
	}
}
