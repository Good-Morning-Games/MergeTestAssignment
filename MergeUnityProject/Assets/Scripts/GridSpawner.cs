using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    private GameObject[,] _gameObjectGrid;
    
    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(10, 10);

    [SerializeField]
    private float cellSize = 1f;
    
    [SerializeField]
    private float cellSpacing = 0.1f;
    
    [SerializeField]
    private Transform gridOrigin;

    /// <summary>
    /// Size of the grid
    /// </summary>
    public Vector2Int GridSize => gridSize;
    private void Start()
    {
        _gameObjectGrid = new GameObject[gridSize.x,gridSize.y];
    }
    
    /// <summary>
    /// Converts a grid position to a world position
    /// </summary>
    /// <param name="gridPosition">Position on the grid</param>
    /// <returns>position in the world</returns>
    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        var offset = gridOrigin.position;
        return (new Vector3(gridPosition.x * (cellSize + cellSpacing), gridPosition.y * (cellSize + cellSpacing)) + offset);
    }
    
    
    /// <summary>
    /// Converts a world position to a grid position
    /// </summary>
    /// <param name="worldPosition">Position in the world</param>
    /// <returns>Position on the grid</returns>
    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / (cellSize + cellSpacing)), Mathf.FloorToInt(worldPosition.y / (cellSize + cellSpacing)));
    }
    
    /// <summary>
    /// Returns the object at the given grid position
    /// </summary>
    /// <param name="gridPosition">Position on the grid</param>
    /// <returns>Object at the given grid position</returns>
    public GameObject GetObjectAtGridPosition(Vector2Int gridPosition)
    {
        return _gameObjectGrid[gridPosition.x, gridPosition.y];
    }
    
    /// <summary>
    /// Sets the object at the given grid position
    /// </summary>
    /// <param name="gameObject">Object to set</param>
    /// <param name="gridPosition">Position on the grid</param>
    public void SetObjectAtGridPosition(GameObject gameObject, Vector2Int gridPosition)
    {
        _gameObjectGrid[gridPosition.x, gridPosition.y] = gameObject;
        gameObject.transform.position = GridToWorldPosition(gridPosition);
    }
    
    /// <summary>
    /// Destroys the object at the given grid position
    /// </summary>
    /// <param name="gridPosition">Position on the grid</param>
    public void DestroyObjectAtGridPosition(Vector2Int gridPosition)
    {
        Destroy(_gameObjectGrid[gridPosition.x, gridPosition.y]);
        _gameObjectGrid[gridPosition.x, gridPosition.y] = null;
    }
    
    
    /// <summary>
    /// Moves the given object to the given grid position
    /// </summary>
    /// <param name="gameObject">GameObject to move</param>
    /// <param name="endPosition">Position on the grid</param>
    public void MoveObjectToGridPosition(GameObject gameObject, Vector2Int endPosition)
    {
        DestroyObjectAtGridPosition(WorldToGridPosition(gameObject.transform.position));
        SetObjectAtGridPosition(gameObject, endPosition);
        gameObject.transform.position = GridToWorldPosition(endPosition);
    }
    
    /// <summary>
    /// Moves the object at the given start position to the given grid position
    /// </summary>
    /// <param name="startPosition">Start position of the object</param>
    /// <param name="endPosition">End position of the object</param>
    public void MoveObjectToGridPosition(Vector2Int startPosition, Vector2Int endPosition)
    {
        var objectToMove = GetObjectAtGridPosition(startPosition);
        MoveObjectToGridPosition(objectToMove, endPosition);
    }
    
    /// <summary>
    /// Checks if there is an object at the given grid position
    /// </summary>
    /// <param name="gridPosition">Position on the grid</param>
    /// <returns>true if there is an object at the given grid position</returns>
    public bool HasCardAtGridPosition(Vector2Int gridPosition)
    {
        return _gameObjectGrid[gridPosition.x, gridPosition.y] != null;
    }

    /// <summary>
    /// Checks if there is an empty grid position available on the grid
    /// </summary>
    /// <returns>true if there is an empty grid position available on the grid</returns>
    public bool HasEmptyGridPositions()
    {
        foreach (var gameObject in _gameObjectGrid)
        {
            if (gameObject == null)
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// Spawns the given gameObject at the given grid position
    /// </summary>
    /// <param name="gameObjectPrefab">Prefab to spawn</param>
    /// <param name="gridPosition">Position on the grid</param>
    /// <returns>Spawned gameObject</returns>
    public GameObject SpawnObjectAtGridPosition(GameObject gameObjectPrefab, Vector2Int gridPosition)
    {
        var instance = Instantiate(gameObjectPrefab, gridOrigin);
        SetObjectAtGridPosition(instance, gridPosition);
        instance.transform.position = GridToWorldPosition(gridPosition);
        return instance;
    }
    
    private void OnDrawGizmosSelected()
    {
        //draw grid
        Gizmos.color = Color.white;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Gizmos.DrawWireCube(GridToWorldPosition(new Vector2Int(x, y)), new Vector3(cellSize, cellSize));
            }
        }
    }

    
}
