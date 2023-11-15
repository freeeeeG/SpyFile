using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012CF RID: 4815
	public sealed class FanaticCall : Behaviour
	{
		// Token: 0x06005F44 RID: 24388 RVA: 0x00116FF5 File Offset: 0x001151F5
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this.SetSpawnPoint(controller.target);
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x0011700C File Offset: 0x0011520C
		private void SetSpawnPoint(Character target)
		{
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			foreach (object obj in this._spawnPositionParent)
			{
				Transform transform = (Transform)obj;
				float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
				float y = bounds.max.y;
				transform.position = new Vector2(x, y);
			}
		}

		// Token: 0x06005F46 RID: 24390 RVA: 0x001170B8 File Offset: 0x001152B8
		public bool CanUse()
		{
			return this._action.canUse;
		}

		// Token: 0x04004C8C RID: 19596
		[SerializeField]
		private Transform _spawnPositionParent;

		// Token: 0x04004C8D RID: 19597
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
