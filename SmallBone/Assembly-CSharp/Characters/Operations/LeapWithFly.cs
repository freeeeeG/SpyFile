using System;
using System.Collections;
using Characters.AI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations
{
	// Token: 0x02000DC7 RID: 3527
	public class LeapWithFly : CharacterOperation
	{
		// Token: 0x060046DA RID: 18138 RVA: 0x000CDB2B File Offset: 0x000CBD2B
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CMoveToTarget(owner));
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x000CDB3B File Offset: 0x000CBD3B
		private IEnumerator CMoveToTarget(Character owner)
		{
			Vector3 destination = (this._target == null) ? this._aiController.target.transform.position : this._target.transform.position;
			Vector3 source = owner.transform.position;
			for (float elapsed = 0f; elapsed < this.curve.duration; elapsed += owner.chronometer.master.deltaTime)
			{
				yield return null;
				while (owner.stunedOrFreezed)
				{
					yield return null;
				}
				if (elapsed < this._lookingTime)
				{
					destination = ((this._target == null) ? this._aiController.target.transform.position : this._target.transform.position);
					owner.ForceToLookAt(destination.x);
				}
				Vector2 a = Vector2.Lerp(source, destination, this.curve.Evaluate(elapsed));
				owner.movement.force = a - owner.transform.position;
			}
			yield break;
		}

		// Token: 0x040035BC RID: 13756
		[SerializeField]
		private AIController _aiController;

		// Token: 0x040035BD RID: 13757
		[SerializeField]
		private Transform _target;

		// Token: 0x040035BE RID: 13758
		[SerializeField]
		private Curve curve;

		// Token: 0x040035BF RID: 13759
		[FormerlySerializedAs("_chaseTime")]
		[SerializeField]
		private float _lookingTime;
	}
}
