using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int pickupsToAdvance;
    public float secondsToLose;
    public float cameraSize;
    public Vector2 spawnBoundaries;
}