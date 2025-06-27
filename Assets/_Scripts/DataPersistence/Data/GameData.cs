using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etorium.DataPersistence.Data
{
    [Serializable]
    public class GameData
    {
        public int deathCount;
        public Vector3 playerPosition;
        public Vector3 lastRestPosition;
        public string lastRestId;
        // public SerializableDictionary<string, bool> coinsCollected;

        // the values defined in this constructor will be the default values
        // the game starts with when there's no data to load
        public GameData() 
        {
            this.deathCount = 0;
            playerPosition = Vector3.zero;
            lastRestPosition = Vector3.zero;
            lastRestId = "";
            // coinsCollected = new SerializableDictionary<string, bool>();
        }
    }
}
