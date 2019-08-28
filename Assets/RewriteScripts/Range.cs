using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Node target;
    private float range;
    public GameObject rangeSize;

    public static Range instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }  

    public void SetTarget(Node _target)
    {
        target = _target;
        rangeSize.transform.position = target.GetBuildPosition();
        if (target.turret == null)
        {
            return;
        }
        if(target.turret.GetComponent<TowerBase>())
            range = target.turret.GetComponent<TowerBase>().range * 2;
        else
            range = target.turret.GetComponentInChildren<TowerBase>().range * 2;
        rangeSize.transform.localScale = new Vector3(range, .1f, range);
        rangeSize.SetActive(true);
    }
    public void HoverTarget(GameObject _target)
    {
        target = _target.GetComponent<Node>();
        Vector3 nodePosition = _target.transform.position;
        nodePosition.y += .5f;
        if(target.buildManager.GetTurretToBuild().prefab.GetComponent<TowerBase>())
            range = target.buildManager.GetTurretToBuild().prefab.GetComponent<TowerBase>().range * 2;
        else
            range = target.buildManager.GetTurretToBuild().prefab.GetComponentInChildren<TowerBase>().range * 2;
        rangeSize.transform.localScale = new Vector3(range, .1f, range);
        rangeSize.transform.position = nodePosition;
        rangeSize.SetActive(true);
    }

    public void Hide()
    {
        rangeSize.SetActive(false);
    }
}
