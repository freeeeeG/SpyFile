using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Pope;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001358 RID: 4952
	public sealed class Teleport : Move
	{
		// Token: 0x0600619D RID: 24989 RVA: 0x0011D894 File Offset: 0x0011BA94
		public override IEnumerator CRun(AIController controller)
		{
			this._teleportDestination.position = this._navigation.destination.position;
			this._in.TryStart();
			while (this._in.running)
			{
				yield return null;
			}
			this._out.TryStart();
			while (this._out.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x0011D8A3 File Offset: 0x0011BAA3
		public override void SetDestination(Point.Tag tag)
		{
			this._navigation.destinationTag = tag;
		}

		// Token: 0x04004EBE RID: 20158
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Teleport))]
		private Teleport _teleport;

		// Token: 0x04004EBF RID: 20159
		[SerializeField]
		private Characters.Actions.Action _in;

		// Token: 0x04004EC0 RID: 20160
		[SerializeField]
		private Characters.Actions.Action _out;

		// Token: 0x04004EC1 RID: 20161
		[SerializeField]
		private Transform _teleportDestination;

		// Token: 0x04004EC2 RID: 20162
		[SerializeField]
		private Navigation _navigation;

		// Token: 0x04004EC3 RID: 20163
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(LineEffect))]
		private LineEffect _lineEffect;
	}
}
