using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public List<EnemyData> Datas;
    public EnemyData currentData;

    [Tooltip("Meduse : 0 , Shark : 1 , Serpent : 2")]
    public List<Transform> enemiesPositions = new();
    public int currentEnemy;

    public GameObject vfx;
    public AudioClip sfx;
    public AudioSource audioSource;
    public string dialogue;

    public enum TypeEnemy { Meduse, Shark, Serpent }
    public TypeEnemy typeEnemy;

    void Awake()
    {
      
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

        Instantiate(vfx, enemiesPositions[currentEnemy].position, Quaternion.identity);

        //SFX
        audioSource.PlayOneShot(sfx);
    }

    void LaunchDialogue()
    {

    }

    public void UpdateEnemy() 
    {
        currentEnemy++;
        currentData = Datas[currentEnemy];
       GetComponent<Health>().InitializeEnemy(currentData);
    }


}
