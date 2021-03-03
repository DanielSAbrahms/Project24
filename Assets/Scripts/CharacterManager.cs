using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<Character> characters { get; private set; }

    private void Awake()
    {
        characters = new List<Character>();
    }
}
