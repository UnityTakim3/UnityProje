using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawCube(getChild(i), new Vector3(0.2f, 0.2f, 0.2f));
            int j = getNextChild(i);
            Gizmos.DrawLine(getChild(i), getChild(j));
        }
    }
    public Vector3 getChild(int i)
    {
        return transform.GetChild(i).position;
    }
    public int getNextChild(int i)
    {
        if (i == transform.childCount - 1)
        {
            return 0;
        }
        return i + 1;
    }


}
