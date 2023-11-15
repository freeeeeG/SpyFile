using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public static class LeanTweenExt
{
	// Token: 0x06000353 RID: 851 RVA: 0x00014470 File Offset: 0x00012670
	public static LTDescr LeanAlpha(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alpha(gameObject, to, time);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001447A File Offset: 0x0001267A
	public static LTDescr LeanAlphaVertex(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alphaVertex(gameObject, to, time);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00014484 File Offset: 0x00012684
	public static LTDescr LeanAlpha(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alpha(rectTransform, to, time);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001448E File Offset: 0x0001268E
	public static LTDescr LeanAlpha(this CanvasGroup canvas, float to, float time)
	{
		return LeanTween.alphaCanvas(canvas, to, time);
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00014498 File Offset: 0x00012698
	public static LTDescr LeanAlphaText(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alphaText(rectTransform, to, time);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000144A2 File Offset: 0x000126A2
	public static void LeanCancel(this GameObject gameObject)
	{
		LeanTween.cancel(gameObject);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000144AA File Offset: 0x000126AA
	public static void LeanCancel(this GameObject gameObject, bool callOnComplete)
	{
		LeanTween.cancel(gameObject, callOnComplete);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x000144B3 File Offset: 0x000126B3
	public static void LeanCancel(this GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		LeanTween.cancel(gameObject, uniqueId, callOnComplete);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x000144BD File Offset: 0x000126BD
	public static void LeanCancel(this RectTransform rectTransform)
	{
		LeanTween.cancel(rectTransform);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x000144C5 File Offset: 0x000126C5
	public static LTDescr LeanColor(this GameObject gameObject, Color to, float time)
	{
		return LeanTween.color(gameObject, to, time);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000144CF File Offset: 0x000126CF
	public static LTDescr LeanColorText(this RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.colorText(rectTransform, to, time);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000144D9 File Offset: 0x000126D9
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000144E3 File Offset: 0x000126E3
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000144ED File Offset: 0x000126ED
	public static bool LeanIsPaused(this GameObject gameObject)
	{
		return LeanTween.isPaused(gameObject);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x000144F5 File Offset: 0x000126F5
	public static bool LeanIsPaused(this RectTransform rectTransform)
	{
		return LeanTween.isPaused(rectTransform);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x000144FD File Offset: 0x000126FD
	public static bool LeanIsTweening(this GameObject gameObject)
	{
		return LeanTween.isTweening(gameObject);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00014505 File Offset: 0x00012705
	public static LTDescr LeanMove(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0001450F File Offset: 0x0001270F
	public static LTDescr LeanMove(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001451E File Offset: 0x0001271E
	public static LTDescr LeanMove(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.move(rectTransform, to, time);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00014528 File Offset: 0x00012728
	public static LTDescr LeanMove(this GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00014532 File Offset: 0x00012732
	public static LTDescr LeanMove(this Transform transform, Vector2 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00014541 File Offset: 0x00012741
	public static LTDescr LeanMove(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001454B File Offset: 0x0001274B
	public static LTDescr LeanMove(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00014555 File Offset: 0x00012755
	public static LTDescr LeanMove(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001455F File Offset: 0x0001275F
	public static LTDescr LeanMove(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001456E File Offset: 0x0001276E
	public static LTDescr LeanMove(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0001457D File Offset: 0x0001277D
	public static LTDescr LeanMove(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001458C File Offset: 0x0001278C
	public static LTDescr LeanMoveLocal(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00014596 File Offset: 0x00012796
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x000145A0 File Offset: 0x000127A0
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000145AA File Offset: 0x000127AA
	public static LTDescr LeanMoveLocal(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000145B9 File Offset: 0x000127B9
	public static LTDescr LeanMoveLocal(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000145C8 File Offset: 0x000127C8
	public static LTDescr LeanMoveLocal(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000145D7 File Offset: 0x000127D7
	public static LTDescr LeanMoveLocalX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalX(gameObject, to, time);
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000145E1 File Offset: 0x000127E1
	public static LTDescr LeanMoveLocalY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalY(gameObject, to, time);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x000145EB File Offset: 0x000127EB
	public static LTDescr LeanMoveLocalZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalZ(gameObject, to, time);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x000145F5 File Offset: 0x000127F5
	public static LTDescr LeanMoveLocalX(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalX(transform.gameObject, to, time);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00014604 File Offset: 0x00012804
	public static LTDescr LeanMoveLocalY(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalY(transform.gameObject, to, time);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00014613 File Offset: 0x00012813
	public static LTDescr LeanMoveLocalZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalZ(transform.gameObject, to, time);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00014622 File Offset: 0x00012822
	public static LTDescr LeanMoveSpline(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0001462C File Offset: 0x0001282C
	public static LTDescr LeanMoveSpline(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00014636 File Offset: 0x00012836
	public static LTDescr LeanMoveSpline(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00014645 File Offset: 0x00012845
	public static LTDescr LeanMoveSpline(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00014654 File Offset: 0x00012854
	public static LTDescr LeanMoveSplineLocal(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(gameObject, to, time);
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0001465E File Offset: 0x0001285E
	public static LTDescr LeanMoveSplineLocal(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(transform.gameObject, to, time);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0001466D File Offset: 0x0001286D
	public static LTDescr LeanMoveX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveX(gameObject, to, time);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00014677 File Offset: 0x00012877
	public static LTDescr LeanMoveX(this Transform transform, float to, float time)
	{
		return LeanTween.moveX(transform.gameObject, to, time);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00014686 File Offset: 0x00012886
	public static LTDescr LeanMoveX(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveX(rectTransform, to, time);
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00014690 File Offset: 0x00012890
	public static LTDescr LeanMoveY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveY(gameObject, to, time);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0001469A File Offset: 0x0001289A
	public static LTDescr LeanMoveY(this Transform transform, float to, float time)
	{
		return LeanTween.moveY(transform.gameObject, to, time);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000146A9 File Offset: 0x000128A9
	public static LTDescr LeanMoveY(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveY(rectTransform, to, time);
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000146B3 File Offset: 0x000128B3
	public static LTDescr LeanMoveZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveZ(gameObject, to, time);
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000146BD File Offset: 0x000128BD
	public static LTDescr LeanMoveZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveZ(transform.gameObject, to, time);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000146CC File Offset: 0x000128CC
	public static LTDescr LeanMoveZ(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveZ(rectTransform, to, time);
	}

	// Token: 0x06000389 RID: 905 RVA: 0x000146D6 File Offset: 0x000128D6
	public static void LeanPause(this GameObject gameObject)
	{
		LeanTween.pause(gameObject);
	}

	// Token: 0x0600038A RID: 906 RVA: 0x000146DE File Offset: 0x000128DE
	public static LTDescr LeanPlay(this RectTransform rectTransform, Sprite[] sprites)
	{
		return LeanTween.play(rectTransform, sprites);
	}

	// Token: 0x0600038B RID: 907 RVA: 0x000146E7 File Offset: 0x000128E7
	public static void LeanResume(this GameObject gameObject)
	{
		LeanTween.resume(gameObject);
	}

	// Token: 0x0600038C RID: 908 RVA: 0x000146EF File Offset: 0x000128EF
	public static LTDescr LeanRotate(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.rotate(gameObject, to, time);
	}

	// Token: 0x0600038D RID: 909 RVA: 0x000146F9 File Offset: 0x000128F9
	public static LTDescr LeanRotate(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.rotate(transform.gameObject, to, time);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00014708 File Offset: 0x00012908
	public static LTDescr LeanRotate(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.rotate(rectTransform, to, time);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00014712 File Offset: 0x00012912
	public static LTDescr LeanRotateAround(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(gameObject, axis, add, time);
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001471D File Offset: 0x0001291D
	public static LTDescr LeanRotateAround(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(transform.gameObject, axis, add, time);
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001472D File Offset: 0x0001292D
	public static LTDescr LeanRotateAround(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(rectTransform, axis, add, time);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00014738 File Offset: 0x00012938
	public static LTDescr LeanRotateAroundLocal(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(gameObject, axis, add, time);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00014743 File Offset: 0x00012943
	public static LTDescr LeanRotateAroundLocal(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(transform.gameObject, axis, add, time);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00014753 File Offset: 0x00012953
	public static LTDescr LeanRotateAroundLocal(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(rectTransform, axis, add, time);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0001475E File Offset: 0x0001295E
	public static LTDescr LeanRotateX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateX(gameObject, to, time);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00014768 File Offset: 0x00012968
	public static LTDescr LeanRotateX(this Transform transform, float to, float time)
	{
		return LeanTween.rotateX(transform.gameObject, to, time);
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00014777 File Offset: 0x00012977
	public static LTDescr LeanRotateY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateY(gameObject, to, time);
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00014781 File Offset: 0x00012981
	public static LTDescr LeanRotateY(this Transform transform, float to, float time)
	{
		return LeanTween.rotateY(transform.gameObject, to, time);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00014790 File Offset: 0x00012990
	public static LTDescr LeanRotateZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateZ(gameObject, to, time);
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001479A File Offset: 0x0001299A
	public static LTDescr LeanRotateZ(this Transform transform, float to, float time)
	{
		return LeanTween.rotateZ(transform.gameObject, to, time);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x000147A9 File Offset: 0x000129A9
	public static LTDescr LeanScale(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.scale(gameObject, to, time);
	}

	// Token: 0x0600039C RID: 924 RVA: 0x000147B3 File Offset: 0x000129B3
	public static LTDescr LeanScale(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.scale(transform.gameObject, to, time);
	}

	// Token: 0x0600039D RID: 925 RVA: 0x000147C2 File Offset: 0x000129C2
	public static LTDescr LeanScale(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.scale(rectTransform, to, time);
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000147CC File Offset: 0x000129CC
	public static LTDescr LeanScaleX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleX(gameObject, to, time);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000147D6 File Offset: 0x000129D6
	public static LTDescr LeanScaleX(this Transform transform, float to, float time)
	{
		return LeanTween.scaleX(transform.gameObject, to, time);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x000147E5 File Offset: 0x000129E5
	public static LTDescr LeanScaleY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleY(gameObject, to, time);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000147EF File Offset: 0x000129EF
	public static LTDescr LeanScaleY(this Transform transform, float to, float time)
	{
		return LeanTween.scaleY(transform.gameObject, to, time);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000147FE File Offset: 0x000129FE
	public static LTDescr LeanScaleZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleZ(gameObject, to, time);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00014808 File Offset: 0x00012A08
	public static LTDescr LeanScaleZ(this Transform transform, float to, float time)
	{
		return LeanTween.scaleZ(transform.gameObject, to, time);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00014817 File Offset: 0x00012A17
	public static LTDescr LeanSize(this RectTransform rectTransform, Vector2 to, float time)
	{
		return LeanTween.size(rectTransform, to, time);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00014821 File Offset: 0x00012A21
	public static LTDescr LeanValue(this GameObject gameObject, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001482C File Offset: 0x00012A2C
	public static LTDescr LeanValue(this GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00014837 File Offset: 0x00012A37
	public static LTDescr LeanValue(this GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00014842 File Offset: 0x00012A42
	public static LTDescr LeanValue(this GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001484D File Offset: 0x00012A4D
	public static LTDescr LeanValue(this GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001485A File Offset: 0x00012A5A
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00014867 File Offset: 0x00012A67
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00014874 File Offset: 0x00012A74
	public static LTDescr LeanValue(this GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00014881 File Offset: 0x00012A81
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001488E File Offset: 0x00012A8E
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001489B File Offset: 0x00012A9B
	public static void LeanSetPosX(this Transform transform, float val)
	{
		transform.position = new Vector3(val, transform.position.y, transform.position.z);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000148BF File Offset: 0x00012ABF
	public static void LeanSetPosY(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, val, transform.position.z);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x000148E3 File Offset: 0x00012AE3
	public static void LeanSetPosZ(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, val);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00014907 File Offset: 0x00012B07
	public static void LeanSetLocalPosX(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001492B File Offset: 0x00012B2B
	public static void LeanSetLocalPosY(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, val, transform.localPosition.z);
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0001494F File Offset: 0x00012B4F
	public static void LeanSetLocalPosZ(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, val);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00014973 File Offset: 0x00012B73
	public static Color LeanColor(this Transform transform)
	{
		return transform.GetComponent<Renderer>().material.color;
	}
}
