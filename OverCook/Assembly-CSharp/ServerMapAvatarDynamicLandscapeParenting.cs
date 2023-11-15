using System;
using UnityEngine;

// Token: 0x020009F9 RID: 2553
public class ServerMapAvatarDynamicLandscapeParenting : MonoBehaviour
{
	// Token: 0x060031EB RID: 12779 RVA: 0x000EA2C5 File Offset: 0x000E86C5
	private void Awake()
	{
		this.m_dynamicLandscapeParenting = base.gameObject.RequireComponent<MapAvatarDynamicLandscapeParenting>();
		this.m_groundCast = base.gameObject.RequireComponent<MapAvatarGroundCast>();
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x000EA2E9 File Offset: 0x000E86E9
	private void OnEnable()
	{
		if (this.m_groundCast != null)
		{
			this.m_groundCast.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
			this.OnGroundChanged(this.m_groundCast.GetGroundCollider());
		}
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x000EA324 File Offset: 0x000E8724
	private void OnDisable()
	{
		if (this.m_groundCast != null)
		{
			this.m_groundCast.UnregisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x000EA34E File Offset: 0x000E874E
	private void OnGroundChanged(Collider _collider)
	{
		this.m_dynamicLandscapeParenting.OnGroundChange(_collider);
	}

	// Token: 0x0400281D RID: 10269
	private MapAvatarDynamicLandscapeParenting m_dynamicLandscapeParenting;

	// Token: 0x0400281E RID: 10270
	private MapAvatarGroundCast m_groundCast;
}
