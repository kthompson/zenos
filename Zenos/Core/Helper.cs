using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Zenos.Core
{
    public static class Helper
    {

        #region assertion stuff
        /// <summary>
        /// Stops the debugger if attached and does nothing with the exception
        /// Simply used to keep the compiler from complaining about an unused variable
        /// </summary>
        /// <param name="ex"></param>
        [DebuggerHidden]
        public static void Suppress(Exception ex)
        {
            Trace.WriteLine(ex.Message);
            Break();
        }

        /// <summary>
        /// Stops the debugger if attached
        /// </summary>
        [DebuggerHidden]
        public static void Break()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        /// <summary>
        /// Generic assertion that will throw whatever exception you specify if the expression is false
        /// </summary>
        /// <param name="expression">Expression that evaluates to a boolean to check</param>
        /// <param name="exceptionCreator"></param>
        [DebuggerHidden]
        public static void True(bool expression)
        {
            if (expression != true)
                Throw(new Exception());
        }

        /// <summary>
        /// Generic assertion that will throw whatever exception you specify if the expression is false
        /// </summary>
        /// <param name="expression">Expression that evaluates to a boolean to check</param>
        /// <param name="exceptionCreator"></param>
        [DebuggerHidden]
        public static void False(bool expression)
        {
            if (expression != false)
                Throw(new Exception());
        }

        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="arg">Value to validate</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerHidden]
        public static void IsNotNull<T>(T arg, string argName)
            where T : class
        {
            if (arg == null)
                Throw(new ArgumentNullException(argName));
        }

        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="arg">Value to validate</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerHidden]
        public static void IsNotNullOrEmpty(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                Throw(new ArgumentNullException());
        }

        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="arg">Value to validate</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerHidden]
        public static void IsNotNull<T>(T arg)
            where T : class
        {
            if (arg == null)
                Throw(new ArgumentNullException());
        }

        /// <summary>
        /// Throws an ArgumentNullException if the value does not equal arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <exception cref="System.ArgumentException"></exception>
        [DebuggerHidden]
        public static void AreEqual<T>(T arg, T value, string message)
        {
            if (!Equals(arg, value))
                Throw(new ArgumentException(message, string.Empty));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not in the list of valid values
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="values"></param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsOneOf<T>(T arg, string argName, params T[] values)
            where T : struct, IComparable
        {
            foreach (var value in values)
                if (Equals(arg, value))
                    return;

            Throw(new ArgumentOutOfRangeException(argName));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not within the specified range
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="minValue">Minimum value anything lower throws exception</param>
        /// <param name="maxValue">Maximum value anything higher throws exception</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsBetween<T>(T arg, string argName, T minValue, T maxValue)
            where T : struct, IComparable
        {
            if (arg.CompareTo(minValue) < 0 || arg.CompareTo(maxValue) > 0)
                Throw(new ArgumentOutOfRangeException(argName));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not greater than specified value
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="value">Value the argument must be greater than in order to not throw</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsGreaterThan<T>(T arg, string argName, T value)
            where T : struct, IComparable
        {
            if (arg.CompareTo(value) <= 0)
                Throw(new ArgumentOutOfRangeException(argName));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not greater than specified value
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="value">Value the argument must be greater than in order to not throw</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsGreaterThanEqual<T>(T arg, string argName, T value)
            where T : struct, IComparable
        {
            if (arg.CompareTo(value) < 0)
                Throw(new ArgumentOutOfRangeException(argName));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not less than specified value
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="value">Value the argument must be less than in order to not throw</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsLessThan<T>(T arg, string argName, T value)
            where T : struct, IComparable
        {
            if (arg.CompareTo(value) >= 0)
                Throw(new ArgumentOutOfRangeException(argName));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the argument is not less than specified value
        /// </summary>
        /// <typeparam name="T">Struct</typeparam>
        /// <param name="arg">Value to be validated</param>
        /// <param name="argName">Name of the argument being validated</param>
        /// <param name="value">Value the argument must be less than in order to not throw</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerHidden]
        public static void IsLessThanEqual<T>(T arg, string argName, T value)
            where T : struct, IComparable
        {
            if (arg.CompareTo(value) > 0)
                Throw(new ArgumentOutOfRangeException(argName));
        }

        [DebuggerHidden]
        public static void NotSupported()
        {
            NotSupported(string.Empty);
        }

        [DebuggerHidden]
        public static void NotSupported(string fmt = "", params object[] args)
        {
            Throw(new NotSupportedException(string.Format(fmt, args)));
        }

        [DebuggerHidden]
        public static void Stop(Exception ex = null)
        {
            Throw(ex ?? new Exception());
        }

        [DebuggerHidden]
        public static void Stop(string fmt = "", params object[] args)
        {
            Throw(new Exception(string.Format(fmt, args)));
        }

        /// <summary>
        /// Stops the debugger if attached and throws the exception
        /// </summary>
        /// <param name="ex"></param>
        [DebuggerHidden]
        public static void Throw(Exception ex)
        {
            Break();
            throw ex;
        }
        #endregion

        #region utility methods

        public static string Execute(string command)
        {
            string output;
            string error;
            Execute(command, out error, out output);
            return output;
        }

        public static int Execute(string command, out string error, out string output)
        {
            //separate arguments from commands
            var parts = command.Split(' ');
            var cmd = parts.First();
            var args = string.Join(" ", parts.Skip(1).ToArray());

            //execute the command
            var proc = Process.Start(new ProcessStartInfo(GetFullCommandPath(cmd), args)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            });

            var outputTmp = new StringBuilder();
            var errorTmp = new StringBuilder();

            proc.OutputDataReceived += (sender, e) => outputTmp.AppendLine(e.Data);
            proc.ErrorDataReceived += (sender, e) => errorTmp.AppendLine(e.Data);

            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            outputTmp.Remove(outputTmp.Length - 2, 2);
            errorTmp.Remove(errorTmp.Length - 2, 2);

            output = outputTmp.ToString().Trim();
            error = errorTmp.ToString();

            return proc.ExitCode;
        }


        private static string GetFullCommandPath(string command)
        {
            //check our PATH variable to find the complete absolue path for a command
            var path = Directory.GetCurrentDirectory().Append(";",
                       Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine), ";",
                       Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User));

            var paths = path.Split(';');

            var commands = from dir in paths
                           from file in new[] { Path.Combine(dir, command), Path.Combine(dir, command + ".exe") }
                           where File.Exists(file)
                           select file;

            var commandPath = commands.FirstOrDefault();

            if(string.IsNullOrEmpty(commandPath))
                Throw(new ApplicationException(command + " command not found."));

            return commandPath;
        }

        #endregion

        public static void IsNull<T>(T arg)
            where T : class
        {
            if (arg != null)
                Throw(new ArgumentNullException());
        }
    }
}
