using System;
using System.Collections;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DE9 RID: 3561
	public class Leap : CharacterOperation
	{
		// Token: 0x06004753 RID: 18259 RVA: 0x000CF4DD File Offset: 0x000CD6DD
		private void Awake()
		{
			if (this._target != null)
			{
				this._target.transform.parent = null;
			}
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x000CF4FE File Offset: 0x000CD6FE
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CMoveToTarget(owner));
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x000CF50E File Offset: 0x000CD70E
		private IEnumerator CMoveToTarget(Character owner)
		{
			float destination = (this._target == null) ? this._aiController.target.transform.position.x : this._target.transform.position.x;
			float source = owner.transform.position.x;
			float elapsed = 0f;
			for (;;)
			{
				yield return null;
				float num = Mathf.Lerp(source, destination, elapsed / this._duration);
				owner.movement.force.x = num - owner.transform.position.x;
				if (!owner.stunedOrFreezed)
				{
					if (elapsed > this._duration)
					{
						break;
					}
					elapsed += owner.chronometer.master.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x04003660 RID: 13920
		[SerializeField]
		private AIController _aiController;

		// Token: 0x04003661 RID: 13921
		[SerializeField]
		private Transform _target;

		// Token: 0x04003662 RID: 13922
		[SerializeField]
		private float _duration;
	}
}
