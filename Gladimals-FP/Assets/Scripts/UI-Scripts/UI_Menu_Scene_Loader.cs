using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Scene_Loader : MonoBehaviour
{
    public GameObject treePrefab;

    void Start()
    {
        CreateTreeCircles(9, new Vector3(0f,0f,0f), 10);
        CreateTreeCircles(18, new Vector3(0f,0f,0f), 13);
        CreateTreeCircles(27, new Vector3(0f,0f,0f), 16);
        CreateTreeCircles(36, new Vector3(0f,0f,0f), 19);
        CreateTreeCircles(45, new Vector3(0f,0f,0f), 21);
    }

    public void CreateTreeCircles (int num, Vector3 point, float radius)
    {
        for(int i = 0; i < num; i++)
        {
            var radians = 2 * Mathf.PI / num * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = point + spawnDir * radius;
            var tree = Instantiate(treePrefab, spawnPos, Quaternion.identity) as GameObject;

            tree.transform.LookAt(point);
            tree.transform.Translate(new Vector3(0, tree.transform.localScale.y / 2, 0));
        }
    }
}
