using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
[CreateAssetMenu]
public class SavableAbilityResource : ScriptableObject
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000248 RID: 584 RVA: 0x00009A87 File Offset: 0x00007C87
	public AudioClip curseAttachSound
	{
		get
		{
			return this._curseAttachSound;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000249 RID: 585 RVA: 0x00009A8F File Offset: 0x00007C8F
	public RuntimeAnimatorController curseAttachEffect
	{
		get
		{
			return this._curseAttachEffect;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600024A RID: 586 RVA: 0x00009A97 File Offset: 0x00007C97
	public RuntimeAnimatorController curseAttackEffect
	{
		get
		{
			return this._curseAttackEffect;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600024B RID: 587 RVA: 0x00009A9F File Offset: 0x00007C9F
	public Sprite curseIcon
	{
		get
		{
			return this._curseIcon;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600024C RID: 588 RVA: 0x00009AA7 File Offset: 0x00007CA7
	public Sprite[] fogWolfBuffIcons
	{
		get
		{
			return this._fogWolfBuffIcons;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600024D RID: 589 RVA: 0x00009AAF File Offset: 0x00007CAF
	public RuntimeAnimatorController cloverShieldEffect
	{
		get
		{
			return this._cloverShieldEffect;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600024E RID: 590 RVA: 0x00009AB7 File Offset: 0x00007CB7
	public static SavableAbilityResource instance
	{
		get
		{
			if (SavableAbilityResource._instance == null)
			{
				SavableAbilityResource._instance = Resources.Load<SavableAbilityResource>("SavableAbilityResource");
				SavableAbilityResource._instance.Initialize();
			}
			return SavableAbilityResource._instance;
		}
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00009AE4 File Offset: 0x00007CE4
	private void Initialize()
	{
		this._fogWolfBuffIcons = new Sprite[]
		{
			this._fogWolfPhysicalAttackDamage,
			this._fogWolfMagicalAttackDamage,
			this._fogWolfAttackSpeed,
			this._fogWolfCriticalChance,
			this._fogWolfHealth
		};
	}

	// Token: 0x040001FB RID: 507
	private static SavableAbilityResource _instance;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	[Header("Curse")]
	private AudioClip _curseAttachSound;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	private RuntimeAnimatorController _curseAttachEffect;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private RuntimeAnimatorController _curseAttackEffect;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private Sprite _curseIcon;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	[Header("FogWolf")]
	private Sprite _fogWolfPhysicalAttackDamage;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private Sprite _fogWolfMagicalAttackDamage;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	private Sprite _fogWolfAttackSpeed;

	// Token: 0x04000203 RID: 515
	[SerializeField]
	private Sprite _fogWolfCriticalChance;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	private Sprite _fogWolfHealth;

	// Token: 0x04000205 RID: 517
	[SerializeField]
	[Header("품목순환장치")]
	private RuntimeAnimatorController _cloverShieldEffect;

	// Token: 0x04000206 RID: 518
	private Sprite[] _fogWolfBuffIcons;
}
