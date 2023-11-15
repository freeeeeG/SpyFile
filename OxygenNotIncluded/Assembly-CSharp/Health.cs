using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06003A02 RID: 14850 RVA: 0x001432B5 File Offset: 0x001414B5
	public AmountInstance GetAmountInstance
	{
		get
		{
			return this.amountInstance;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06003A03 RID: 14851 RVA: 0x001432BD File Offset: 0x001414BD
	// (set) Token: 0x06003A04 RID: 14852 RVA: 0x001432CA File Offset: 0x001414CA
	public float hitPoints
	{
		get
		{
			return this.amountInstance.value;
		}
		set
		{
			this.amountInstance.value = value;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06003A05 RID: 14853 RVA: 0x001432D8 File Offset: 0x001414D8
	public float maxHitPoints
	{
		get
		{
			return this.amountInstance.GetMax();
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x001432E5 File Offset: 0x001414E5
	public float percent()
	{
		return this.hitPoints / this.maxHitPoints;
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x001432F4 File Offset: 0x001414F4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		this.amountInstance.value = this.amountInstance.GetMax();
		AmountInstance amountInstance = this.amountInstance;
		amountInstance.OnDelta = (Action<float>)Delegate.Combine(amountInstance.OnDelta, new Action<float>(this.OnHealthChanged));
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x00143370 File Offset: 0x00141570
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.State == Health.HealthState.Incapacitated || this.hitPoints == 0f)
		{
			if (this.CanBeIncapacitated)
			{
				this.Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				this.Kill();
			}
		}
		if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
		{
			this.UpdateStatus();
		}
		this.effects = base.GetComponent<Effects>();
		this.UpdateHealthBar();
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x001433DE File Offset: 0x001415DE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x001433F4 File Offset: 0x001415F4
	public void UpdateHealthBar()
	{
		if (NameDisplayScreen.Instance == null)
		{
			return;
		}
		bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || this.hitPoints >= this.maxHitPoints || base.gameObject.HasTag("HideHealthBar");
		NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, new Func<float>(this.percent), !flag);
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x00143468 File Offset: 0x00141668
	private void Recover()
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0014347C File Offset: 0x0014167C
	public void OnHealthChanged(float delta)
	{
		base.Trigger(-1664904872, delta);
		if (this.State != Health.HealthState.Invincible)
		{
			if (this.hitPoints == 0f && !this.IsDefeated())
			{
				if (this.CanBeIncapacitated)
				{
					this.Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					this.Kill();
				}
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		this.UpdateStatus();
		this.UpdateWoundEffects();
		this.UpdateHealthBar();
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x001434F7 File Offset: 0x001416F7
	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		this.Damage(1f);
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x00143504 File Offset: 0x00141704
	public void Damage(float amount)
	{
		if (this.State != Health.HealthState.Invincible)
		{
			this.hitPoints = Mathf.Max(0f, this.hitPoints - amount);
		}
		this.OnHealthChanged(-amount);
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x00143530 File Offset: 0x00141730
	private void UpdateWoundEffects()
	{
		if (!this.effects)
		{
			return;
		}
		switch (this.State)
		{
		case Health.HealthState.Perfect:
			this.effects.Remove("LightWounds");
			this.effects.Remove("ModerateWounds");
			this.effects.Remove("SevereWounds");
			return;
		case Health.HealthState.Alright:
			this.effects.Remove("LightWounds");
			this.effects.Remove("ModerateWounds");
			this.effects.Remove("SevereWounds");
			return;
		case Health.HealthState.Scuffed:
			this.effects.Remove("ModerateWounds");
			this.effects.Remove("SevereWounds");
			if (!this.effects.HasEffect("LightWounds"))
			{
				this.effects.Add("LightWounds", true);
				return;
			}
			break;
		case Health.HealthState.Injured:
			this.effects.Remove("LightWounds");
			this.effects.Remove("SevereWounds");
			if (!this.effects.HasEffect("ModerateWounds"))
			{
				this.effects.Add("ModerateWounds", true);
				return;
			}
			break;
		case Health.HealthState.Critical:
			this.effects.Remove("LightWounds");
			this.effects.Remove("ModerateWounds");
			if (!this.effects.HasEffect("SevereWounds"))
			{
				this.effects.Add("SevereWounds", true);
				return;
			}
			break;
		case Health.HealthState.Incapacitated:
			this.effects.Remove("LightWounds");
			this.effects.Remove("ModerateWounds");
			this.effects.Remove("SevereWounds");
			return;
		case Health.HealthState.Dead:
			this.effects.Remove("LightWounds");
			this.effects.Remove("ModerateWounds");
			this.effects.Remove("SevereWounds");
			break;
		default:
			return;
		}
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x00143710 File Offset: 0x00141910
	private void UpdateStatus()
	{
		float num = this.hitPoints / this.maxHitPoints;
		Health.HealthState healthState;
		if (this.State == Health.HealthState.Invincible)
		{
			healthState = Health.HealthState.Invincible;
		}
		else if (num >= 1f)
		{
			healthState = Health.HealthState.Perfect;
		}
		else if (num >= 0.85f)
		{
			healthState = Health.HealthState.Alright;
		}
		else if (num >= 0.66f)
		{
			healthState = Health.HealthState.Scuffed;
		}
		else if ((double)num >= 0.33)
		{
			healthState = Health.HealthState.Injured;
		}
		else if (num > 0f)
		{
			healthState = Health.HealthState.Critical;
		}
		else if (num == 0f)
		{
			healthState = Health.HealthState.Incapacitated;
		}
		else
		{
			healthState = Health.HealthState.Dead;
		}
		if (this.State != healthState)
		{
			if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
			{
				this.Recover();
			}
			if (healthState == Health.HealthState.Perfect)
			{
				base.Trigger(-1491582671, this);
			}
			this.State = healthState;
			KSelectable component = base.GetComponent<KSelectable>();
			if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, this.State);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null, null);
		}
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x00143826 File Offset: 0x00141A26
	public bool IsIncapacitated()
	{
		return this.State == Health.HealthState.Incapacitated;
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x00143831 File Offset: 0x00141A31
	public bool IsDefeated()
	{
		return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x00143847 File Offset: 0x00141A47
	public void Incapacitate(Tag cause)
	{
		this.State = Health.HealthState.Incapacitated;
		base.GetComponent<KPrefabID>().AddTag(cause, false);
		this.Damage(this.hitPoints);
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x00143869 File Offset: 0x00141A69
	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}

	// Token: 0x0400269A RID: 9882
	[Serialize]
	public bool CanBeIncapacitated;

	// Token: 0x0400269B RID: 9883
	[Serialize]
	public Health.HealthState State;

	// Token: 0x0400269C RID: 9884
	[Serialize]
	private Death source_of_death;

	// Token: 0x0400269D RID: 9885
	public HealthBar healthBar;

	// Token: 0x0400269E RID: 9886
	private Effects effects;

	// Token: 0x0400269F RID: 9887
	private AmountInstance amountInstance;

	// Token: 0x020015D3 RID: 5587
	public enum HealthState
	{
		// Token: 0x040069A3 RID: 27043
		Perfect,
		// Token: 0x040069A4 RID: 27044
		Alright,
		// Token: 0x040069A5 RID: 27045
		Scuffed,
		// Token: 0x040069A6 RID: 27046
		Injured,
		// Token: 0x040069A7 RID: 27047
		Critical,
		// Token: 0x040069A8 RID: 27048
		Incapacitated,
		// Token: 0x040069A9 RID: 27049
		Dead,
		// Token: 0x040069AA RID: 27050
		Invincible
	}
}
