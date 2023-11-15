using System;
using Game.General.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Tantawowa.Demo.DemoScripts
{
	// Token: 0x02000079 RID: 121
	public class Robot : MonoBehaviour
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007FA3 File Offset: 0x000061A3
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007FAB File Offset: 0x000061AB
		public int Points
		{
			get
			{
				return this.points;
			}
			set
			{
				this.points = value;
				this.Message.text = ((this.points > 0) ? this.points.ToString() : "");
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00007FDA File Offset: 0x000061DA
		public void AddScore(int points)
		{
			this.Points += points;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007FEA File Offset: 0x000061EA
		public void ResetScore()
		{
			this.Points = 0;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007FF4 File Offset: 0x000061F4
		public void SetState(RobotState state)
		{
			this.currentState = state;
			switch (state)
			{
			case RobotState.Sleeping:
				this.Agent.speed = 0f;
				this.NavMeshFollower.Target = null;
				return;
			case RobotState.GoToWork:
				this.NavMeshFollower.Target = this.Work;
				this.Agent.speed = 3f;
				return;
			case RobotState.GoHome:
				this.NavMeshFollower.Target = this.Home;
				this.Agent.speed = 6f;
				return;
			default:
				throw new ArgumentOutOfRangeException("state", state, null);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008090 File Offset: 0x00006290
		private void Update()
		{
			Vector3 b = Camera.main.transform.position - base.transform.position;
			b.x = (b.z = 0f);
			this.Message.transform.LookAt(Camera.main.transform.position - b);
			this.Message.transform.Rotate(0f, 180f, 0f);
		}

		// Token: 0x040001A6 RID: 422
		[SerializeField]
		private int points;

		// Token: 0x040001A7 RID: 423
		[SerializeField]
		private RobotState currentState;

		// Token: 0x040001A8 RID: 424
		public TextMesh Message;

		// Token: 0x040001A9 RID: 425
		public NavMeshAgent Agent;

		// Token: 0x040001AA RID: 426
		public NavMeshFollower NavMeshFollower;

		// Token: 0x040001AB RID: 427
		public Transform Work;

		// Token: 0x040001AC RID: 428
		public Transform Home;
	}
}
