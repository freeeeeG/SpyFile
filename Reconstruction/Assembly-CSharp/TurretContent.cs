using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022F RID: 559
public abstract class TurretContent : ConcreteContent
{
	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00024AE6 File Offset: 0x00022CE6
	// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00024AEE File Offset: 0x00022CEE
	public float NextAttackTime
	{
		get
		{
			return this.nextAttackTime;
		}
		set
		{
			this.nextAttackTime = value;
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00024AF7 File Offset: 0x00022CF7
	protected override void Awake()
	{
		base.Awake();
		this.turretAnim = base.GetComponent<Animator>();
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.outputAudioMixerGroup = Singleton<Sound>.Instance.effectMixer;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00024B2C File Offset: 0x00022D2C
	public virtual void InitializeTurret(StrategyBase strategy)
	{
		this.Strategy = strategy;
		this.Strategy.Concrete = this;
		this.rotTrans.localRotation = Quaternion.identity;
		this.bulletPrefab = this.Strategy.Attribute.Bullet;
		this.ShootClip = this.Strategy.Attribute.ShootSound;
		this.SetGraphic();
		this.GenerateRange();
		this.Activated = true;
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00024B9C File Offset: 0x00022D9C
	protected virtual void PlayAudio(AudioClip clip, bool isAuto)
	{
		this.audioSource.volume = StaticData.EnvrionmentBaseVolume;
		this.audioSource.clip = clip;
		if (isAuto)
		{
			this.audioSource.loop = true;
			this.audioSource.Play();
			return;
		}
		this.audioSource.PlayOneShot(clip);
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00024BEC File Offset: 0x00022DEC
	public override void SetGraphic()
	{
		this.shootPoint.transform.localPosition = this.Strategy.Attribute.TurretLevels[this.Strategy.Quality - 1].ShootPointOffset;
		this.CannonSprite.sprite = this.Strategy.Attribute.TurretLevels[this.Strategy.Quality - 1].CannonSprite;
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00024C67 File Offset: 0x00022E67
	protected override void OnActivating()
	{
		base.OnActivating();
		if (this.TrackTarget() || this.AcquireTarget())
		{
			if (!this.Activated)
			{
				return;
			}
			this.RotateTowards();
			this.FireProjectile();
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00024C94 File Offset: 0x00022E94
	protected virtual bool TrackTarget()
	{
		for (int i = 0; i < base.Target.Count; i++)
		{
			if (!base.Target[i].gameObject.activeSelf)
			{
				this.targetList.Remove(base.Target[i]);
				base.Target.Remove(base.Target[i]);
			}
		}
		return base.Target.Count >= this.Strategy.FinalTargetCount;
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00024D1C File Offset: 0x00022F1C
	private bool AcquireTarget()
	{
		if (this.targetList.Count <= 0)
		{
			return false;
		}
		base.Target.Clear();
		foreach (int index in StaticData.SelectNoRepeat(this.targetList.Count, this.Strategy.FinalTargetCount))
		{
			base.Target.Add(this.targetList[index]);
		}
		return base.Target.Count > 0;
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00024D9C File Offset: 0x00022F9C
	protected virtual void RotateTowards()
	{
		if (base.Target.Count == 0)
		{
			return;
		}
		Vector3 vector = base.Target[0].transform.position - this.rotTrans.position;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f;
		this.look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.rotTrans.rotation = Quaternion.LerpUnclamped(this.rotTrans.rotation, this.look_Rotation, this.Strategy.RotSpeed * Time.deltaTime);
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00024E3F File Offset: 0x0002303F
	protected virtual bool AngleCheck()
	{
		return Quaternion.Angle(this.rotTrans.rotation, this.look_Rotation) < this.Strategy.CheckAngle;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00024E68 File Offset: 0x00023068
	protected virtual void FireProjectile()
	{
		if (Time.time - this.NextAttackTime > 1f / this.Strategy.FinalFireRate)
		{
			if (base.Target == null || !this.AngleCheck())
			{
				return;
			}
			this.Shoot();
			this.NextAttackTime = Time.time;
		}
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00024EB8 File Offset: 0x000230B8
	protected virtual void Shoot()
	{
		this.turretAnim.SetTrigger("Attack");
		this.ShootEffect.Play();
		this.PlayAudio(this.ShootClip, false);
		List<TargetPoint>.Enumerator enumerator = base.Target.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Bullet bullet = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab) as Bullet;
			bullet.transform.position = this.shootPoint.position;
			bullet.Initialize(this, enumerator.Current, null);
		}
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x00024F48 File Offset: 0x00023148
	public override void SaveContent(out ContentStruct contentSruct)
	{
		base.SaveContent(out contentSruct);
		contentSruct = this.m_ContentStruct;
		this.m_ContentStruct.TotalDamage = this.Strategy.TotalDamage.ToString();
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00024F82 File Offset: 0x00023182
	public override void OnContentSelected(bool value)
	{
		base.OnContentSelected(value);
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowTurreTips(this.Strategy, StaticData.LeftTipsPos, 0);
			Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.TurretSelect);
		}
	}

	// Token: 0x040006D7 RID: 1751
	private float nextAttackTime;

	// Token: 0x040006D8 RID: 1752
	protected Quaternion look_Rotation;

	// Token: 0x040006D9 RID: 1753
	protected Bullet bulletPrefab;

	// Token: 0x040006DA RID: 1754
	protected Animator turretAnim;

	// Token: 0x040006DB RID: 1755
	protected AudioSource audioSource;

	// Token: 0x040006DC RID: 1756
	protected AudioClip ShootClip;

	// Token: 0x040006DD RID: 1757
	[SerializeField]
	protected ParticleSystem ShootEffect;

	// Token: 0x040006DE RID: 1758
	private const float invisibleDistance = 3.5f;
}
