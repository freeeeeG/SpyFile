using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.General.Navigation
{
	// Token: 0x0200007B RID: 123
	public class NavMeshFollower : MonoBehaviour
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00008164 File Offset: 0x00006364
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000816C File Offset: 0x0000636C
		public Transform Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
				if (this.Agent != null && this.target != null && base.gameObject.activeInHierarchy)
				{
					this.Agent.enabled = true;
					this.Agent.destination = this.target.position;
					this.Agent.updateRotation = true;
					this.Agent.enabled = base.enabled;
					return;
				}
				this.Agent.enabled = false;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000081F8 File Offset: 0x000063F8
		private void Update()
		{
			if (this.Agent.enabled && this.Agent.remainingDistance <= this.Agent.stoppingDistance)
			{
				this.Agent.enabled = false;
				if (this.OnArrive != null)
				{
					this.OnArrive(this);
				}
			}
		}

		// Token: 0x040001AF RID: 431
		public Action<NavMeshFollower> OnArrive;

		// Token: 0x040001B0 RID: 432
		public NavMeshAgent Agent;

		// Token: 0x040001B1 RID: 433
		[SerializeField]
		private Transform target;
	}
}
