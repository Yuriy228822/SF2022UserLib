using NUnit.Framework;
using SF2022UserLib;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
//using Assert = NUnit.Framework.Assert;


namespace SF2022UserLibTests
{
    [TestFixture]
    public class CalculationsTests
    {
        private Calculations? calculations;

        [SetUp]
        public void SetUp()
        {
            calculations = new Calculations();
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArgumentNullException_WhenStartTimesIsNull()
        {
            TimeSpan[] startTimes = null;
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            NUnit.Framework.Assert.Throws<ArgumentNullException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArgumentNullException_WhenDurationsIsNull()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0), new TimeSpan(10, 0, 0) };
            int[] durations = null;
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            NUnit.Framework.Assert.Throws<ArgumentNullException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArrayMismatchException_WhenArraysAreOfDifferentLengths()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0) };
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            Assert.ThrowsException<ArrayMismatchException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArgumentException_WhenConsultationTimeIsNonPositive()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0), new TimeSpan(10, 0, 0) };
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 0;

            NUnit.Framework.Assert.Throws<ArgumentException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArgumentOutOfRangeException_WhenBeginWorkingTimeIsAfterEndWorkingTime()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0), new TimeSpan(10, 0, 0) };
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(17, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(9, 0, 0);
            int consultationTime = 30;

            NUnit.Framework.Assert.Throws<ArgumentOutOfRangeException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldThrowArgumentOutOfRangeException_WhenStartTimesAreOutsideWorkingHours()
        {
            TimeSpan[] startTimes = { new TimeSpan(8, 30, 0), new TimeSpan(18, 0, 0) };
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            NUnit.Framework.Assert.Throws<ArgumentOutOfRangeException>(() =>
                calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void AvailablePeriods_ShouldReturnFullWorkingDay_WhenNoBusyPeriods()
        {
            TimeSpan[] startTimes = Array.Empty<TimeSpan>();
            int[] durations = Array.Empty<int>();
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            string[] expected = {
        "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00",
        "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00",
        "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00"
    };

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AvailablePeriods_ShouldReturnCorrectPeriods_WhenBusyPeriodsExist()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(13, 0, 0) };
            int[] durations = { 60, 90 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "09:00-09:30", "09:30-10:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00",
                "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00"
            };

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AvailablePeriods_ShouldReturnEmpty_WhenNoAvailableSlots()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 60, 60, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 60;

            string[] expected = Array.Empty<string>();

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AvailablePeriods_ShouldHandleExactFitPeriods()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0) };
            int[] durations = { 180, 120 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 60;

            string[] expected = {
                "15:00-16:00", "16:00-17:00"
            };

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AvailablePeriods_ShouldHandleBackToBackAppointments()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0) };
            int[] durations = { 60, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "11:00-11:30", "11:30-12:00"
            };

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(expected, result);
        }
    }
}

