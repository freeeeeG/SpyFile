using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200011A RID: 282
	public class SpriteTrail : MonoBehaviour
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0002156C File Offset: 0x0001F76C
		private void Start()
		{
			this.mSpawnInterval = this.mTrailTime / (float)this.mTrailSegments;
			this.mTrailObjectsInUse = new List<GameObject>();
			this.mTrailObjectsNotInUse = new Queue<GameObject>();
			for (int i = 0; i < this.mTrailSegments + 1; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.mTrailObject);
				gameObject.transform.SetParent(base.transform);
				this.mTrailObjectsNotInUse.Enqueue(gameObject);
			}
			this.mbEnabled = new BoolToggle(false);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000215EC File Offset: 0x0001F7EC
		private void Update()
		{
			if (this.mbEnabled.value)
			{
				this.mSpawnTimer += Time.deltaTime;
				if (this.mSpawnTimer >= this.mSpawnInterval)
				{
					GameObject gameObject = this.mTrailObjectsNotInUse.Dequeue();
					if (gameObject != null)
					{
						gameObject.GetComponent<SpriteTrailObject>().Initiate(this.mTrailTime, this.mLeadingSprite.sprite, base.transform.position, this);
						this.mTrailObjectsInUse.Add(gameObject);
						this.mSpawnTimer = 0f;
					}
				}
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002167F File Offset: 0x0001F87F
		public void RemoveTrailObject(GameObject obj)
		{
			this.mTrailObjectsInUse.Remove(obj);
			this.mTrailObjectsNotInUse.Enqueue(obj);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002169A File Offset: 0x0001F89A
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.mbEnabled.Flip();
			}
			else
			{
				this.mbEnabled.UnFlip();
			}
			if (this.mbEnabled.value)
			{
				this.mSpawnTimer = this.mSpawnInterval;
			}
		}

		// Token: 0x040005A1 RID: 1441
		public SpriteRenderer mLeadingSprite;

		// Token: 0x040005A2 RID: 1442
		public int mTrailSegments;

		// Token: 0x040005A3 RID: 1443
		public float mTrailTime;

		// Token: 0x040005A4 RID: 1444
		public GameObject mTrailObject;

		// Token: 0x040005A5 RID: 1445
		private float mSpawnInterval;

		// Token: 0x040005A6 RID: 1446
		private float mSpawnTimer;

		// Token: 0x040005A7 RID: 1447
		private BoolToggle mbEnabled;

		// Token: 0x040005A8 RID: 1448
		private List<GameObject> mTrailObjectsInUse;

		// Token: 0x040005A9 RID: 1449
		private Queue<GameObject> mTrailObjectsNotInUse;
	}
}
