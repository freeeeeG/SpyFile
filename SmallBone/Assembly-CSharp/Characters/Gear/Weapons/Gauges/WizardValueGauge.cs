using System;

namespace Characters.Gear.Weapons.Gauges
{
	// Token: 0x02000839 RID: 2105
	public sealed class WizardValueGauge : ValueGauge
	{
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x00086656 File Offset: 0x00084856
		protected override string maxValueText
		{
			get
			{
				return this._currentValue.ToString();
			}
		}
	}
}
