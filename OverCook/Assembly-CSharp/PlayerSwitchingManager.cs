using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000517 RID: 1303
[ExecutionDependency(typeof(CampaignKitchenLoaderManager))]
[ExecutionDependency(typeof(CompetitiveKitchenLoaderManager))]
public class PlayerSwitchingManager : Manager
{
	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06001847 RID: 6215 RVA: 0x0007B360 File Offset: 0x00079760
	// (remove) Token: 0x06001848 RID: 6216 RVA: 0x0007B398 File Offset: 0x00079798
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<PlayerInputLookup.Player, PlayerControls> AvatarSelectChangeCallback = delegate(PlayerInputLookup.Player _param1, PlayerControls _param2)
	{
	};

	// Token: 0x06001849 RID: 6217 RVA: 0x0007B3D0 File Offset: 0x000797D0
	public PlayerControls SelectedAvatar(PlayerInputLookup.Player _player)
	{
		PlayerSwitchingManager.AvatarSet avatarSet = null;
		this.m_avatarSets.TryGetValue(_player, out avatarSet);
		return (avatarSet == null) ? null : avatarSet.SelectedAvatar;
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x0007B400 File Offset: 0x00079800
	public void ForceSwitchToNext(PlayerInputLookup.Player _player)
	{
		if (base.enabled && this.m_avatarSets.ContainsKey(_player))
		{
			PlayerSwitchingManager.AvatarSet avatarSet = this.m_avatarSets[_player];
			this.SelectNextActiveAvatar(avatarSet, PlayerSwitchingManager.PreviousOrNext.Next, true);
		}
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x0007B43F File Offset: 0x0007983F
	private void Awake()
	{
		this.m_OnChefsSetupComplete = new GenericVoid(this.InitialiseAvatars);
		DisconnectionHandler.OnChefBeingDeleted = (GenericVoid<EntitySerialisationEntry>)Delegate.Combine(DisconnectionHandler.OnChefBeingDeleted, new GenericVoid<EntitySerialisationEntry>(this.OnChefDeleted));
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x0007B473 File Offset: 0x00079873
	private void OnDestroy()
	{
		DisconnectionHandler.OnChefBeingDeleted = (GenericVoid<EntitySerialisationEntry>)Delegate.Remove(DisconnectionHandler.OnChefBeingDeleted, new GenericVoid<EntitySerialisationEntry>(this.OnChefDeleted));
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x0007B498 File Offset: 0x00079898
	public void InitialiseAvatars()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < 4; i++)
		{
			PlayerSwitchingManager.AvatarSet avatarSet = new PlayerSwitchingManager.AvatarSet();
			PlayerInputLookup.Player playerId = (PlayerInputLookup.Player)i;
			avatarSet.Avatars = array.FindAll((GameObject x) => x.RequireComponent<PlayerIDProvider>().GetID() == playerId);
			avatarSet.PlayerId = playerId;
			avatarSet.ActiveAvatar = 0;
			for (int j = 0; j < avatarSet.Avatars.Length; j++)
			{
				GameObject obj = avatarSet.Avatars[j];
				ActivationCallback activationCallback = obj.RequireComponent<ActivationCallback>();
				int acopy = j;
				activationCallback.DeactivateCallbacks += delegate()
				{
					if (this != null && this.gameObject != null && avatarSet.ActiveAvatar == acopy)
					{
						this.SelectNextActiveAvatar(avatarSet, PlayerSwitchingManager.PreviousOrNext.Next, false);
					}
				};
			}
			avatarSet.SwitchButtons = new ILogicalButton[2];
			avatarSet.SwitchButtons[0] = new ComboLogicalButton(new ILogicalButton[0], ComboLogicalButton.ComboType.Or);
			avatarSet.SwitchButtons[1] = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.PlayerSwitch, playerId);
			this.m_avatarSets.Add(playerId, avatarSet);
			this.m_AvatarSetList.Add(avatarSet);
		}
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0007B5E8 File Offset: 0x000799E8
	private void SelectNextActiveAvatar(PlayerSwitchingManager.AvatarSet _avatarSet, PlayerSwitchingManager.PreviousOrNext _direction = PlayerSwitchingManager.PreviousOrNext.Next, bool _force = false)
	{
		if (_avatarSet.Avatars.Length > 0)
		{
			int activeAvatar = _avatarSet.ActiveAvatar;
			for (int i = 1; i < _avatarSet.Avatars.Length; i++)
			{
				int num = MathUtils.Wrap(_avatarSet.ActiveAvatar + ((_direction != PlayerSwitchingManager.PreviousOrNext.Next) ? (-i) : i), 0, _avatarSet.Avatars.Length);
				if (_avatarSet.Avatars[num] != null)
				{
					PlayerControls playerControls = _avatarSet.Avatars[num].RequireComponent<PlayerControls>();
					bool flag = (playerControls.enabled && !playerControls.IsSuppressed() && !TimeManager.IsPaused(playerControls.gameObject)) || playerControls.AllowSwitchingWhenDisabled;
					if (_force || flag)
					{
						this.SwitchAvatars(_avatarSet, num);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x0007B6B8 File Offset: 0x00079AB8
	private void Update()
	{
		for (int i = 0; i < this.m_AvatarSetList.Count; i++)
		{
			this.UpdateAvatarSet(this.m_AvatarSetList._items[i]);
		}
		for (int j = this.m_TransitionParticles.Count - 1; j >= 0; j--)
		{
			if (!this.m_TransitionParticles._items[j].routine.MoveNext())
			{
				this.m_TransitionParticles._items[j].particleLoop.Stop();
				this.m_TransitionParticles.RemoveAt(j);
			}
		}
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x0007B75C File Offset: 0x00079B5C
	private void UpdateAvatarSet(PlayerSwitchingManager.AvatarSet _set)
	{
		if (_set.Avatars.Length == 0)
		{
			return;
		}
		int num = _set.SwitchButtons.FindIndex_Predicate((ILogicalButton x) => x.JustPressed());
		if (num != -1)
		{
			this.SelectNextActiveAvatar(_set, (PlayerSwitchingManager.PreviousOrNext)num, false);
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x0007B7B4 File Offset: 0x00079BB4
	private void SwitchAvatars(PlayerSwitchingManager.AvatarSet _set, int _avatarId)
	{
		if (_set.ActiveAvatar != _avatarId && _set.ActiveAvatar != -1)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_set.SelectedAvatar.gameObject);
			EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(_set.Avatars[_avatarId]);
			ParticleSystem particleLoop = this.m_transitionParticlePrefab.InstantiatePFX(_set.Avatars[_avatarId].transform.position);
			this.m_TransitionParticles.Add(new PlayerSwitchingManager.TransitionParticle
			{
				routine = this.RunTransitionParticle(_set.SelectedAvatar.gameObject, _set.Avatars[_avatarId], particleLoop),
				from = entry,
				to = entry2,
				particleLoop = particleLoop
			});
		}
		_set.ActiveAvatar = _avatarId;
		this.AvatarSelectChangeCallback(_set.PlayerId, _set.SelectedAvatar);
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x0007B884 File Offset: 0x00079C84
	private void OnChefDeleted(EntitySerialisationEntry chef)
	{
		for (int i = this.m_TransitionParticles.Count - 1; i >= 0; i--)
		{
			if (chef.m_Header.m_uEntityID == this.m_TransitionParticles._items[i].from.m_Header.m_uEntityID || chef.m_Header.m_uEntityID == this.m_TransitionParticles._items[i].to.m_Header.m_uEntityID)
			{
				this.m_TransitionParticles._items[i].particleLoop.Stop();
				this.m_TransitionParticles.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x0007B938 File Offset: 0x00079D38
	private IEnumerator RunTransitionParticle(GameObject _from, GameObject _to, ParticleSystem particleLoop)
	{
		if (_from != null && _to != null)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.ChangeChef, base.gameObject.layer);
			ParticleSystem emitParticle = this.m_transitionStartParticlePrefab.InstantiatePFX(_from.transform.position);
			Vector3 diff = _to.transform.position - _from.transform.position;
			float length = diff.magnitude + 1E-07f;
			Vector3 forward = diff / length;
			Vector3 up = Vector3.up;
			Vector3 right;
			if (Vector3.Dot(forward, Vector3.up) == 1f)
			{
				right = Vector3.right;
			}
			else
			{
				right = Vector3.Cross(up, forward);
			}
			float prop = 0f;
			float angle = 0f;
			particleLoop.transform.position = _from.transform.position;
			particleLoop.RestartPFX();
			while (prop < 1f && _from != null && _to != null)
			{
				float dt = TimeManager.GetDeltaTime(base.gameObject.layer);
				float distance = this.m_transitionLinearSpeed * dt;
				length = (_to.transform.position - _from.transform.position).magnitude;
				prop = Mathf.Clamp01(prop + distance / length);
				angle += this.m_transitionAngularSpeed * dt;
				float offsetDistance = MathUtils.ClampedRemap(Mathf.Abs(prop - 0.5f), 0f, 0.5f, this.m_transitionSpiralRadius, 0f);
				Vector3 offset = offsetDistance * (right * Mathf.Cos(angle) + up * Mathf.Sin(angle));
				Vector3 linePos = Vector3.Lerp(_from.transform.position, _to.transform.position, prop);
				particleLoop.transform.position = linePos + offset;
				yield return null;
			}
			ParticleSystem endParticle = this.m_transitionEndParticlePrefab.InstantiatePFX(_to.transform);
			endParticle.gameObject.layer = base.gameObject.layer;
		}
		yield break;
	}

	// Token: 0x04001387 RID: 4999
	[SerializeField]
	private float m_transitionLinearSpeed = 45f;

	// Token: 0x04001388 RID: 5000
	[SerializeField]
	private float m_transitionAngularSpeed = 25.132742f;

	// Token: 0x04001389 RID: 5001
	[SerializeField]
	private float m_transitionSpiralRadius = 0.5f;

	// Token: 0x0400138A RID: 5002
	[SerializeField]
	private ParticleSystem m_transitionParticlePrefab;

	// Token: 0x0400138B RID: 5003
	[SerializeField]
	private ParticleSystem m_transitionStartParticlePrefab;

	// Token: 0x0400138C RID: 5004
	[SerializeField]
	private ParticleSystem m_transitionEndParticlePrefab;

	// Token: 0x0400138D RID: 5005
	private Dictionary<PlayerInputLookup.Player, PlayerSwitchingManager.AvatarSet> m_avatarSets = new Dictionary<PlayerInputLookup.Player, PlayerSwitchingManager.AvatarSet>(new PlayerSwitchingManager.PlayerComparer());

	// Token: 0x0400138E RID: 5006
	private FastList<PlayerSwitchingManager.AvatarSet> m_AvatarSetList = new FastList<PlayerSwitchingManager.AvatarSet>();

	// Token: 0x0400138F RID: 5007
	private GenericVoid m_OnChefsSetupComplete;

	// Token: 0x04001391 RID: 5009
	private FastList<PlayerSwitchingManager.TransitionParticle> m_TransitionParticles = new FastList<PlayerSwitchingManager.TransitionParticle>(2);

	// Token: 0x02000518 RID: 1304
	public class PlayerComparer : IEqualityComparer<PlayerInputLookup.Player>
	{
		// Token: 0x06001857 RID: 6231 RVA: 0x0007B97A File Offset: 0x00079D7A
		public bool Equals(PlayerInputLookup.Player x, PlayerInputLookup.Player y)
		{
			return x == y;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0007B980 File Offset: 0x00079D80
		public int GetHashCode(PlayerInputLookup.Player obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000519 RID: 1305
	private struct TransitionParticle
	{
		// Token: 0x04001394 RID: 5012
		public IEnumerator routine;

		// Token: 0x04001395 RID: 5013
		public EntitySerialisationEntry from;

		// Token: 0x04001396 RID: 5014
		public EntitySerialisationEntry to;

		// Token: 0x04001397 RID: 5015
		public ParticleSystem particleLoop;
	}

	// Token: 0x0200051A RID: 1306
	private class AvatarSet
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x0007B992 File Offset: 0x00079D92
		// (set) Token: 0x0600185B RID: 6235 RVA: 0x0007B99C File Offset: 0x00079D9C
		public int ActiveAvatar
		{
			get
			{
				return this.m_activeAvatar;
			}
			set
			{
				this.m_activeAvatar = value;
				for (int i = 0; i < this.Avatars.Length; i++)
				{
					PlayerControls playerControls = this.Avatars[i].RequireComponent<PlayerControls>();
					playerControls.SetDirectlyUnderPlayerControl(i == this.m_activeAvatar);
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x0007B9E6 File Offset: 0x00079DE6
		public PlayerControls SelectedAvatar
		{
			get
			{
				if (this.ActiveAvatar != -1 && this.ActiveAvatar < this.Avatars.Length)
				{
					return this.Avatars[this.ActiveAvatar].RequireComponent<PlayerControls>();
				}
				return null;
			}
		}

		// Token: 0x04001398 RID: 5016
		private int m_activeAvatar = -1;

		// Token: 0x04001399 RID: 5017
		public GameObject[] Avatars;

		// Token: 0x0400139A RID: 5018
		public ILogicalButton[] SwitchButtons;

		// Token: 0x0400139B RID: 5019
		public PlayerInputLookup.Player PlayerId;
	}

	// Token: 0x0200051B RID: 1307
	private enum PreviousOrNext
	{
		// Token: 0x0400139D RID: 5021
		Previous,
		// Token: 0x0400139E RID: 5022
		Next
	}
}
