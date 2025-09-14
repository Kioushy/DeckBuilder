using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    public TurnManager turnManager;
    public DeckManager deckManager;
    public Health health;
    public List<EnemyData> Datas;
    public EnemyData currentData;
    
    [Tooltip("Meduse : 0 , Shark : 1 , Serpent : 2")]
    public List<Transform> enemiesPositions = new();
    public int currentEnemy;

    public AudioSource audioSource;

    public enum TypeEnemy { Meduse, Shark, Serpent }
    public TypeEnemy typeEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
        UpdateEnemy();
    }

    public void Attack()
    {
        //Damage
        GameFlowManager.Instance.healthP.TakeDamagePlayer(-currentData.damage);

        //VFX
        switch (typeEnemy)
        {
            case TypeEnemy.Meduse:
                currentEnemy = 0;
                break;
            case TypeEnemy.Shark:
                currentEnemy = 1;
                break;
            case TypeEnemy.Serpent:
                currentEnemy = 2;
                break;
        }
        StartCoroutine(PlayVFXAndSFX());

    }



    public void UpdateEnemy()
    {
        Debug.Log("Update turnM");

        if (health.currentHealth <0)
        {
            if (currentEnemy < 0)
            {
                enemiesPositions[0].gameObject.SetActive(false);
            }
            else
            {
                enemiesPositions[currentEnemy].gameObject.SetActive(false);
            }
        }

        currentEnemy++;

        switch (currentEnemy)
        {
            case 0:
                typeEnemy = TypeEnemy.Meduse;
                break;
            case 1:
                typeEnemy = TypeEnemy.Shark;
                break;
            case 2:
                typeEnemy = TypeEnemy.Serpent;
                break;

        }

        Camera.main.GetComponent<CameraFollow>().SetTarget();
        currentData = Datas[currentEnemy];
        health.InitializeEnemy(currentData);
        deckManager.ResetDeck();
        turnManager.BattleSetup();
    }

    public IEnumerator PlayVFXAndSFX()
    {
        audioSource.pitch = Random.Range(0.8f,1.2f);

        //VFX
        GameObject vfx = Instantiate(currentData.vfx, enemiesPositions[currentEnemy].position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        //SFX
        audioSource.PlayOneShot(currentData.attackSound);
        Destroy(vfx, 2f);


    }


}
