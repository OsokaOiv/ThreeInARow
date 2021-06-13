using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region Attributes

    [SerializeField] private GameObject[] gridObjects;

    private bool gameover = false;
    private int[] scores = { 0, 0 };
    private int playerTurn;
    private int[,] grid;

    private float resetTime = .5f;
    private float resetTimer = 0f;

    #endregion

    #region Start and Update

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameover && resetTimer < 0f)
        {
            MouseInput();
            TouchInput();
        }
        else
        {
            resetTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region Input

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo)
            {
                GameObject cell = hitInfo.transform.gameObject;
                if (IsCell(cell))
                {
                    OnCellClick(cell);
                }
            }
        }
    }

    private void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Debug.Log("Touched");
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                if (hitInfo)
                {
                    Debug.Log(hitInfo.transform.gameObject.name);
                    // Here you can check hitInfo to see which collider has been hit, and act appropriately.
                }
            }
        }
    }
    private bool IsCell(GameObject obj)
    {
        foreach(GameObject cell in gridObjects)
        {
            if (cell == obj)
                return true;
        }
        return false;
    }

    #endregion

    #region Logic of the Game

    public void Reset()
    {
        gameover = false;
        ResetGrid();
        playerTurn = Random.Range(0, 1);
        ChangeTurn();
        ResetSprites();
        resetTimer = resetTime;
    }

    public void Rematch()
    {
        Reset();
        UIManager.Instance.ToggleEndScreen();
    }

    private void ResetGrid()
    {
        grid = new int[3, 3];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                grid[i, j] = -1;
            }
        }
    }

    private void ResetSprites()
    {
        foreach (GameObject cell in gridObjects)
        {
            cell.GetComponent<Cell>().ResetSprite();
        }
    }

    private void OnCellClick(GameObject cell)
    {
        if (grid[cell.GetComponent<Cell>().index[0], cell.GetComponent<Cell>().index[1]] == -1)
        {
            UpdateCellValue(cell);
            ActivateSprite(cell);
            if (IsWon(playerTurn))
            {
                OnWin();
                OnGameOver();
            }
            else if (IsDraw())
            {
                OnDraw();
                OnGameOver();
            }
            ChangeTurn();
        }
    }

    private void OnWin()
    {
        Debug.Log("Player " + (playerTurn + 1) + " won");
        scores[playerTurn]++;
        UIManager.Instance.UpdatePlayerWonText(playerTurn + 1);
        UIManager.Instance.UpdateScoreText(scores);
    }

    private void OnDraw()
    {
        Debug.Log("Draw!");
        UIManager.Instance.UpdatePlayerWonText(0);
    }

    private void OnGameOver()
    {
        gameover = true;
        UIManager.Instance.ToggleEndScreen();
    }

    private void UpdateCellValue(GameObject cell)
    {
        grid[cell.GetComponent<Cell>().index[0], cell.GetComponent<Cell>().index[1]] = playerTurn;
    }

    private void ActivateSprite(GameObject cell)
    {
        cell.GetComponent<Cell>().ActivatePlayerSprite(playerTurn);
    }

    private void ChangeTurn()
    {
        playerTurn = playerTurn == 1 ? 0 : 1;
        UIManager.Instance.UpdatePlayerTurnText(playerTurn + 1);
    }

    #endregion

    #region GameOverCondition

    private bool IsDraw()
    {
        foreach (int value in grid)
        {
            if (value == -1)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsWon(int player)
    {
        return IsRowsWon(player) || IsColumnWon(player) || IsXWon(player);
    }

    private bool IsRowsWon(int player)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                if (grid[i, j] != player)
                {
                    break;
                }
                else if (grid[i, j] == player && j == grid.GetLength(0)-1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsColumnWon(int player)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                if (grid[j, i] != player)
                {
                    break;
                }
                else if (j == grid.GetLength(0)-1 && grid[j, i] == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsXWon(int player)
    {
        return IsTopLeftToBottomRightWon(player) || IsBottomLeftToTopRightWon(player);
    }

    // \
    private bool IsTopLeftToBottomRightWon(int player)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            if (grid[i, i] != player)
            {
                break;
            }
            else if (i == grid.GetLength(0)-1 && grid[i, i] == player)
            {
                return true;
            }
        }
        return false;
    }

    // /
    private bool IsBottomLeftToTopRightWon(int player)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            if (grid[grid.GetLength(0)-1 - i, i] != player)
            {
                break;
            }
            else if (i == grid.GetLength(0)-1 && grid[grid.GetLength(0)-1 - i, i] == player)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
