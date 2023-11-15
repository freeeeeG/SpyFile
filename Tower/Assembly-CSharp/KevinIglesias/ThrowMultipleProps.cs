using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D3 RID: 211
	public class ThrowMultipleProps : MonoBehaviour
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000D660 File Offset: 0x0000B860
		private void Start()
		{
			this.characterRoot = base.transform;
			this.zeroPosition1 = this.propToThrow1.localPosition;
			this.zeroRotation1 = this.propToThrow1.localRotation;
			this.zeroPosition2 = this.propToThrow2.localPosition;
			this.zeroRotation2 = this.propToThrow2.localRotation;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000D6C0 File Offset: 0x0000B8C0
		private void Update()
		{
			if (this.launched1 && !this.propLanded1)
			{
				float x = this.startPos1.x;
				float x2 = this.targetPos.position.x;
				float num = x2 - x;
				float num2 = Mathf.MoveTowards(this.propToThrow1.position.x, x2, this.speed * Time.deltaTime);
				float num3 = Mathf.Lerp(this.startPos1.y, this.targetPos.position.y, (num2 - x) / num);
				float num4 = this.arcHeight * (num2 - x) * (num2 - x2) / (-0.25f * num * num);
				Vector3 vector = new Vector3(num2, num3 + num4, this.propToThrow1.position.z);
				this.propToThrow1.rotation = ThrowMultipleProps.LookAt2D(vector - this.propToThrow1.position);
				this.propToThrow1.position = vector;
				if (Mathf.Abs(this.targetPos.position.x - this.propToThrow1.position.x) < 0.5f)
				{
					this.propLanded1 = true;
				}
			}
			if (this.launched2 && !this.propLanded2)
			{
				float x3 = this.startPos2.x;
				float x4 = this.targetPos.position.x;
				float num5 = x4 - x3;
				float num6 = Mathf.MoveTowards(this.propToThrow2.position.x, x4, this.speed * Time.deltaTime);
				float num7 = Mathf.Lerp(this.startPos2.y, this.targetPos.position.y, (num6 - x3) / num5);
				float num8 = this.arcHeight * (num6 - x3) * (num6 - x4) / (-0.25f * num5 * num5);
				Vector3 vector2 = new Vector3(num6, num7 + num8, this.propToThrow2.position.z);
				this.propToThrow2.rotation = ThrowMultipleProps.LookAt2D(vector2 - this.propToThrow2.position);
				this.propToThrow2.position = vector2;
				if (Mathf.Abs(this.targetPos.position.x - this.propToThrow2.position.x) < 0.5f)
				{
					this.propLanded2 = true;
				}
			}
			if (this.retargeter1.localPosition.y > 0f)
			{
				if (!this.launched1 && !this.recoverProp1)
				{
					this.Throw1();
				}
				if (this.launched1 && this.recoverProp1)
				{
					this.RecoverProp1();
				}
			}
			else
			{
				if (!this.recoverProp1 && this.launched1)
				{
					this.recoverProp1 = true;
				}
				if (this.recoverProp1 && !this.launched1)
				{
					this.recoverProp1 = false;
				}
			}
			if (this.retargeter2.localPosition.y > 0f)
			{
				if (!this.launched2 && !this.recoverProp2)
				{
					this.Throw2();
				}
				if (this.launched2 && this.recoverProp2)
				{
					this.RecoverProp2();
					return;
				}
			}
			else
			{
				if (!this.recoverProp2 && this.launched2)
				{
					this.recoverProp2 = true;
				}
				if (this.recoverProp2 && !this.launched2)
				{
					this.recoverProp2 = false;
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D9F9 File Offset: 0x0000BBF9
		private static Quaternion LookAt2D(Vector3 forward)
		{
			return Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * 57.29578f - 90f);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000DA27 File Offset: 0x0000BC27
		public void Throw1()
		{
			this.startPos1 = this.propToThrow1.position;
			this.propToThrow1.SetParent(this.characterRoot);
			this.launched1 = true;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000DA52 File Offset: 0x0000BC52
		public void Throw2()
		{
			this.startPos2 = this.propToThrow2.position;
			this.propToThrow2.SetParent(this.characterRoot);
			this.launched2 = true;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000DA80 File Offset: 0x0000BC80
		public void RecoverProp1()
		{
			this.propLanded1 = false;
			this.launched1 = false;
			this.propToThrow1.SetParent(this.hand1);
			this.propToThrow1.localPosition = this.zeroPosition1;
			this.propToThrow1.localRotation = this.zeroRotation1;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
		public void RecoverProp2()
		{
			this.propLanded2 = false;
			this.launched2 = false;
			this.propToThrow2.SetParent(this.hand2);
			this.propToThrow2.localPosition = this.zeroPosition2;
			this.propToThrow2.localRotation = this.zeroRotation2;
		}

		// Token: 0x040002BE RID: 702
		public Transform retargeter1;

		// Token: 0x040002BF RID: 703
		public Transform retargeter2;

		// Token: 0x040002C0 RID: 704
		public Transform propToThrow1;

		// Token: 0x040002C1 RID: 705
		public Transform propToThrow2;

		// Token: 0x040002C2 RID: 706
		public Transform hand1;

		// Token: 0x040002C3 RID: 707
		public Transform hand2;

		// Token: 0x040002C4 RID: 708
		public Transform targetPos;

		// Token: 0x040002C5 RID: 709
		public float speed = 10f;

		// Token: 0x040002C6 RID: 710
		public float arcHeight = 1f;

		// Token: 0x040002C7 RID: 711
		public bool launched1;

		// Token: 0x040002C8 RID: 712
		public bool launched2;

		// Token: 0x040002C9 RID: 713
		public bool recoverProp1;

		// Token: 0x040002CA RID: 714
		public bool recoverProp2;

		// Token: 0x040002CB RID: 715
		public bool propLanded1;

		// Token: 0x040002CC RID: 716
		public bool propLanded2;

		// Token: 0x040002CD RID: 717
		private Transform characterRoot;

		// Token: 0x040002CE RID: 718
		private Vector3 startPos1;

		// Token: 0x040002CF RID: 719
		private Vector3 startPos2;

		// Token: 0x040002D0 RID: 720
		private Vector3 zeroPosition1;

		// Token: 0x040002D1 RID: 721
		private Quaternion zeroRotation1;

		// Token: 0x040002D2 RID: 722
		private Vector3 zeroPosition2;

		// Token: 0x040002D3 RID: 723
		private Quaternion zeroRotation2;

		// Token: 0x040002D4 RID: 724
		private Vector3 nextPos;
	}
}
