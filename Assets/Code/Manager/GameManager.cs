using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Controller;


public enum GameState
{
    READY,
    BLACK_TURN,
    WHITE_TURN,
    ACTING,
    GAMEOVER
}


public class GameManager : Singleton<GameManager>
{

    public int leftover { get; set; } = 20;
    public float turnTimer { get; set; } = 30.0f;

    public bool isPAUSE { get; set; } = false;

    public string win { get; set; }="WIN";
    public Dictionary<string, string> arrangent { get; private set; } = new Dictionary<string, string>();
    public string selectBackground { get; private set; } = "Spring";
    public string my_color { get; private set; } = "Black";


    public bool isSpawnFinished { get; set; } = false;


    public bool isShot = false; // 돌이 발사된 상태인지 여부

    public GameState gameState { get; set; } = GameState.READY;
    public GameState currentState= GameState.READY;
    private bool isBlackTurn;    

    public void  CheckTurnEnd()
    {
        
        if(currentState == GameState.BLACK_TURN)
            changeState(GameState.WHITE_TURN);
        else if(currentState == GameState.WHITE_TURN)
            changeState(GameState.BLACK_TURN);

    }


    public bool IsTurn(TURN stoneColor)
    {
        // 행동 중이거나 게임 오버면 무조건 금지
        if (currentState == GameState.ACTING || currentState == GameState.GAMEOVER)
            return false;


        StartCoroutine(WaitForIt());
        // "현재 상태가 흑 턴이고, 니가 가져온 돌이 흑색이면 OK"
        if (currentState == GameState.BLACK_TURN && stoneColor == TURN.BLACK)
            return true;

        // "현재 상태가 백 턴이고, 니가 가져온 돌이 백색이면 OK"
        if (currentState == GameState.WHITE_TURN && stoneColor == TURN.WHITE)
            return true;

        return false;
    }

        
    private void GameOver()
    {
        if(isSpawnFinished==false)
            return;
        int[] counts = new int[2];

        counts = StoneManager.Inst.currntCount();

        int blackCount = counts[0];
        int whiteCount = counts[1];

        

        if (blackCount == 0 || whiteCount == 0)
        {
            string winner = "";

            if (blackCount == 0 && whiteCount == 0) winner =currentState.ToString(); // 무승부/버그 상황
            else if (whiteCount == 0) winner = "Black WIN";
            else if (blackCount == 0) winner = "White WIN";

            EndGame(winner);
        }

        if (leftover <= 0)
        {
            string winner = (blackCount > whiteCount) ? "Black" : "White";
            EndGame(winner);
        }
    }

    private void EndGame(string winnerName)
    {
        currentState = GameState.GAMEOVER;
        win = winnerName;
        changeState(GameState.GAMEOVER);
    }

    private void changeState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.READY:
                leftover = 20;
                turnTimer = 30.0f;
                break;

            case GameState.BLACK_TURN:
                isBlackTurn= true;
                currentState = GameState.BLACK_TURN;
                turnTimer = 30.0f;
                leftover--;
                break;

            case GameState.WHITE_TURN:
                isBlackTurn=false;
                currentState = GameState.WHITE_TURN;
                turnTimer = 30.0f;
                leftover--;
                break;
            case GameState.ACTING:
                currentState = GameState.ACTING;
                break;

            case GameState.GAMEOVER:

                GameObject UIRestart = GameObject.Find("Restart");
                UIRestart.transform.localScale = new Vector3(1, 1, 1);
                break;
        }

    }

    private void Update()
    {
        if(!isPAUSE)
        {
            switch (currentState)
            {
                case GameState.READY:
                    // setting phase에서 아무것도 못하게 play씬에가면 black turn으로 변경
                    if (SceneManager.GetActiveScene().name == "PlaySences")

                        changeState(GameState.BLACK_TURN);
                    break;

                case GameState.BLACK_TURN:

                    turnTimer -= Time.deltaTime;
                    if (turnTimer <= 0) changeState(GameState.WHITE_TURN);

                    // 혹은 trun이 끝나면 white turn으로 변경
                    if (isShot)
                        changeState(GameState.ACTING);
                    GameOver();
                    break;


                case GameState.WHITE_TURN:

                    turnTimer -= Time.deltaTime;
                    if (turnTimer <= 0) changeState(GameState.BLACK_TURN);
                    // 혹은 trun이 끝나면 black turn으로 변경
                    if (isShot)
                        changeState(GameState.ACTING);
                    GameOver();
                    break;
                case GameState.ACTING:
                    if (!isShot)
                        if (isBlackTurn)
                            changeState(GameState.WHITE_TURN);
                        else
                            changeState(GameState.BLACK_TURN);
                    GameOver();
                    break;

                case GameState.GAMEOVER:
                    // 재시작 시 READY 상태로 변경
                    if (SceneManager.GetActiveScene().name != "PlaySences")
                    {
                        currentState = GameState.READY;
                        changeState(GameState.READY);
                    }
                    break;
            }

        }

    }



    public void OnButtonClick(string pattern,string color)
    {
        arrangent[color] = pattern;
    }
    public void OnButtonClick(string background)
    {
        this.selectBackground = background;
    }
    public void OnButtonClick_color(string color)
    {
        this.my_color = color;
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
