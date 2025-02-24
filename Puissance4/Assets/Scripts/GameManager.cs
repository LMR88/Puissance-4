using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] board = new SpriteRenderer[42];
    private TokenState[,] visualBoard = new TokenState[6, 7];
    private Stack<(int, int, TokenState)> playToken = new Stack<(int, int, TokenState)>();
    private Stack<(int, int, TokenState)> redoStack = new Stack<(int, int, TokenState)>();
    public bool haveIa;
    public TokenState currentPlayerState = TokenState.Yellow;
    public static GameManager Instance;
    public Sprite yellowToken;
    public Sprite redToken;
    public Image tokenDisplay;
    public Sprite player1Sprite;
    public Sprite player2Sprite;
    public TMP_Text Win;
    public TMP_Text matchNul;
    public Ia iaReference;
    public bool iaTurnToPlay;

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
                playToken.Push((i, columnNb, currentPlayerState));
                redoStack.Clear();
                
                if (IsWin())
                {
                    Win.text = currentPlayerState + " win! bien joué poto on t'aime";
                    Win.gameObject.SetActive(true);
                    return;
                }
                
                if (IsDraw())
                {
                    matchNul.text = "Match Nul ! Vous êtes guez ouuu?? ";
                    matchNul.gameObject.SetActive(true);
                    return;
                }

                iaTurnToPlay = !iaTurnToPlay;
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
        if (haveIa && iaTurnToPlay)
        {
            iaReference.IaTurn();
        }
        tokenDisplay.sprite = currentPlayerState == TokenState.Yellow ? player1Sprite : player2Sprite;
    }

    public void Undo()
    {
        if (playToken.Count > 0)
        {
            var lastMove = playToken.Pop();
            visualBoard[lastMove.Item1, lastMove.Item2] = TokenState.Empty;
            DisplayToken(board[lastMove.Item2 + 7 * lastMove.Item1], TokenState.Empty);
            redoStack.Push(lastMove);
            SwitchPlayer();
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            var redoMove = redoStack.Pop();
            visualBoard[redoMove.Item1, redoMove.Item2] = redoMove.Item3;
            DisplayToken(board[redoMove.Item2 + 7 * redoMove.Item1], redoMove.Item3);
            playToken.Push(redoMove);
            SwitchPlayer();
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    
    public struct UneLigneDe4
    {
        public int baseX;  // x première case de la ligne
        public int baseY;   // y première case de la ligne
        public int incX;  // direction x de la ligne
        public int incY;   // direction y de la ligne



    }


    List<UneLigneDe4> LesLignesDe4 = new List<UneLigneDe4>
    {

        new UneLigneDe4{baseX= 0 , baseY= 0,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 0,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 0,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 0,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 1,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 1,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 1,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 1,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 2,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 2,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 2,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 2,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 3,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 3,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 3,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 3,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 4,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 4,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 4,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 4,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 5,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 1 , baseY= 5,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 2 , baseY= 5,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 3 , baseY= 5,incX= 1, incY= 0 },
        new UneLigneDe4{baseX= 0 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 0 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 0 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 0,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 1,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 2,incX= 0, incY= 1 },
        new UneLigneDe4{baseX= 0 , baseY= 2,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 0 , baseY= 1,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 2,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 0 , baseY= 0,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 1,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 2,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 1 , baseY= 0,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 1,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 2,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 2 , baseY= 0,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 1,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 0,incX= 1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 0,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 0,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 1,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 0,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 1,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 3 , baseY= 2,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 0,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 1,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 4 , baseY= 2,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 1,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 5 , baseY= 2,incX= -1, incY= 1 },
        new UneLigneDe4{baseX= 6 , baseY= 2,incX= -1, incY= 1 }

    };
}