using System;
using System.Collections;
using Characters.AI.Pope;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001355 RID: 4949
	public sealed class Fly : Move
	{
		// Token: 0x0600618D RID: 24973 RVA: 0x0011D602 File Offset: 0x0011B802
		public override IEnumerator CRun(AIController controller)
		{
			controller.destination = this._navigation.destination.position;
			yield return this.CMoveToTarget(controller, controller.character);
			yield break;
		}

		// Token: 0x0600618E RID: 24974 RVA: 0x0011D618 File Offset: 0x0011B818
		public override void SetDestination(Point.Tag tag)
		{
			this._navigation.destinationTag = tag;
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x0011D626 File Offset: 0x0011B826
		private IEnumerator CMoveToTarget(AIController controller, Character owner)
		{
			Vector2 destination = controller.destination;
			Vector3 source = owner.transform.position;
			float elapsed = 0f;
			float num = Mathf.Abs(Vector2.Distance(destination, source));
			double duration = (double)num * owner.stat.GetConstant(Stat.Kind.MovementSpeed) / 60.0;
			this.curve.duration = (float)duration;
			while (elapsed < this.curve.duration)
			{
				yield return null;
				if (!owner.stunedOrFreezed)
				{
					if ((double)elapsed < duration)
					{
						owner.ForceToLookAt(destination.x);
					}
					Vector2 a = Vector2.Lerp(source, destination, this.curve.Evaluate(elapsed));
					owner.movement.force = a - owner.transform.position;
					elapsed += owner.chronometer.master.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x04004EAE RID: 20142
		[SerializeField]
		private Navigation _navigation;

		// Token: 0x04004EAF RID: 20143
		[SerializeField]
		private Curve curve;

		// Token: 0x04004EB0 RID: 20144
		[SerializeField]
		private float _durationMultiplierPerDistance = 1f;
	}
}
