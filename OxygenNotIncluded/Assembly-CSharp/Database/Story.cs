using System;
using ProcGen;

namespace Database
{
	// Token: 0x02000D2C RID: 3372
	public class Story : Resource, IComparable<Story>
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x002981BB File Offset: 0x002963BB
		// (set) Token: 0x06006A39 RID: 27193 RVA: 0x002981C3 File Offset: 0x002963C3
		public int HashId { get; private set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x002981CC File Offset: 0x002963CC
		public WorldTrait StoryTrait
		{
			get
			{
				if (this._cachedStoryTrait == null)
				{
					this._cachedStoryTrait = SettingsCache.GetCachedStoryTrait(this.worldgenStoryTraitKey, false);
				}
				return this._cachedStoryTrait;
			}
		}

		// Token: 0x06006A3B RID: 27195 RVA: 0x002981EE File Offset: 0x002963EE
		public Story(string id, string worldgenStoryTraitKey, int displayOrder)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.kleiUseOnlyCoordinateOffset = -1;
			this.updateNumber = -1;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06006A3C RID: 27196 RVA: 0x00298228 File Offset: 0x00296428
		public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOffset, int updateNumber)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.updateNumber = updateNumber;
			DebugUtil.Assert(kleiUseOnlyCoordinateOffset < 20, "More than 19 stories is unsupported!");
			this.kleiUseOnlyCoordinateOffset = kleiUseOnlyCoordinateOffset;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x0029827C File Offset: 0x0029647C
		public int CompareTo(Story other)
		{
			return this.displayOrder.CompareTo(other.displayOrder);
		}

		// Token: 0x06006A3E RID: 27198 RVA: 0x0029829D File Offset: 0x0029649D
		public bool IsNew()
		{
			return this.updateNumber == LaunchInitializer.UpdateNumber();
		}

		// Token: 0x06006A3F RID: 27199 RVA: 0x002982AC File Offset: 0x002964AC
		public Story AutoStart()
		{
			this.autoStart = true;
			return this;
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x002982B6 File Offset: 0x002964B6
		public Story SetKeepsake(string prefabId)
		{
			this.keepsakePrefabId = prefabId;
			return this;
		}

		// Token: 0x04004D8B RID: 19851
		public const int MODDED_STORY = -1;

		// Token: 0x04004D8C RID: 19852
		public int kleiUseOnlyCoordinateOffset;

		// Token: 0x04004D8E RID: 19854
		public bool autoStart;

		// Token: 0x04004D8F RID: 19855
		public string keepsakePrefabId;

		// Token: 0x04004D90 RID: 19856
		public readonly string worldgenStoryTraitKey;

		// Token: 0x04004D91 RID: 19857
		private readonly int displayOrder;

		// Token: 0x04004D92 RID: 19858
		private readonly int updateNumber;

		// Token: 0x04004D93 RID: 19859
		private WorldTrait _cachedStoryTrait;
	}
}
