using System;
using UnityEngine;

namespace DentedPixel.LTExamples
{
	// Token: 0x0200026C RID: 620
	public class PathBezier : MonoBehaviour
	{
		// Token: 0x06000D4F RID: 3407 RVA: 0x00030510 File Offset: 0x0002E710
		private void OnEnable()
		{
			this.cr = new LTBezierPath(new Vector3[]
			{
				this.trans[0].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[3].position,
				this.trans[3].position,
				this.trans[5].position,
				this.trans[4].position,
				this.trans[6].position
			});
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000305D0 File Offset: 0x0002E7D0
		private void Start()
		{
			this.avatar1 = GameObject.Find("Avatar1");
			LTDescr ltdescr = LeanTween.move(this.avatar1, this.cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
			Debug.Log("length of path 1:" + this.cr.length);
			Debug.Log("length of path 2:" + ltdescr.optional.path.length);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00030658 File Offset: 0x0002E858
		private void Update()
		{
			this.iter += Time.deltaTime * 0.07f;
			if (this.iter > 1f)
			{
				this.iter = 0f;
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003068A File Offset: 0x0002E88A
		private void OnDrawGizmos()
		{
			if (this.cr != null)
			{
				this.OnEnable();
			}
			Gizmos.color = Color.red;
			if (this.cr != null)
			{
				this.cr.gizmoDraw(-1f);
			}
		}

		// Token: 0x040009AF RID: 2479
		public Transform[] trans;

		// Token: 0x040009B0 RID: 2480
		private LTBezierPath cr;

		// Token: 0x040009B1 RID: 2481
		private GameObject avatar1;

		// Token: 0x040009B2 RID: 2482
		private float iter;
	}
}
