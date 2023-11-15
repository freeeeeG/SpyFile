using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000019 RID: 25
	public interface MB2_EditorMethodsInterface
	{
		// Token: 0x0600003A RID: 58
		void Clear();

		// Token: 0x0600003B RID: 59
		void RestoreReadFlagsAndFormats(ProgressUpdateDelegate progressInfo);

		// Token: 0x0600003C RID: 60
		void SetReadWriteFlag(Texture2D tx, bool isReadable, bool addToList);

		// Token: 0x0600003D RID: 61
		void AddTextureFormat(Texture2D tx, bool isNormalMap);

		// Token: 0x0600003E RID: 62
		void SaveAtlasToAssetDatabase(Texture2D atlas, ShaderTextureProperty texPropertyName, int atlasNum, Material resMat);

		// Token: 0x0600003F RID: 63
		void SetMaterialTextureProperty(Material target, ShaderTextureProperty texPropName, string texturePath);

		// Token: 0x06000040 RID: 64
		void SetNormalMap(Texture2D tx);

		// Token: 0x06000041 RID: 65
		bool IsNormalMap(Texture2D tx);

		// Token: 0x06000042 RID: 66
		string GetPlatformString();

		// Token: 0x06000043 RID: 67
		void SetTextureSize(Texture2D tx, int size);

		// Token: 0x06000044 RID: 68
		bool IsCompressed(Texture2D tx);

		// Token: 0x06000045 RID: 69
		void CheckBuildSettings(long estimatedAtlasSize);

		// Token: 0x06000046 RID: 70
		bool CheckPrefabTypes(MB_ObjsToCombineTypes prefabType, List<GameObject> gos);

		// Token: 0x06000047 RID: 71
		bool ValidateSkinnedMeshes(List<GameObject> mom);

		// Token: 0x06000048 RID: 72
		void CommitChangesToAssets();

		// Token: 0x06000049 RID: 73
		void Destroy(UnityEngine.Object o);
	}
}
