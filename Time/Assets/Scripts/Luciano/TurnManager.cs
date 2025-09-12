using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public DeckManager DeckM;
    public enum State {PlayerTurn,EnemyTurn,Busy}
    public State currentState;

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
    void Start()
    {
       
        StartCoroutine(BattleSetupTimer());
    }

    void Update()
    {

    }


    public void BattleSetup()
    {
        DeckM.Shuffle();
        DeckM.DrawCard(HandManager.Instance.maxHandSize);
        currentState = State.PlayerTurn;
    }

    public void EnemyTurn() 
    {
        Debug.Log("Enemy turn");
        currentState = State.EnemyTurn;
        StartCoroutine(AttackEnemy());

    }

    public IEnumerator BattleSetupTimer()
    {
        yield return new WaitForSeconds(1f);
        BattleSetup();
    }

    public IEnumerator AttackEnemy() 
    {
        // attack enemy
        //Damage Player / Heal Enemy
        yield return new WaitForSeconds(1f);
        PlayerTurn();
    }

    public void PlayerTurn() 
    {
        currentState = State.PlayerTurn;
        DeckM.DrawCard(HandManager.Instance.maxHandSize);
    }

    public void Lose() { }
    public void Win() { }

}
