using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class ClientWokEffectsCosmeticDecisions : ClientSynchroniserBase, IClientCookingRegionNotified
{
	// Token: 0x060012DC RID: 4828 RVA: 0x00069CC8 File Offset: 0x000680C8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_wokCosmetics = (WokEffectsCosmeticDecisions)synchronisedObject;
		this.m_emissionModules = new ParticleSystem.EmissionModule[this.m_wokCosmetics.m_particleSystems.Length];
		this.m_initialEmissionRoT = new float[this.m_wokCosmetics.m_particleSystems.Length];
		for (int i = 0; i < this.m_wokCosmetics.m_particleSystems.Length; i++)
		{
			ParticleSystem particleSystem = this.m_wokCosmetics.m_particleSystems[i];
			if (!(particleSystem == null))
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				this.m_initialEmissionRoT[i] = emission.rateOverTimeMultiplier;
				this.m_emissionModules[i] = emission;
			}
		}
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		if (this.m_wokCosmetics.m_flameRenderer != null)
		{
			this.m_flameMaterial = this.m_wokCosmetics.m_flameRenderer.material;
		}
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x00069DBC File Offset: 0x000681BC
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_emissionModules == null)
		{
			return;
		}
		switch (this.m_state)
		{
		case ClientWokEffectsCosmeticDecisions.EffectState.TransitionOff:
			if (this.m_wokCosmetics.m_transitionDuration > 0f)
			{
				this.m_progress = Mathf.Max(this.m_progress - TimeManager.GetDeltaTime(base.gameObject.layer) / this.m_wokCosmetics.m_transitionDuration, 0f);
			}
			else
			{
				this.m_progress = 0f;
			}
			if (this.m_progress <= 0.05f)
			{
				this.m_progress = 0f;
				this.m_state = ClientWokEffectsCosmeticDecisions.EffectState.Off;
			}
			this.SetEffectsProgress(this.m_progress);
			break;
		case ClientWokEffectsCosmeticDecisions.EffectState.TransitionOn:
			if (this.m_wokCosmetics.m_transitionDuration > 0f)
			{
				this.m_progress = Mathf.Min(this.m_progress + TimeManager.GetDeltaTime(base.gameObject.layer) / this.m_wokCosmetics.m_transitionDuration, 1f);
			}
			else
			{
				this.m_progress = 1f;
			}
			if (this.m_progress >= 0.95f)
			{
				this.m_progress = 1f;
				this.m_state = ClientWokEffectsCosmeticDecisions.EffectState.On;
			}
			this.SetEffectsProgress(this.m_progress);
			break;
		}
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x00069F20 File Offset: 0x00068320
	private void SetEffectsProgress(float _progress)
	{
		for (int i = 0; i < this.m_emissionModules.Length; i++)
		{
			ParticleSystem.EmissionModule emissionModule = this.m_emissionModules[i];
			emissionModule.rateOverTimeMultiplier = this.m_initialEmissionRoT[i] * this.m_wokCosmetics.m_emissionCurve.Evaluate(_progress);
		}
		if (this.m_flameMaterial != null)
		{
			this.m_flameMaterial.SetFloat(this.m_materialParamID, _progress);
		}
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00069F9C File Offset: 0x0006839C
	public void EnterCookingRegion()
	{
		if (this.m_state != ClientWokEffectsCosmeticDecisions.EffectState.On)
		{
			this.m_state = ClientWokEffectsCosmeticDecisions.EffectState.TransitionOn;
			GameUtils.StartAudio(GameLoopingAudioTag.DLC_04_Flames, this, base.gameObject.layer);
		}
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00069FC5 File Offset: 0x000683C5
	public void ExitCookingRegion()
	{
		if (this.m_state != ClientWokEffectsCosmeticDecisions.EffectState.Off)
		{
			this.m_state = ClientWokEffectsCosmeticDecisions.EffectState.TransitionOff;
			GameUtils.StopAudio(GameLoopingAudioTag.DLC_04_Flames, this);
		}
	}

	// Token: 0x04000EEA RID: 3818
	private WokEffectsCosmeticDecisions m_wokCosmetics;

	// Token: 0x04000EEB RID: 3819
	private ParticleSystem.EmissionModule[] m_emissionModules;

	// Token: 0x04000EEC RID: 3820
	private float[] m_initialEmissionRoT;

	// Token: 0x04000EED RID: 3821
	private GridManager m_gridManager;

	// Token: 0x04000EEE RID: 3822
	private const string c_materialParam = "_FlameOnOff";

	// Token: 0x04000EEF RID: 3823
	private int m_materialParamID = Shader.PropertyToID("_FlameOnOff");

	// Token: 0x04000EF0 RID: 3824
	private Material m_flameMaterial;

	// Token: 0x04000EF1 RID: 3825
	private ClientWokEffectsCosmeticDecisions.EffectState m_state = ClientWokEffectsCosmeticDecisions.EffectState.TransitionOff;

	// Token: 0x04000EF2 RID: 3826
	private float m_progress;

	// Token: 0x0200041A RID: 1050
	private enum EffectState
	{
		// Token: 0x04000EF4 RID: 3828
		Off,
		// Token: 0x04000EF5 RID: 3829
		TransitionOff,
		// Token: 0x04000EF6 RID: 3830
		TransitionOn,
		// Token: 0x04000EF7 RID: 3831
		On
	}
}
