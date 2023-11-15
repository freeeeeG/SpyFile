using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012CA RID: 4810
	public sealed class ContinuousTackle : Behaviour
	{
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06005F2A RID: 24362 RVA: 0x00116C2C File Offset: 0x00114E2C
		// (set) Token: 0x06005F2B RID: 24363 RVA: 0x00116C34 File Offset: 0x00114E34
		public bool canUse { get; private set; } = true;

		// Token: 0x06005F2C RID: 24364 RVA: 0x00116C3D File Offset: 0x00114E3D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			int num;
			for (int i = 0; i < this._count; i = num + 1)
			{
				if (this._actionOnce.TryStart())
				{
					while (this._actionOnce.running && base.result == Behaviour.Result.Doing)
					{
						yield return null;
					}
					character.ForceToLookAt((character.lookingDirection == Character.LookingDirection.Right) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
				}
				num = i;
			}
			base.StartCoroutine(this.CCooldown(controller.character.chronometer.master));
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005F2D RID: 24365 RVA: 0x00116C53 File Offset: 0x00114E53
		private IEnumerator CCooldown(Chronometer chronometer)
		{
			this.canUse = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this.canUse = true;
			yield break;
		}

		// Token: 0x04004C74 RID: 19572
		[SerializeField]
		private Characters.Actions.Action _actionOnce;

		// Token: 0x04004C75 RID: 19573
		[Range(1f, 10f)]
		[SerializeField]
		private int _count;

		// Token: 0x04004C76 RID: 19574
		[Range(0f, 20f)]
		[SerializeField]
		private float _coolTime;
	}
}
