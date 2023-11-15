using System;
using UnityEngine;

// Token: 0x02000BDB RID: 3035
[RequireComponent(typeof(Collider))]
public class WalkingBread : MonoBehaviour
{
	// Token: 0x06003DF1 RID: 15857 RVA: 0x00127A19 File Offset: 0x00125E19
	protected virtual void Awake()
	{
		this.m_triggerHash = Animator.StringToHash(this.m_trigger);
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x00127A2C File Offset: 0x00125E2C
	private void OnTriggerEnter(Collider other)
	{
		if (this.m_targetAnimator != null)
		{
			this.m_targetAnimator.SetTrigger(this.m_triggerHash);
		}
		if (this.m_fallRotation != WalkingBread.FallRotation.None)
		{
			Vector3 vector = Vector3.Normalize((other.transform.position - base.transform.position).WithY(0f));
			if (this.m_fallRotation == WalkingBread.FallRotation.Towards)
			{
				vector *= -1f;
			}
			Quaternion rhs = Quaternion.FromToRotation(base.transform.forward, vector);
			base.transform.rotation *= rhs;
		}
	}

	// Token: 0x040031B7 RID: 12727
	[SerializeField]
	public string m_trigger = string.Empty;

	// Token: 0x040031B8 RID: 12728
	[SerializeField]
	public Animator m_targetAnimator;

	// Token: 0x040031B9 RID: 12729
	private int m_triggerHash;

	// Token: 0x040031BA RID: 12730
	[SerializeField]
	public WalkingBread.FallRotation m_fallRotation;

	// Token: 0x02000BDC RID: 3036
	public enum FallRotation
	{
		// Token: 0x040031BC RID: 12732
		Towards,
		// Token: 0x040031BD RID: 12733
		Away,
		// Token: 0x040031BE RID: 12734
		None
	}
}
