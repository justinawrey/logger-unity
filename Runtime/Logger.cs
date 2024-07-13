using System;
using UnityEngine;

namespace StructuredLogger
{
    public enum LogLevel
    {
        None,
        Info,
        Verbose
    }

    [Serializable]
    public class Logger
    {
        [SerializeField]
        private LogLevel _editorLogLevel = LogLevel.Info;

        [SerializeField]
        private LogLevel _buildLogLevel = LogLevel.Verbose;

        [SerializeField]
        private string _prefix = "";

        [SerializeField]
        private Color _color = Color.green;

        private string GetLogLevelPrefix(LogLevel logLevel) =>
            logLevel switch
            {
                LogLevel.Info => "info",
                LogLevel.Verbose => "verbose",
                _ => ""
            };

        private string Now => DateTime.UtcNow.ToString("HH:mm:ss.ff");

        private void LogRaw(LogLevel logLevel, bool timestamp, object message)
        {
            string timestampStr = timestamp ? $"[{Now}] " : "";
            Debug.Log($"{timestampStr}({GetLogLevelPrefix(logLevel)}) {_prefix}: {message}");
        }

        private void LogRichText(LogLevel logLevel, bool timestamp, object message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGBA(_color);
            string timestampStr = timestamp ? $"<b>{Now}</b> " : "";

            Debug.Log(
                $"{timestampStr}<b>({GetLogLevelPrefix(logLevel)})</b> <color=#{colorHex}><b>{_prefix}</b></color>: {message}"
            );
        }

        public void Info(object message)
        {
            if (Application.isEditor && _editorLogLevel != LogLevel.None)
            {
                LogRichText(LogLevel.Info, false, message);
                return;
            }

            if (!Application.isEditor && _buildLogLevel != LogLevel.None)
            {
                LogRaw(LogLevel.Info, true, message);
            }
        }

        public void Verbose(object message)
        {
            if (Application.isEditor && _editorLogLevel == LogLevel.Verbose)
            {
                LogRichText(LogLevel.Verbose, false, message);
                return;
            }

            if (!Application.isEditor && _buildLogLevel == LogLevel.Verbose)
            {
                LogRaw(LogLevel.Verbose, true, message);
            }
        }
    }
}
