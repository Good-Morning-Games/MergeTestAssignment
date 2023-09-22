using Sirenix.OdinInspector;
using UnityEngine;

public class RandomCardSpawner : MonoBehaviour
{

    [SerializeField]
    private GridSpawner gridSpawner;
        
    [SerializeField]
    private GameObject cardPrefab;
        
    [Button]
    private void SpawnRandomCard()
    {
        if (!gridSpawner.HasEmptyGridPositions())
        {
            Debug.LogError("No empty grid positions!");
            return;
        }
        
        var gridSize = gridSpawner.GridSize;

        var maxX = gridSize.x;
        var maxY = gridSize.y;
            
        Vector2Int randomGridPosition;
            
        do
        {
            var randomX = Random.Range(0, maxX);
            var randomY = Random.Range(0, maxY);
                
            randomGridPosition = new Vector2Int(randomX, randomY);
        } while (gridSpawner.HasCardAtGridPosition(randomGridPosition));
            
        gridSpawner.SpawnObjectAtGridPosition(cardPrefab, randomGridPosition);
    }
}