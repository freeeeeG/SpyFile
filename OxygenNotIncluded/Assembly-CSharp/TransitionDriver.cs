using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class TransitionDriver
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x000A968F File Offset: 0x000A788F
	public Navigator.ActiveTransition GetTransition
	{
		get
		{
			return this.transition;
		}
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000A9697 File Offset: 0x000A7897
	public TransitionDriver(Navigator navigator)
	{
		this.log = new LoggerFS("TransitionDriver", 35);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000A96C8 File Offset: 0x000A78C8
	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Init(transition, defaultSpeed);
		this.BeginTransition(navigator, instance);
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000A96F0 File Offset: 0x000A78F0
	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = this.interruptOverrideStack.Count != 0;
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			if (!flag || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		this.isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			KBatchedAnimController component = navigator.GetComponent<KBatchedAnimController>();
			sceneLayer = Grid.SceneLayer.FXFront;
			component.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.GetComponent<KBatchedAnimController>().SetSceneLayer(sceneLayer);
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		this.targetPos = Grid.CellToPosCBC(cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase component2 = navigator.GetComponent<KAnimControllerBase>();
			component2.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = component2.CurrentAnim != null && component2.CurrentAnim.name == transition.anim;
			if (flag2 && component2.CurrentAnim != null && component2.CurrentAnim.name == transition.preAnim)
			{
				component2.ClearQueue();
				component2.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else if (flag3)
			{
				if (component2.PlayMode != KAnim.PlayMode.Loop)
				{
					component2.ClearQueue();
					component2.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else if (flag2)
			{
				component2.Play(transition.preAnim, KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else
			{
				component2.Play(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		else if (transition.anim != null)
		{
			KAnimControllerBase component3 = navigator.GetComponent<KAnimControllerBase>();
			component3.PlaySpeedMultiplier = transition.animSpeed;
			component3.Play(transition.anim, KAnim.PlayMode.Once, 1f, 0f);
			navigator.Subscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x < 0);
			}
		}
		this.brain = navigator.GetComponent<Brain>();
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000A9A7C File Offset: 0x000A7C7C
	public void UpdateTransition(float dt)
	{
		if (this.navigator == null)
		{
			return;
		}
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			bool flag = this.interruptOverrideStack.Count != 0;
			bool flag2 = overrideLayer is TransitionDriver.InterruptOverrideLayer;
			if (!flag || !flag2 || this.interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(this.navigator, this.transition);
			}
		}
		if (!this.isComplete && this.transition.isCompleteCB != null)
		{
			this.isComplete = this.transition.isCompleteCB();
		}
		if (this.brain != null)
		{
			bool flag3 = this.isComplete;
		}
		if (this.transition.isLooping)
		{
			float speed = this.transition.speed;
			Vector3 position = this.navigator.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (this.transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.x = this.targetPos.x;
			}
			if (this.transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.y = this.targetPos.y;
			}
			this.navigator.transform.SetPosition(position);
			int num2 = Grid.PosToCell(position);
			if (num2 != num)
			{
				this.navigator.Trigger(915392638, num2);
			}
		}
		if (this.isComplete)
		{
			this.isComplete = false;
			Navigator navigator = this.navigator;
			navigator.SetCurrentNavType(this.transition.end);
			navigator.transform.SetPosition(this.targetPos);
			this.EndTransition();
			navigator.AdvancePath(true);
		}
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000A9D20 File Offset: 0x000A7F20
	public void EndTransition()
	{
		if (this.navigator != null)
		{
			this.interruptOverrideStack.Clear();
			foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
			{
				overrideLayer.EndTransition(this.navigator, this.transition);
			}
			this.navigator.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 1f;
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
			if (this.brain != null)
			{
				this.brain.Resume("move_handler");
			}
			if (this.navigator.animEventHandler != null)
			{
				this.navigator.animEventHandler.SetDirty();
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.transition);
			this.transition = null;
			this.navigator = null;
			this.brain = null;
		}
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000A9E30 File Offset: 0x000A8030
	private void OnAnimComplete(object data)
	{
		if (this.navigator != null)
		{
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		this.isComplete = true;
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000A9E63 File Offset: 0x000A8063
	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Copy(src);
		src.Copy(TransitionDriver.emptyTransition);
		return instance;
	}

	// Token: 0x040011BB RID: 4539
	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	// Token: 0x040011BC RID: 4540
	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

	// Token: 0x040011BD RID: 4541
	private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);

	// Token: 0x040011BE RID: 4542
	private Navigator.ActiveTransition transition;

	// Token: 0x040011BF RID: 4543
	private Navigator navigator;

	// Token: 0x040011C0 RID: 4544
	private Vector3 targetPos;

	// Token: 0x040011C1 RID: 4545
	private bool isComplete;

	// Token: 0x040011C2 RID: 4546
	private Brain brain;

	// Token: 0x040011C3 RID: 4547
	public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();

	// Token: 0x040011C4 RID: 4548
	private LoggerFS log;

	// Token: 0x020011C9 RID: 4553
	public class OverrideLayer
	{
		// Token: 0x06007AC7 RID: 31431 RVA: 0x002DCF4A File Offset: 0x002DB14A
		public OverrideLayer(Navigator navigator)
		{
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x002DCF52 File Offset: 0x002DB152
		public virtual void Destroy()
		{
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x002DCF54 File Offset: 0x002DB154
		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x002DCF56 File Offset: 0x002DB156
		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x002DCF58 File Offset: 0x002DB158
		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	// Token: 0x020011CA RID: 4554
	public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06007ACC RID: 31436 RVA: 0x002DCF5A File Offset: 0x002DB15A
		protected bool InterruptInProgress
		{
			get
			{
				return this.originalTransition != null;
			}
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x002DCF65 File Offset: 0x002DB165
		public InterruptOverrideLayer(Navigator navigator) : base(navigator)
		{
			this.driver = navigator.transitionDriver;
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x002DCF7A File Offset: 0x002DB17A
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			this.driver.interruptOverrideStack.Push(this);
			this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x002DCF9C File Offset: 0x002DB19C
		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (!this.IsOverrideComplete())
			{
				return;
			}
			this.driver.interruptOverrideStack.Pop();
			transition.Copy(this.originalTransition);
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
			this.EndTransition(navigator, transition);
			this.driver.BeginTransition(navigator, transition);
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x002DCFFB File Offset: 0x002DB1FB
		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (this.originalTransition == null)
			{
				return;
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x002DD025 File Offset: 0x002DB225
		protected virtual bool IsOverrideComplete()
		{
			return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
		}

		// Token: 0x04005D85 RID: 23941
		protected Navigator.ActiveTransition originalTransition;

		// Token: 0x04005D86 RID: 23942
		protected TransitionDriver driver;
	}
}
