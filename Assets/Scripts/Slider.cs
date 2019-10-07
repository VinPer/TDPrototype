using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public RectTransform panel; // hold the ScrollPanel
    public Button[] btn;
    public RectTransform centerToCompare; //Center to Compare
    public float scrollSpeed = 1.0f;
    public float[] distRepostion;
    public DialogueTrigger dialogTrigger;
    public DialogSystem dialogSystem;


    private float[] distance; // btns distance to the center
    private bool draggin = false; //True when drag panel  
    private int btnDist;
    private int minBtnNum; // Numb of button 
    private bool triggerMessage;


    private void Start()
    {
        int btnLength = btn.Length;
        distance = new float[btnLength];
        distRepostion = new float[btnLength];

        btnDist = (int)Mathf.Abs(btn[1].GetComponent<RectTransform>().anchoredPosition.x - btn[0].GetComponent<RectTransform>().anchoredPosition.x);
        Debug.Log(btnDist);
        //Debug.Log(distance);
        //Debug.Log(distRepostion);
    }

    private void Update()
    {
        Sliding();
    }

    void LerpToBtn(float pos)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, pos, Time.deltaTime * scrollSpeed);
            
        if (Mathf.Abs(newX) >= Mathf.Abs(pos) -1f && Mathf.Abs(newX) <= Mathf.Abs(pos) + 1 && !triggerMessage)
        {
            triggerMessage = true;
            Debug.Log("newX: " + newX);
            Debug.Log("pos: " + pos);
            Debug.Log(btn[minBtnNum].name);
            switch (btn[minBtnNum].name)
            {
                case "Lv (1)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
                case "Lv (2)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
                case "Lv (3)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
                case "Lv (4)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
                case "Lv (5)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
                case "Lv (6)":
                    dialogTrigger.TriggerDialogue();
                    Debug.Log("Selected Level: " + btn[minBtnNum].name);
                    break;
            }
        }

        Vector2 newPos = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPos;
    }
    public void Sliding()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            distRepostion[i] = centerToCompare.GetComponent<RectTransform>().position.x - btn[i].GetComponent<RectTransform>().position.x;

            distance[i] = Mathf.Abs(distRepostion[i]);
            //Debug.Log(i+ ":" + distRepostion[i]);
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
            LerpToBtn(-btn[minBtnNum].GetComponent<RectTransform>().anchoredPosition.x);
        }

    }
    public void StartDrag()
    {
        draggin = true;
        triggerMessage = false;
        scrollSpeed = 2f;
    }

    public void EndDrag()
    {
        draggin = false;
        scrollSpeed = 10f;
    }
}

