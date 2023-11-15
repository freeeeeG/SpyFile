using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009B RID: 155
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_dynamic_grid_obstacle.php")]
	public class DynamicGridObstacle : GraphModifier
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0002B700 File Offset: 0x00029900
		private Bounds bounds
		{
			get
			{
				if (this.coll != null)
				{
					return this.coll.bounds;
				}
				Bounds bounds = this.coll2D.bounds;
				bounds.extents += new Vector3(0f, 0f, 10000f);
				return bounds;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0002B75A File Offset: 0x0002995A
		private bool colliderEnabled
		{
			get
			{
				if (!(this.coll != null))
				{
					return this.coll2D.enabled;
				}
				return this.coll.enabled;
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002B784 File Offset: 0x00029984
		protected override void Awake()
		{
			base.Awake();
			this.coll = base.GetComponent<Collider>();
			this.coll2D = base.GetComponent<Collider2D>();
			this.tr = base.transform;
			if (this.coll == null && this.coll2D == null && Application.isPlaying)
			{
				throw new Exception("A collider or 2D collider must be attached to the GameObject(" + base.gameObject.name + ") for the DynamicGridObstacle to work");
			}
			this.prevBounds = this.bounds;
			this.prevRotation = this.tr.rotation;
			this.prevEnabled = false;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002B822 File Offset: 0x00029A22
		public override void OnPostScan()
		{
			if (this.coll == null)
			{
				this.Awake();
			}
			if (this.coll != null)
			{
				this.prevEnabled = this.colliderEnabled;
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002B854 File Offset: 0x00029A54
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.coll == null && this.coll2D == null)
			{
				Debug.LogError("Removed collider from DynamicGridObstacle", this);
				base.enabled = false;
				return;
			}
			while (this.pendingGraphUpdates.Count > 0 && this.pendingGraphUpdates.Peek().stage != GraphUpdateStage.Pending)
			{
				this.pendingGraphUpdates.Dequeue();
			}
			if (AstarPath.active == null || AstarPath.active.isScanning || Time.realtimeSinceStartup - this.lastCheckTime < this.checkTime || !Application.isPlaying || this.pendingGraphUpdates.Count > 0)
			{
				return;
			}
			this.lastCheckTime = Time.realtimeSinceStartup;
			if (this.colliderEnabled)
			{
				Bounds bounds = this.bounds;
				Quaternion rotation = this.tr.rotation;
				Vector3 vector = this.prevBounds.min - bounds.min;
				Vector3 vector2 = this.prevBounds.max - bounds.max;
				float num = bounds.extents.magnitude * Quaternion.Angle(this.prevRotation, rotation) * 0.017453292f;
				if (vector.sqrMagnitude > this.updateError * this.updateError || vector2.sqrMagnitude > this.updateError * this.updateError || num > this.updateError || !this.prevEnabled)
				{
					this.DoUpdateGraphs();
					return;
				}
			}
			else if (this.prevEnabled)
			{
				this.DoUpdateGraphs();
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002B9DC File Offset: 0x00029BDC
		protected override void OnDisable()
		{
			base.OnDisable();
			if (AstarPath.active != null && Application.isPlaying)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
				this.prevEnabled = false;
			}
			this.pendingGraphUpdates.Clear();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002BA38 File Offset: 0x00029C38
		public void DoUpdateGraphs()
		{
			if (this.coll == null && this.coll2D == null)
			{
				return;
			}
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
			if (!this.colliderEnabled)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			else
			{
				Bounds bounds = this.bounds;
				Bounds b = bounds;
				b.Encapsulate(this.prevBounds);
				if (DynamicGridObstacle.BoundsVolume(b) < DynamicGridObstacle.BoundsVolume(bounds) + DynamicGridObstacle.BoundsVolume(this.prevBounds))
				{
					GraphUpdateObject graphUpdateObject2 = new GraphUpdateObject(b);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject2);
					AstarPath.active.UpdateGraphs(graphUpdateObject2);
				}
				else
				{
					GraphUpdateObject graphUpdateObject3 = new GraphUpdateObject(this.prevBounds);
					GraphUpdateObject graphUpdateObject4 = new GraphUpdateObject(bounds);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject3);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject4);
					AstarPath.active.UpdateGraphs(graphUpdateObject3);
					AstarPath.active.UpdateGraphs(graphUpdateObject4);
				}
				this.prevBounds = bounds;
			}
			this.prevEnabled = this.colliderEnabled;
			this.prevRotation = this.tr.rotation;
			this.lastCheckTime = Time.realtimeSinceStartup;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002BB62 File Offset: 0x00029D62
		private static float BoundsVolume(Bounds b)
		{
			return Math.Abs(b.size.x * b.size.y * b.size.z);
		}

		// Token: 0x0400041F RID: 1055
		private Collider coll;

		// Token: 0x04000420 RID: 1056
		private Collider2D coll2D;

		// Token: 0x04000421 RID: 1057
		private Transform tr;

		// Token: 0x04000422 RID: 1058
		public float updateError = 1f;

		// Token: 0x04000423 RID: 1059
		public float checkTime = 0.2f;

		// Token: 0x04000424 RID: 1060
		private Bounds prevBounds;

		// Token: 0x04000425 RID: 1061
		private Quaternion prevRotation;

		// Token: 0x04000426 RID: 1062
		private bool prevEnabled;

		// Token: 0x04000427 RID: 1063
		private float lastCheckTime = -9999f;

		// Token: 0x04000428 RID: 1064
		private Queue<GraphUpdateObject> pendingGraphUpdates = new Queue<GraphUpdateObject>();
	}
}
