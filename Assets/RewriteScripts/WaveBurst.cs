using UnityEngine;

[System.Serializable]
public class WaveBurst
{
    public GameObject enemyType;
    public int count;
    public float rate;

    public WaveBurst(GameObject _enemyType,int _count,float _rate)
    {
        enemyType = _enemyType;
        count = _count;
        rate = _rate;
    }
}

