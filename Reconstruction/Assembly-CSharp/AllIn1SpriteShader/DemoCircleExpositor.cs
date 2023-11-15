using System;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C8 RID: 712
	public class DemoCircleExpositor : MonoBehaviour
	{
		// Token: 0x06001157 RID: 4439 RVA: 0x00031A88 File Offset: 0x0002FC88
		private void Start()
		{
			this.dummyRotation = base.transform.rotation;
			this.iniY = base.transform.position.y;
			this.items = new Transform[base.transform.childCount];
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				this.items[this.count] = transform;
				this.count++;
			}
			this.offsetRotation = 360f / (float)this.count;
			for (int i = 0; i < this.count; i++)
			{
				float f = (float)i * 3.1415927f * 2f / (float)this.count;
				Vector3 position = new Vector3(Mathf.Sin(f) * this.radius, this.iniY, -Mathf.Cos(f) * this.radius);
				this.items[i].position = position;
			}
			this.zOffset = this.radius - 40f;
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.zOffset);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00031BF0 File Offset: 0x0002FDF0
		private void Update()
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.dummyRotation, this.rotateSpeed * Time.deltaTime);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00031C20 File Offset: 0x0002FE20
		public void ChangeTarget(int offset)
		{
			this.currentTarget += offset;
			if (this.currentTarget > this.items.Length - 1)
			{
				this.currentTarget = 0;
			}
			else if (this.currentTarget < 0)
			{
				this.currentTarget = this.items.Length - 1;
			}
			this.dummyRotation *= Quaternion.Euler(Vector3.up * (float)offset * this.offsetRotation);
		}

		// Token: 0x040009A9 RID: 2473
		[SerializeField]
		private float radius = 40f;

		// Token: 0x040009AA RID: 2474
		[SerializeField]
		private float rotateSpeed = 10f;

		// Token: 0x040009AB RID: 2475
		private float zOffset;

		// Token: 0x040009AC RID: 2476
		private Transform[] items;

		// Token: 0x040009AD RID: 2477
		private int count;

		// Token: 0x040009AE RID: 2478
		private int currentTarget;

		// Token: 0x040009AF RID: 2479
		private float offsetRotation;

		// Token: 0x040009B0 RID: 2480
		private float iniY;

		// Token: 0x040009B1 RID: 2481
		private Quaternion dummyRotation;
	}
}
