using System;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class FollowPilotDirection : MonoBehaviour
{
	// Token: 0x06001B62 RID: 7010 RVA: 0x00087A7C File Offset: 0x00085E7C
	private void Start()
	{
		this.m_defaultRotations = new Quaternion[this.m_followingTransforms.Length];
		for (int i = 0; i < this.m_defaultRotations.Length; i++)
		{
			this.m_defaultRotations[i] = this.m_followingTransforms[i].rotation;
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x00087AD4 File Offset: 0x00085ED4
	private void Update()
	{
		Vector3 forward = this.m_target.transform.position - this.m_lastPosition;
		bool flag = forward.sqrMagnitude > this.m_threshold;
		for (int i = 0; i < this.m_followingTransforms.Length; i++)
		{
			if (flag)
			{
				this.m_followingTransforms[i].rotation = Quaternion.RotateTowards(this.m_followingTransforms[i].rotation, Quaternion.LookRotation(forward, Vector3.up), 10f);
			}
			else
			{
				this.m_followingTransforms[i].rotation = Quaternion.RotateTowards(this.m_followingTransforms[i].rotation, this.m_defaultRotations[i], 10f);
			}
		}
		this.m_lastPosition = this.m_target.transform.position;
	}

	// Token: 0x04001577 RID: 5495
	[SerializeField]
	private PilotMovement m_target;

	// Token: 0x04001578 RID: 5496
	[SerializeField]
	private Transform[] m_followingTransforms;

	// Token: 0x04001579 RID: 5497
	private float m_threshold = 0.001f;

	// Token: 0x0400157A RID: 5498
	private Vector3 m_lastPosition = Vector3.zero;

	// Token: 0x0400157B RID: 5499
	private Quaternion[] m_defaultRotations;
}
