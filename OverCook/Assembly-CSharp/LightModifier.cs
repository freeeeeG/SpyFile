using System;
using UnityEngine;

// Token: 0x020003CD RID: 973
[RequireComponent(typeof(Light))]
public abstract class LightModifier : MonoBehaviour
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x000653AC File Offset: 0x000637AC
	protected Light BaseLight
	{
		get
		{
			return this.m_light;
		}
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x000653B4 File Offset: 0x000637B4
	protected virtual void Awake()
	{
		this.m_allmodifiers = base.gameObject.GetComponents<LightModifier>();
		Array.Sort<LightModifier>(this.m_allmodifiers, (LightModifier x, LightModifier y) => x.m_applicationOrder.CompareTo(y.m_applicationOrder));
		this.m_light = base.gameObject.RequireComponent<Light>();
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x0006540B File Offset: 0x0006380B
	protected void Start()
	{
		this.Update();
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00065414 File Offset: 0x00063814
	protected void Update()
	{
		if (this.m_allmodifiers[0] == this)
		{
			for (int i = 0; i < this.m_allmodifiers.Length; i++)
			{
				this.m_allmodifiers[i].ModifyLight(this.m_light);
			}
		}
	}

	// Token: 0x06001208 RID: 4616
	protected abstract void ModifyLight(Light _light);

	// Token: 0x04000E0F RID: 3599
	[SerializeField]
	private int m_applicationOrder;

	// Token: 0x04000E10 RID: 3600
	private LightModifier[] m_allmodifiers;

	// Token: 0x04000E11 RID: 3601
	private Light m_light;
}
