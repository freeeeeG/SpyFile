using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000587 RID: 1415
public class ClientFlammable : ClientSynchroniserBase
{
	// Token: 0x06001AD8 RID: 6872 RVA: 0x000865DA File Offset: 0x000849DA
	public override EntityType GetEntityType()
	{
		return EntityType.Flammable;
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x000865DD File Offset: 0x000849DD
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_flammable = (Flammable)synchronisedObject;
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000865EC File Offset: 0x000849EC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		FlammableMessage flammableMessage = (FlammableMessage)serialisable;
		this.SetClampedFireStrength(flammableMessage.m_fireStrength);
		if (flammableMessage.m_onFire && !this.OnFire())
		{
			this.Ignite();
		}
		else if (!flammableMessage.m_onFire && this.OnFire())
		{
			this.Extinguish(false, flammableMessage.m_playerExtinguished);
		}
		if (this.OnFire())
		{
			this.m_strengthVelocity = flammableMessage.m_fireStrengthVelocity;
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x00086667 File Offset: 0x00084A67
	public static IEnumerable<ClientFlammable> GetAllOnFire()
	{
		return ClientFlammable.s_objectsOnFire.GetContents();
	}

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06001ADC RID: 6876 RVA: 0x00086673 File Offset: 0x00084A73
	// (remove) Token: 0x06001ADD RID: 6877 RVA: 0x00086680 File Offset: 0x00084A80
	public static event VoidGeneric<int> OnObjectsOnFireChanged
	{
		add
		{
			ClientFlammable.s_objectsOnFire.OnObjectsChanged += value;
		}
		remove
		{
			ClientFlammable.s_objectsOnFire.OnObjectsChanged -= value;
		}
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x0008668D File Offset: 0x00084A8D
	public void RegisterIgnitionCallback(ClientFlammable.IgnitionCallback _callback)
	{
		this.m_ignitionCallbacks = (ClientFlammable.IgnitionCallback)Delegate.Combine(this.m_ignitionCallbacks, _callback);
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x000866A6 File Offset: 0x00084AA6
	public void UnregisterIgnitionCallback(ClientFlammable.IgnitionCallback _callback)
	{
		this.m_ignitionCallbacks = (ClientFlammable.IgnitionCallback)Delegate.Remove(this.m_ignitionCallbacks, _callback);
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000866BF File Offset: 0x00084ABF
	protected override void OnDestroy()
	{
		this.Extinguish(false, false);
		base.OnDestroy();
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000866CF File Offset: 0x00084ACF
	protected override void OnDisable()
	{
		this.Extinguish(false, false);
		base.OnDisable();
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000866E0 File Offset: 0x00084AE0
	public override void UpdateSynchronising()
	{
		if (ClientFlammable.s_objectsOnFire.Count == 0)
		{
			return;
		}
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		if (this.OnFire())
		{
			if (this.m_fireStrength >= 1f && this.m_hideUITime > 0f)
			{
				this.m_hideUITime -= deltaTime;
				if (this.m_hideUITime <= 0f)
				{
					this.m_fireGUIBar.gameObject.SetActive(false);
				}
			}
			if (Mathf.Abs(this.m_strengthVelocity) > 0f)
			{
				float clampedFireStrength = this.m_fireStrength + this.m_strengthVelocity * TimeManager.GetDeltaTime(base.gameObject);
				this.SetClampedFireStrength(clampedFireStrength);
			}
		}
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x00086799 File Offset: 0x00084B99
	public bool OnFire()
	{
		return this.m_fireEffect != null;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000867A8 File Offset: 0x00084BA8
	private void Ignite()
	{
		if (!this.OnFire())
		{
			this.m_fireEffect = this.m_flammable.m_fireEffectPrefab.InstantiateOnParent(base.gameObject.transform, true);
			GameObject obj = GameUtils.InstantiateUIController(this.m_flammable.m_progressUIPrefab.gameObject, "HoverIconCanvas");
			this.m_fireGUIBar = obj.RequireComponent<ProgressUIController>();
			this.m_fireGUIBar.SetFollowTransform(base.transform, Vector3.zero);
			this.m_fireGUIBar.gameObject.SetActive(false);
			this.m_hideUITime = 0f;
			if (ClientFlammable.s_igniteAudio == null || !ClientFlammable.s_igniteAudio.isPlaying)
			{
				ClientFlammable.s_igniteAudio = GameUtils.TriggerAudio(GameOneShotAudioTag.FireIgnition, base.gameObject.layer);
			}
			if (ClientFlammable.s_audio == null)
			{
				ClientFlammable.s_audio = GameUtils.StartAudio(this.m_flammable.m_audioTag, ClientFlammable.s_objectsOnFire, base.gameObject.layer);
				GameUtils.StartNXRumble(this.m_flammable.m_audioTag);
			}
			this.SetClampedFireStrength(1f);
			ClientFlammable.s_objectsOnFire.Add(this);
			this.m_ignitionCallbacks(true);
			this.AdjustFireVolume();
		}
		else
		{
			this.SetClampedFireStrength(1f);
		}
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x000868F0 File Offset: 0x00084CF0
	private void Extinguish(bool shutdown = false, bool playerExtinguished = false)
	{
		if (this.OnFire())
		{
			ParticleSystem[] array = this.m_fireEffect.RequestComponentsRecursive<ParticleSystem>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Stop();
			}
			this.m_fireEffect = null;
			if (this.m_fireGUIBar != null && this.m_fireGUIBar.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.m_fireGUIBar.gameObject);
				this.m_fireGUIBar = null;
			}
			this.AdjustFireVolume();
			if (ClientFlammable.s_audio != null && ClientFlammable.s_objectsOnFire.Count == 1)
			{
				GameUtils.StopNXRumble(this.m_flammable.m_audioTag);
				GameUtils.StopAudio(this.m_flammable.m_audioTag, ClientFlammable.s_objectsOnFire);
				ClientFlammable.s_audio = null;
			}
			ClientFlammable.s_objectsOnFire.Remove(this);
			if (ClientFlammable.s_objectsOnFire.Count == 0 && !shutdown && playerExtinguished)
			{
				OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
				if (overcookedAchievementManager != null)
				{
					overcookedAchievementManager.AddIDStat(7, 1, ControlPadInput.PadNum.One);
				}
			}
			this.m_ignitionCallbacks(false);
		}
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x00086A14 File Offset: 0x00084E14
	private void SetClampedFireStrength(float _value)
	{
		this.m_fireStrength = Mathf.Clamp01(_value);
		if (this.OnFire())
		{
			this.m_fireGUIBar.SetProgress(this.m_fireStrength);
			if (this.m_fireStrength < 1f)
			{
				this.m_fireGUIBar.gameObject.SetActive(true);
				this.m_hideUITime = 1f;
			}
		}
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x00086A75 File Offset: 0x00084E75
	private void AdjustFireVolume()
	{
		if (ClientFlammable.s_audio != null)
		{
			ClientFlammable.s_audio.volume = MathUtils.ClampedRemap((float)ClientFlammable.s_objectsOnFire.Count, 0f, 5f, 0.2f, 0.6f);
		}
	}

	// Token: 0x04001542 RID: 5442
	private Flammable m_flammable;

	// Token: 0x04001543 RID: 5443
	private static StaticList<ClientFlammable> s_objectsOnFire = new StaticList<ClientFlammable>();

	// Token: 0x04001544 RID: 5444
	private static AudioSource s_audio;

	// Token: 0x04001545 RID: 5445
	private static AudioSource s_igniteAudio;

	// Token: 0x04001546 RID: 5446
	private float m_fireStrength;

	// Token: 0x04001547 RID: 5447
	private float m_strengthVelocity;

	// Token: 0x04001548 RID: 5448
	private GameObject m_fireEffect;

	// Token: 0x04001549 RID: 5449
	private ProgressUIController m_fireGUIBar;

	// Token: 0x0400154A RID: 5450
	private float m_hideUITime;

	// Token: 0x0400154B RID: 5451
	private ClientFlammable.IgnitionCallback m_ignitionCallbacks = delegate(bool _onFire)
	{
	};

	// Token: 0x02000588 RID: 1416
	// (Invoke) Token: 0x06001AEB RID: 6891
	public delegate void IgnitionCallback(bool _onFire);
}
