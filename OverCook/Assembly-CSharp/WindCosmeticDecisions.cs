using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000410 RID: 1040
[RequireComponent(typeof(WindVolume))]
public class WindCosmeticDecisions : MonoBehaviour
{
	// Token: 0x060012C9 RID: 4809 RVA: 0x000694D4 File Offset: 0x000678D4
	private void Awake()
	{
		this.m_volume = base.gameObject.RequireComponent<WindVolume>();
		this.m_particleSystems = base.gameObject.RequestComponentsRecursive<ParticleSystem>();
		this.m_meshRenderers = base.gameObject.RequestComponentsRecursive<MeshRenderer>();
		this.m_materialPropertyID = Shader.PropertyToID(this.m_materialProperty);
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x00069528 File Offset: 0x00067928
	private void OnEnable()
	{
		this.m_showing = false;
		for (int i = 0; i < this.m_particleSystems.Length; i++)
		{
			ParticleSystem particleSystem = this.m_particleSystems[i];
			particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		this.SetParticlesEnabled(false);
		for (int j = 0; j < this.m_particleSystems.Length; j++)
		{
			ParticleSystem particleSystem2 = this.m_particleSystems[j];
			particleSystem2.Play(true);
		}
		this.m_materialValue = this.m_lerp.MinValue;
		this.ApplyMaterialFade(this.m_materialValue);
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x000695B4 File Offset: 0x000679B4
	private void Update()
	{
		bool flag = this.m_volume.enabled && this.m_volume.GetVelocity().magnitude > this.m_windSpeedMin;
		if (flag)
		{
			if (!this.m_showing)
			{
				this.m_showing = true;
				this.SetParticlesEnabled(true);
				this.m_lerpRoutine = this.LerpTowardsValue(this.m_materialValue, this.m_lerp.MaxValue, this.m_lerp);
			}
		}
		else if (this.m_showing)
		{
			this.m_showing = false;
			this.SetParticlesEnabled(false);
			this.m_lerpRoutine = this.LerpTowardsValue(this.m_materialValue, this.m_lerp.MinValue, this.m_lerp);
		}
		if (this.m_lerpRoutine != null)
		{
			if (this.m_lerpRoutine.MoveNext())
			{
				this.m_materialValue = (float)this.m_lerpRoutine.Current;
				this.ApplyMaterialFade(this.m_materialValue);
			}
			else
			{
				this.m_lerpRoutine = null;
			}
		}
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x000696BC File Offset: 0x00067ABC
	private IEnumerator LerpTowardsValue(float _value, float _targetValue, WindCosmeticDecisions.MaterialLerp _lerp)
	{
		float startValue = _value;
		float remaining = Mathf.Abs(_targetValue - _value) / Mathf.Abs(_lerp.MaxValue - _lerp.MinValue);
		float speed = 1f / _lerp.LerpTime;
		while (remaining > 0f)
		{
			remaining -= speed * TimeManager.GetDeltaTime(base.gameObject.layer);
			_value = Mathf.Lerp(startValue, _targetValue, 1f - remaining);
			yield return _value;
		}
		yield break;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x000696EC File Offset: 0x00067AEC
	private void SetParticlesEnabled(bool _enabled)
	{
		for (int i = 0; i < this.m_particleSystems.Length; i++)
		{
			ParticleSystem particleSystem = this.m_particleSystems[i];
			particleSystem.emission.enabled = _enabled;
		}
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0006972C File Offset: 0x00067B2C
	private void ApplyMaterialFade(float _value)
	{
		if (this.m_materialPropertyBlock == null)
		{
			this.m_materialPropertyBlock = new MaterialPropertyBlock();
		}
		this.m_materialPropertyBlock.SetFloat(this.m_materialPropertyID, _value);
		for (int i = 0; i < this.m_meshRenderers.Length; i++)
		{
			this.m_meshRenderers[i].SetPropertyBlock(this.m_materialPropertyBlock);
		}
	}

	// Token: 0x04000EC7 RID: 3783
	[SerializeField]
	private float m_windSpeedMin;

	// Token: 0x04000EC8 RID: 3784
	[SerializeField]
	private string m_materialProperty = string.Empty;

	// Token: 0x04000EC9 RID: 3785
	[SerializeField]
	private WindCosmeticDecisions.MaterialLerp m_lerp = new WindCosmeticDecisions.MaterialLerp();

	// Token: 0x04000ECA RID: 3786
	private ParticleSystem[] m_particleSystems;

	// Token: 0x04000ECB RID: 3787
	private MeshRenderer[] m_meshRenderers;

	// Token: 0x04000ECC RID: 3788
	private WindVolume m_volume;

	// Token: 0x04000ECD RID: 3789
	private MaterialPropertyBlock m_materialPropertyBlock;

	// Token: 0x04000ECE RID: 3790
	private int m_materialPropertyID = -1;

	// Token: 0x04000ECF RID: 3791
	private float m_materialValue;

	// Token: 0x04000ED0 RID: 3792
	private IEnumerator m_lerpRoutine;

	// Token: 0x04000ED1 RID: 3793
	private bool m_showing;

	// Token: 0x02000411 RID: 1041
	[Serializable]
	private class MaterialLerp
	{
		// Token: 0x04000ED2 RID: 3794
		public float MinValue;

		// Token: 0x04000ED3 RID: 3795
		public float MaxValue;

		// Token: 0x04000ED4 RID: 3796
		public float LerpTime;
	}
}
