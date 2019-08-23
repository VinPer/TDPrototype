using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Click();
        }
    }

    private void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.gameObject.GetComponent<Node>())
            {
                Node node = hit.transform.gameObject.GetComponent<Node>();
                node.SelectNode();
            }
        }

        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit))
        //{
        //    if (hit.transform.gameObject.GetComponent<Node>())
        //    {
        //        Node node = hit.transform.gameObject.GetComponent<Node>();
        //        node.SelectNode();
        //    }
        //}

    }
}
