using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FDF RID: 4063
	public sealed class DropManatechPart : CharacterOperation
	{
		// Token: 0x06004E8B RID: 20107 RVA: 0x000EB4D4 File Offset: 0x000E96D4
		public override void Run(Character owner)
		{
			float num = UnityEngine.Random.Range(this._countRange.x, this._countRange.y);
			int num2 = 0;
			while ((float)num2 < num)
			{
				Vector3 position = base.transform.position;
				position.y += 0.5f;
				this._manatechPart.poolObject.Spawn(position, true).GetComponent<DroppedManatechPart>().cooldownReducingAmount = 1f;
				num2++;
			}
		}

		// Token: 0x04003E9C RID: 16028
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _countRange;

		// Token: 0x04003E9D RID: 16029
		[SerializeField]
		private DroppedManatechPart _manatechPart;
	}
}
