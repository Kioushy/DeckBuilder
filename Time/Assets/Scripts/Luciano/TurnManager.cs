using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Abyss;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

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
        BattleSetup();
    }

    void Update()
    {

    }


    public void BattleSetup()
    {
        DeckManager.Instance.Shuffle();
        DeckManager.Instance.DrawCard(HandManager.Instance.maxHandSize);
        currentState = State.PlayerTurn;
    }

    public void EnemyTurn() 
    {
        currentState = State.EnemyTurn;
        StartCoroutine(AttackEnemy());

    }

    public IEnumerator AttackEnemy() 
    {
        // attack enemy
        //Damage Player / Heal Enemy
        yield return new WaitForSeconds(1f);
        currentState = State.PlayerTurn;
    }

    public void Lose() { }
    public void Win() { }

}
