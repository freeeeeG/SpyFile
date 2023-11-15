using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000862 RID: 2146
[AddComponentMenu("KMonoBehaviour/scripts/FaceGraph")]
public class FaceGraph : KMonoBehaviour
{
	// Token: 0x06003EBF RID: 16063 RVA: 0x0015CFE1 File Offset: 0x0015B1E1
	public IEnumerator<Expression> GetEnumerator()
	{
		return this.expressions.GetEnumerator();
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x0015CFF3 File Offset: 0x0015B1F3
	// (set) Token: 0x06003EC1 RID: 16065 RVA: 0x0015CFFB File Offset: 0x0015B1FB
	public Expression overrideExpression { get; private set; }

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06003EC2 RID: 16066 RVA: 0x0015D004 File Offset: 0x0015B204
	// (set) Token: 0x06003EC3 RID: 16067 RVA: 0x0015D00C File Offset: 0x0015B20C
	public Expression currentExpression { get; private set; }

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0015D015 File Offset: 0x0015B215
	public void AddExpression(Expression expression)
	{
		if (this.expressions.Contains(expression))
		{
			return;
		}
		this.expressions.Add(expression);
		this.UpdateFace();
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x0015D038 File Offset: 0x0015B238
	public void RemoveExpression(Expression expression)
	{
		if (this.expressions.Remove(expression))
		{
			this.UpdateFace();
		}
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0015D04E File Offset: 0x0015B24E
	public void SetOverrideExpression(Expression expression)
	{
		if (expression != this.overrideExpression)
		{
			this.overrideExpression = expression;
			this.UpdateFace();
		}
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0015D068 File Offset: 0x0015B268
	public void ApplyShape()
	{
		KAnimFile anim = Assets.GetAnim(FaceGraph.HASH_HEAD_MASTER_SWAP_KANIM);
		bool should_use_sideways_symbol = this.ShouldUseSidewaysSymbol(this.m_controller);
		if (this.m_blinkMonitor == null)
		{
			this.m_blinkMonitor = this.m_accessorizer.GetSMI<BlinkMonitor.Instance>();
		}
		if (this.m_speechMonitor == null)
		{
			this.m_speechMonitor = this.m_accessorizer.GetSMI<SpeechMonitor.Instance>();
		}
		if (this.m_blinkMonitor.IsNullOrStopped() || !this.m_blinkMonitor.IsBlinking())
		{
			KAnim.Build.Symbol symbol = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes).symbol;
			this.ApplyShape(symbol, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_EYES, should_use_sideways_symbol);
		}
		if (this.m_speechMonitor.IsNullOrStopped() || !this.m_speechMonitor.IsPlayingSpeech())
		{
			KAnim.Build.Symbol symbol2 = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol;
			this.ApplyShape(symbol2, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_MOUTH, should_use_sideways_symbol);
			return;
		}
		this.m_speechMonitor.DrawMouth();
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0015D168 File Offset: 0x0015B368
	private bool ShouldUseSidewaysSymbol(KBatchedAnimController controller)
	{
		KAnim.Anim currentAnim = controller.GetCurrentAnim();
		if (currentAnim == null)
		{
			return false;
		}
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		if (currentFrameIndex <= 0)
		{
			return false;
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag);
		KAnim.Anim.Frame frame;
		batchGroupData.TryGetFrame(currentFrameIndex, out frame);
		for (int i = 0; i < frame.numElements; i++)
		{
			KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + i);
			if (frameElement.symbol == FaceGraph.ANIM_HASH_SNAPTO_EYES && frameElement.frame >= FaceGraph.FIRST_SIDEWAYS_FRAME)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0015D1F8 File Offset: 0x0015B3F8
	private void ApplyShape(KAnim.Build.Symbol variation_symbol, KBatchedAnimController controller, KAnimFile shapes_file, KAnimHashedString symbol_name_in_shape_file, bool should_use_sideways_symbol)
	{
		HashedString hashedString = FaceGraph.ANIM_HASH_NEUTRAL;
		if (this.currentExpression != null)
		{
			hashedString = this.currentExpression.face.hash;
		}
		KAnim.Anim anim = null;
		KAnim.Anim.FrameElement frameElement = default(KAnim.Anim.FrameElement);
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		while (num < shapes_file.GetData().animCount && !flag)
		{
			KAnim.Anim anim2 = shapes_file.GetData().GetAnim(num);
			if (anim2.hash == hashedString)
			{
				anim = anim2;
				KAnim.Anim.Frame frame;
				if (anim.TryGetFrame(shapes_file.GetData().build.batchTag, 0, out frame))
				{
					for (int i = 0; i < frame.numElements; i++)
					{
						frameElement = KAnimBatchManager.Instance().GetBatchGroupData(shapes_file.GetData().animBatchTag).GetFrameElement(frame.firstElementIdx + i);
						if (!(frameElement.symbol != symbol_name_in_shape_file))
						{
							if (flag2 || !should_use_sideways_symbol)
							{
								flag = true;
							}
							flag2 = true;
							break;
						}
					}
				}
			}
			num++;
		}
		if (anim == null)
		{
			DebugUtil.Assert(false, "Could not find shape for expression: " + HashCache.Get().Get(hashedString));
		}
		if (!flag2)
		{
			DebugUtil.Assert(false, "Could not find shape element for shape:" + HashCache.Get().Get(variation_symbol.hash));
		}
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(controller.batchGroupID).GetSymbol(symbol_name_in_shape_file);
		KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(variation_symbol.build.batchTag).symbolFrameInstances[variation_symbol.firstFrameIdx + frameElement.frame];
		symbolFrameInstance.buildImageIdx = this.m_symbolOverrideController.GetAtlasIdx(variation_symbol.build.GetTexture(0));
		controller.SetSymbolOverride(symbol.firstFrameIdx, ref symbolFrameInstance);
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0015D3A8 File Offset: 0x0015B5A8
	private void UpdateFace()
	{
		Expression expression = null;
		if (this.overrideExpression != null)
		{
			expression = this.overrideExpression;
		}
		else if (this.expressions.Count > 0)
		{
			this.expressions.Sort((Expression a, Expression b) => b.priority.CompareTo(a.priority));
			expression = this.expressions[0];
		}
		if (expression != this.currentExpression || expression == null)
		{
			this.currentExpression = expression;
			this.m_symbolOverrideController.MarkDirty();
		}
		AccessorySlot headEffects = Db.Get().AccessorySlots.HeadEffects;
		if (this.currentExpression != null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadEffects);
			HashedString hashedString = HashedString.Invalid;
			foreach (Expression expression2 in this.expressions)
			{
				if (expression2.face.headFXHash.IsValid)
				{
					hashedString = expression2.face.headFXHash;
					break;
				}
			}
			Accessory accessory2 = (hashedString != HashedString.Invalid) ? headEffects.Lookup(hashedString) : null;
			if (accessory != accessory2)
			{
				if (accessory != null)
				{
					this.m_accessorizer.RemoveAccessory(accessory);
				}
				if (accessory2 != null)
				{
					this.m_accessorizer.AddAccessory(accessory2);
				}
			}
			this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, accessory2 != null);
			return;
		}
		this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, false);
	}

	// Token: 0x040028A0 RID: 10400
	private List<Expression> expressions = new List<Expression>();

	// Token: 0x040028A3 RID: 10403
	[MyCmpGet]
	private KBatchedAnimController m_controller;

	// Token: 0x040028A4 RID: 10404
	[MyCmpGet]
	private Accessorizer m_accessorizer;

	// Token: 0x040028A5 RID: 10405
	[MyCmpGet]
	private SymbolOverrideController m_symbolOverrideController;

	// Token: 0x040028A6 RID: 10406
	private BlinkMonitor.Instance m_blinkMonitor;

	// Token: 0x040028A7 RID: 10407
	private SpeechMonitor.Instance m_speechMonitor;

	// Token: 0x040028A8 RID: 10408
	private static HashedString HASH_HEAD_MASTER_SWAP_KANIM = "head_master_swap_kanim";

	// Token: 0x040028A9 RID: 10409
	private static KAnimHashedString ANIM_HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x040028AA RID: 10410
	private static KAnimHashedString ANIM_HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x040028AB RID: 10411
	private static KAnimHashedString ANIM_HASH_NEUTRAL = "neutral";

	// Token: 0x040028AC RID: 10412
	private static int FIRST_SIDEWAYS_FRAME = 29;
}
