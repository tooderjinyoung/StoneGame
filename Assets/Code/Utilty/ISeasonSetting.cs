using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISeasonSetting
{
    int StoneCount { get; }
    float StoneSpeed { get; }
    float StoneFriction { get; }
    float StoneBounce { get; }
}


