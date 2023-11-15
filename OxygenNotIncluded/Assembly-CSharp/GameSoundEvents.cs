using System;

// Token: 0x020007D0 RID: 2000
public static class GameSoundEvents
{
	// Token: 0x04002467 RID: 9319
	public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");

	// Token: 0x04002468 RID: 9320
	public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");

	// Token: 0x04002469 RID: 9321
	public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

	// Token: 0x02001565 RID: 5477
	public class Event
	{
		// Token: 0x060087AB RID: 34731 RVA: 0x0030BE4B File Offset: 0x0030A04B
		public Event(string name)
		{
			this.Name = name;
		}

		// Token: 0x0400680C RID: 26636
		public HashedString Name;
	}
}
