using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RadialSection
{
    public Sprite m_Icon = null;
    public SpriteRenderer m_IconRenderer = null;
    public UnityEvent m_OnPress = new UnityEvent();
    public delegate void OnSelect(PossibleNavigations currentNavMethod);
    public static event OnSelect OnSelectEvent;
}
