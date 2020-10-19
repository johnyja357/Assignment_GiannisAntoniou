using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject map = null;
    [SerializeField] private GameObject foodMouse = null;
    [SerializeField] private GameObject foodSpider = null;
    [SerializeField] private GamePlayManager gamePlayManager = null;

    private int maxWidth;
    private int maxHeight;

    private int currentGameScore = 0;

    private PlayerStatus playerStatus;
    private FoodObject foodObj;

    private GameObject randomFood;
    private Vector2 foodPosition;
    private Transform foodParent;

    private string lastEatenFood = "";
    private int streakMouse = 0;
    private int streakSpider = 0;


    private void Start()
    {
        playerStatus = PlayerStatus.Instance;
        foodObj = FoodObject.Instance;

        /// Get Max Width and Height
        Vector2 mapSize = BoardManager.MapSize; 
        maxWidth = (int)mapSize.x;
        maxHeight = (int)mapSize.y;

        List<Vector2> snakeFullBodyList = new List<Vector2>();

        /// On start spawn a random food on random position.
        SpawnFood(snakeFullBodyList);
    }

    private void SpawnFood(List<Vector2> snakeFullBodyList)
    {
        if (snakeFullBodyList != null)
        {
            foodPosition = GenerateRandomSpawnPosition(maxWidth, maxHeight);

            /// If random food position is occupied by snake body, 
            /// recall function and check for new random food position 
            if (snakeFullBodyList.Contains(foodPosition))
            {
                /// If there are no more empty tiles, win game - Show popup,
                if (snakeFullBodyList.Count == map.transform.childCount)                
                    gamePlayManager.GameWin(currentGameScore);
                else
                    SpawnFood(snakeFullBodyList);
            }
            /// Else if position is empty, spawn food.
            else
            {
                randomFood = PickRandomFood();
                foodParent = GetFoodParentPosition(foodPosition);

                /// Instatiate tile.
                GameObject food = Instantiate(randomFood, foodPosition, Quaternion.identity, foodParent);
                food.name = randomFood.name;
            }
        }
    }

    private Transform GetFoodParentPosition(Vector3 foodPosition)
    {
        /// Set corresponing tile as the food parent.
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Transform tileTransform = map.transform.GetChild(i).gameObject.GetComponent<RectTransform>().transform;

            if (tileTransform.position == foodPosition)           
                return tileTransform;           
        }

        return null;
    }

    public bool SnakeAteFoodAtNewPosition(Vector2 snakePosition, List<Vector2> snakeFullBodyList)
    {
        /// If there is food on the new snake position tile, then:
        ///     get points, destroy current food, spawn new food.
        if (foodPosition == snakePosition)
        {
            /// Set current food as the last eaten food.
            lastEatenFood = randomFood.name;

            /// Set points.
            bool resetStreak = SetCurrentGameScore();

            /// Set Streak and current Score text.
            SetTexts(resetStreak);

            /// Add snake head position to the snakeFullbody list.
            snakeFullBodyList.Add(snakePosition);             

            /// Eat (destroy) current food.
            EatFood();

            /// Spawn new food at new random position.
            SpawnFood(snakeFullBodyList);

            /// If current score is bigger than all time High Score,
            /// set current score as the High Score and save.
            CheckHighScore();

            return true;
        }

        return false;
    }

    private void CheckHighScore()
    {
        if (currentGameScore > playerStatus.GetHighScore())
        {
            playerStatus.SetHighScore(currentGameScore);
            playerStatus.SaveStatusData();
        }
    }

    private bool SetCurrentGameScore()
    {
        bool resetStreak = false;

        if (lastEatenFood == foodObj.FoodDatas[0].Type)
        {
            /// reset Spider Streak.
            streakSpider = 0;

            if (streakMouse == 0)
                resetStreak = true;

            streakMouse++;
            currentGameScore += foodObj.FoodDatas[0].Points * streakMouse;
        }
        else
        {
            /// reset Mouse Streak.
            streakMouse = 0;

            if (streakSpider == 0)
                resetStreak = true;

            streakSpider++;
            currentGameScore += foodObj.FoodDatas[1].Points * streakSpider;
        }

        return resetStreak;
    }

    private void SetTexts(bool resetStreak)
    {
        /// Set Streak Text
        if (resetStreak)
            gamePlayManager.SetStrikeText("1");
        else if (streakSpider > streakMouse)
            gamePlayManager.SetStrikeText(streakSpider.ToString());
        else
            gamePlayManager.SetStrikeText(streakMouse.ToString());

        /// Set Current Score text.
        gamePlayManager.SetCurrentScoreText(currentGameScore.ToString());
    }

    public int GetCurrentGameScore()
    {
        return currentGameScore;
    }

    private void EatFood()
    {
        /// Destroy (eat) food
        GameObject food = foodParent.GetChild(0).gameObject;
        Destroy(food);
    }

    private Vector3 GenerateRandomSpawnPosition(int mapWidth, int mapHeight)
    {
        int x = Random.Range(0, mapWidth);
        int y = Random.Range(0, mapHeight);        

        return BoardManager.GetTilePosition(x, y);
    }

    private GameObject PickRandomFood()
    {
        int randomNum = Random.Range(0, 10);

        /// Pick mouse or spider.
        /// 0 - 5 spawn mouse
        /// 6 - 9 spawn spider
        if (randomNum <= 5)
            return foodMouse;
        else
            return foodSpider;
    }
}
