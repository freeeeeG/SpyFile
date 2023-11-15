using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class AutoLogo : MonoBehaviour
{
	// Token: 0x060000B2 RID: 178 RVA: 0x00005574 File Offset: 0x00003774
	private void Awake()
	{
		if (this.image == null)
		{
			this.image = base.gameObject.GetComponent<Image>();
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00005595 File Offset: 0x00003795
	private void Update()
	{
		this.UpdateLanguage();
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00005595 File Offset: 0x00003795
	private void OnEnable()
	{
		this.UpdateLanguage();
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000055A0 File Offset: 0x000037A0
	private void UpdateLanguage()
	{
		if (Setting.Inst == null)
		{
			return;
		}
		if (this.lang != Setting.Inst.language)
		{
			this.lang = Setting.Inst.language;
			EnumLanguage enumLanguage = this.lang;
			if (enumLanguage != EnumLanguage.ENGLISH)
			{
				if (enumLanguage == EnumLanguage.CHINESE_SIM)
				{
					this.image.sprite = ResourceLibrary.Inst.logoCN;
					return;
				}
			}
			else
			{
				this.image.sprite = ResourceLibrary.Inst.logoENG;
			}
		}
	}

	// Token: 0x0400011B RID: 283
	[SerializeField]
	private Image image;

	// Token: 0x0400011C RID: 284
	[SerializeField]
	private EnumLanguage lang = EnumLanguage.UNINITED;
}
