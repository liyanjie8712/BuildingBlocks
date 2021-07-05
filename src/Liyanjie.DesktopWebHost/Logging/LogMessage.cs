using System;

namespace Liyanjie.DesktopWebHost.Logging
{
    readonly struct LogMessage
    {
        public LogMessage(
            string message,
            string timeStamp = null,
            string levelString = null)
        {
            Message = message;
            TimeStamp = timeStamp;
            LevelString = levelString;
        }

        public readonly string TimeStamp;
        public readonly string LevelString;
        public readonly string Message;
    }
}
