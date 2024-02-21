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

  /*  public void ShowPeople()
    {
        // Fetch win ratio data from server (replace with your implementation)
        var winRatios = GetWinRatiosFromServer();

        // Order people by win ratio (descending)
        var sortedPeople = People.instance.list.users
            .Join(winRatios, p => p.id, wr => wr.user_id, (p, wr) => new { Person = p, WinRatio = wr.win_ratio })
            .OrderByDescending(p => p.WinRatio)
            .Select(p => p.Person)
            .ToList();

        // Reset list and iterate through sorted people
        ResetList();
        foreach (People.Person person in sortedPeople)
        {
            GameObject personInst = Instantiate(personTamplate);
            personInst.transform.SetParent(scrollView.transform);
            personInst.GetComponent<RankPersonTemplate>().SetData(
                person.id, person.username, person.image, person.level, person.win_ratio, ShowPeople); // Pass win ratio
        }
    }
  */

    private void ResetList()
    {
        foreach (RankPersonTemplate person in scrollView.transform.GetComponentsInChildren<RankPersonTemplate>())
            Destroy(person.gameObject);
    }
}
