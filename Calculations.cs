using System;
using System.Collections.Generic;
using System.Linq;

namespace SF2022UserLib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            List<string> periods = new();

            if (startTimes is null)
                throw new ArgumentNullException(nameof(startTimes),
                    "Массив занятого времени не проинициализирован"
                    );

            if (durations is null)
                throw new ArgumentNullException(nameof(durations),
                    "Массив продолжительности занятого времени не проинициализирован"
                    );

            if (startTimes.Length != durations.Length)
                throw new ArrayMismatchException("Длины массивов не равны!",
                    nameof(startTimes), nameof(durations)
                    );

            if (consultationTime <= 0)
                throw new ArgumentException("Необходимое время должно быть больше нуля!",
                    nameof(consultationTime)
                    );

            if (beginWorkingTime > endWorkingTime)
                throw new ArgumentOutOfRangeException(nameof(beginWorkingTime),
                    "Рабочий день не может начинаться позже, чем он заканчивается!"                    
                    );

            if (startTimes.Length == 0)
            {
                periods.AddRange(GetPeriodsInRange(beginWorkingTime, endWorkingTime, consultationTime));
                Console.WriteLine(string.Join("\n", periods.ToArray()));
                return periods.ToArray();
            }

            if (startTimes.Any(x => x < beginWorkingTime || x > endWorkingTime))
                throw new ArgumentOutOfRangeException(nameof(startTimes),
                    "Сотрудник не может быть занят вне своего рабочего времени!"                    
                    );

            var sortedByTimes = startTimes
            .Zip(durations, (t, s) => new { time = t, duration = s })
            .OrderBy(x => x.time).ToList();

            periods.AddRange(GetPeriodsInRange(beginWorkingTime, sortedByTimes[0].time, consultationTime));
            for (int i = 0; i < sortedByTimes.Count - 1; i++)
            {
                periods.AddRange(GetPeriodsInRange(sortedByTimes[i].time + TimeSpan.FromMinutes(sortedByTimes[i].duration), sortedByTimes[i + 1].time, consultationTime));
            }
            periods.AddRange(GetPeriodsInRange(sortedByTimes.Last().time + TimeSpan.FromMinutes(sortedByTimes.Last().duration), endWorkingTime, consultationTime));

            return periods.ToArray();
        }

        private static IEnumerable<string> GetPeriodsInRange(TimeSpan begin, TimeSpan end, int consultationTime)
        {
            List<string> periods = new();
            while (begin.Add(TimeSpan.FromMinutes(consultationTime)) <= end)
            {
                periods.Add(Format(begin) + "-" + Format(begin.Add(TimeSpan.FromMinutes(consultationTime))));
                begin = begin.Add(TimeSpan.FromMinutes(consultationTime));
            }
            return periods;
        }
        private static string Format(TimeSpan ts)
        {
            return ts.ToString(@"hh\:mm");
        }
    }
}