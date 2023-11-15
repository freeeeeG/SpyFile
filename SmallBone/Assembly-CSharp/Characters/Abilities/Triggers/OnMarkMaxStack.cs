using System;
using Characters.Marks;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B56 RID: 2902
	[Serializable]
	public class OnMarkMaxStack : Trigger
	{
		// Token: 0x06003A39 RID: 14905 RVA: 0x000AC1D7 File Offset: 0x000AA3D7
		public override void Attach(Character character)
		{
			MarkInfo mark = this._mark;
			mark.onStack = (MarkInfo.OnStackDelegate)Delegate.Combine(mark.onStack, new MarkInfo.OnStackDelegate(this.OnStack));
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x000AC200 File Offset: 0x000AA400
		public override void Detach()
		{
			MarkInfo mark = this._mark;
			mark.onStack = (MarkInfo.OnStackDelegate)Delegate.Remove(mark.onStack, new MarkInfo.OnStackDelegate(this.OnStack));
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x000AC22C File Offset: 0x000AA42C
		private void OnStack(Mark mark, float stack)
		{
			if (stack < (float)this._mark.maxStack || !base.canBeTriggered)
			{
				return;
			}
			if (this._moveToTargetPosition != null)
			{
				this._moveToTargetPosition.position = MMMaths.RandomPointWithinBounds(mark.owner.collider.bounds);
			}
			if (this._clearMarkOnTriggered)
			{
				mark.ClearStack(this._mark);
			}
			base.Invoke();
		}

		// Token: 0x04002E4B RID: 11851
		[SerializeField]
		private Transform _moveToTargetPosition;

		// Token: 0x04002E4C RID: 11852
		[SerializeField]
		private MarkInfo _mark;

		// Token: 0x04002E4D RID: 11853
		[SerializeField]
		private bool _clearMarkOnTriggered;

		// Token: 0x04002E4E RID: 11854
		private Character _character;
	}
}
