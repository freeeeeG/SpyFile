using System;

// Token: 0x0200019E RID: 414
public class FossilMineWorkable : ComplexFabricatorWorkable
{
	// Token: 0x0600082C RID: 2092 RVA: 0x00030037 File Offset: 0x0002E237
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00030048 File Offset: 0x0002E248
	public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
	{
		this.shouldShowSkillPerkStatusItem = shouldItBeShown;
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			this.skillsUpdateHandle = -1;
		}
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}
}
