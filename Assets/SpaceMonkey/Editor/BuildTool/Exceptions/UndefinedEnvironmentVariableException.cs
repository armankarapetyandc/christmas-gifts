using System;

namespace SpaceMonkey.Editor.BuildTool.Exceptions
{
    public class UndefinedEnvironmentVariableException : Exception
    {
        public string VariableName { get; private set; }

        public UndefinedEnvironmentVariableException(string variableName) : base(
            $"Environment variable '{variableName}' does not exist.")
        {
            VariableName = variableName;
        }
    }
}