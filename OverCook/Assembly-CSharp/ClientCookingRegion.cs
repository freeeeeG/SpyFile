using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class ClientCookingRegion : ClientSynchroniserBase
{
	// Token: 0x060014B8 RID: 5304 RVA: 0x00070E19 File Offset: 0x0006F219
	public override EntityType GetEntityType()
	{
		return EntityType.CookingRegion;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x00070E20 File Offset: 0x0006F220
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_CookingRegion = (CookingRegion)synchronisedObject;
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_gridIndex = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		this.m_recorder = base.gameObject.RequireComponent<TriggerRecorder>();
		this.m_wasBaseEnabled = this.m_CookingRegion.enabled;
		this.OnEnableChanged(this.m_wasBaseEnabled);
		if (this.m_CookingRegion.m_burnerRenderer != null)
		{
			this.m_burnerMaterials = this.m_CookingRegion.m_burnerRenderer.materials;
			this.m_burnerMaterials.AllRemoved_Predicate((Material x) => x == null || !x.HasProperty(this.m_burnerMatEmissParamID));
		}
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x00070EDF File Offset: 0x0006F2DF
	protected void Awake()
	{
		if (ClientCookingRegion.s_sharedInfo == null)
		{
			ClientCookingRegion.s_sharedInfo = new ClientCookingRegion.SharedInfo();
		}
		ClientCookingRegion.s_sharedInfo.m_allRegions.Add(this);
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x00070F08 File Offset: 0x0006F308
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		bool enabled = this.m_CookingRegion.enabled;
		if (enabled != this.m_wasBaseEnabled)
		{
			this.OnEnableChanged(enabled);
			this.m_wasBaseEnabled = enabled;
		}
		if (this.m_transitionRoutine != null && !this.m_transitionRoutine.MoveNext())
		{
			this.m_transitionRoutine = null;
		}
		if (this.m_CookingRegion != null)
		{
			bool flag = this.AnyPFXPlaying();
			if (this.m_CookingRegion.m_TriggerArea != null && this.m_CookingRegion.enabled != this.m_CookingRegion.m_TriggerArea.enabled)
			{
				this.m_CookingRegion.m_TriggerArea.enabled = (this.m_CookingRegion.enabled || flag);
			}
			if (this.m_CookingRegion.enabled || flag)
			{
				List<Collider> recentCollisions = this.m_recorder.GetRecentCollisions();
				for (int i = 0; i < recentCollisions.Count; i++)
				{
					Collider collider = recentCollisions[i];
					IClientCookable clientCookable = collider.gameObject.RequestInterface<IClientCookable>();
					if (clientCookable != null && clientCookable.GetRequiredStationType() == this.m_CookingRegion.m_StationType)
					{
						this.UpdateFlameParticles(collider);
						break;
					}
				}
				GameObject gameObject = this.m_gridManager.GetGridOccupant(this.m_gridIndex);
				if (gameObject != null)
				{
					GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(gameObject.transform.position);
					if (gridLocationFromPos != this.m_gridIndex)
					{
						gameObject = null;
					}
				}
				if (gameObject != null && gameObject != this.m_prevOccupant)
				{
					IClientCookingRegionNotified[] array = gameObject.RequestInterfacesRecursive<IClientCookingRegionNotified>();
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] as MonoBehaviour != null)
						{
							array[j].EnterCookingRegion();
							this.m_prevOccupant = gameObject;
						}
					}
				}
				else if (gameObject == null && this.m_prevOccupant != null)
				{
					IClientCookingRegionNotified[] array2 = this.m_prevOccupant.RequestInterfacesRecursive<IClientCookingRegionNotified>();
					for (int k = 0; k < array2.Length; k++)
					{
						if (array2[k] as MonoBehaviour != null)
						{
							array2[k].ExitCookingRegion();
							this.m_prevOccupant = null;
						}
					}
				}
			}
			else if (this.m_prevOccupant != null)
			{
				IClientCookingRegionNotified[] array3 = this.m_prevOccupant.RequestInterfacesRecursive<IClientCookingRegionNotified>();
				for (int l = 0; l < array3.Length; l++)
				{
					if (array3[l] as MonoBehaviour != null)
					{
						array3[l].ExitCookingRegion();
						this.m_prevOccupant = null;
					}
				}
			}
		}
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x000711DC File Offset: 0x0006F5DC
	private bool AnyPFXPlaying()
	{
		for (int i = 0; i < this.m_CookingRegion.m_flameEffects.Length; i++)
		{
			ParticleSystem particleSystem = this.m_CookingRegion.m_flameEffects[i];
			if (!(particleSystem == null))
			{
				if (particleSystem.isPlaying)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x00071234 File Offset: 0x0006F634
	private void SetPFXEnabled(bool bOn)
	{
		for (int i = 0; i < this.m_CookingRegion.m_flameEffects.Length; i++)
		{
			ParticleSystem particleSystem = this.m_CookingRegion.m_flameEffects[i];
			if (!(particleSystem == null))
			{
				if (bOn)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Stop();
				}
			}
		}
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x00071298 File Offset: 0x0006F698
	private void UpdateFlameParticles(Collider _collider)
	{
		for (int i = 0; i < this.m_CookingRegion.m_flameEffects.Length; i++)
		{
			ParticleSystem particleSystem = this.m_CookingRegion.m_flameEffects[i];
			if (!(particleSystem == null))
			{
				Vector3 center = _collider.bounds.center;
				Vector3 position = particleSystem.transform.position;
				Vector2 vector = new Vector2(center.x - position.x, center.z - position.z);
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude > this.m_CookingRegion.m_flameOffRadius * this.m_CookingRegion.m_flameOffRadius)
				{
					if (!particleSystem.isEmitting && this.m_CookingRegion.enabled)
					{
						particleSystem.Play();
					}
				}
				else
				{
					float num = this.m_CookingRegion.m_heightCurve.Evaluate(Mathf.Sqrt(sqrMagnitude));
					if (num > 0.01f)
					{
						if (!particleSystem.isEmitting && this.m_CookingRegion.enabled)
						{
							particleSystem.Play();
						}
					}
					else if (particleSystem.isEmitting)
					{
						particleSystem.Stop();
					}
					if (particleSystem.isPlaying)
					{
						if (this.m_particles == null || this.m_particles.Length < particleSystem.particleCount)
						{
							this.m_particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
						}
						int particles = particleSystem.GetParticles(this.m_particles);
						float num2 = position.y + num;
						bool flag = false;
						for (int j = 0; j < particles; j++)
						{
							ParticleSystem.Particle particle = this.m_particles[j];
							if (particle.position.y >= num2)
							{
								this.m_particles[j].remainingLifetime = -1f;
								flag = true;
							}
						}
						if (flag)
						{
							particleSystem.SetParticles(this.m_particles, particles);
						}
					}
				}
			}
		}
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x000714A0 File Offset: 0x0006F8A0
	private IEnumerator TransitionEnabledRoutine(bool bOn)
	{
		if (this.m_CookingRegion.m_glowEffect != null)
		{
			if (bOn)
			{
				this.m_CookingRegion.m_glowEffect.Play();
			}
			else
			{
				this.m_CookingRegion.m_glowEffect.Stop();
			}
		}
		this.SetPFXEnabled(bOn);
		int layer = base.gameObject.layer;
		float timeLeft = this.m_CookingRegion.m_fadeDuration;
		float percentEnabled = 0f;
		Color[] emissionColors = null;
		if (this.m_burnerMaterials != null)
		{
			emissionColors = new Color[this.m_burnerMaterials.Length];
			for (int i = 0; i < this.m_burnerMaterials.Length; i++)
			{
				emissionColors[i] = this.m_burnerMaterials[i].GetColor(this.m_burnerMatEmissParamID);
			}
		}
		while (timeLeft >= 0f)
		{
			timeLeft -= TimeManager.GetDeltaTime(layer);
			percentEnabled = Mathf.Clamp01(timeLeft / Mathf.Max(this.m_CookingRegion.m_fadeDuration, 1E-06f));
			if (bOn)
			{
				percentEnabled = 1f - percentEnabled;
			}
			if (this.m_burnerMaterials != null)
			{
				for (int j = 0; j < this.m_burnerMaterials.Length; j++)
				{
					Color color = emissionColors[j];
					this.m_burnerMaterials[j].SetColor(this.m_burnerMatEmissParamID, new Color(color.r, color.g, color.b, percentEnabled));
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000714C4 File Offset: 0x0006F8C4
	private void OnEnableChanged(bool bOn)
	{
		this.m_transitionRoutine = this.TransitionEnabledRoutine(bOn);
		if (bOn)
		{
			ClientCookingRegion.s_sharedInfo.m_activeRegions.Add(this);
			if (ClientCookingRegion.s_sharedInfo.m_activeRegions.Count == 1)
			{
				GameUtils.StartAudio(GameLoopingAudioTag.DLC_04_Embers, this.m_fireAudioToken, base.gameObject.layer);
			}
			if (ClientTime.Time() - ClientCookingRegion.s_sharedInfo.m_lastIgniteTime > 0.25f)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_04_FlameIgnite, base.gameObject.layer);
				ClientCookingRegion.s_sharedInfo.m_lastIgniteTime = ClientTime.Time();
			}
		}
		else
		{
			ClientCookingRegion.s_sharedInfo.m_activeRegions.Remove(this);
			if (ClientCookingRegion.s_sharedInfo.m_activeRegions.Count == 0)
			{
				GameUtils.StopAudio(GameLoopingAudioTag.DLC_04_Embers, this.m_fireAudioToken);
			}
			if (ClientTime.Time() - ClientCookingRegion.s_sharedInfo.m_lastExtinguishTime > 0.25f)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_04_FlameDie, base.gameObject.layer);
				ClientCookingRegion.s_sharedInfo.m_lastExtinguishTime = ClientTime.Time();
			}
		}
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000715D7 File Offset: 0x0006F9D7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ClientCookingRegion.s_sharedInfo.m_allRegions.Remove(this);
		if (ClientCookingRegion.s_sharedInfo.m_allRegions.Count == 0)
		{
			ClientCookingRegion.s_sharedInfo = null;
		}
	}

	// Token: 0x04000FF0 RID: 4080
	private CookingRegion m_CookingRegion;

	// Token: 0x04000FF1 RID: 4081
	private TriggerRecorder m_recorder;

	// Token: 0x04000FF2 RID: 4082
	private GridManager m_gridManager;

	// Token: 0x04000FF3 RID: 4083
	private GridIndex m_gridIndex;

	// Token: 0x04000FF4 RID: 4084
	private const string c_burnerMatEmissParam = "_EmissiveColour";

	// Token: 0x04000FF5 RID: 4085
	private int m_burnerMatEmissParamID = Shader.PropertyToID("_EmissiveColour");

	// Token: 0x04000FF6 RID: 4086
	private Material[] m_burnerMaterials;

	// Token: 0x04000FF7 RID: 4087
	private IEnumerator m_transitionRoutine;

	// Token: 0x04000FF8 RID: 4088
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x04000FF9 RID: 4089
	private bool m_wasBaseEnabled;

	// Token: 0x04000FFA RID: 4090
	private static ClientCookingRegion.SharedInfo s_sharedInfo;

	// Token: 0x04000FFB RID: 4091
	private const float c_timeBeforeAudioRetrigger = 0.25f;

	// Token: 0x04000FFC RID: 4092
	private object m_fireAudioToken = new object();

	// Token: 0x04000FFD RID: 4093
	private GameObject m_prevOccupant;

	// Token: 0x0200045F RID: 1119
	private class SharedInfo
	{
		// Token: 0x04000FFE RID: 4094
		public List<ClientCookingRegion> m_allRegions = new List<ClientCookingRegion>();

		// Token: 0x04000FFF RID: 4095
		public List<ClientCookingRegion> m_activeRegions = new List<ClientCookingRegion>();

		// Token: 0x04001000 RID: 4096
		public float m_lastIgniteTime = float.MinValue;

		// Token: 0x04001001 RID: 4097
		public float m_lastExtinguishTime = float.MinValue;
	}
}
