using WorldSimulator.Models.NatureBase;
using System;

namespace WorldSimulator.Utils
{
    public class NatureLogger
    {
        private readonly Action<string> _logCallback;
        private readonly List<string> _logEntries;
        public NatureLogger(Action<string> logCallback, List<string> logEntries)
        {
            _logCallback = logCallback;
            _logEntries = logEntries;
        }
        private void Log(string message)
        {
            _logEntries?.Add(message);
            _logCallback?.Invoke(message);
        }
        public void LogPlanted(string name, int age) => Log($"{name} was planted at age {age}.");
        public void LogGrowth(string name, GrowthStage from, GrowthStage to, int age) => Log($"{name} grew {from} to {to} at age {age}.");
        public void LogProduced(string name, string produceType, int age) => Log($"{name} produced {produceType.ToLower()} at age {age}");
        public void LogDecayed(string name, string produceType, int age) => Log($"{name}'s {produceType.ToLower()} decayed at age {age}");
    }
}