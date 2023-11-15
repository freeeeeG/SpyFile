using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000366 RID: 870
	public class Sequence : Trigger
	{
		// Token: 0x06001016 RID: 4118 RVA: 0x0002FF78 File Offset: 0x0002E178
		protected override bool Check()
		{
			if (this._triggers == null)
			{
				return true;
			}
			using (IEnumerator<bool> enumerator = this._triggers.CCheckNext().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000D32 RID: 3378
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TriggerArray))]
		private TriggerArray.Subcomponents _triggers;
	}
}
