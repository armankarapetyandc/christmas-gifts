using System;

namespace SpaceMonkey.Editor.BuildTool.Exceptions
{
    public class InvalidBuildNumberException : Exception
    {
        public string BuildNumber { get; private set; }

        public InvalidBuildNumberException(string buildNumber) : base(
            $"Invalid build number '{buildNumber}'.")
        {
            BuildNumber = buildNumber;
        }
    }
}