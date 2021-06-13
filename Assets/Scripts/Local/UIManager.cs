using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    #region Attributes

    [SerializeField] private Animator endScreenAnim;

    [SerializeField] private Text playerWonText;
    [SerializeField] private Text endScoreText;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text playerTurnText;

    #endregion

    #region Animation

    public void ToggleEndScreen()
    {
        if (endScreenAnim.enabled == false)
        {
            endScreenAnim.enabled = true;
        }
        else
        {
            endScreenAnim.SetBool("InScreen", !endScreenAnim.GetBool("InScreen"));
        }
    }

    #endregion

    #region Text Functions

    public void UpdatePlayerWonText(int winningPlayer)
    {
        if (winningPlayer == 0)
        {
            playerWonText.text = "Draw!";
        }
        else
        {
            playerWonText.text = "Player " + winningPlayer + " won!";
        }
    }

    public void UpdateScoreText(int[] scores)
    {
        scoreText.text = "Score:\n" + scores[0] + " - " + scores[1];
        endScoreText.text = scores[0] + " - " + scores[1];
    }

    public void UpdatePlayerTurnText(int playerTurn)
    {
        playerTurnText.text = "Turn: Player " + playerTurn;
        if (playerTurn == 1)
        {
            playerTurnText.color = Color.red;
        }
        else
        {
            playerTurnText.color = Color.blue;
        }
    }

    #endregion

    #region Button Functions

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RematchButton()
    {
        GameManager.Instance.Rematch();
    }

    #endregion
}
