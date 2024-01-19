using System;

public class HttpConst
{
    private HttpConst(string value) { Value = value; }

    public string Value { get; private set; }

    public static HttpConst LOGIN { get { return new HttpConst("https://chronoclashapi-production.up.railway.app/api/auth/login"); } }
    public static HttpConst REGISTER { get { return new HttpConst("https://chronoclashapi-production.up.railway.app/api/auth/signup"); } }
    public static HttpConst CREATEGAME { get { return new HttpConst("https://chronoclashapi-production.up.railway.app/api/user/game"); } }
    public static HttpConst LOGOUT { get { return new HttpConst("https://chronoclashapi-production.up.railway.app/api/auth/logout"); } }


    public override string ToString()
    {
        return Value;
    }
}

