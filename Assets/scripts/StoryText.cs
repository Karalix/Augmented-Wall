using UnityEngine;
using System.Collections;

public class StoryText : MonoBehaviour {

    public float changeRate = 1f;
    private float changeCooldown;

    // Use this for initialization
    void Start()
    {
        changeCooldown = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (changeCooldown > 0)
        {
            changeCooldown -= Time.deltaTime;
        }
        else {
            changeCooldown = changeRate;

            var textMesh = GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = "";
                foreach(Story s in NewsService.Instance.Stories)
                {
                    string title = s.Title;
                    if(title.Length > 35)
                    {
                        title = title.Substring(0, 34);
                        title += "...";
                    }
                    textMesh.text += s.score +"  "+title+'\n';
                }
            }
        }
    }
}
