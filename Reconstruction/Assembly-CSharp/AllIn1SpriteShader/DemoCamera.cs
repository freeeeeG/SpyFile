using System;
using System.Collections;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C7 RID: 711
	public class DemoCamera : MonoBehaviour
	{
		// Token: 0x06001153 RID: 4435 RVA: 0x000319D8 File Offset: 0x0002FBD8
		private void Awake()
		{
			this.offset = base.transform.position - this.targetedItem.position;
			base.StartCoroutine(this.SetCamAfterStart());
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00031A08 File Offset: 0x0002FC08
		private void Update()
		{
			if (!this.canUpdate)
			{
				return;
			}
			this.target.y = (float)this.demoController.GetCurrExpositor() * this.demoController.expositorDistance;
			base.transform.position = Vector3.Lerp(base.transform.position, this.target, this.speed * Time.deltaTime);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00031A6E File Offset: 0x0002FC6E
		private IEnumerator SetCamAfterStart()
		{
			yield return null;
			base.transform.position = this.targetedItem.position + this.offset;
			this.target = base.transform.position;
			this.canUpdate = true;
			yield break;
		}

		// Token: 0x040009A3 RID: 2467
		[SerializeField]
		private Transform targetedItem;

		// Token: 0x040009A4 RID: 2468
		[SerializeField]
		private All1ShaderDemoController demoController;

		// Token: 0x040009A5 RID: 2469
		[SerializeField]
		private float speed;

		// Token: 0x040009A6 RID: 2470
		private Vector3 offset;

		// Token: 0x040009A7 RID: 2471
		private Vector3 target;

		// Token: 0x040009A8 RID: 2472
		private bool canUpdate;
	}
}
