using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008E1 RID: 2273
	public class RunOperation : UseQuintessence
	{
		// Token: 0x06003099 RID: 12441 RVA: 0x00091A35 File Offset: 0x0008FC35
		protected override void Awake()
		{
			base.Awake();
			this._quintessence.onEquipped += this._operations.Initialize;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x00091A5C File Offset: 0x0008FC5C
		protected override void OnUse()
		{
			if (this._flipObject != null)
			{
				if (this._quintessence.owner.lookingDirection == Character.LookingDirection.Right)
				{
					this._flipObject.localScale = new Vector2(1f, 1f);
				}
				else
				{
					this._flipObject.localScale = new Vector2(-1f, 1f);
				}
			}
			base.StartCoroutine(this._operations.CRun(this._quintessence.owner));
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x00091AE6 File Offset: 0x0008FCE6
		private void OnDisable()
		{
			this._operations.StopAll();
		}

		// Token: 0x04002820 RID: 10272
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04002821 RID: 10273
		[SerializeField]
		private Transform _flipObject;
	}
}
