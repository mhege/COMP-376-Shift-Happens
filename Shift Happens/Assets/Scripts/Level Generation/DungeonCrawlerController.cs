using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    right = 2,
    down = 3
}

public class DungeonCrawlerController : MonoBehaviour
{
    

    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left },
        {Direction.right, Vector2Int.right },
        {Direction.down, Vector2Int.down },
    };

    public static List<Vector2Int> GenerateDungeon(int numberOfCrawler, int minimumRooms, int maximumRooms)
    {
        List<DungeonCrawler> crawlers = new List<DungeonCrawler>();

        for(int i=0; i < numberOfCrawler; i++)
        {
            crawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iterations = Random.Range(minimumRooms, maximumRooms);

        for(int i = 0; i < iterations; i++)
        {
            foreach(DungeonCrawler dungeonCrawler in crawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;

    }


}
