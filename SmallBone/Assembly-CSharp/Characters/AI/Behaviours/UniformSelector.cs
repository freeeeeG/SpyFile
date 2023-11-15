using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012BD RID: 4797
	public class UniformSelector : Decorator
	{
		// Token: 0x06005EFB RID: 24315 RVA: 0x001165CD File Offset: 0x001147CD
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._container.Count <= 0)
			{
				this.Fill();
			}
			Behaviour behaviour = this._container.Random<Behaviour>();
			this._container.Remove(behaviour);
			yield return behaviour.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x001165E4 File Offset: 0x001147E4
		private void Fill()
		{
			foreach (Weight weight in this._weights.components)
			{
				for (int j = 0; j < weight.value; j++)
				{
					this._container.Add(weight.key);
				}
			}
		}

		// Token: 0x04004C4A RID: 19530
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Weight))]
		private Weight.Subcomponents _weights;

		// Token: 0x04004C4B RID: 19531
		private List<Behaviour> _container = new List<Behaviour>();
	}
}
