using System;

public class HttpConst
{
    private HttpConst(string value) { Value = value; }

    public string Value { get; private set; }

    private static string HOST = "https://chronoclashapi-production.up.railway.app/api";
    public static HttpConst LOGIN { get { return new HttpConst($"{HOST}/auth/login"); } }
    public static HttpConst REGISTER { get { return new HttpConst($"{HOST}/auth/signup"); } }
    public static HttpConst CREATEGAME { get { return new HttpConst($"{HOST}/user/game"); } }
    public static HttpConst LOGOUT { get { return new HttpConst($"{HOST}/auth/logout"); } }
    public static HttpConst LOGOUT_ALL_DEVICES { get { return new HttpConst($"{HOST}/auth/logout-all"); } }
    public static HttpConst CHANGE_PASSWORD { get { return new HttpConst($"{HOST}/auth/change-password"); } }
    public static HttpConst REFRESH_TOKEN { get { return new HttpConst($"{HOST}/auth/refresh-token"); } }
    public static HttpConst CONNECTIONS { get { return new HttpConst($"{HOST}/user/connections"); } }
    public static HttpConst UPDATE_USER { get { return new HttpConst($"{HOST}/user/me"); } }

    public override string ToString()
    {
        return Value;
    }
}

