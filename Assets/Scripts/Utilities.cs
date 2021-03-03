using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static int GetRandomFromRange(int[] range)
    {
        return Random.Range(range[0], range[1]);
    }
}
