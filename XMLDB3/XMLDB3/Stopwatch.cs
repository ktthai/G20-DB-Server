using System;
using System.Runtime.InteropServices;

namespace XMLDB3
{
	public class Stopwatch
	{
		public static readonly long Frequency;

		public static readonly bool IsHighResolution;

		private long elapsed;

		private bool isRunning;

		private long startTimeStamp;

		private static readonly double tickFrequency;

		public TimeSpan Elapsed => new TimeSpan(GetElapsedDateTimeTicks());

		public long ElapsedMilliseconds => GetElapsedDateTimeTicks() / 10000;

		public long ElapsedTicks => GetRawElapsedTicks();

		public bool IsRunning => isRunning;

		[DllImport("kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		[DllImport("kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		static Stopwatch()
		{
			if (!QueryPerformanceFrequency(out Frequency))
			{
				IsHighResolution = false;
				Frequency = 10000000L;
				tickFrequency = 1.0;
			}
			else
			{
				IsHighResolution = true;
				tickFrequency = 10000000.0;
				tickFrequency /= Frequency;
			}
		}

		public Stopwatch()
		{
			Reset();
		}

		private long GetElapsedDateTimeTicks()
		{
			long rawElapsedTicks = GetRawElapsedTicks();
			if (IsHighResolution)
			{
				double num = rawElapsedTicks;
				num *= tickFrequency;
				return (long)num;
			}
			return rawElapsedTicks;
		}

		private long GetRawElapsedTicks()
		{
			long num = elapsed;
			if (isRunning)
			{
				long timestamp = GetTimestamp();
				long num2 = timestamp - startTimeStamp;
				num += num2;
			}
			return num;
		}

		public static long GetTimestamp()
		{
			if (IsHighResolution)
			{
				long lpPerformanceCount = 0L;
				QueryPerformanceCounter(out lpPerformanceCount);
				return lpPerformanceCount;
			}
			return DateTime.UtcNow.Ticks;
		}

		public void Reset()
		{
			elapsed = 0L;
			isRunning = false;
			startTimeStamp = 0L;
		}

		public void Start()
		{
			if (!isRunning)
			{
				startTimeStamp = GetTimestamp();
				isRunning = true;
			}
		}

		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		public static long GetElapsedMilliseconds(long _startTime)
		{
			double num = GetTimestamp() - _startTime;
			num *= tickFrequency;
			return (long)num;
		}

		public void Stop()
		{
			if (isRunning)
			{
				long timestamp = GetTimestamp();
				long num = timestamp - startTimeStamp;
				elapsed += num;
				isRunning = false;
			}
		}
	}
}
