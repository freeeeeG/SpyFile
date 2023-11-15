using System;
using UnityEngine;

// Token: 0x020008F3 RID: 2291
public class ChefLerp : MonoBehaviour
{
	// Token: 0x06002CA3 RID: 11427 RVA: 0x000D2FC8 File Offset: 0x000D13C8
	public void Setup(Rigidbody rigidbody)
	{
		this.m_Rigidbody = rigidbody;
		this.m_Transform = base.transform;
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x000D2FE0 File Offset: 0x000D13E0
	public virtual void Update()
	{
		if (this.m_LerpTimer > 0f)
		{
			float t = this.m_LerpTimer / 0.3f;
			this.m_Transform.position = Vector3.Lerp(this.m_Rigidbody.position, this.m_LerpStart, t);
			this.m_LerpTimer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_LerpTimer <= 0f)
			{
				this.m_Transform.localPosition = Vector3.zero;
			}
		}
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x000D3064 File Offset: 0x000D1464
	public void StartLerp(Vector3 lerpStart)
	{
		this.m_Transform.position = lerpStart;
		this.m_LerpStart = lerpStart;
		this.m_LerpTimer = 0.3f;
	}

	// Token: 0x040023F1 RID: 9201
	private Transform m_Transform;

	// Token: 0x040023F2 RID: 9202
	private Rigidbody m_Rigidbody;

	// Token: 0x040023F3 RID: 9203
	private Vector3 m_LerpStart = Vector3.zero;

	// Token: 0x040023F4 RID: 9204
	private float m_LerpTimer;

	// Token: 0x040023F5 RID: 9205
	private const float kLerpLength = 0.3f;
}
