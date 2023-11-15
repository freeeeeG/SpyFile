using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001328 RID: 4904
	public class Teleport : Behaviour
	{
		// Token: 0x060060CD RID: 24781 RVA: 0x0011BA93 File Offset: 0x00119C93
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			float num = controller.target.transform.position.x - controller.character.transform.position.x;
			controller.character.lookingDirection = ((num > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			if (this._teleportStart == null || this._teleportStart.TryStart())
			{
				if (this._teleportStart != null)
				{
					while (this._teleportStart.running)
					{
						yield return null;
					}
				}
				yield return this._hide.CRun(controller);
				this._teleportEnd.TryStart();
				while (this._teleportEnd.running)
				{
					yield return null;
				}
				yield return this._idle.CRun(controller);
				base.result = Behaviour.Result.Success;
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0011BAA9 File Offset: 0x00119CA9
		public bool CanUse()
		{
			return this._teleportEnd.canUse && this._teleportStart.canUse;
		}

		// Token: 0x04004E0A RID: 19978
		[SerializeField]
		private Characters.Actions.Action _teleportStart;

		// Token: 0x04004E0B RID: 19979
		[SerializeField]
		private Characters.Actions.Action _teleportEnd;

		// Token: 0x04004E0C RID: 19980
		[UnityEditor.Subcomponent(typeof(Hide))]
		[SerializeField]
		private Hide _hide;

		// Token: 0x04004E0D RID: 19981
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x02001329 RID: 4905
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060060D0 RID: 24784 RVA: 0x0011BAC5 File Offset: 0x00119CC5
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Teleport.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004E0E RID: 19982
			public new static readonly Type[] types = new Type[]
			{
				typeof(Teleport),
				typeof(TeleportBehind)
			};
		}
	}
}
