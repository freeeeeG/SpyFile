using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class DeformingFoliage : MonoBehaviour
{
	// Token: 0x06002580 RID: 9600 RVA: 0x000B167A File Offset: 0x000AFA7A
	public void AddDeformer(Transform _deformer)
	{
		if (this.m_deforemer != _deformer && this.CalculatePushMagnitude(_deformer.position) > 0f)
		{
			this.m_deforemer = _deformer;
		}
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000B16AC File Offset: 0x000AFAAC
	private void Awake()
	{
		Renderer[] array = base.gameObject.RequestComponentsRecursive<Renderer>();
		for (int i = 0; i < array.Length; i++)
		{
			this.m_materials.AddRange(array[i].materials);
		}
		this.m_materials.RemoveAll((Material x) => x == null);
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000B1718 File Offset: 0x000AFB18
	private void Update()
	{
		if (this.m_wobbleTimer > this.WobbleTime && this.m_deforemer == null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (TimeManager.IsPaused(TimeManager.PauseLayer.Main))
		{
			return;
		}
		bool flag = false;
		Vector4 vector = Vector3.zero;
		if (this.m_deforemer != null)
		{
			float num = this.CalculatePushMagnitude(this.m_deforemer.position);
			Vector4 vector2 = base.transform.position - this.m_deforemer.position;
			vector2.y = 0f;
			Vector4 b = num * vector2.normalized;
			vector += b;
			if (num == 0f)
			{
				flag = true;
			}
		}
		this.m_push = Vector4.zero;
		if (this.m_deforemer != null)
		{
			this.m_push = vector;
		}
		if (flag)
		{
			this.m_wobbleAmount = UnityEngine.Random.Range(this.MinWobbleAmount, this.MaxWobbleAmount);
			this.m_randomXTiming = UnityEngine.Random.Range(300f, 400f);
			this.m_randomZTiming = UnityEngine.Random.Range(300f, 400f);
			this.m_deforemer = null;
		}
		Vector3 v = base.transform.InverseTransformDirection(this.m_push);
		this.m_wobbleTimer += Time.deltaTime;
		float value = this.m_wobbleAmount * MathUtils.ClampedRemap(this.m_wobbleTimer, 0f, this.WobbleTime, 1f, 0f);
		for (int i = 0; i < this.m_materials.Count; i++)
		{
			this.m_materials[i].SetVector("_PushVector", v);
			this.m_materials[i].SetVector("_Pivot", base.transform.position);
			this.m_materials[i].SetFloat("_WobbleAmount", value);
			this.m_materials[i].SetFloat("_RandomXTiming", this.m_randomXTiming);
			this.m_materials[i].SetFloat("_RandomZTiming", this.m_randomZTiming);
		}
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000B1958 File Offset: 0x000AFD58
	private float CalculatePushMagnitude(Vector3 _deformerPos)
	{
		Vector4 vector = base.GetComponent<Collider>().ClosestPointOnBounds(_deformerPos) - _deformerPos;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return MathUtils.ClampedRemap(magnitude, 1f, 0.5f, 0f, 0.5f);
	}

	// Token: 0x04001D0D RID: 7437
	public float WobbleTime = 1f;

	// Token: 0x04001D0E RID: 7438
	public float MaxWobbleAmount = 0.3f;

	// Token: 0x04001D0F RID: 7439
	public float MinWobbleAmount = 0.1f;

	// Token: 0x04001D10 RID: 7440
	private List<Material> m_materials = new List<Material>();

	// Token: 0x04001D11 RID: 7441
	private Transform m_deforemer;

	// Token: 0x04001D12 RID: 7442
	private Vector4 m_push = new Vector4(0f, 1f, 0f, 0f);

	// Token: 0x04001D13 RID: 7443
	private float m_wobbleAmount;

	// Token: 0x04001D14 RID: 7444
	private float m_randomXTiming;

	// Token: 0x04001D15 RID: 7445
	private float m_randomZTiming;

	// Token: 0x04001D16 RID: 7446
	private float m_wobbleTimer;

	// Token: 0x04001D17 RID: 7447
	private const float c_minWobbleTiming = 300f;

	// Token: 0x04001D18 RID: 7448
	private const float c_maxWobbleTiming = 400f;
}
