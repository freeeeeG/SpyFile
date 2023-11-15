using System;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FC3 RID: 4035
	public sealed class BigBang : CharacterOperation
	{
		// Token: 0x06004E23 RID: 20003 RVA: 0x000E9D27 File Offset: 0x000E7F27
		public override void Initialize()
		{
			this._multipleFireProjectile.Initialize();
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x000E9D34 File Offset: 0x000E7F34
		public override void Run(Character owner)
		{
			this.MakeFireTransfom();
			if (owner.lookingDirection == Character.LookingDirection.Left)
			{
				this._fireTransformContainer.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				this._fireTransformContainer.localScale = new Vector3(1f, 1f, 1f);
			}
			this._multipleFireProjectile.Run(owner);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x000E9D9C File Offset: 0x000E7F9C
		private void MakeFireTransfom()
		{
			float num = (float)(360 / this._fireTransformContainer.childCount);
			float num2 = UnityEngine.Random.Range(0f, num);
			for (int i = 0; i < this._fireTransformContainer.childCount; i++)
			{
				Transform child = this._fireTransformContainer.GetChild(i);
				child.transform.localPosition = new Vector2(Mathf.Cos(num2 * 0.017453292f), Mathf.Sin(num2 * 0.017453292f)) * this._radius;
				child.transform.localRotation = Quaternion.Euler(0f, 0f, 180f + num2);
				num2 += num;
			}
		}

		// Token: 0x04003E2A RID: 15914
		[SerializeField]
		[Header("Gain Energy")]
		private Transform _fireTransformContainer;

		// Token: 0x04003E2B RID: 15915
		[SerializeField]
		private float _radius = 20f;

		// Token: 0x04003E2C RID: 15916
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MultipleFireProjectile))]
		private MultipleFireProjectile _multipleFireProjectile;
	}
}
