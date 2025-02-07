using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] board = new SpriteRenderer[42];
    private TokenState[,] visualBoard = new TokenState[6, 7];
    public TokenState currentPlayerState = TokenState.Yellow;
    public static GameManager Instance;
    public Sprite yellowToken;
    public Sprite redToken;
    public Image tokenDisplay;
    public Sprite player1Sprite;
    public Sprite player2Sprite;
    public TMP_Text Win;

    public enum TokenState
    {
        Empty,
        Yellow,
        Red
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeBoard();
    }

    private void DisplayToken(SpriteRenderer tokenToDisplay, TokenState newState)
    {
        switch (newState)
        {
            case TokenState.Yellow:
                tokenToDisplay.sprite = yellowToken;
                break;
            case TokenState.Red:
                tokenToDisplay.sprite = redToken;
                break;
            case TokenState.Empty:
                tokenToDisplay.sprite = null;
                break;
        }
    }

    public void AddToken(int columnNb)
    {
        for (int i = 0; i < 6; i++)
        {
            if (visualBoard[i, columnNb] == TokenState.Empty)
            {
                visualBoard[i, columnNb] = currentPlayerState;
                DisplayToken(board[columnNb + 7 * i], currentPlayerState);
                
                if (IsWin())
                {
                    Win.text = currentPlayerState + " win";
                    Win.GameObject().SetActive(true);
                    return;
                }
                
                if (IsDraw())
                {
                    Debug.Log("Match nul !");
                    return;
                }
                
                SwitchPlayer();
                return;
            }
        }
    }

    private void InitializeBoard()
    {
        foreach (var sprite in board)
        {
            DisplayToken(sprite, TokenState.Empty);
        }
    }

    private void SwitchPlayer()
    {
        currentPlayerState = currentPlayerState == TokenState.Yellow ? TokenState.Red : TokenState.Yellow;
        tokenDisplay.sprite = currentPlayerState == TokenState.Yellow ? player1Sprite : player2Sprite;
    }

    private bool IsWin()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (visualBoard[i, j] == currentPlayerState && visualBoard[i, j + 1] == currentPlayerState && 
                    visualBoard[i, j + 2] == currentPlayerState && visualBoard[i, j + 3] == currentPlayerState)
                {
                    return true;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (visualBoard[i, j] == currentPlayerState && visualBoard[i + 1, j] == currentPlayerState && 
                    visualBoard[i + 2, j] == currentPlayerState && visualBoard[i + 3, j] == currentPlayerState)
                {
                    return true;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (visualBoard[i, j] == currentPlayerState && visualBoard[i + 1, j + 1] == currentPlayerState && 
                    visualBoard[i + 2, j + 2] == currentPlayerState && visualBoard[i + 3, j + 3] == currentPlayerState)
                {
                    return true;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 3; j < 7; j++)
            {
                if (visualBoard[i, j] == currentPlayerState && visualBoard[i + 1, j - 1] == currentPlayerState && 
                    visualBoard[i + 2, j - 2] == currentPlayerState && visualBoard[i + 3, j - 3] == currentPlayerState)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsDraw()
    {
        for (int col = 0; col < 7; col++)
        {
            if (visualBoard[5, col] == TokenState.Empty)
            {
                return false;
            }
        }
        return true;
    }

    public void ReloadGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}


   