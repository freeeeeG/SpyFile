using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x02001658 RID: 5720
	public class DemoFollowScript : MonoBehaviour
	{
		// Token: 0x06006D01 RID: 27905 RVA: 0x00137980 File Offset: 0x00135B80
		private void Start()
		{
			this.isHomeAndShouldDeactivate = false;
			this.movingToDeactivationTarget = false;
			this.acquiredTargets = new List<Transform>();
			if (this.target == null)
			{
				Debug.Log("No target found for the FollowScript on: " + base.gameObject.name);
			}
		}

		// Token: 0x06006D02 RID: 27906 RVA: 0x00002191 File Offset: 0x00000391
		private void OnEnable()
		{
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x001379D0 File Offset: 0x00135BD0
		private void Update()
		{
			if (this.shouldFollow && this.target != null)
			{
				this.newPosition = Vector2.Lerp(base.transform.position, this.target.position, Time.deltaTime * this.speed);
				base.transform.position = new Vector3(this.newPosition.x, this.newPosition.y, base.transform.position.z);
			}
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x00002191 File Offset: 0x00000391
		private void OnDisable()
		{
		}

		// Token: 0x040058C6 RID: 22726
		public Transform target;

		// Token: 0x040058C7 RID: 22727
		public float speed;

		// Token: 0x040058C8 RID: 22728
		public bool shouldFollow;

		// Token: 0x040058C9 RID: 22729
		public bool isHomeAndShouldDeactivate;

		// Token: 0x040058CA RID: 22730
		public bool movingToDeactivationTarget;

		// Token: 0x040058CB RID: 22731
		private Vector3 newPosition;

		// Token: 0x040058CC RID: 22732
		public List<Transform> acquiredTargets;
	}
}
