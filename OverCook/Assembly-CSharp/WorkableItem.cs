using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
[AddComponentMenu("Scripts/Game/Items/WorkableItem")]
public class WorkableItem : MonoBehaviour
{
	// Token: 0x060027F4 RID: 10228 RVA: 0x000BB505 File Offset: 0x000B9905
	protected virtual void Awake()
	{
		this.m_iAnimationVariable = Animator.StringToHash(this.m_animationVariable);
		this.m_iGatherVariable = Animator.StringToHash(this.m_gatherVariable);
		this.m_iDropVariable = Animator.StringToHash(this.m_dropVariable);
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000BB53A File Offset: 0x000B993A
	public GameObject GetNextPrefab()
	{
		return this.m_nextPrefab;
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000BB544 File Offset: 0x000B9944
	public int GetChopTimeMultiplier(int _playerCount)
	{
		GameConfig gameConfig = GameUtils.GetGameConfig();
		if (gameConfig != null)
		{
			int result = 1;
			GameSession gameSession = GameUtils.GetGameSession();
			GameSession.GameType type = gameSession.TypeSettings.Type;
			if (type != GameSession.GameType.Cooperative)
			{
				if (type == GameSession.GameType.Competitive)
				{
					result = ((_playerCount >= 4) ? 1 : gameConfig.SingleplayerChopTimeMultiplier);
				}
			}
			else
			{
				result = ((_playerCount != 1) ? 1 : gameConfig.SingleplayerChopTimeMultiplier);
			}
			return result;
		}
		return 1;
	}

	// Token: 0x04001F61 RID: 8033
	[SerializeField]
	public ProgressUIController m_progressUIPrefab;

	// Token: 0x04001F62 RID: 8034
	[SerializeField]
	public GameObject m_nextPrefab;

	// Token: 0x04001F63 RID: 8035
	[SerializeField]
	public int m_stages = 8;

	// Token: 0x04001F64 RID: 8036
	[SerializeField]
	public string m_animationVariable = "Progress";

	// Token: 0x04001F65 RID: 8037
	[SerializeField]
	public string m_gatherVariable = "Gather";

	// Token: 0x04001F66 RID: 8038
	[SerializeField]
	public string m_dropVariable = "Drop";

	// Token: 0x04001F67 RID: 8039
	[NonSerialized]
	public int m_iAnimationVariable;

	// Token: 0x04001F68 RID: 8040
	[NonSerialized]
	public int m_iGatherVariable;

	// Token: 0x04001F69 RID: 8041
	[NonSerialized]
	public int m_iDropVariable;
}
