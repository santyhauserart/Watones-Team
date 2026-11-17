using UnityEngine;
using System.Collections.Generic;
public static class GameState
{
    public static Vector2 playerPosition;
    public static bool hasSavedPosition = false;
    public static Vector2 savePointPosition;
    public static bool hasSavePoint = false;
    public static List<int> defeatedGuards = new List<int>();
    public static List<int> encounterGuards = new List<int>();
    public static List<int> savedDefeatedGuards = new List<int>();
    public static float encounterImmuneUntil = 0f;
    public static int playerLife = -1;
    public static Dictionary<int, Vector2> guardPositions = new Dictionary<int, Vector2>();
    public static Dictionary<int, int> guardWaypoints = new Dictionary<int, int>();
    public static List<int> collectedPickups = new List<int>();
}
