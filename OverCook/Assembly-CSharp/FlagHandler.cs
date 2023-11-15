using System;
using GameModes.Horde;
using Team17.Online;
using UnityEngine;

// Token: 0x02000BA4 RID: 2980
public class FlagHandler : MonoBehaviour
{
	// Token: 0x06003D09 RID: 15625 RVA: 0x00123920 File Offset: 0x00121D20
	private void Start()
	{
		if (this.m_levelMapNode != null)
		{
			GameProgress.GameProgressData.LevelProgress levelProgress = this.m_levelMapNode.GetLevelProgress();
			if (levelProgress != null)
			{
				int num = ClientUserSystem.m_Users.Count;
				if (num == 0)
				{
					num = 1;
				}
				int levelIndex = this.m_levelMapNode.LevelIndex;
				SceneDirectoryData sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
				SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelIndex];
				SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(num);
				LevelConfigBase levelConfigBase = (sceneVarient == null) ? null : sceneVarient.LevelConfig;
				if (this.m_renderer != null)
				{
					Material[] sharedMaterials = this.m_renderer.sharedMaterials;
					if (sharedMaterials.Length > this.m_lightMaterialIndex)
					{
						if ((sceneVarient != null && null != levelConfigBase && levelConfigBase.m_objectives != null && levelConfigBase.m_objectives.Length == 0) || levelProgress.ObjectivesCompleted)
						{
							sharedMaterials[this.m_lightMaterialIndex] = this.m_lightOff;
						}
						else
						{
							sharedMaterials[this.m_lightMaterialIndex] = this.m_lightOn;
						}
					}
					this.m_renderer.sharedMaterials = sharedMaterials;
				}
				if (this.m_mesh != null)
				{
					if (levelProgress.Completed)
					{
						if (levelConfigBase != null && levelConfigBase as HordeLevelConfig != null)
						{
							int num2 = 0;
							int highScore = levelProgress.HighScore;
							if (highScore != -2147483648)
							{
								int requiredBitCount = GameUtils.GetRequiredBitCount(65535);
								float num3 = FloatUtils.FromUnorm(highScore, requiredBitCount);
								if (num3 > 0f && num3 < 1f)
								{
									num2 = 1 + (int)Mathf.Round(MathUtils.ClampedRemap(num3, 0f, 1f, -0.49f, (float)(this.m_completeMeshs.Length - 2) - 0.51f));
								}
								else if (num3 >= 1f)
								{
									num2 = this.m_completeMeshs.Length - 1;
								}
							}
							this.m_mesh.mesh = this.m_completeMeshs[num2];
						}
						else
						{
							GameSession gameSession = GameUtils.GetGameSession();
							int b = (!gameSession.Progress.SaveData.IsNGPEnabledForLevel(this.m_levelMapNode.LevelIndex)) ? 3 : 4;
							this.m_mesh.mesh = this.m_completeMeshs[Mathf.Min(levelProgress.ScoreStars, b)];
						}
					}
					else
					{
						this.m_mesh.mesh = this.m_unCompleteMesh;
					}
				}
			}
		}
	}

	// Token: 0x0400311B RID: 12571
	[SerializeField]
	private LevelPortalMapNode m_levelMapNode;

	// Token: 0x0400311C RID: 12572
	[SerializeField]
	private MeshRenderer m_renderer;

	// Token: 0x0400311D RID: 12573
	[SerializeField]
	private MeshFilter m_mesh;

	// Token: 0x0400311E RID: 12574
	[Space]
	[SerializeField]
	private Mesh m_unCompleteMesh;

	// Token: 0x0400311F RID: 12575
	[SerializeField]
	private Mesh[] m_completeMeshs;

	// Token: 0x04003120 RID: 12576
	[Space]
	[SerializeField]
	private int m_lightMaterialIndex = 1;

	// Token: 0x04003121 RID: 12577
	[SerializeField]
	private Material m_lightOff;

	// Token: 0x04003122 RID: 12578
	[SerializeField]
	private Material m_lightOn;
}
