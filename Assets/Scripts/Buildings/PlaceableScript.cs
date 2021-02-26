using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableScript : MonoBehaviour
{
    [Tooltip("X and Y dimensions of the object, starting from the bottom right")]
    public Vector2 dimensions;
    public int cost;

    public Vector2 positionOffset;

    [Tooltip("Name of the specific building")]
    public string buildingName;
}
