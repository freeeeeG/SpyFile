using System;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000980 RID: 2432
	public class MotionConstraint : Constraint
	{
		// Token: 0x0600343B RID: 13371 RVA: 0x0009AA90 File Offset: 0x00098C90
		public override void Initilaize(Action action)
		{
			base.Initilaize(action);
			foreach (MotionConstraint.Info info in this._infos.values)
			{
				if (info.animationRange.y == 1f)
				{
					info.animationRange.y = float.PositiveInfinity;
				}
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x0009AAE4 File Offset: 0x00098CE4
		public override bool Pass()
		{
			Motion runningMotion = this._action.owner.runningMotion;
			if (runningMotion == null)
			{
				return true;
			}
			foreach (MotionConstraint.Info info in this._infos.values)
			{
				if (runningMotion == info.motion)
				{
					return MMMaths.Range(runningMotion.normalizedTime, info.animationRange);
				}
			}
			return true;
		}

		// Token: 0x04002A49 RID: 10825
		[SerializeField]
		private MotionConstraint.Info.Reorderable _infos;

		// Token: 0x02000981 RID: 2433
		[Serializable]
		private class Info
		{
			// Token: 0x04002A4A RID: 10826
			[SerializeField]
			internal Action action;

			// Token: 0x04002A4B RID: 10827
			[SerializeField]
			internal Motion motion;

			// Token: 0x04002A4C RID: 10828
			[SerializeField]
			[MinMaxSlider(0f, 1f)]
			internal Vector2 animationRange;

			// Token: 0x02000982 RID: 2434
			[Serializable]
			internal class Reorderable : ReorderableArray<MotionConstraint.Info>
			{
			}
		}
	}
}
