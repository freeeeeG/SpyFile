using System;
using System.Collections;
using UnityEngine;

namespace FX
{
	// Token: 0x02000241 RID: 577
	public class LineEffect : MonoBehaviour
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0001F546 File Offset: 0x0001D746
		// (set) Token: 0x06000B58 RID: 2904 RVA: 0x0001F54E File Offset: 0x0001D74E
		public Vector2 startPoint
		{
			get
			{
				return this._startPoint;
			}
			set
			{
				this._startPoint = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0001F557 File Offset: 0x0001D757
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0001F55F File Offset: 0x0001D75F
		public Vector2 endPoint
		{
			get
			{
				return this._endPoint;
			}
			set
			{
				this._endPoint = value;
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0001F568 File Offset: 0x0001D768
		public void Run()
		{
			this._body.gameObject.SetActive(false);
			float y = Vector2.Distance(this._startPoint, this._endPoint);
			this._body.localScale = new Vector2(this._thickness, y);
			Vector3 vector = this._endPoint - this._startPoint;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f;
			this._body.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			base.transform.position = new Vector2(this._startPoint.x, this._startPoint.y + 1f);
			this._body.gameObject.SetActive(true);
			base.StartCoroutine(this.CDeactive());
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0001F64E File Offset: 0x0001D84E
		public void Hide()
		{
			this._body.gameObject.SetActive(false);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0001F661 File Offset: 0x0001D861
		private IEnumerator CDeactive()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration);
			this._body.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000973 RID: 2419
		[SerializeField]
		private Transform _body;

		// Token: 0x04000974 RID: 2420
		[SerializeField]
		private float _thickness = 1f;

		// Token: 0x04000975 RID: 2421
		[SerializeField]
		private float _duration = 0.1f;

		// Token: 0x04000976 RID: 2422
		private Vector2 _startPoint;

		// Token: 0x04000977 RID: 2423
		private Vector2 _endPoint;
	}
}
