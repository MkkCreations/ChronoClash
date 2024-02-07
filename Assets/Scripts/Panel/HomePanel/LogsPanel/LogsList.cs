using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogsList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject operationTamplate;

    private void Start()
    {
        foreach (User.Log log in User.instance.user.user.logs)
        {
            foreach (User.Log.LogsOperation op in log.operations)
            {
                GameObject opInst = Instantiate(operationTamplate);
                opInst.transform.SetParent(scrollView.transform);
                opInst.GetComponent<LogTemplate>().SetData(op.type, op.description, DateTime.Parse(op.date));
            }
        }
    }
}
