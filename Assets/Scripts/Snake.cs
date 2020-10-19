using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject snakeBody = null;
    private FoodSpawner foodSpawner;
    private GamePlayManager gamePlayManager;

    private Vector2 snakePosition;
    private Vector2 moveDirection;
    private float moveTimer;
    private float moveTimerMax;

    private int snakeBodySize;
    private List<Vector2> snakeMovePositionList;

    private bool isRunning = true;
    private Vector2 positiveMapBounds; /// bounds for Map right and top side 
    private Vector2 negativeMapBounds; /// bounds for Map left and down side 


    private void Start()
    {
        /// Get other scripts instance.
        foodSpawner = FindObjectOfType<FoodSpawner>();
        gamePlayManager = FindObjectOfType<GamePlayManager>();

        /// Set Snake initial position using the board coordination.
        SetInitialPosition();

        /// Set movement speed
        SetMovementSpeed();

        /// Set initial movement direction -> move to the right.
        SetInitialMovementDirection();

        snakeMovePositionList = new List<Vector2>();
        snakeBodySize = 0;

        /// Get Max Width and Height
        Vector2 mapSize = BoardManager.MapSize;
        positiveMapBounds = BoardManager.GetTilePosition((int)mapSize.x, (int)mapSize.y);
        negativeMapBounds = new Vector2(-positiveMapBounds.x, -positiveMapBounds.y);
    }

    private void Update()
    {
        if (isRunning)
        {
            /// Check which keyboard button pressed (left, right, up, down)
            HandleInput();

            HandleSnakeMovement();            
        }     
    }

    #region Initialization
    private void SetInitialPosition()
    {
        snakePosition = BoardManager.GetTilePosition(-1, 0);
    }

    private void SetMovementSpeed()
    {
        moveTimerMax = GlobalVariables.SNAKE_MOVE_TIMER;
        moveTimer = moveTimerMax;
    }

    private void SetInitialMovementDirection()
    {
        moveDirection = new Vector2(1, 0);
        transform.eulerAngles = GlobalVariables.ROTATION_RIGHT;
    }
    #endregion

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            /// If is moving Downwards (opposite direction), then don't move Upwards.
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = +1;
                /// Set head rotation
                transform.eulerAngles = GlobalVariables.ROTATION_UP;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (moveDirection.y != +1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;

                transform.eulerAngles = GlobalVariables.ROTATION_DOWN;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveDirection.x != +1)
            {
                moveDirection.x = -1;
                moveDirection.y = 0;

                transform.eulerAngles = GlobalVariables.ROTATION_LEFT;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = +1;
                moveDirection.y = 0;

                transform.eulerAngles = GlobalVariables.ROTATION_RIGHT;
            }
        }
    }

    private void HandleSnakeMovement()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveTimerMax)
        {
            moveTimer -= moveTimerMax;

            /// Store snake current position. 
            snakeMovePositionList.Insert(0, snakePosition);

            /// Move snake.
            snakePosition += moveDirection;

            /// Check if snake hit wall or it's own body.
            if (IsSnakePositionIsOutOfMapBounds() || IsSnakeBodyInNextTile())
            {
                isRunning = false;

                /// Show Game Over popup.
                gamePlayManager.GameOver(foodSpawner.GetCurrentGameScore());

                return;
            }

            /// Check if snake ate food at new position
            bool ateFood = foodSpawner.SnakeAteFoodAtNewPosition(snakePosition, snakeMovePositionList);

            /// If ate food, increase body size by 1.
            if (ateFood)
                snakeBodySize++;

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                /// Get snake move position
                Vector2 snakeMovePosition = snakeMovePositionList[i];

                GameObject newSnakeBodyPart = Instantiate(snakeBody);
                newSnakeBodyPart.transform.position = snakeMovePosition;

                /// Get last piece of snake tail
                Vector3 lastSnakeBodyGameObj = snakeMovePositionList[snakeMovePositionList.Count - 1];

                /// Set last piece of snake tail to black 
                /// so it stands out of the rest body
                if (newSnakeBodyPart.transform.position == lastSnakeBodyGameObj && snakeMovePositionList.Count > 1)
                    newSnakeBodyPart.GetComponent<SpriteRenderer>().color = GlobalVariables.COLOR_BLACK;                

                Destroy(newSnakeBodyPart, moveTimerMax);
            }

            transform.position = new Vector3(snakePosition.x, snakePosition.y);
        }
    }

    private bool IsSnakePositionIsOutOfMapBounds()
    {
        /// Check If snake position is out of Map's right or top bounds
        if (snakePosition.x >= positiveMapBounds.x || snakePosition.y >= positiveMapBounds.y)
            return true;

        /// Check If snake position is out of Map's left or down bounds
        if (snakePosition.x <= negativeMapBounds.x || snakePosition.y <= negativeMapBounds.y)
            return true;

        return false;
    }

    private bool IsSnakeBodyInNextTile()
    {
        /// Check if snake hit it's own tail (body)
        if (snakeMovePositionList.Contains(snakePosition))
            return true;

        return false;
    }
}
