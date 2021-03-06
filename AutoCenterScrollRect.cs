using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoCenterScrollRect : MonoBehaviour
{
	public RectTransform m_scrollViewportRT;

	public RectTransform m_scrollRectContentsRT;

	public float m_autoCenterDuration;

	public iTween.EaseType m_easeType;

	public float m_minSwipeDistance;

	public float m_maxSwipeDistance;

	private float m_targetX;

	private float m_touchStartX;

	private float m_touchEndX;

	private AutoCenterItem m_centeredItem;

	private void OnAutoCenterUpdate(float newX)
	{
		Vector3 vector = this.m_scrollRectContentsRT.get_anchoredPosition();
		vector.x = newX;
		this.m_scrollRectContentsRT.set_anchoredPosition(vector);
	}

	private void OnAutoCenterComplete()
	{
		Vector3 vector = this.m_scrollRectContentsRT.get_anchoredPosition();
		vector.x = this.m_targetX;
		this.m_scrollRectContentsRT.set_anchoredPosition(vector);
		this.m_centeredItem.SetCentered(true);
	}

	public void OnTouchStart(BaseEventData eventData)
	{
	}

	public void OnTouchEnd(BaseEventData eventData)
	{
	}

	public void CenterOnItem(int itemIndex)
	{
		AutoCenterItem[] componentsInChildren = this.m_scrollRectContentsRT.GetComponentsInChildren<AutoCenterItem>(true);
		RectTransform component = componentsInChildren[itemIndex].GetComponent<RectTransform>();
		this.m_targetX = -component.get_anchoredPosition().x;
		if (componentsInChildren[itemIndex].IsCentered())
		{
			return;
		}
		AutoCenterItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			AutoCenterItem autoCenterItem = array[i];
			autoCenterItem.SetCentered(false);
		}
		this.m_centeredItem = componentsInChildren[itemIndex];
		iTween.Stop(this.m_scrollRectContentsRT.get_gameObject());
		iTween.ValueTo(this.m_scrollRectContentsRT.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"autocenter",
			"from",
			this.m_scrollRectContentsRT.get_anchoredPosition().x,
			"to",
			this.m_targetX,
			"time",
			this.m_autoCenterDuration,
			"easetype",
			this.m_easeType,
			"onupdatetarget",
			base.get_gameObject(),
			"onupdate",
			"OnAutoCenterUpdate",
			"oncompletetarget",
			base.get_gameObject(),
			"oncomplete",
			"OnAutoCenterComplete"
		}));
	}
}
