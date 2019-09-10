using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public RectTransform panel; // hold the ScrollPanel
    public Button[] btn;
    public RectTransform center; //Center to Compare

    private float[] distance; // btns distance to the center
    public float[] distRepostion;
    private bool draggin = false; //True when drag panel  
    private int btnDist;
    private int minBtnNum; // Numb of button 


    private void Start()
    {
        int btnLength = btn.Length;
        distance = new float[btnLength];
        distRepostion = new float[btnLength];

        btnDist = (int)Mathf.Abs(btn[1].GetComponent<RectTransform>().anchoredPosition.x - btn[0].GetComponent<RectTransform>().anchoredPosition.x);
        Debug.Log(btnDist);
        Debug.Log(distance);
        Debug.Log(distRepostion);
    }

    private void Update()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            distRepostion[i] = center.GetComponent<RectTransform>().position.x - btn[i].GetComponent<RectTransform>().position.x;

            distance[i] = Mathf.Abs(center.GetComponent<RectTransform>().position.x - btn[i].GetComponent<RectTransform>().position.x);
            Debug.Log(i+ ":" + distRepostion[i]);
            if (distRepostion[i] > -11)
            {
                float curX = btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = btn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (btn.Length * btnDist), curY);
                btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;

            }
        }
        float minDist = Mathf.Min(distance);

        for (int a = 0; a < btn.Length; a++)
        {
            if (minDist == distance[a])
            {
                minBtnNum = a;
            }
        }

        if (!draggin)
        {
            LerpToBtn(minBtnNum * -btnDist);
        }
    }

    void LerpToBtn(int pos)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, pos, Time.deltaTime * 10f);
        Vector2 newPos = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPos;
    }

    public void StartDrag()
    {
        draggin = true;
    }

    public void EndDrag()
    {
        draggin = false;
    }
}

