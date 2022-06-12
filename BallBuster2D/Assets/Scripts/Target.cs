using System;
using UnityEngine;

[Serializable]
public class Target
{

    public Sprite targetSprite;
    public int targetValue;
    public Sprite missionComplateImage;
    public TargetTypes targetType;
}



public enum TargetTypes
{
    Ball,
    Box
}
