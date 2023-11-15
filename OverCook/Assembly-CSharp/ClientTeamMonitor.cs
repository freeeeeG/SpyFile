using System;
using UnityEngine;

// Token: 0x02000779 RID: 1913
public class ClientTeamMonitor
{
	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000AEBC2 File Offset: 0x000ACFC2
	public TeamMonitor.TeamScoreStats Score
	{
		get
		{
			return this.m_score;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000AEBCA File Offset: 0x000ACFCA
	public ClientOrderControllerBase OrdersController
	{
		get
		{
			return this.m_ordersController;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060024F5 RID: 9461 RVA: 0x000AEBD2 File Offset: 0x000ACFD2
	public PlayerInputLookup.Player[] Members
	{
		get
		{
			return this.m_players;
		}
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000AEBDA File Offset: 0x000ACFDA
	public virtual void Update()
	{
		this.m_ordersController.Update();
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x000AEBE8 File Offset: 0x000ACFE8
	public virtual void Initialise(TeamMonitor _monitor, TeamID _teamID, ClientOrderControllerBuilder _controllerBuilder)
	{
		this.m_monitor = _monitor;
		this.m_ordersController = _controllerBuilder(this.m_monitor.m_recipeBarUIController);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		array = array.FindAll((GameObject x) => x.RequireComponent<PlayerIDProvider>().GetTeam() == _teamID);
		this.m_players = array.ConvertAll((GameObject x) => x.RequireComponent<PlayerIDProvider>().GetID());
	}

	// Token: 0x04001C81 RID: 7297
	private TeamMonitor m_monitor;

	// Token: 0x04001C82 RID: 7298
	private ClientOrderControllerBase m_ordersController;

	// Token: 0x04001C83 RID: 7299
	private PlayerInputLookup.Player[] m_players;

	// Token: 0x04001C84 RID: 7300
	private TeamMonitor.TeamScoreStats m_score = new TeamMonitor.TeamScoreStats();
}
