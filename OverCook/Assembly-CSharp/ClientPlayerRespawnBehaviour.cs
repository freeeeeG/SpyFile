using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A1D RID: 2589
public class ClientPlayerRespawnBehaviour : ClientSynchroniserBase
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x0600334B RID: 13131 RVA: 0x000F0945 File Offset: 0x000EED45
	public bool IsRespawning
	{
		get
		{
			return this.m_isRespawning;
		}
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x000F0950 File Offset: 0x000EED50
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_PlayerRespawnBehaviour = (PlayerRespawnBehaviour)synchronisedObject;
		this.m_spawnEffect = this.m_PlayerRespawnBehaviour.m_spawnEffect.InstantiateOnParent(this.m_PlayerRespawnBehaviour.m_startParent, true);
		this.m_spawnEffectParticleSystem = this.m_spawnEffect.RequestComponent<ParticleSystem>();
		this.m_spawnEffectParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		this.m_clientChefSynchroniser = base.gameObject.RequestComponent<ClientChefSynchroniser>();
		this.m_respawnLayerMask = LayerMask.NameToLayer(ClientPlayerRespawnBehaviour.k_respawnLayer);
		this.m_activeLayerMask = LayerMask.NameToLayer(ClientPlayerRespawnBehaviour.k_activeLayer);
		this.m_hoverIcon = GameUtils.InstantiateHoverIconUIController(this.m_PlayerRespawnBehaviour.m_respawnCounterPrefab, this.m_PlayerRespawnBehaviour.m_startParent, "HoverIconCanvas", this.m_PlayerRespawnBehaviour.m_startLocation);
		this.m_hoverIconUiController = this.m_hoverIcon.RequireComponent<HoverIconUIController>();
		this.m_respawnUiController = this.m_hoverIcon.RequireComponent<RespawnCounterUIController>();
		this.m_hoverIcon.gameObject.SetActive(false);
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x000F0A3E File Offset: 0x000EEE3E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_hoverIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_hoverIcon.gameObject);
		}
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x000F0A67 File Offset: 0x000EEE67
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x000F0A6C File Offset: 0x000EEE6C
	public override void UpdateSynchronising()
	{
		if (this.m_currentRespawn != null)
		{
			if (!this.m_currentRespawn.MoveNext() || !MultiplayerController.IsSynchronisationActive())
			{
				this.m_currentRespawn = null;
			}
			else
			{
				this.m_spawnEffect.transform.position = base.gameObject.transform.position;
			}
		}
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x000F0ACC File Offset: 0x000EEECC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		RespawnMessage respawnMessage = (RespawnMessage)serialisable;
		if (respawnMessage.m_Phase == RespawnMessage.Phase.Begin && this.m_currentRespawn == null)
		{
			this.m_currentRespawn = this.BeginRespawn(respawnMessage.m_RespawnType);
			this.m_currentRespawn.MoveNext();
		}
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x000F0B1C File Offset: 0x000EEF1C
	private IEnumerator BeginRespawn(RespawnCollider.RespawnType respawnType)
	{
		if (this.m_isRespawning)
		{
			yield break;
		}
		this.m_isRespawning = true;
		base.gameObject.RequireComponent<Collider>().enabled = false;
		RigidbodyMotion motion = null;
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			motion = this.m_PlayerRespawnBehaviour.m_playerControls.Motion;
			motion.SetVelocity(Vector3.zero);
		}
		WindAccumulator windAccumulator = base.gameObject.RequireComponent<WindAccumulator>();
		windAccumulator.Reset();
		this.m_PlayerRespawnBehaviour.m_playerControls.m_bRespawning = true;
		ParticleSystem pfx = base.gameObject.RequestComponentRecursive<ParticleSystem>();
		if (pfx != null)
		{
			pfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		Animator animator = base.gameObject.RequireComponentRecursive<Animator>();
		animator.SetBool(ClientPlayerRespawnBehaviour.m_iDead, true);
		PlayerIDProvider idProvider = base.gameObject.RequireComponent<PlayerIDProvider>();
		GameUtils.TriggerNXRumble(idProvider.GetID(), GameOneShotAudioTag.Boom);
		if (respawnType != RespawnCollider.RespawnType.FallDeath)
		{
			if (respawnType != RespawnCollider.RespawnType.Drowning)
			{
				if (respawnType == RespawnCollider.RespawnType.Car)
				{
					this.m_PlayerRespawnBehaviour.m_playerControls.m_bApplyGravity = false;
					this.m_PlayerRespawnBehaviour.m_playerControls.Motion.SetKinematic(true);
					GameUtils.TriggerAudio(this.m_PlayerRespawnBehaviour.m_carHitAudioTag, base.gameObject.layer);
				}
			}
			else
			{
				GameUtils.TriggerAudio(this.m_PlayerRespawnBehaviour.m_drownAudioTag, base.gameObject.layer);
			}
		}
		else
		{
			this.m_fallEffectInstance = this.m_PlayerRespawnBehaviour.m_fallingEffect.InstantiateOnParent(this.m_PlayerRespawnBehaviour.m_startParent, true);
			this.m_fallEffectInstance.transform.position = base.transform.position;
			GameUtils.TriggerAudio(this.m_PlayerRespawnBehaviour.m_fallAudioTag, base.gameObject.layer);
		}
		animator.SetTrigger(respawnType.ToDescription());
		base.StartCoroutine(this.RunSwitchToNextDelayed(this.m_PlayerRespawnBehaviour.m_switchDelay));
		IEnumerator parallel = CoroutineUtils.ParallelRoutine(new IEnumerator[]
		{
			this.RunHoverCounter(),
			this.RunAvatarDestroy(respawnType)
		});
		while (parallel.MoveNext())
		{
			if ((ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession()) && respawnType == RespawnCollider.RespawnType.Drowning)
			{
				Vector3 velocity = motion.GetVelocity();
				velocity.y = Mathf.Max(this.m_PlayerRespawnBehaviour.m_maxDrowingVerticalVelocity, velocity.y);
				motion.SetVelocity(velocity);
			}
			yield return null;
		}
		if (this.m_PlayerRespawnBehaviour.m_windAccumulator != null)
		{
			this.m_PlayerRespawnBehaviour.m_windAccumulator.enabled = false;
		}
		IEnumerator spawn = this.RunSpawnAtSpawnPoint();
		while (spawn.MoveNext())
		{
			yield return null;
		}
		this.m_isRespawning = false;
		this.m_PlayerRespawnBehaviour.m_playerControls.m_bRespawning = false;
		if (pfx != null)
		{
			pfx.Play();
		}
		this.m_PlayerRespawnBehaviour.m_playerControls.m_bApplyGravity = true;
		if (idProvider.IsLocallyControlled() || ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			this.m_PlayerRespawnBehaviour.m_playerControls.Motion.SetKinematic(false);
		}
		yield break;
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x000F0B40 File Offset: 0x000EEF40
	private IEnumerator RunSwitchToNextDelayed(float seconds)
	{
		IEnumerator wait = CoroutineUtils.TimerRoutine(seconds, base.gameObject.layer);
		while (wait.MoveNext())
		{
			yield return null;
		}
		PlayerInputLookup.Player player = this.m_PlayerRespawnBehaviour.m_playerControls.PlayerIDProvider.GetID();
		PlayerSwitchingManager switchingManager = GameUtils.RequireManager<PlayerSwitchingManager>();
		if (switchingManager.SelectedAvatar(player) == this.m_PlayerRespawnBehaviour.m_playerControls)
		{
			switchingManager.ForceSwitchToNext(player);
		}
		yield break;
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x000F0B64 File Offset: 0x000EEF64
	public void MoveRespawnPoint(Vector3 _startLocation, Transform _startParent = null)
	{
		this.m_PlayerRespawnBehaviour.m_startLocation = _startLocation;
		this.m_PlayerRespawnBehaviour.m_startParent = _startParent;
		if (this.m_hoverIcon != null)
		{
			RespawnCounterUIController respawnCounterUIController = this.m_hoverIcon.RequireComponent<RespawnCounterUIController>();
			respawnCounterUIController.SetFollowTransform(this.m_PlayerRespawnBehaviour.m_startParent, this.m_PlayerRespawnBehaviour.m_startLocation);
		}
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x000F0BC4 File Offset: 0x000EEFC4
	private IEnumerator RunHoverCounter()
	{
		this.m_hoverIcon.gameObject.SetActive(true);
		this.m_hoverIconUiController.SetFollowTransform(this.m_PlayerRespawnBehaviour.m_startParent, this.m_PlayerRespawnBehaviour.m_startLocation);
		this.m_respawnUiController.SetTarget(this.m_PlayerRespawnBehaviour.gameObject);
		this.m_respawnUiController.SetCountdown(this.m_PlayerRespawnBehaviour.m_respawnTime);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(base.gameObject);
		uint uEntityID = entry.m_Header.m_uEntityID;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			if (uEntityID == user.EntityID || uEntityID == user.Entity2ID)
			{
				this.m_respawnUiController.SetTeam(user.Team);
				this.m_respawnUiController.SetPlayerNum(i);
			}
		}
		IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_PlayerRespawnBehaviour.m_respawnTime, base.gameObject.layer);
		while (wait.MoveNext())
		{
			yield return null;
		}
		this.m_hoverIcon.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x000F0BE0 File Offset: 0x000EEFE0
	private IEnumerator RunAvatarDestroy(RespawnCollider.RespawnType respawnType)
	{
		this.PauseMovement();
		int idx = this.m_PlayerRespawnBehaviour.m_respawnParams.FindIndex((PlayerRespawnBehaviour.RespawnParams x) => x.m_deathBy == respawnType);
		if (idx != -1)
		{
			PlayerRespawnBehaviour.RespawnParams respawnParams = this.m_PlayerRespawnBehaviour.m_respawnParams[idx];
			float totalRespawnEffectDuration = respawnParams.m_delay + respawnParams.m_duration;
			if (respawnParams.m_type == PlayerRespawnBehaviour.RespawnParams.Type.Fade)
			{
				IEnumerator wait = CoroutineUtils.TimerRoutine(respawnParams.m_delay, base.gameObject.layer);
				while (wait.MoveNext())
				{
					yield return null;
				}
				SkinnedMeshRenderer[] allRenderers = base.gameObject.RequestComponentsRecursive<SkinnedMeshRenderer>();
				Material[] allMaterials = allRenderers.ConvertAll((SkinnedMeshRenderer x) => x.material);
				for (int i = 0; i < allRenderers.Length; i++)
				{
					Material material = new Material(allRenderers[i].material);
					material.shader = this.m_PlayerRespawnBehaviour.m_fadeShader;
					allRenderers[i].material = material;
				}
				float prop = 0f;
				while (prop < 1f)
				{
					for (int j = 0; j < allRenderers.Length; j++)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = allRenderers[j];
						if (null != skinnedMeshRenderer && null != skinnedMeshRenderer.material && skinnedMeshRenderer.material.HasProperty("_Color"))
						{
							Color color = allRenderers[j].material.color;
							color.a = 1f - prop;
							allRenderers[j].material.color = color;
						}
					}
					prop += TimeManager.GetDeltaTime(base.gameObject) / respawnParams.m_duration;
					yield return null;
				}
				if (this.m_clientChefSynchroniser != null && this.m_PlayerRespawnBehaviour.m_playerControls.ControlScheme != null)
				{
					this.m_clientChefSynchroniser.Pause();
				}
				this.DeactivateAndMove();
				for (int k = 0; k < allRenderers.Length; k++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer2 = allRenderers[k];
					if (null != skinnedMeshRenderer2)
					{
						skinnedMeshRenderer2.material = allMaterials[k];
						if (null != skinnedMeshRenderer2.material && skinnedMeshRenderer2.material.HasProperty("_Color"))
						{
							Color color2 = skinnedMeshRenderer2.material.color;
							color2.a = 1f;
							skinnedMeshRenderer2.material.color = color2;
						}
					}
				}
			}
			else if (respawnParams.m_type == PlayerRespawnBehaviour.RespawnParams.Type.Scale)
			{
				IEnumerator wait2 = CoroutineUtils.TimerRoutine(respawnParams.m_delay, base.gameObject.layer);
				while (wait2.MoveNext())
				{
					yield return null;
				}
				float progress = respawnParams.m_duration;
				wait2 = CoroutineUtils.TimerRoutine(respawnParams.m_duration, base.gameObject.layer);
				while (wait2.MoveNext())
				{
					progress = Mathf.Clamp01(progress - TimeManager.GetDeltaTime(base.gameObject.layer) / respawnParams.m_duration);
					base.transform.localScale = new Vector3(progress, progress, progress);
					yield return null;
				}
				wait2 = CoroutineUtils.TimerRoutine(this.m_PlayerRespawnBehaviour.m_respawnTime - totalRespawnEffectDuration, base.gameObject.layer);
				while (wait2.MoveNext())
				{
					yield return null;
				}
				if (this.m_clientChefSynchroniser != null && this.m_PlayerRespawnBehaviour.m_playerControls.ControlScheme != null)
				{
					this.m_clientChefSynchroniser.Pause();
				}
				this.DeactivateAndMove();
			}
			else if (respawnParams.m_type == PlayerRespawnBehaviour.RespawnParams.Type.None)
			{
				IEnumerator wait3 = CoroutineUtils.TimerRoutine(this.m_PlayerRespawnBehaviour.m_respawnTime, base.gameObject.layer);
				while (wait3.MoveNext())
				{
					yield return null;
				}
				if (this.m_clientChefSynchroniser != null && this.m_PlayerRespawnBehaviour.m_playerControls.ControlScheme != null)
				{
					this.m_clientChefSynchroniser.Pause();
				}
				this.DeactivateAndMove();
			}
		}
		else
		{
			IEnumerator wait4 = CoroutineUtils.TimerRoutine(this.m_PlayerRespawnBehaviour.m_respawnTime, base.gameObject.layer);
			while (wait4.MoveNext())
			{
				yield return null;
			}
			if (this.m_clientChefSynchroniser != null && this.m_PlayerRespawnBehaviour.m_playerControls.ControlScheme != null)
			{
				this.m_clientChefSynchroniser.Pause();
			}
			this.DeactivateAndMove();
		}
		if (this.m_fallEffectInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_fallEffectInstance);
		}
		yield break;
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x000F0C02 File Offset: 0x000EF002
	private void DeactivateAndMove()
	{
		this.SetRendererActive(false);
		this.SetCharacterCollisionsEnabled(false);
		base.transform.SetParent(this.m_PlayerRespawnBehaviour.m_startParent, false);
		base.transform.localPosition = this.m_PlayerRespawnBehaviour.m_startLocation;
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000F0C40 File Offset: 0x000EF040
	public IEnumerator RunSpawnAtSpawnPoint()
	{
		base.transform.SetParent(this.m_PlayerRespawnBehaviour.m_startParent, false);
		base.transform.localPosition = this.m_PlayerRespawnBehaviour.m_startLocation;
		base.transform.localScale = Vector3.one;
		base.gameObject.RequireComponent<Collider>().enabled = true;
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			this.m_PlayerRespawnBehaviour.m_playerControls.Motion.SetVelocity(new Vector3(0f, 0f, 0f));
		}
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = base.gameObject.RequireComponent<ServerWorldObjectSynchroniser>();
			if (serverWorldObjectSynchroniser != null)
			{
				serverWorldObjectSynchroniser.ResumeAllClients(true);
			}
		}
		if (this.m_clientChefSynchroniser != null)
		{
			while (!this.m_clientChefSynchroniser.IsReadyToResume())
			{
				yield return null;
			}
			this.m_clientChefSynchroniser.Resume();
		}
		if (this.m_spawnEffectParticleSystem != null)
		{
			this.m_spawnEffect.transform.SetParent(this.m_PlayerRespawnBehaviour.m_startParent);
			this.m_spawnEffect.transform.localPosition = this.m_PlayerRespawnBehaviour.m_startLocation;
			this.m_spawnEffectParticleSystem.Play();
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.PlayerSpawn, base.gameObject.layer);
		Animator animator = base.gameObject.RequireComponentRecursive<Animator>();
		animator.SetBool(ClientPlayerRespawnBehaviour.m_iDead, false);
		IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_PlayerRespawnBehaviour.m_particleTime, base.gameObject.layer);
		while (wait.MoveNext())
		{
			yield return null;
		}
		if (this.m_spawnEffectParticleSystem != null)
		{
			this.m_spawnEffectParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		if (this.m_PlayerRespawnBehaviour.m_windAccumulator != null)
		{
			this.m_PlayerRespawnBehaviour.m_windAccumulator.enabled = true;
		}
		this.SetRendererActive(true);
		this.SetCharacterCollisionsEnabled(true);
		this.ResumeMovement();
		yield break;
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x000F0C5C File Offset: 0x000EF05C
	private void SetRendererActive(bool bActive)
	{
		Animator componentInChildren = base.gameObject.GetComponentInChildren<Animator>(true);
		if (componentInChildren != null)
		{
			componentInChildren.gameObject.SetActive(bActive);
		}
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x000F0C8E File Offset: 0x000EF08E
	private void SetCharacterCollisionsEnabled(bool bEnabled)
	{
		base.gameObject.SetObjectLayer((!bEnabled) ? this.m_respawnLayerMask : this.m_activeLayerMask);
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x000F0CB2 File Offset: 0x000EF0B2
	private void PauseMovement()
	{
		if (this.m_PlayerRespawnBehaviour.m_playerControls.PlayerIDProvider.IsLocallyControlled())
		{
			this.m_controlsSuppressor = this.m_PlayerRespawnBehaviour.m_playerControls.Suppress(this);
		}
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x000F0CE5 File Offset: 0x000EF0E5
	private void ResumeMovement()
	{
		if (this.m_PlayerRespawnBehaviour.m_playerControls.PlayerIDProvider.IsLocallyControlled())
		{
			this.m_controlsSuppressor.Release();
			this.m_controlsSuppressor = null;
		}
	}

	// Token: 0x04002944 RID: 10564
	private PlayerRespawnBehaviour m_PlayerRespawnBehaviour;

	// Token: 0x04002945 RID: 10565
	private GameObject m_spawnEffect;

	// Token: 0x04002946 RID: 10566
	private ParticleSystem m_spawnEffectParticleSystem;

	// Token: 0x04002947 RID: 10567
	private GameObject m_fallEffectInstance;

	// Token: 0x04002948 RID: 10568
	private ClientChefSynchroniser m_clientChefSynchroniser;

	// Token: 0x04002949 RID: 10569
	private GameObject m_hoverIcon;

	// Token: 0x0400294A RID: 10570
	private HoverIconUIController m_hoverIconUiController;

	// Token: 0x0400294B RID: 10571
	private RespawnCounterUIController m_respawnUiController;

	// Token: 0x0400294C RID: 10572
	private Suppressor m_controlsSuppressor;

	// Token: 0x0400294D RID: 10573
	private bool m_isRespawning;

	// Token: 0x0400294E RID: 10574
	private static readonly string k_activeLayer = "Players";

	// Token: 0x0400294F RID: 10575
	private static readonly string k_respawnLayer = "PlayersRespawn";

	// Token: 0x04002950 RID: 10576
	private int m_respawnLayerMask;

	// Token: 0x04002951 RID: 10577
	private int m_activeLayerMask;

	// Token: 0x04002952 RID: 10578
	private IEnumerator m_currentRespawn;

	// Token: 0x04002953 RID: 10579
	private static int m_iDead = Animator.StringToHash("Dead");
}
