using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Player player { get; set; }
    //public List<Character> characters { get; private set; }
    public List<Enemy> enemies { get; private set; }

    private void Awake()
    {
        //characters = new List<Character>();
        enemies = new List<Enemy>();
    }

    public void RegisterPlayer(Player newPlayer)
    {
        player = newPlayer;
    }

    public void RegisterEnemy(Enemy newEnemy)
    {
        if (!enemies.Contains(newEnemy))
        {
            enemies.Add(newEnemy);
        }
    }
}
