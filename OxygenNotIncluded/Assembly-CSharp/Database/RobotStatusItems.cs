using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D16 RID: 3350
	public class RobotStatusItems : StatusItems
	{
		// Token: 0x060069EB RID: 27115 RVA: 0x00291DFD File Offset: 0x0028FFFD
		public RobotStatusItems(ResourceSet parent) : base("RobotStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x00291E14 File Offset: 0x00290014
		private void CreateStatusItems()
		{
			this.CantReachStation = new StatusItem("CantReachStation", "ROBOTS", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.CantReachStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBattery = new StatusItem("LowBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBatteryNoCharge = new StatusItem("LowBatteryNoCharge", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBatteryNoCharge.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBattery = new StatusItem("DeadBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DustBinFull = new StatusItem("DustBinFull", "ROBOTS", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.DustBinFull.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.Working = new StatusItem("Working", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.Working.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.MovingToChargeStation = new StatusItem("MovingToChargeStation", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.MovingToChargeStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.UnloadingStorage = new StatusItem("UnloadingStorage", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.UnloadingStorage.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.ReactPositive = new StatusItem("ReactPositive", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactPositive.resolveStringCallback = ((string str, object data) => str);
			this.ReactNegative = new StatusItem("ReactNegative", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactNegative.resolveStringCallback = ((string str, object data) => str);
		}

		// Token: 0x04004CB0 RID: 19632
		public StatusItem LowBattery;

		// Token: 0x04004CB1 RID: 19633
		public StatusItem LowBatteryNoCharge;

		// Token: 0x04004CB2 RID: 19634
		public StatusItem DeadBattery;

		// Token: 0x04004CB3 RID: 19635
		public StatusItem CantReachStation;

		// Token: 0x04004CB4 RID: 19636
		public StatusItem DustBinFull;

		// Token: 0x04004CB5 RID: 19637
		public StatusItem Working;

		// Token: 0x04004CB6 RID: 19638
		public StatusItem UnloadingStorage;

		// Token: 0x04004CB7 RID: 19639
		public StatusItem ReactPositive;

		// Token: 0x04004CB8 RID: 19640
		public StatusItem ReactNegative;

		// Token: 0x04004CB9 RID: 19641
		public StatusItem MovingToChargeStation;
	}
}
