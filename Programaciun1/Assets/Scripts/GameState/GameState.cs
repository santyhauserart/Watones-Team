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
}
