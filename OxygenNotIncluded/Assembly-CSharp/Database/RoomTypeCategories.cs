using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D17 RID: 3351
	public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
	{
		// Token: 0x060069ED RID: 27117 RVA: 0x00292160 File Offset: 0x00290360
		private RoomTypeCategory Add(string id, string name, string colorName, string icon)
		{
			RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
			base.Add(roomTypeCategory);
			return roomTypeCategory;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x00292184 File Offset: 0x00290384
		public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent)
		{
			base.Initialize();
			this.None = this.Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
			this.Food = this.Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
			this.Sleep = this.Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
			this.Recreation = this.Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
			this.Bathroom = this.Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
			this.Hospital = this.Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
			this.Industrial = this.Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
			this.Agricultural = this.Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
			this.Park = this.Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
			this.Science = this.Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
		}

		// Token: 0x04004CBA RID: 19642
		public RoomTypeCategory None;

		// Token: 0x04004CBB RID: 19643
		public RoomTypeCategory Food;

		// Token: 0x04004CBC RID: 19644
		public RoomTypeCategory Sleep;

		// Token: 0x04004CBD RID: 19645
		public RoomTypeCategory Recreation;

		// Token: 0x04004CBE RID: 19646
		public RoomTypeCategory Bathroom;

		// Token: 0x04004CBF RID: 19647
		public RoomTypeCategory Hospital;

		// Token: 0x04004CC0 RID: 19648
		public RoomTypeCategory Industrial;

		// Token: 0x04004CC1 RID: 19649
		public RoomTypeCategory Agricultural;

		// Token: 0x04004CC2 RID: 19650
		public RoomTypeCategory Park;

		// Token: 0x04004CC3 RID: 19651
		public RoomTypeCategory Science;
	}
}
