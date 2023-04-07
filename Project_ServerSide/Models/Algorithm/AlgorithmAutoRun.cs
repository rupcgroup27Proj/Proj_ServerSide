using System;
using System.Threading;
namespace Project_ServerSide.Models.Algorithm;

public class AlgorithmAutoRun
{
    public static void Main()
    {
        /* 
         const int day = 86400000;
          DateTime startDate = new DateTime(2023,3,6,3,0,0);//Should change the date to the right day
          Timer timer = new Timer(AutoRun, null, (int)(startDate - DateTime.Now).TotalMilliseconds, day); //call TimerCallback every 1 second
       */

        const int day = 86400000;
        DateTime startDate = new DateTime(2023, 3, 6, 3, 0, 0);//Should change the date to the right day
        Timer timer = new Timer(AutoRun, null, 0, day); //call TimerCallback once a day
    }

    private static void AutoRun(object o)
    {
        Algorithm.RunAlgorithm();
    }
}
