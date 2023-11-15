using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class Cannon : MonoBehaviour
{
	// Token: 0x0600142D RID: 5165 RVA: 0x0006E3C8 File Offset: 0x0006C7C8
	private void Awake()
	{
		this.m_target = base.transform.FindChildRecursive("Target");
		this.m_attachPoint = base.transform.FindChildRecursive("AttachPoint");
		this.m_exitPoint = base.transform.FindChildRecursive("ExitPoint");
	}

	// Token: 0x04000F9C RID: 3996
	[SerializeField]
	public ProjectileAnimation m_animation;

	// Token: 0x04000F9D RID: 3997
	[SerializeField]
	public string m_launchTrigger;

	// Token: 0x04000F9E RID: 3998
	[SerializeField]
	public string m_enableTrigger;

	// Token: 0x04000F9F RID: 3999
	[SerializeField]
	public string m_disableTrigger;

	// Token: 0x04000FA0 RID: 4000
	[SerializeField]
	public GameObject m_button;

	// Token: 0x04000FA1 RID: 4001
	public GenericVoid<GameObject> EndCannonRoutine = delegate(GameObject A_0)
	{
	};

	// Token: 0x04000FA2 RID: 4002
	[HideInInspector]
	public Transform m_target;

	// Token: 0x04000FA3 RID: 4003
	[HideInInspector]
	public Transform m_attachPoint;

	// Token: 0x04000FA4 RID: 4004
	[HideInInspector]
	public Transform m_exitPoint;
}
