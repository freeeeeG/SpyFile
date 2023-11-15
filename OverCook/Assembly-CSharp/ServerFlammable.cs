using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class ServerFlammable : ServerSynchroniserBase
{
	// Token: 0x06001AB1 RID: 6833 RVA: 0x00085B3B File Offset: 0x00083F3B
	public override EntityType GetEntityType()
	{
		return EntityType.Flammable;
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x00085B3E File Offset: 0x00083F3E
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_flammable = (Flammable)synchronisedObject;
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x00085B4C File Offset: 0x00083F4C
	private void AddActiveVelocity(ServerFlammable.VelocityToken _token, float _velocity)
	{
		bool flag = false;
		for (int i = 0; i < this.m_velocityStack.Count; i++)
		{
			ServerFlammable.ActiveVelocities activeVelocities = this.m_velocityStack._items[i];
			if (activeVelocities.Token == _token)
			{
				flag = true;
				ServerFlammable.ActiveVelocities activeVelocities2 = activeVelocities;
				activeVelocities2.Velocity += _velocity;
				this.m_velocityStack._items[i] = activeVelocities2;
				break;
			}
		}
		if (!flag)
		{
			ServerFlammable.ActiveVelocities item = default(ServerFlammable.ActiveVelocities);
			item.Token = _token;
			item.Velocity = _velocity;
			this.m_velocityStack.Add(item);
		}
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x00085BF8 File Offset: 0x00083FF8
	private void RemoveActiveVelocity(ServerFlammable.VelocityToken _token)
	{
		for (int i = this.m_velocityStack.Count - 1; i >= 0; i--)
		{
			ServerFlammable.ActiveVelocities activeVelocities = this.m_velocityStack._items[i];
			if (activeVelocities.Token == _token)
			{
				this.m_velocityStack.RemoveAt(i);
				break;
			}
		}
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x00085C58 File Offset: 0x00084058
	private float GetActiveVelocity(ServerFlammable.VelocityToken _token)
	{
		for (int i = 0; i < this.m_velocityStack.Count; i++)
		{
			ServerFlammable.ActiveVelocities activeVelocities = this.m_velocityStack._items[i];
			if (activeVelocities.Token == _token)
			{
				return activeVelocities.Velocity;
			}
		}
		return 0f;
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x00085CB4 File Offset: 0x000840B4
	private float GetTotalVelocity()
	{
		float num = 0f;
		for (int i = 0; i < this.m_velocityStack.Count; i++)
		{
			num += this.m_velocityStack._items[i].Velocity;
		}
		return num;
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x00085D00 File Offset: 0x00084100
	private void ResetActiveVelocity(ServerFlammable.VelocityToken _token)
	{
		for (int i = 0; i < this.m_velocityStack.Count; i++)
		{
			ServerFlammable.ActiveVelocities activeVelocities = this.m_velocityStack._items[i];
			if (activeVelocities.Token == _token)
			{
				activeVelocities.Velocity = 0f;
				this.m_velocityStack._items[i] = activeVelocities;
				break;
			}
		}
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x00085D78 File Offset: 0x00084178
	private void SynchroniseClientState()
	{
		this.m_data.m_playerExtinguished = this.m_playerExtinguished;
		this.m_data.m_onFire = this.m_onFire;
		this.m_data.m_fireStrength = this.m_fireStrength;
		this.m_data.m_fireStrengthVelocity = this.GetTotalVelocity();
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x00085DD5 File Offset: 0x000841D5
	public static IEnumerable<ServerFlammable> GetAllOnFire()
	{
		return ServerFlammable.s_objectsOnFire.GetContents();
	}

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06001ABA RID: 6842 RVA: 0x00085DE1 File Offset: 0x000841E1
	// (remove) Token: 0x06001ABB RID: 6843 RVA: 0x00085DEE File Offset: 0x000841EE
	public static event VoidGeneric<int> OnObjectsOnFireChanged
	{
		add
		{
			ServerFlammable.s_objectsOnFire.OnObjectsChanged += value;
		}
		remove
		{
			ServerFlammable.s_objectsOnFire.OnObjectsChanged -= value;
		}
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x00085DFB File Offset: 0x000841FB
	public void RegisterIgnitionCallback(ServerFlammable.IgnitionCallback _callback)
	{
		this.m_ignitionCallbacks = (ServerFlammable.IgnitionCallback)Delegate.Combine(this.m_ignitionCallbacks, _callback);
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x00085E14 File Offset: 0x00084214
	public void UnregisterIgnitionCallback(ServerFlammable.IgnitionCallback _callback)
	{
		this.m_ignitionCallbacks = (ServerFlammable.IgnitionCallback)Delegate.Remove(this.m_ignitionCallbacks, _callback);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x00085E30 File Offset: 0x00084230
	private void Awake()
	{
		this.m_immolatedComponents = new MonoBehaviour[]
		{
			base.gameObject.GetComponent<Interactable>(),
			base.gameObject.GetComponent<PickupItemSpawner>(),
			base.gameObject.GetComponent<Workstation>()
		};
		if (GameUtils.GetLevelConfig().m_hazardInfo != null)
		{
			this.m_fireConfigData = GameUtils.GetLevelConfig().m_hazardInfo.FireConfigData;
		}
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x00085EAE File Offset: 0x000842AE
	public override void OnDestroy()
	{
		this._Extinguish(true, false);
		base.OnDestroy();
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x00085EBE File Offset: 0x000842BE
	protected override void OnDisable()
	{
		this._Extinguish(false, false);
		base.OnDisable();
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x00085ED0 File Offset: 0x000842D0
	private void SetImmolatedComponentsEnabled(bool _enabled)
	{
		for (int i = 0; i < this.m_immolatedComponents.Length; i++)
		{
			MonoBehaviour monoBehaviour = this.m_immolatedComponents[i];
			if (monoBehaviour != null)
			{
				monoBehaviour.enabled = _enabled;
			}
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x00085F12 File Offset: 0x00084312
	public void SetCanCatchFire(bool _canCatchFire)
	{
		this.m_canCatchFire = _canCatchFire;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x00085F1B File Offset: 0x0008431B
	public bool OnFire()
	{
		return this.m_onFire;
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x00085F23 File Offset: 0x00084323
	private bool CanCatchFire()
	{
		return this.m_canCatchFire && this.m_fireConfigData != null;
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x00085F40 File Offset: 0x00084340
	public void Ignite()
	{
		if (this.CanCatchFire() && !this.OnFire())
		{
			this.SetClampedFireStrength(1f);
			this.SetImmolatedComponentsEnabled(false);
			ServerFlammable.s_objectsOnFire.Add(this);
			this.m_ignitionCallbacks(true);
			this.m_encourageSupressedTimer = 0f;
			this.m_onFire = true;
			this.m_updatePending = true;
		}
		else if (this.CanCatchFire() && this.OnFire())
		{
			this.SetClampedFireStrength(1f);
			this.m_updatePending = true;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x00085FD2 File Offset: 0x000843D2
	public void StartEncourageFire(ServerFlammable _flammable)
	{
		this.m_fireEncouragedBy.Add(_flammable);
		this.EncourageFire(_flammable);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x00085FE8 File Offset: 0x000843E8
	public void EncourageFire(ServerFlammable _flammable)
	{
		if (this.m_encourageSupressedTimer <= 0f)
		{
			float num = ServerFlammable.CalculateFlammabilityTime(this.m_fireEncouragedBy, this);
			this.AddActiveVelocity(ServerFlammable.VelocityToken.Encourage, 1f / num);
		}
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x00086020 File Offset: 0x00084420
	public void StopEncourageFire(ServerFlammable _flamable)
	{
		this.m_fireEncouragedBy.Remove(_flamable);
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x00086030 File Offset: 0x00084430
	public void FightFire(float _timeToExtinguish, float _deltaTime, bool player = false)
	{
		if (this.OnFire())
		{
			float num = 1f / _timeToExtinguish;
			this.AddActiveVelocity(ServerFlammable.VelocityToken.Fight, -num);
			this.SetClampedFireStrength(this.m_fireStrength - num * _deltaTime);
			this.m_encourageSupressedTimer = this.m_fireConfigData.EncouragementSupressedTime;
			if (this.m_fireStrength == 0f)
			{
				this._Extinguish(false, player);
			}
		}
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x00086092 File Offset: 0x00084492
	public void Extinguish()
	{
		this._Extinguish(false, false);
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x0008609C File Offset: 0x0008449C
	public void _Extinguish(bool _shutdown, bool player = false)
	{
		if (this.OnFire())
		{
			this.SetImmolatedComponentsEnabled(true);
			ServerFlammable.s_objectsOnFire.Remove(this);
			this.m_onFire = false;
			this.m_playerExtinguished = player;
			this.m_ignitionCallbacks(false);
			for (int i = 0; i < this.m_currProximateFlammables.Length; i++)
			{
				ServerFlammable serverFlammable = this.m_currProximateFlammables[i];
				if (serverFlammable != null)
				{
					serverFlammable.StopEncourageFire(this);
					this.m_currProximateFlammables[i] = null;
				}
			}
			this.RemoveActiveVelocity(ServerFlammable.VelocityToken.Encourage);
			this.RemoveActiveVelocity(ServerFlammable.VelocityToken.Fight);
			this.RemoveActiveVelocity(ServerFlammable.VelocityToken.Recover);
			if (!_shutdown)
			{
				this.m_updatePending = true;
			}
		}
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x00086140 File Offset: 0x00084540
	private void SetClampedFireStrength(float _value)
	{
		this.m_fireStrength = Mathf.Clamp01(_value);
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x00086150 File Offset: 0x00084550
	private void OnFireIncrease(float _velocity, float _deltaTime)
	{
		if (this.CanCatchFire() && this.m_encourageSupressedTimer <= 0f)
		{
			this.m_cooldownSupressedTimer = this.m_fireConfigData.CooldownSupressedTime;
			if (this.GetActiveVelocity(ServerFlammable.VelocityToken.Recover) == 0f)
			{
				this.AddActiveVelocity(ServerFlammable.VelocityToken.Recover, 1f / this.m_fireConfigData.FireRecoveryTime);
			}
			this.SetClampedFireStrength(this.m_fireStrength + _velocity * _deltaTime);
			if (this.m_fireStrength == 1f)
			{
				this.Ignite();
			}
		}
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000861D8 File Offset: 0x000845D8
	public override void UpdateSynchronising()
	{
		if (this.m_flammable.m_startOnFire)
		{
			if (!this.OnFire())
			{
				this.Ignite();
			}
			this.m_flammable.m_startOnFire = false;
		}
		if (ServerFlammable.s_objectsOnFire.Count == 0 && !this.m_updatePending)
		{
			return;
		}
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		if (this.m_fireEncouragedBy.Count > 0 && this.m_fireStrength < 1f)
		{
			float activeVelocity = this.GetActiveVelocity(ServerFlammable.VelocityToken.Encourage);
			float velocity = (activeVelocity >= deltaTime) ? activeVelocity : 1f;
			this.OnFireIncrease(velocity, deltaTime);
		}
		if (this.OnFire())
		{
			ServerFlammable[] prevProximateFlammables = this.m_prevProximateFlammables;
			this.m_prevProximateFlammables = this.m_currProximateFlammables;
			this.m_currProximateFlammables = prevProximateFlammables;
			this.GetProximateFlammables(ref this.m_currProximateFlammables);
			for (int i = 0; i < this.m_currProximateFlammables.Length; i++)
			{
				ServerFlammable serverFlammable = this.m_currProximateFlammables[i];
				ServerFlammable serverFlammable2 = this.m_prevProximateFlammables[i];
				if (serverFlammable != serverFlammable2)
				{
					if (serverFlammable != null)
					{
						serverFlammable.StartEncourageFire(this);
					}
					if (serverFlammable2 != null)
					{
						serverFlammable2.StopEncourageFire(this);
					}
				}
				else if (serverFlammable != null)
				{
					serverFlammable.EncourageFire(this);
				}
			}
			if (this.m_fireStrength < 1f)
			{
				float activeVelocity2 = this.GetActiveVelocity(ServerFlammable.VelocityToken.Recover);
				this.OnFireIncrease(activeVelocity2, deltaTime);
			}
		}
		else if (this.m_fireStrength > 0f && this.m_cooldownSupressedTimer <= 0f)
		{
			this.SetClampedFireStrength(this.m_fireStrength - 1f / this.m_fireConfigData.CooldownTime * deltaTime);
		}
		if (this.m_encourageSupressedTimer > 0f)
		{
			this.m_encourageSupressedTimer -= deltaTime;
		}
		if (this.m_cooldownSupressedTimer > 0f)
		{
			this.m_cooldownSupressedTimer = this.m_encourageSupressedTimer - deltaTime;
		}
		float totalVelocity = this.GetTotalVelocity();
		if (totalVelocity != this.m_prevVelocity)
		{
			this.m_updatePending = true;
		}
		if (this.m_updatePending)
		{
			this.SynchroniseClientState();
			this.m_updatePending = false;
		}
		this.ResetActiveVelocity(ServerFlammable.VelocityToken.Encourage);
		this.ResetActiveVelocity(ServerFlammable.VelocityToken.Fight);
		this.ResetActiveVelocity(ServerFlammable.VelocityToken.Recover);
		this.m_prevVelocity = totalVelocity;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x00086428 File Offset: 0x00084828
	private static float CalculateFlammabilityTime(List<ServerFlammable> _instigators, ServerFlammable _target)
	{
		for (int i = 0; i < _instigators.Count; i++)
		{
			if (_instigators[i].m_flammable.m_overrideTargetFlammability)
			{
				return _instigators[i].m_flammable.m_overrideTargetFlammabilityTime;
			}
		}
		return _target.m_fireConfigData.FlammabilityTime;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x00086480 File Offset: 0x00084880
	private void GetProximateFlammables(ref ServerFlammable[] _proximateFlammables)
	{
		float num = this.m_flammable.m_fireSpreadRadius * this.m_flammable.m_fireSpreadRadius;
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		int num2 = -1;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 || j != 0)
				{
					num2++;
					GridIndex index = new GridIndex(gridLocationFromPos.X + i, gridLocationFromPos.Y, gridLocationFromPos.Z + j);
					Vector3 posFromGridLocation = this.m_gridManager.GetPosFromGridLocation(index);
					if ((base.transform.position - posFromGridLocation).sqrMagnitude >= num)
					{
						this.m_currProximateFlammables[num2] = null;
					}
					else
					{
						GameObject gridOccupant = this.m_gridManager.GetGridOccupant(index);
						if (gridOccupant == null)
						{
							this.m_currProximateFlammables[num2] = null;
						}
						else if (gridOccupant != base.gameObject)
						{
							this.m_currProximateFlammables[num2] = ComponentCache<ServerFlammable>.GetComponent(gridOccupant);
						}
					}
				}
			}
		}
	}

	// Token: 0x04001527 RID: 5415
	private Flammable m_flammable;

	// Token: 0x04001528 RID: 5416
	private FlammableMessage m_data = new FlammableMessage();

	// Token: 0x04001529 RID: 5417
	private bool m_updatePending;

	// Token: 0x0400152A RID: 5418
	private FastList<ServerFlammable.ActiveVelocities> m_velocityStack = new FastList<ServerFlammable.ActiveVelocities>(3);

	// Token: 0x0400152B RID: 5419
	private float m_prevVelocity;

	// Token: 0x0400152C RID: 5420
	private static StaticList<ServerFlammable> s_objectsOnFire = new StaticList<ServerFlammable>();

	// Token: 0x0400152D RID: 5421
	private FireConfigData m_fireConfigData;

	// Token: 0x0400152E RID: 5422
	private MonoBehaviour[] m_immolatedComponents;

	// Token: 0x0400152F RID: 5423
	private float m_fireStrength;

	// Token: 0x04001530 RID: 5424
	private List<ServerFlammable> m_fireEncouragedBy = new List<ServerFlammable>();

	// Token: 0x04001531 RID: 5425
	private ServerFlammable[] m_prevProximateFlammables = new ServerFlammable[8];

	// Token: 0x04001532 RID: 5426
	private ServerFlammable[] m_currProximateFlammables = new ServerFlammable[8];

	// Token: 0x04001533 RID: 5427
	private float m_encourageSupressedTimer;

	// Token: 0x04001534 RID: 5428
	private float m_cooldownSupressedTimer;

	// Token: 0x04001535 RID: 5429
	private bool m_canCatchFire = true;

	// Token: 0x04001536 RID: 5430
	private bool m_onFire;

	// Token: 0x04001537 RID: 5431
	private bool m_playerExtinguished;

	// Token: 0x04001538 RID: 5432
	private GridManager m_gridManager;

	// Token: 0x04001539 RID: 5433
	private ServerFlammable.IgnitionCallback m_ignitionCallbacks = delegate(bool _onFire)
	{
	};

	// Token: 0x02000584 RID: 1412
	private enum VelocityToken
	{
		// Token: 0x0400153C RID: 5436
		Encourage,
		// Token: 0x0400153D RID: 5437
		Fight,
		// Token: 0x0400153E RID: 5438
		Recover,
		// Token: 0x0400153F RID: 5439
		COUNT
	}

	// Token: 0x02000585 RID: 1413
	private struct ActiveVelocities
	{
		// Token: 0x04001540 RID: 5440
		public float Velocity;

		// Token: 0x04001541 RID: 5441
		public ServerFlammable.VelocityToken Token;
	}

	// Token: 0x02000586 RID: 1414
	// (Invoke) Token: 0x06001AD4 RID: 6868
	public delegate void IgnitionCallback(bool _onFire);
}
