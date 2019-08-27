using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    public Transform referencePoint;

    public List<SkillBlueprint> selectedSkills;
    public SkillBlueprint bomb;
    public SkillBlueprint spikes;
    public SkillBlueprint puddle;

    public List<Text> skillsText;

    // Start is called before the first frame update
    void Start()
    {
        LoadDefault();

        skillsText = new List<Text>();
        for (int i = 0; i < transform.childCount; i++) skillsText.Add(transform.GetChild(i).GetComponentInChildren<Text>());
    }

    // Update is called once per frame
    void Update()
    {
        RunCooldowns();
    }

    public void SelectSkill(int index)
    {
        SkillBlueprint skill = selectedSkills[index];
        if (skill.currentCooldown > 0f) return;
        skill.currentCooldown = skill.initialCooldown;

        float distanceToScreen;
        Vector3 posMove;
        distanceToScreen = Camera.main.WorldToScreenPoint(referencePoint.position).z;
        posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
        Vector3 position = new Vector3(posMove.x, skill.ySpawn, posMove.z);

        Instantiate(skill.prefab, position, Quaternion.identity);
    }

    private void RunCooldowns()
    {
        SkillBlueprint currentSkill;
        for (int i = 0; i < selectedSkills.Count; i++)
        {
            currentSkill = selectedSkills[i];
            if (currentSkill.currentCooldown > 0f)
            {
                currentSkill.currentCooldown -= Time.deltaTime;
                skillsText[i].text = currentSkill.name + "\n" + Mathf.Round(currentSkill.currentCooldown);
            }
            else
            {
                skillsText[i].text = currentSkill.name + "\nREADY";
            }
        }
    }

    private void LoadDefault()
    {
        selectedSkills = new List<SkillBlueprint>
        {
            puddle,
            bomb,
            spikes
        };
    }
}
