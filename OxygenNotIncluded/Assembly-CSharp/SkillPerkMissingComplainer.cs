using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
[AddComponentMenu("KMonoBehaviour/scripts/SkillPerkMissingComplainer")]
public class SkillPerkMissingComplainer : KMonoBehaviour
{
	// Token: 0x06001E34 RID: 7732 RVA: 0x000A120B File Offset: 0x0009F40B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x000A1249 File Offset: 0x0009F449
	protected override void OnCleanUp()
	{
		if (this.skillUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillUpdateHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x000A126C File Offset: 0x0009F46C
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			return;
		}
		bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId());
		if (!flag && this.workStatusItemHandle == Guid.Empty)
		{
			this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, this.requiredSkillPerk);
			return;
		}
		if (flag && this.workStatusItemHandle != Guid.Empty)
		{
			component.RemoveStatusItem(this.workStatusItemHandle, false);
			this.workStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x040010E8 RID: 4328
	public string requiredSkillPerk;

	// Token: 0x040010E9 RID: 4329
	private int skillUpdateHandle = -1;

	// Token: 0x040010EA RID: 4330
	private Guid workStatusItemHandle;
}
