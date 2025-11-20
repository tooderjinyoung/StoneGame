using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterSetting : ISeasonSetting
{
    public int StoneCount => 5;
    public float StoneSpeed => 5.0f;
    public float StoneFriction => 0.5f;
    public float StoneBounce => 0.3f;
}
