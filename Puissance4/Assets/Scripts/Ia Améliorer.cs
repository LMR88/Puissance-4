using System.Collections;
using UnityEngine;

public class Connect4AI : MonoBehaviour
{
    public GameManager gameManager;
    public int maxDepth = 5;

    public void PlayBestMove()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(0.7f);
        int bestMove = BestMove();
        gameManager.AddToken(bestMove);
    }

    int BestMove()
    {
        int bestColumn = -1;
        int bestScore = int.MinValue;
        GameManager.TokenState[,] board = gameManager.TakeGrid();

        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(board, col))
            {
                int row = GetRow(board, col);
                board[row, col] = GameManager.TokenState.Red;
                int score = Minimax(board, maxDepth, false, int.MinValue, int.MaxValue);
                board[row, col] = GameManager.TokenState.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestColumn = col;
                }
            }
        }
        return bestColumn;
    }

    int Minimax(GameManager.TokenState[,] board, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        if (depth == 0 || gameManager.IsWin() || gameManager.IsDraw())
        {
            return EvaluateBoard(board);
        }

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int row = GetRow(board, col);
                    board[row, col] = GameManager.TokenState.Red;
                    int eval = Minimax(board, depth - 1, false, alpha, beta);
                    board[row, col] = GameManager.TokenState.Empty;
                    maxEval = Mathf.Max(maxEval, eval);
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha) break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int row = GetRow(board, col);
                    board[row, col] = GameManager.TokenState.Yellow;
                    int eval = Minimax(board, depth - 1, true, alpha, beta);
                    board[row, col] = GameManager.TokenState.Empty;
                    minEval = Mathf.Min(minEval, eval);
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha) break;
                }
            }
            return minEval;
        }
    }

    int EvaluateBoard(GameManager.TokenState[,] board)
    {
        int score = 0;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (board[row, col] != GameManager.TokenState.Empty)
                {
                    int points = GetScoreForPosition(board, row, col, board[row, col]);
                    score += (board[row, col] == GameManager.TokenState.Red) ? points : -points;
                }
            }
        }
        return score;
    }

    int GetScoreForPosition(GameManager.TokenState[,] board, int row, int col, GameManager.TokenState player)
    {
        int score = 0;
        score += CountAligned(board, row, col, 1, 0, player);
        score += CountAligned(board, row, col, 0, 1, player);
        score += CountAligned(board, row, col, 1, 1, player);
        score += CountAligned(board, row, col, 1, -1, player);
        return score;
    }

    int CountAligned(GameManager.TokenState[,] board, int row, int col, int dX, int dY, GameManager.TokenState player)
    {
        int count = 0;
        int score = 0;
        for (int i = -3; i <= 3; i++)
        {
            int x = row + i * dX;
            int y = col + i * dY;

            if (x >= 0 && x < 6 && y >= 0 && y < 7)
            {
                if (board[x, y] == player)
                {
                    count++;
                }
                else if (board[x, y] != GameManager.TokenState.Empty)
                {
                    count = 0;
                }
            }
        }
        if (count >= 4) score += 1000;
        else if (count == 3) score += 100;
        else if (count == 2) score += 10;
        return score;
    }

    bool IsValidMove(GameManager.TokenState[,] board, int col)
    {
        return board[5, col] == GameManager.TokenState.Empty;
    }

    int GetRow(GameManager.TokenState[,] board, int col)
    {
        for (int row = 0; row < 6; row++)
        {
            if (board[row, col] == GameManager.TokenState.Empty)
                return row;
        }
        return -1;
    }
}