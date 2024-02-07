using System;


public static class DateTool
{

    public static string Diff(DateTime date)
    {
        int days = DayDiff(date);
        int hours = HourDiff(date);
        int mins = MinDiff(date);
        if (days == 0)
        {
            if (hours == 0)
            {
                return $"{mins} mins ago";
            }
            return $"{hours} hours ago";
        }
        return $"{days} days ago";
    }

    public static int DayDiff(DateTime date)
    {
        DateTime currentDate = DateTime.Now;
        TimeSpan timeDifference = date - currentDate;
        return Math.Abs(timeDifference.Days);
    }

    public static int HourDiff(DateTime date)
    {
        DateTime currentDate = DateTime.Now;
        TimeSpan timeDifference = date - currentDate;
        return Math.Abs(timeDifference.Hours);
    }

    public static int MinDiff(DateTime date)
    {
        DateTime currentDate = DateTime.Now;
        TimeSpan timeDifference = date - currentDate;
        return Math.Abs(timeDifference.Minutes);
    }
}
