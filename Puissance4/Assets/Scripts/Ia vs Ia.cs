using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IavsIa : MonoBehaviour
{
    public GameManagerIavsIa gameManager;
    public int maxDepth = 5;

    private void Start()
    {
        PlayBestMove();
    }

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
        int bestScore = int.MinValue;
        GameManagerIavsIa.TokenState[,] board = gameManager.TakeGrid();

        List<int> columns = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
        Shuffle(columns); // Mélange les colonnes

        List<int> bestMoves = new List<int>();

        foreach (int col in columns)
        {
            if (IsValidMove(board, col))
            {
                int row = GetRow(board, col);
                board[row, col] = GameManagerIavsIa.TokenState.Red;
                int score = Minimax(board, maxDepth, false, int.MinValue, int.MaxValue);
                board[row, col] = GameManagerIavsIa.TokenState.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(col);
                }
                else if (score == bestScore)
                {
                    bestMoves.Add(col);
                }
            }
        }

        return bestMoves[Random.Range(0, bestMoves.Count)];
    }

        int Minimax(GameManagerIavsIa.TokenState[,] board, int depth, bool maximizingPlayer, int alpha, int beta)
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
                        board[row, col] = (GameManagerIavsIa.TokenState.Red);
                        int eval = Minimax(board, depth - 1, false, alpha, beta);
                        board[row, col] = (GameManagerIavsIa.TokenState.Empty);
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
                        board[row, col] = (GameManagerIavsIa.TokenState.Yellow);
                        int eval = Minimax(board, depth - 1, true, alpha, beta);
                        board[row, col] = (GameManagerIavsIa.TokenState.Empty);
                        minEval = Mathf.Min(minEval, eval);
                        beta = Mathf.Min(beta, eval);
                        if (beta <= alpha) break;
                    }
                }

                return minEval;
            }
        }

        int EvaluateBoard(GameManagerIavsIa.TokenState[,] board)
        {
            int score = 0;
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] != (GameManagerIavsIa.TokenState.Empty))
                    {
                        int points = GetScoreForPosition(board, row, col, board[row, col]);
                        score += (board[row, col] == (GameManagerIavsIa.TokenState.Red)) ? points : -points;
                    }
                }
            }

            return score;
        }

        int GetScoreForPosition(GameManagerIavsIa.TokenState[,] board, int row, int col,
            GameManagerIavsIa.TokenState player)
        {
            int score = 0;
            score += CountAligned(board, row, col, 1, 0, player);
            score += CountAligned(board, row, col, 0, 1, player);
            score += CountAligned(board, row, col, 1, 1, player);
            score += CountAligned(board, row, col, 1, -1, player);
            return score;
        }

        int CountAligned(GameManagerIavsIa.TokenState[,] board, int row, int col, int dX, int dY,
            GameManagerIavsIa.TokenState player)
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
                    else if (board[x, y] != (GameManagerIavsIa.TokenState.Empty))
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

        bool IsValidMove(GameManagerIavsIa.TokenState[,] board, int col)
        {
            return board[5, col] == (GameManagerIavsIa.TokenState.Empty);
        }

        int GetRow(GameManagerIavsIa.TokenState[,] board, int col)
        {
            for (int row = 0; row < 6; row++)
            {
                if (board[row, col] == (GameManagerIavsIa.TokenState.Empty))
                    return row;
            }

            return -1;
        }

        void Shuffle(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                int temp = list[i];
                list[i] = list[rnd];
                list[rnd] = temp;
            }
        }
}
