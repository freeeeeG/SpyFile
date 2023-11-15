using System;
using UnityEngine;

// Token: 0x0200041C RID: 1052
[Serializable]
public class PlatingStepData : ScriptableObject
{
	// Token: 0x060012E3 RID: 4835 RVA: 0x00069FF1 File Offset: 0x000683F1
	public override int GetHashCode()
	{
		return this.m_uID.GetHashCode();
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0006A004 File Offset: 0x00068404
	public override bool Equals(object other)
	{
		return this.Equals(other as PlatingStepData);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x0006A012 File Offset: 0x00068412
	public bool Equals(PlatingStepData other)
	{
		return !object.ReferenceEquals(other, null) && (object.ReferenceEquals(other, this) || this.m_uID == other.m_uID);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x0006A03E File Offset: 0x0006843E
	public static bool operator ==(PlatingStepData lhs, PlatingStepData rhs)
	{
		if (object.ReferenceEquals(lhs, null))
		{
			return object.ReferenceEquals(rhs, null);
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x0006A05B File Offset: 0x0006845B
	public static bool operator !=(PlatingStepData lhs, PlatingStepData rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x04000EFD RID: 3837
	public SubTexture2D m_icon;

	// Token: 0x04000EFE RID: 3838
	public Sprite m_iconSprite;

	// Token: 0x04000EFF RID: 3839
	public GameOneShotAudioTag m_addToSound;

	// Token: 0x04000F00 RID: 3840
	[SelfAssignID(true)]
	public int m_uID;
}
