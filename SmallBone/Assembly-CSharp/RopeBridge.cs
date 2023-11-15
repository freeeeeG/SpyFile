using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class RopeBridge : MonoBehaviour
{
	// Token: 0x06000224 RID: 548 RVA: 0x000092E8 File Offset: 0x000074E8
	public bool FindAndMove()
	{
		RopeBridge.nonAllocOverlapper.contactFilter.SetLayerMask(this._layers.Evaluate(base.gameObject));
		return RopeBridge.nonAllocOverlapper.OverlapCollider(this._findRange).results.Count != 0;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00009334 File Offset: 0x00007534
	private void Start()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		Vector3 position = this.StartPoint.position;
		for (int i = 0; i < this.segmentLength; i++)
		{
			this.ropeSegments.Add(new RopeBridge.RopeSegment(position));
			position.y -= this.ropeSegLen;
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x00009391 File Offset: 0x00007591
	private void Update()
	{
		this.DrawRope();
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00009399 File Offset: 0x00007599
	private void FixedUpdate()
	{
		this.Simulate();
	}

	// Token: 0x06000228 RID: 552 RVA: 0x000093A4 File Offset: 0x000075A4
	private void Simulate()
	{
		Vector2 a = new Vector2(0f, -1f);
		for (int i = 1; i < this.segmentLength; i++)
		{
			RopeBridge.RopeSegment ropeSegment = this.ropeSegments[i];
			Vector2 b = ropeSegment.posNow - ropeSegment.posOld;
			ropeSegment.posOld = ropeSegment.posNow;
			ropeSegment.posNow += b;
			ropeSegment.posNow += a * Time.fixedDeltaTime;
			this.ropeSegments[i] = ropeSegment;
		}
		for (int j = 0; j < 50; j++)
		{
			this.ApplyConstraint();
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x00009460 File Offset: 0x00007660
	private void ApplyConstraint()
	{
		RopeBridge.RopeSegment value = this.ropeSegments[0];
		value.posNow = this.StartPoint.position;
		this.ropeSegments[0] = value;
		RopeBridge.RopeSegment value2 = this.ropeSegments[this.ropeSegments.Count - 1];
		value2.posNow = this.EndPoint.position;
		this.ropeSegments[this.ropeSegments.Count - 1] = value2;
		for (int i = 0; i < this.segmentLength - 1; i++)
		{
			RopeBridge.RopeSegment ropeSegment = this.ropeSegments[i];
			RopeBridge.RopeSegment ropeSegment2 = this.ropeSegments[i + 1];
			float magnitude = (ropeSegment.posNow - ropeSegment2.posNow).magnitude;
			float d = Mathf.Abs(magnitude - this.ropeSegLen);
			Vector2 a = Vector2.zero;
			if (magnitude > this.ropeSegLen)
			{
				a = (ropeSegment.posNow - ropeSegment2.posNow).normalized;
			}
			else if (magnitude < this.ropeSegLen)
			{
				a = (ropeSegment2.posNow - ropeSegment.posNow).normalized;
			}
			Vector2 vector = a * d;
			if (i != 0)
			{
				ropeSegment.posNow -= vector * 0.5f;
				this.ropeSegments[i] = ropeSegment;
				ropeSegment2.posNow += vector * 0.5f;
				this.ropeSegments[i + 1] = ropeSegment2;
			}
			else
			{
				ropeSegment2.posNow += vector;
				this.ropeSegments[i + 1] = ropeSegment2;
			}
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00009640 File Offset: 0x00007840
	private void DrawRope()
	{
		float num = this.lineWidth;
		this.lineRenderer.startWidth = num;
		this.lineRenderer.endWidth = num;
		Vector3[] array = new Vector3[this.segmentLength];
		for (int i = 0; i < this.segmentLength; i++)
		{
			array[i] = this.ropeSegments[i].posNow;
		}
		this.lineRenderer.positionCount = array.Length;
		this.lineRenderer.SetPositions(array);
	}

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private TargetLayer _layers;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	private Collider2D _findRange;

	// Token: 0x040001D8 RID: 472
	public Transform StartPoint;

	// Token: 0x040001D9 RID: 473
	public Transform EndPoint;

	// Token: 0x040001DA RID: 474
	private LineRenderer lineRenderer;

	// Token: 0x040001DB RID: 475
	private List<RopeBridge.RopeSegment> ropeSegments = new List<RopeBridge.RopeSegment>();

	// Token: 0x040001DC RID: 476
	private float ropeSegLen = 0.25f;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private int segmentLength = 35;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	private float pointMoveSpeed = 3f;

	// Token: 0x040001DF RID: 479
	private float lineWidth = 0.1f;

	// Token: 0x040001E0 RID: 480
	private static NonAllocOverlapper nonAllocOverlapper = new NonAllocOverlapper(32);

	// Token: 0x02000078 RID: 120
	public struct RopeSegment
	{
		// Token: 0x0600022C RID: 556 RVA: 0x000096FC File Offset: 0x000078FC
		public RopeSegment(Vector2 pos)
		{
			this.posNow = pos;
			this.posOld = pos;
		}

		// Token: 0x040001E1 RID: 481
		public Vector2 posNow;

		// Token: 0x040001E2 RID: 482
		public Vector2 posOld;
	}
}
