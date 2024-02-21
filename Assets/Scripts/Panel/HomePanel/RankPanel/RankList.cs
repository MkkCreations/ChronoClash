using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor.VersionControl;
using System.Linq;

public class RankList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject personTamplate;

    private void OnEnable()
    {
        var request = UnityWebRequest.Get(HttpConst.USERS.Value);
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetUsers(request, ShowPeople));
    }

    public void ShowPeople(){
        

        var sortedPeople = People.instance.list.users.OrderByDescending(person => person.level).ToList();

       
        ResetList();

        foreach (People.Person person in sortedPeople)
        {
            GameObject personInst = Instantiate(personTamplate);
            personInst.transform.SetParent(scrollView.transform);
            personInst.GetComponent<RankPersonTemplate>().SetData(person.id, person.username, person.image, person.level, ShowPeople);
        }
    }


    private void ResetList()
    {
        foreach (RankPersonTemplate person in scrollView.transform.GetComponentsInChildren<RankPersonTemplate>())
            Destroy(person.gameObject);
    }
}
