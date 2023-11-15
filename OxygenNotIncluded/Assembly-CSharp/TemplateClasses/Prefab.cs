using System;
using System.Collections.Generic;

namespace TemplateClasses
{
	// Token: 0x02000CC7 RID: 3271
	[Serializable]
	public class Prefab
	{
		// Token: 0x06006896 RID: 26774 RVA: 0x00278FBB File Offset: 0x002771BB
		public Prefab()
		{
			this.type = Prefab.Type.Other;
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x00278FCC File Offset: 0x002771CC
		public Prefab(string _id, Prefab.Type _type, int loc_x, int loc_y, SimHashes _element, float _temperature = -1f, float _units = 1f, string _disease = null, int _disease_count = 0, Orientation _rotation = Orientation.Neutral, Prefab.template_amount_value[] _amount_values = null, Prefab.template_amount_value[] _other_values = null, int _connections = 0)
		{
			this.id = _id;
			this.type = _type;
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.connections = _connections;
			this.element = _element;
			this.temperature = _temperature;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.rotationOrientation = _rotation;
			if (_amount_values != null && _amount_values.Length != 0)
			{
				this.amounts = _amount_values;
			}
			if (_other_values != null && _other_values.Length != 0)
			{
				this.other_values = _other_values;
			}
		}

		// Token: 0x06006898 RID: 26776 RVA: 0x00279058 File Offset: 0x00277258
		public Prefab Clone(Vector2I offset)
		{
			Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections);
			if (this.rottable != null)
			{
				prefab.rottable = new Rottable();
				prefab.rottable.rotAmount = this.rottable.rotAmount;
			}
			if (this.storage != null && this.storage.Count > 0)
			{
				prefab.storage = new List<StorageItem>();
				foreach (StorageItem storageItem in this.storage)
				{
					prefab.storage.Add(storageItem.Clone());
				}
			}
			return prefab;
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x00279164 File Offset: 0x00277364
		public void AssignStorage(StorageItem _storage)
		{
			if (this.storage == null)
			{
				this.storage = new List<StorageItem>();
			}
			this.storage.Add(_storage);
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600689A RID: 26778 RVA: 0x00279185 File Offset: 0x00277385
		// (set) Token: 0x0600689B RID: 26779 RVA: 0x0027918D File Offset: 0x0027738D
		public string id { get; set; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600689C RID: 26780 RVA: 0x00279196 File Offset: 0x00277396
		// (set) Token: 0x0600689D RID: 26781 RVA: 0x0027919E File Offset: 0x0027739E
		public int location_x { get; set; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600689E RID: 26782 RVA: 0x002791A7 File Offset: 0x002773A7
		// (set) Token: 0x0600689F RID: 26783 RVA: 0x002791AF File Offset: 0x002773AF
		public int location_y { get; set; }

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060068A0 RID: 26784 RVA: 0x002791B8 File Offset: 0x002773B8
		// (set) Token: 0x060068A1 RID: 26785 RVA: 0x002791C0 File Offset: 0x002773C0
		public SimHashes element { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060068A2 RID: 26786 RVA: 0x002791C9 File Offset: 0x002773C9
		// (set) Token: 0x060068A3 RID: 26787 RVA: 0x002791D1 File Offset: 0x002773D1
		public float temperature { get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060068A4 RID: 26788 RVA: 0x002791DA File Offset: 0x002773DA
		// (set) Token: 0x060068A5 RID: 26789 RVA: 0x002791E2 File Offset: 0x002773E2
		public float units { get; set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060068A6 RID: 26790 RVA: 0x002791EB File Offset: 0x002773EB
		// (set) Token: 0x060068A7 RID: 26791 RVA: 0x002791F3 File Offset: 0x002773F3
		public string diseaseName { get; set; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060068A8 RID: 26792 RVA: 0x002791FC File Offset: 0x002773FC
		// (set) Token: 0x060068A9 RID: 26793 RVA: 0x00279204 File Offset: 0x00277404
		public int diseaseCount { get; set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060068AA RID: 26794 RVA: 0x0027920D File Offset: 0x0027740D
		// (set) Token: 0x060068AB RID: 26795 RVA: 0x00279215 File Offset: 0x00277415
		public Orientation rotationOrientation { get; set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060068AC RID: 26796 RVA: 0x0027921E File Offset: 0x0027741E
		// (set) Token: 0x060068AD RID: 26797 RVA: 0x00279226 File Offset: 0x00277426
		public List<StorageItem> storage { get; set; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060068AE RID: 26798 RVA: 0x0027922F File Offset: 0x0027742F
		// (set) Token: 0x060068AF RID: 26799 RVA: 0x00279237 File Offset: 0x00277437
		public Prefab.Type type { get; set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060068B0 RID: 26800 RVA: 0x00279240 File Offset: 0x00277440
		// (set) Token: 0x060068B1 RID: 26801 RVA: 0x00279248 File Offset: 0x00277448
		public int connections { get; set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060068B2 RID: 26802 RVA: 0x00279251 File Offset: 0x00277451
		// (set) Token: 0x060068B3 RID: 26803 RVA: 0x00279259 File Offset: 0x00277459
		public Rottable rottable { get; set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060068B4 RID: 26804 RVA: 0x00279262 File Offset: 0x00277462
		// (set) Token: 0x060068B5 RID: 26805 RVA: 0x0027926A File Offset: 0x0027746A
		public Prefab.template_amount_value[] amounts { get; set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060068B6 RID: 26806 RVA: 0x00279273 File Offset: 0x00277473
		// (set) Token: 0x060068B7 RID: 26807 RVA: 0x0027927B File Offset: 0x0027747B
		public Prefab.template_amount_value[] other_values { get; set; }

		// Token: 0x02001C0D RID: 7181
		public enum Type
		{
			// Token: 0x04007EBA RID: 32442
			Building,
			// Token: 0x04007EBB RID: 32443
			Ore,
			// Token: 0x04007EBC RID: 32444
			Pickupable,
			// Token: 0x04007EBD RID: 32445
			Other
		}

		// Token: 0x02001C0E RID: 7182
		[Serializable]
		public class template_amount_value
		{
			// Token: 0x06009B86 RID: 39814 RVA: 0x00349C06 File Offset: 0x00347E06
			public template_amount_value()
			{
			}

			// Token: 0x06009B87 RID: 39815 RVA: 0x00349C0E File Offset: 0x00347E0E
			public template_amount_value(string id, float value)
			{
				this.id = id;
				this.value = value;
			}

			// Token: 0x17000A59 RID: 2649
			// (get) Token: 0x06009B88 RID: 39816 RVA: 0x00349C24 File Offset: 0x00347E24
			// (set) Token: 0x06009B89 RID: 39817 RVA: 0x00349C2C File Offset: 0x00347E2C
			public string id { get; set; }

			// Token: 0x17000A5A RID: 2650
			// (get) Token: 0x06009B8A RID: 39818 RVA: 0x00349C35 File Offset: 0x00347E35
			// (set) Token: 0x06009B8B RID: 39819 RVA: 0x00349C3D File Offset: 0x00347E3D
			public float value { get; set; }
		}
	}
}
