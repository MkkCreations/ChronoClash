using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PeopleList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject personTamplate;

    private void OnEnable()
    {
        var request = UnityWebRequest.Get(HttpConst.USERS.Value);
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetUsers(request, ShowPeople));
    }

    public void ShowPeople()
    {
        ResetList();
        foreach (People.Person person in People.instance.list.users)
        {
            GameObject personInst = Instantiate(personTamplate);
            personInst.transform.SetParent(scrollView.transform);
            personInst.GetComponent<PersonTemplate>().SetData(person.id, person.username, person.image, person.level, ShowPeople);
        }
    }

    public void ResetList()
    {
        foreach (PersonTemplate person in scrollView.transform.GetComponentsInChildren<PersonTemplate>())
            Destroy(person.gameObject);
    }
}
