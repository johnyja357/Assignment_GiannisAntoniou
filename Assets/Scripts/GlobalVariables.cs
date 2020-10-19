using UnityEngine;

public class GlobalVariables
{
    public const string JSON_FILES_PATH = "Data/";
    public const string JSON_FOOD = "Food";

    public const string TEXT_GAME_OVER = "Game Over!";
    public const string TEXT_GAME_WIN = "Congratulations!";

    public const int MAP_WIDTH_MAX = 15;
    public const int MAP_HEIGHT_MAX = 15;

    public const float SNAKE_MOVE_TIMER = 0.5f;

    public static Vector3 ROTATION_RIGHT = new Vector3(0, 0, 90);
    public static Vector3 ROTATION_UP = new Vector3(0, 0, 180);
    public static Vector3 ROTATION_LEFT = new Vector3(0, 0, 270);
    public static Vector3 ROTATION_DOWN = new Vector3(0, 0, 360);

    public static Color COLOR_BLACK = new Color(0, 0, 0, 255);
    public static Color COLOR_GREEN_LIGHT = new Color(0.216f, 1f, 0f, 1f);
    public static Color COLOR_GREEN_DARK = new Color(0.176f, 0.557f, 0.114f, 1f);

}
