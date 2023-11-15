using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
[Serializable]
public class TalentSetting
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000107 RID: 263 RVA: 0x0000526F File Offset: 0x0000346F
	public eTalentType TalentType
	{
		get
		{
			return this.talentType;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000108 RID: 264 RVA: 0x00005277 File Offset: 0x00003477
	public int ExpCost
	{
		get
		{
			return this.expCost;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000109 RID: 265 RVA: 0x0000527F File Offset: 0x0000347F
	public Sprite Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600010A RID: 266 RVA: 0x00005287 File Offset: 0x00003487
	public bool LockInDemoVersion
	{
		get
		{
			return this.lockInDemoVersion;
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000528F File Offset: 0x0000348F
	public void SetTalentType(eTalentType type)
	{
		this.talentType = type;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00005298 File Offset: 0x00003498
	public void SetIcon(Sprite sprite)
	{
		this.icon = sprite;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000052A1 File Offset: 0x000034A1
	public void SetLockInDemoVersion(bool isLock)
	{
		this.lockInDemoVersion = isLock;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x000052AA File Offset: 0x000034AA
	private void CopyTalentTypeToClip()
	{
		GUIUtility.systemCopyBuffer = this.talentType.ToString();
	}

	// Token: 0x040000BA RID: 186
	[SerializeField]
	private eTalentType talentType;

	// Token: 0x040000BB RID: 187
	[SerializeField]
	private int expCost;

	// Token: 0x040000BC RID: 188
	[SerializeField]
	private Sprite icon;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private bool lockInDemoVersion;
}
