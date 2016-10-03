public class Timer
{
    public static float Time = 0;
    public static string FormattedTime
    {
        get
        {
            var minutes = Time / 60;
            var seconds = Time % 60;
            var fraction = (Time * 100) % 100;
            return string.Format("{0:00} : {1:00}.{2:000}", minutes, seconds, fraction);
        }
    }
}