using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class UI_Func_FollowUITarget : MonoBehaviour
{
	// Token: 0x06000813 RID: 2067 RVA: 0x0001ECEE File Offset: 0x0001CEEE
	private void Start()
	{
		this.lastPosition = base.transform.position;
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0001ED04 File Offset: 0x0001CF04
	private void LateUpdate()
	{
		if (!this.doFollow)
		{
			return;
		}
		base.transform.position = Vector3.Lerp(this.lastPosition, this.target.position, Time.deltaTime * this.lerpSpeed);
		this.lastPosition = base.transform.position;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0001ED58 File Offset: 0x0001CF58
	public void PreservePosition()
	{
		base.transform.position = this.lastPosition;
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0001ED6B File Offset: 0x0001CF6B
	public void ToggleFollowing(bool isOn)
	{
		this.doFollow = isOn;
		if (isOn)
		{
			this.lastPosition = base.transform.position;
		}
	}

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	private RectTransform target;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private float lerpSpeed = 3f;

	// Token: 0x04000690 RID: 1680
	private Vector3 lastPosition;

	// Token: 0x04000691 RID: 1681
	private float rotationMomentum;

	// Token: 0x04000692 RID: 1682
	private bool doFollow = true;
}
