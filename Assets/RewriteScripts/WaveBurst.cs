using UnityEngine;

[System.Serializable]
public class WaveBurst
{
    public Enums.EnemyType enemyType;
    public int count;
    public float rate;

    public WaveBurst(Enums.EnemyType _enemyType,int _count,float _rate)
    {
        enemyType = _enemyType;
        count = _count;
        rate = _rate;
    }
}

