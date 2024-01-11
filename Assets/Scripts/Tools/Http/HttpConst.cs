using System;

public class HttpConst
{
    private HttpConst(string value) { Value = value; }

    public string Value { get; private set; }

    public static HttpConst LOGIN { get { return new HttpConst("http://127.0.0.1:8081/api/auth/login"); } }
    public static HttpConst REGISTER { get { return new HttpConst("http://127.0.0.1:8081/api/auth/signup"); } }
    public static HttpConst CREATEGAME { get { return new HttpConst("http://127.0.0.1:8081/api/user/game"); } }
    public static HttpConst LOGOUT { get { return new HttpConst("http://127.0.0.1:8081/api/auth/logout"); } }


    public override string ToString()
    {
        return Value;
    }
}

