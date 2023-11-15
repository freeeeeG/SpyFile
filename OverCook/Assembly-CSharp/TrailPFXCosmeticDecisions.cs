using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000407 RID: 1031
public class TrailPFXCosmeticDecisions : MonoBehaviour
{
	// Token: 0x060012B7 RID: 4791 RVA: 0x00068F38 File Offset: 0x00067338
	private void RecordInitParticle(ParticleSystem _system)
	{
		TrailPFXCosmeticDecisions.InitParticleData value = default(TrailPFXCosmeticDecisions.InitParticleData);
		value.m_scale = _system.transform.localScale;
		value.m_particleSize = _system.startSize;
		this.m_initParticleDataLookup.Add(_system, value);
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x00068F7C File Offset: 0x0006737C
	private void Start()
	{
		for (int i = 0; i < this.m_movingPFX.Length; i++)
		{
			this.RecordInitParticle(this.m_movingPFX[i]);
			this.m_movingPFX[i].Play();
		}
		for (int j = 0; j < this.m_staticPFX.Length; j++)
		{
			this.RecordInitParticle(this.m_staticPFX[j]);
			this.m_staticPFX[j].Play();
		}
		if (base.transform.parent != null)
		{
			this.m_rigidBody = base.transform.parent.GetComponent<Rigidbody>();
			this.m_prevPosition = base.transform.parent.position;
		}
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x00069034 File Offset: 0x00067434
	private void LateUpdate()
	{
		if (this.m_rigidBody != null)
		{
			Vector3 position = base.transform.parent.position;
			Vector3 vector = position - this.m_prevPosition;
			this.m_prevPosition = position;
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			float num = Mathf.Clamp01(vector.magnitude / (0.5f * Mathf.Max(deltaTime, 0.0001f)));
			float num2 = Mathf.Sin(1.5707964f * num);
			float num3 = Mathf.Cos(1.5707964f * num);
			for (int i = 0; i < this.m_movingPFX.Length; i++)
			{
				TrailPFXCosmeticDecisions.InitParticleData initParticleData = this.m_initParticleDataLookup[this.m_movingPFX[i]];
				this.m_movingPFX[i].startSize = num2 * initParticleData.m_particleSize;
			}
			for (int j = 0; j < this.m_staticPFX.Length; j++)
			{
				TrailPFXCosmeticDecisions.InitParticleData initParticleData2 = this.m_initParticleDataLookup[this.m_staticPFX[j]];
				this.m_staticPFX[j].startSize = num3 * initParticleData2.m_particleSize;
			}
		}
	}

	// Token: 0x04000EB1 RID: 3761
	[SerializeField]
	private ParticleSystem[] m_movingPFX = new ParticleSystem[0];

	// Token: 0x04000EB2 RID: 3762
	[SerializeField]
	private ParticleSystem[] m_staticPFX = new ParticleSystem[0];

	// Token: 0x04000EB3 RID: 3763
	private Dictionary<ParticleSystem, TrailPFXCosmeticDecisions.InitParticleData> m_initParticleDataLookup = new Dictionary<ParticleSystem, TrailPFXCosmeticDecisions.InitParticleData>();

	// Token: 0x04000EB4 RID: 3764
	private Rigidbody m_rigidBody;

	// Token: 0x04000EB5 RID: 3765
	private Vector3 m_prevPosition;

	// Token: 0x02000408 RID: 1032
	private struct InitParticleData
	{
		// Token: 0x04000EB6 RID: 3766
		public Vector3 m_scale;

		// Token: 0x04000EB7 RID: 3767
		public float m_particleSize;
	}
}
