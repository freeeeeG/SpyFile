using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C5 RID: 197
	public class LineController : MonoBehaviour
	{
		// Token: 0x06000648 RID: 1608 RVA: 0x0001CD90 File Offset: 0x0001AF90
		private void Start()
		{
			this.lineRenderer.positionCount = this.targets.Length;
			this.SetLinePositions();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001CDAB File Offset: 0x0001AFAB
		private void Update()
		{
			this.SetLinePositions();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
		private void SetLinePositions()
		{
			Vector3[] array = new Vector3[this.targets.Length];
			for (int i = 0; i < this.targets.Length; i++)
			{
				array[i] = this.targets[i].transform.position;
			}
			this.lineRenderer.SetPositions(array);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001CE08 File Offset: 0x0001B008
		public Vector3[] GetPositions()
		{
			Vector3[] array = new Vector3[this.lineRenderer.positionCount];
			this.lineRenderer.GetPositions(array);
			return array;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001CE34 File Offset: 0x0001B034
		public float GetWidth()
		{
			return this.lineRenderer.startWidth;
		}

		// Token: 0x0400040C RID: 1036
		[SerializeField]
		private LineRenderer lineRenderer;

		// Token: 0x0400040D RID: 1037
		[SerializeField]
		private GameObject[] targets;
	}
}
