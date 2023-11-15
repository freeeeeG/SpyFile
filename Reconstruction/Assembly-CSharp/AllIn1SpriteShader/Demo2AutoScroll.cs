using System;
using System.Collections;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C6 RID: 710
	public class Demo2AutoScroll : MonoBehaviour
	{
		// Token: 0x06001150 RID: 4432 RVA: 0x00031918 File Offset: 0x0002FB18
		private void Start()
		{
			this.sceneDescription.SetActive(false);
			Camera.main.fieldOfView = 60f;
			this.children = base.GetComponentsInChildren<Transform>();
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i].gameObject != base.gameObject)
				{
					this.children[i].gameObject.SetActive(false);
					this.children[i].localPosition = Vector3.zero;
				}
			}
			this.totalTime /= (float)this.children.Length;
			base.StartCoroutine(this.ScrollElements());
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x000319C1 File Offset: 0x0002FBC1
		private IEnumerator ScrollElements()
		{
			int i = 0;
			for (;;)
			{
				if (this.children[i].gameObject == base.gameObject)
				{
					i = (i + 1) % this.children.Length;
				}
				else
				{
					this.children[i].gameObject.SetActive(true);
					yield return new WaitForSeconds(this.totalTime);
					this.children[i].gameObject.SetActive(false);
					i = (i + 1) % this.children.Length;
				}
			}
			yield break;
		}

		// Token: 0x040009A0 RID: 2464
		private Transform[] children;

		// Token: 0x040009A1 RID: 2465
		public float totalTime;

		// Token: 0x040009A2 RID: 2466
		public GameObject sceneDescription;
	}
}
