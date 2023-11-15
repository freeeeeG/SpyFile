using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B58 RID: 2904
	[Serializable]
	public class OnMove : Trigger
	{
		// Token: 0x06003A3E RID: 14910 RVA: 0x000AC2A6 File Offset: 0x000AA4A6
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.movement.onMoved += this.OnMoved;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000AC2CB File Offset: 0x000AA4CB
		public override void Detach()
		{
			this._character.movement.onMoved -= this.OnMoved;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000AC2EC File Offset: 0x000AA4EC
		private void OnMoved(Vector2 distance)
		{
			this._distance += Mathf.Abs(distance.x) * this._horizontalMultiplier + Mathf.Abs(distance.y) * this._verticalMultiplier;
			if (this._distance > this._distanceToTrigger)
			{
				this._distance -= this._distanceToTrigger;
				base.Invoke();
			}
		}

		// Token: 0x04002E4F RID: 11855
		private Character _character;

		// Token: 0x04002E50 RID: 11856
		[SerializeField]
		private float _distanceToTrigger = 1f;

		// Token: 0x04002E51 RID: 11857
		[SerializeField]
		private float _horizontalMultiplier = 1f;

		// Token: 0x04002E52 RID: 11858
		[SerializeField]
		private float _verticalMultiplier;

		// Token: 0x04002E53 RID: 11859
		private float _distance;
	}
}
