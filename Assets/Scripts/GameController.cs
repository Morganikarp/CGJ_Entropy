using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float BaseGameSpeed = 1;
    public static float CurrentGameSpeed = 1;

    public static int CurrentLevel = 0;
    public static int DeathCount = 0;

    public static bool PlayerAlive = true;

    public static List<GameObject> ActiveProjectiles = new List<GameObject>();

    public static Vector3 bounceInsideBoxCorner = new(6.5f, 3f, 0);

}
