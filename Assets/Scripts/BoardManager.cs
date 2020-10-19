using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject map = null;
    [SerializeField] private RectTransform mapRectTransform = null;
    [SerializeField] private GameObject tilePrefab = null;

    private static int maxWidth = GlobalVariables.MAP_WIDTH_MAX;
    private static int maxHeight = GlobalVariables.MAP_HEIGHT_MAX;
    public static Vector2 MapSize;

    private void Awake()
    {
        /// Create and adjust map to be equal to screen size.
        /// Set Map size to be equal to Camera size.
        MapSize = GetMapSize();
        CreateMap();
    }

    private Vector2 GetMapSize()
    {
        /// Get camera size and convert Screen to WorldPoint.
        Vector3 vector = Camera.main.ScreenToWorldPoint(mapRectTransform.position);
        /// Get the absolute value of Width and Height.
        float mapWidth = System.Math.Abs(vector.x * 2);
        float mapHeight = System.Math.Abs(vector.y * 2);
        /// Set Map size.
        mapRectTransform.sizeDelta = new Vector2(mapWidth, mapHeight);

        return mapRectTransform.sizeDelta;
    }

    private void CreateMap()
    {
        /// Set Max Width and Height to Map Width and Height
        maxWidth = (int)mapRectTransform.sizeDelta.x;
        maxHeight = (int)mapRectTransform.sizeDelta.y;

        for (int w = 0; w < maxWidth; w++)
        {
            for (int h = 0; h < maxHeight; h++)
            {
                /// Set position of tile.
                Vector3 position = GetTilePosition(w, h);

                /// Instatiate tile on map.
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, map.transform);
                SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();

                /// Set Tile Name - 0,0 is the lower left tile.
                tile.name = w + "," + h;

                /// Check if current tile is odd or even to set it's color.
                if (w % 2 != 0)
                {
                    if (h % 2 != 0)
                        tileRenderer.color = GlobalVariables.COLOR_GREEN_LIGHT;
                    else
                        tileRenderer.color = GlobalVariables.COLOR_GREEN_DARK;
                }
                else
                {
                    if (h % 2 != 0)
                        tileRenderer.color = GlobalVariables.COLOR_GREEN_DARK;
                    else
                        tileRenderer.color = GlobalVariables.COLOR_GREEN_LIGHT;
                }                
            }
        }
    }     

    public static Vector3 GetTilePosition(int width, int height)
    {
        float offset = 0f;

        /// If map width is even number, add 0.5(half tile size) offset.
        if (maxWidth % 2 == 0)        
            offset = 0.5f;        

        float newW = -((maxWidth / 2) - width) + offset;
        /// Always add 0.5f(half tile size) offset to the height.
        float newH = -((maxHeight / 2) - height) + 0.5f; 

        return new Vector3(newW, newH, 0);
    }
}
