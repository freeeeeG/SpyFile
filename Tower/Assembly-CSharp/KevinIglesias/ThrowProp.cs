using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D5 RID: 213
	public class ThrowProp : MonoBehaviour
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		private void Start()
		{
			this.characterRoot = base.transform;
			this.zeroPosition = this.propToThrow.localPosition;
			this.zeroRotation = this.propToThrow.localRotation;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		private void Update()
		{
			if (this.launched && (this.propType == PropType.Spear || this.propType == PropType.Knife) && !this.propLanded)
			{
				float x = this.startPos.x;
				float x2 = this.targetPos.position.x;
				float num = x2 - x;
				float num2 = Mathf.MoveTowards(this.propToThrow.position.x, x2, this.speed * Time.deltaTime);
				float num3 = Mathf.Lerp(this.startPos.y, this.targetPos.position.y, (num2 - x) / num);
				float num4 = this.arcHeight * (num2 - x) * (num2 - x2) / (-0.25f * num * num);
				Vector3 vector = new Vector3(num2, num3 + num4, this.propToThrow.position.z);
				this.propToThrow.rotation = ThrowProp.LookAt2D(vector - this.propToThrow.position);
				this.propToThrow.position = vector;
				if (Mathf.Abs(this.targetPos.position.x - this.propToThrow.position.x) < 0.5f)
				{
					this.propLanded = true;
				}
			}
			if (this.launched && this.propType == PropType.Tomahawk && !this.propLanded)
			{
				float x3 = this.startPos.x;
				float x4 = this.targetPos.position.x;
				float num5 = x4 - x3;
				float num6 = Mathf.MoveTowards(this.propToThrow.position.x, x4, this.speed * Time.deltaTime);
				float num7 = Mathf.Lerp(this.startPos.y, this.targetPos.position.y, (num6 - x3) / num5);
				float num8 = this.arcHeight * (num6 - x3) * (num6 - x4) / (-0.25f * num5 * num5);
				Vector3 position = new Vector3(num6, num7 + num8, this.propToThrow.position.z);
				this.propToThrow.transform.Rotate(19f, 0f, 0f, Space.Self);
				this.propToThrow.position = position;
				if (Mathf.Abs(this.targetPos.position.x - this.propToThrow.position.x) < 0.5f)
				{
					this.propLanded = true;
				}
			}
			if (this.retargeter.localPosition.y > 0f)
			{
				if (!this.launched && !this.recoverProp)
				{
					this.Throw();
				}
				if (this.launched && this.recoverProp)
				{
					this.RecoverProp();
					return;
				}
			}
			else
			{
				if (!this.recoverProp && this.launched)
				{
					this.recoverProp = true;
				}
				if (this.recoverProp && !this.launched)
				{
					this.recoverProp = false;
				}
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000DE50 File Offset: 0x0000C050
		private static Quaternion LookAt2D(Vector3 forward)
		{
			return Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * 57.29578f - 90f);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000DE7E File Offset: 0x0000C07E
		public void Throw()
		{
			this.launched = true;
			this.startPos = this.propToThrow.position;
			this.propToThrow.SetParent(this.characterRoot);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		public void RecoverProp()
		{
			this.propLanded = false;
			this.launched = false;
			this.propToThrow.SetParent(this.hand);
			this.propToThrow.localPosition = this.zeroPosition;
			this.propToThrow.localRotation = this.zeroRotation;
		}

		// Token: 0x040002D9 RID: 729
		public Transform retargeter;

		// Token: 0x040002DA RID: 730
		public PropType propType;

		// Token: 0x040002DB RID: 731
		public Transform propToThrow;

		// Token: 0x040002DC RID: 732
		public Transform hand;

		// Token: 0x040002DD RID: 733
		public Transform targetPos;

		// Token: 0x040002DE RID: 734
		public float speed = 10f;

		// Token: 0x040002DF RID: 735
		public float arcHeight = 1f;

		// Token: 0x040002E0 RID: 736
		public bool launched;

		// Token: 0x040002E1 RID: 737
		public bool recoverProp;

		// Token: 0x040002E2 RID: 738
		public bool propLanded;

		// Token: 0x040002E3 RID: 739
		private Transform characterRoot;

		// Token: 0x040002E4 RID: 740
		private Vector3 startPos;

		// Token: 0x040002E5 RID: 741
		private Vector3 zeroPosition;

		// Token: 0x040002E6 RID: 742
		private Quaternion zeroRotation;

		// Token: 0x040002E7 RID: 743
		private Vector3 nextPos;
	}
}
