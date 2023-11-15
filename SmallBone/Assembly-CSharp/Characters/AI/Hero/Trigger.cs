using System;
using Characters.AI.Conditions;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x0200127B RID: 4731
	public class Trigger : MonoBehaviour
	{
		// Token: 0x06005DC4 RID: 24004 RVA: 0x00113FB7 File Offset: 0x001121B7
		public bool InShortRange(AIController controller)
		{
			return this._inShortRange.IsSatisfied(controller);
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x00113FC5 File Offset: 0x001121C5
		public bool InMiddleRange(AIController controller)
		{
			return this._inMiddleRange.IsSatisfied(controller);
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x00113FD3 File Offset: 0x001121D3
		public bool CanRunBackSlash(AIController controller)
		{
			return this._backslashCondition.IsSatisfied(controller);
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x00113FE1 File Offset: 0x001121E1
		public bool CanRunVerticalPierce(AIController controller)
		{
			return this._verticalPierceCondition.IsSatisfied(controller);
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x00113FEF File Offset: 0x001121EF
		public bool CanRunDashBreakAway(AIController controller)
		{
			return this._dashBreakAwayCondition.IsSatisfied(controller);
		}

		// Token: 0x06005DC9 RID: 24009 RVA: 0x00113FFD File Offset: 0x001121FD
		public bool ShouldBackStep(AIController controller)
		{
			return this._backstepCondition.IsSatisfied(controller);
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x0011400B File Offset: 0x0011220B
		public bool CanRunBehavourE(AIController controller)
		{
			return this._behaviourECondition.IsSatisfied(controller);
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x0011400B File Offset: 0x0011220B
		public bool CanRunBehavourJ(AIController controller)
		{
			return this._behaviourECondition.IsSatisfied(controller);
		}

		// Token: 0x04004B59 RID: 19289
		[Condition.SubcomponentAttribute(true)]
		[SerializeField]
		private Condition _backslashCondition;

		// Token: 0x04004B5A RID: 19290
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _verticalPierceCondition;

		// Token: 0x04004B5B RID: 19291
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _backstepCondition;

		// Token: 0x04004B5C RID: 19292
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _inShortRange;

		// Token: 0x04004B5D RID: 19293
		[Condition.SubcomponentAttribute(true)]
		[SerializeField]
		private Condition _inMiddleRange;

		// Token: 0x04004B5E RID: 19294
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _dashBreakAwayCondition;

		// Token: 0x04004B5F RID: 19295
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _behaviourECondition;

		// Token: 0x04004B60 RID: 19296
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _behaviourJCondition;
	}
}
