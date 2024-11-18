namespace GameBench
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DamageNumbersPro;

    public class CoinController : MonoBehaviour
    {
        public MazecraftGameManager instance;
        public DamageNumber numberPrefab;
        private RandomCoins randomCoins;

/*        void Start()
        {
            instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
            randomCoins = GameObject.Find("Terrain").GetComponent<RandomCoins>();
        }

        void Update()
        {
            if(instance.forestTerrain.activeInHierarchy == false)
            {
                randomCoins.currentObjects = 0;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            int randomNumber = Random.Range(1, 10);

            DamageNumber damageNumber = numberPrefab.Spawn(transform.position, instance.notificationDescriptions[Random.Range(0, instance.notificationDescriptions.Count - 1)]);

            Destroy(gameObject);

            randomCoins.currentObjects -= 1;

            if (randomNumber == 7)
            {
                PlayerData.Instance.Coins += 10;
            }
        }
*/    }
}