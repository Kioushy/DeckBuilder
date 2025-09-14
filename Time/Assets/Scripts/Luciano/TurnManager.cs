using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public DeckManager DeckM;
    public enum State {PlayerTurn,EnemyTurn,Busy}
    public State currentState;

    public Enemies Enemies;
    public ChatManager chat;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }


    }

    public void BattleSetup()
    {
        if (Enemies.currentEnemy < 0)
        {
            chat.UpdateChat(Enemies.Datas[0].dialogues[0]);
        }
        else
        {
            chat.UpdateChat(Enemies.currentData.dialogues[0]);
        }

        DeckM.Shuffle();
        PlayerTurn();
    }

    public void EnemyTurn() 
    {
        Debug.Log("turnM turn");
        currentState = State.EnemyTurn;
        StartCoroutine(AttackEnemy());

    }

    public IEnumerator AttackEnemy() 
    {
        // attack enemy
        Enemies.Attack(); 
        yield return new WaitForSeconds(3f);
        PlayerTurn();
    }

    public void PlayerTurn() 
    {
        currentState = State.PlayerTurn;
        DeckM.DrawCard(HandManager.Instance.maxHandSize);
    }

}
