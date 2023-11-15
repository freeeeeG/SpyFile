using System;

namespace Characters
{
	// Token: 0x0200069D RID: 1693
	public interface ITrigger
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060021CB RID: 8651
		// (remove) Token: 0x060021CC RID: 8652
		event Action onTriggered;

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060021CD RID: 8653
		float cooldownTime { get; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060021CE RID: 8654
		float remainCooldownTime { get; }

		// Token: 0x060021CF RID: 8655
		void Attach(Character character);

		// Token: 0x060021D0 RID: 8656
		void Detach();

		// Token: 0x060021D1 RID: 8657
		void UpdateTime(float deltaTime);

		// Token: 0x060021D2 RID: 8658
		void Refresh();
	}
}
