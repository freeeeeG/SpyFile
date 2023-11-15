using System;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B60 RID: 2912
	[Serializable]
	public class OnUpdate : Trigger
	{
		// Token: 0x06003A53 RID: 14931 RVA: 0x000AC7CA File Offset: 0x000AA9CA
		public override void Attach(Character character)
		{
			if (this._times == 0)
			{
				this._times = int.MaxValue;
			}
			this._remainTimes = this._times;
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x000AC7EB File Offset: 0x000AA9EB
		public override void Refresh()
		{
			base.Refresh();
			this._remainCooldownTime = 0f;
			this._remainTimes = this._times;
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00002191 File Offset: 0x00000391
		public override void Detach()
		{
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x000AC80C File Offset: 0x000AAA0C
		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (this._remainTimes <= 0 || this._remainCooldownTime > 0f)
			{
				return;
			}
			if (!this._constraint.components.Pass())
			{
				return;
			}
			if (MMMaths.PercentChance(this._possibility))
			{
				this._remainTimes--;
				Action onTriggered = this._onTriggered;
				if (onTriggered != null)
				{
					onTriggered();
				}
			}
			this._remainCooldownTime = base.cooldownTime;
		}

		// Token: 0x04002E67 RID: 11879
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _constraint;

		// Token: 0x04002E68 RID: 11880
		[SerializeField]
		private int _times;

		// Token: 0x04002E69 RID: 11881
		private int _remainTimes;
	}
}
