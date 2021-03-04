using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    Player player;

    public List<Enemy> enemies { get; private set; }
    public int numberOfEnemiesTotal { get; private set; }
    public int numberOfEnemiesRemaining => enemies.Count;

    public UnityAction<Enemy, int> onRemoveEnemy;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        //DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);

        enemies = new List<Enemy>();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);

        numberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(Enemy enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesRemaining - 1;

        if (onRemoveEnemy != null)
        {
            onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);
        }

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
    }
}