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

    private int leftover = 20;
    private float turnTimer = 30.0f;

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

        // "현재 상태가 흑 턴이고, 니가 가져온 돌이 흑색이면 OK"
        if (currentState == GameState.BLACK_TURN && stoneColor == TURN.BLACK)
            return true;

        // "현재 상태가 백 턴이고, 니가 가져온 돌이 백색이면 OK"
        if (currentState == GameState.WHITE_TURN && stoneColor == TURN.WHITE)
            return true;

        return false;
    }


    private void GameOver ()
    {
        if(leftover == 0)
            changeState(GameState.GAMEOVER);
        else
            leftover--;
    }

    public void changeState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.READY:
                // 원래 작업에서 필요한작업들을 여기에 추가
                leftover = 20;
                turnTimer = 30.0f;
                break;

            case GameState.BLACK_TURN:
                isBlackTurn= true;
                currentState = GameState.BLACK_TURN;
                // 블랙 타이머 리셋
                turnTimer = 30.0f;
                // ui 텍스트 변경
                // 해당 턴 플레이어는 입력가능하게


                break;

            case GameState.WHITE_TURN:
                isBlackTurn=false;
                currentState = GameState.WHITE_TURN;
                // 화이트 타이머 리셋
                turnTimer = 30.0f;
                // ui 텍스트 변경
                // 해당 턴 플레이어는 입력가능하게
                GameOver();
                break;
            case GameState.ACTING:
                currentState = GameState.ACTING;
                break;

            case GameState.GAMEOVER:
                // 재시작 버튼 띄우기


                break;
        }

    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.READY:
                // setting phase에서 아무것도 못하게 play씬에가면 black turn으로 변경
                if (SceneManager.GetActiveScene().name == "PlaySences")
                 
                    changeState(GameState.BLACK_TURN);
                break;

            case GameState.BLACK_TURN:

                // 시간이 다되면 white turn으로 변경
                turnTimer -= Time.deltaTime;
                if (turnTimer <= 0) changeState(GameState.WHITE_TURN);

                // 혹은 trun이 끝나면 white turn으로 변경
                if (isShot)
                    changeState(GameState.ACTING);
                GameOver();
                break;

            case GameState.WHITE_TURN:
                // 시간이 다되면 black turn으로 변경
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
                break;
        }
    }


    public Dictionary<string, string> arrangent { get; private set; } = new Dictionary<string, string>();
    public string  selectBackground { get; private set; } = "Spring";
    public string my_color { get; private set; } = "Black";

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
}
