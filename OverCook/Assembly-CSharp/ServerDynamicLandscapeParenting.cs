using System;
using UnityEngine;

// Token: 0x020009DC RID: 2524
public class ServerDynamicLandscapeParenting : MonoBehaviour
{
	// Token: 0x06003155 RID: 12629 RVA: 0x000E753D File Offset: 0x000E593D
	private void Awake()
	{
		if (GameUtils.GetLevelConfig().m_disableDynamicParenting)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.m_dynamicLandscapeParenting = base.gameObject.RequireComponent<DynamicLandscapeParenting>();
		this.m_groundCast = base.gameObject.RequireComponent<GroundCast>();
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x000E7577 File Offset: 0x000E5977
	private void OnEnable()
	{
		if (this.m_groundCast != null)
		{
			this.m_groundCast.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
			this.OnGroundChanged(this.m_groundCast.GetGroundCollider());
		}
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x000E75B2 File Offset: 0x000E59B2
	private void OnDisable()
	{
		if (this.m_groundCast != null)
		{
			this.m_groundCast.UnregisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
		}
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x000E75DC File Offset: 0x000E59DC
	private void OnGroundChanged(Collider _collider)
	{
		this.m_dynamicLandscapeParenting.OnGroundChange(_collider);
	}

	// Token: 0x040027A0 RID: 10144
	private DynamicLandscapeParenting m_dynamicLandscapeParenting;

	// Token: 0x040027A1 RID: 10145
	private GroundCast m_groundCast;
}
