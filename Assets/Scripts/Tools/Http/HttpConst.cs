using System;

public class HttpConst
{
    private HttpConst(string value) { Value = value; }

    public string Value { get; private set; }

    // private static readonly string HOST = "https://chronoclashapi-production.up.railway.app/api";
    private static readonly string HOST = "http://localhost:8081/api";
    public static HttpConst LOGIN { get { return new HttpConst($"{HOST}/auth/login"); } }
    public static HttpConst REGISTER { get { return new HttpConst($"{HOST}/auth/signup"); } }
    public static HttpConst LOGOUT { get { return new HttpConst($"{HOST}/auth/logout"); } }
    public static HttpConst LOGOUT_ALL_DEVICES { get { return new HttpConst($"{HOST}/auth/logout-all"); } }
    public static HttpConst CHANGE_PASSWORD { get { return new HttpConst($"{HOST}/auth/change-password"); } }
    public static HttpConst REFRESH_TOKEN { get { return new HttpConst($"{HOST}/auth/refresh-token"); } }

    public static HttpConst SSE { get { return new HttpConst($"{HOST}/sse"); } }

    public static HttpConst CREATEGAME { get { return new HttpConst($"{HOST}/user/game"); } }

    public static HttpConst ME { get { return new HttpConst($"{HOST}/user/me"); } }
    public static HttpConst UPDATE_USER { get { return new HttpConst($"{HOST}/user/me"); } }

    public static HttpConst USERS { get { return new HttpConst($"{HOST}/user/users"); } }

    public static HttpConst FRIENDS { get { return new HttpConst($"{HOST}/friend/all"); } }
    public static HttpConst ADD_FRIEND { get { return new HttpConst($"{HOST}/friend/add"); } }
    public static HttpConst FRIEND_NOTIFICATIONS { get { return new HttpConst($"{HOST}/friend/notifications"); } }
    public static HttpConst ACCEPT_FRIEND { get { return new HttpConst($"{HOST}/friend/accept"); } }

    public static HttpConst CONNECTIONS { get { return new HttpConst($"{HOST}/network/connections"); } }
    public static HttpConst DELETE_CONNECTION { get { return new HttpConst($"{HOST}/network/disconnect"); } }

    public override string ToString()
    {
        return Value;
    }
}

