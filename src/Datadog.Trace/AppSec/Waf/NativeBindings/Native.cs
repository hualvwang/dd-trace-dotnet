// <copyright file="Native.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Datadog.Trace.Logging;

namespace Datadog.Trace.AppSec.Waf.NativeBindings
{
    internal class Native
    {
#if NETFRAMEWORK
        private const string DllName = "Sqreen.dll";
#else
        private const string DllName = "Sqreen";
#endif

        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor(typeof(Native));

        static Native()
        {
#if NETFRAMEWORK
            var libName = DllName;
#else
            var (libPrefix, libExt) =
                Environment.OSVersion.Platform switch
                {
                    PlatformID.MacOSX => ("lib", "dylib"),
                    PlatformID.Unix => ("lib", "so"),
                    PlatformID.Win32NT => (string.Empty, "dll"),
                    PlatformID.Win32S => (string.Empty, "dll"),
                    PlatformID.Win32Windows => (string.Empty, "dll"),
                    PlatformID.WinCE => (string.Empty, "dll"),
                    PlatformID.Xbox => throw new NotSupportedException(),
                    _ => throw new NotSupportedException(),
                };
            var libName = libPrefix + DllName + "." + libExt;
#endif

            var runtimeIdPart1 =
                Environment.OSVersion.Platform switch
                {
                    PlatformID.MacOSX => "osx",
                    PlatformID.Unix => "linux",
                    PlatformID.Win32NT => "win",
                    PlatformID.Win32S => "win",
                    PlatformID.Win32Windows => "win",
                    PlatformID.WinCE => "win",
                    PlatformID.Xbox => throw new NotSupportedException(),
                    _ => throw new NotSupportedException(),
                };

            var runtimeId = Environment.Is64BitProcess ? runtimeIdPart1 + "-x64" : runtimeIdPart1 + "-x32";

            var currentDir =
                string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) ?
                    AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;
            var libDir = Path.Combine(currentDir, $"runtimes/{runtimeId}/native");
            var libFullPath = Path.Combine(libDir, libName);

            Log.Information($"Checking for '{libName}' path in '{libDir}'");

            if (runtimeIdPart1 == "win")
            {
                var path = Environment.GetEnvironmentVariable("PATH");

                if (File.Exists(libFullPath) && !path.Contains(libDir))
                {
                    Environment.SetEnvironmentVariable("PATH", path + ";" + libDir);
                    Log.Information($"Updated path");
                }
            }
            else
            {
                // this probably isn't a good idea, but since modify the path doesn't work
                // on *nix type systems, this will make the integration tests pass
                File.Copy(libFullPath, Path.Combine(currentDir, libName));
            }
        }

#pragma warning disable SA1300 // Element should begin with upper-case letter

        [DllImport(DllName)]
        internal static extern PWVersion pw_getVersion();

        [DllImport(DllName)]
        internal static extern IntPtr pw_initH(string wafRule, ref PWConfig config, ref string errors);

        [DllImport(DllName)]
        internal static extern void pw_clearRuleH(IntPtr wafHandle);

        [DllImport(DllName)]
        internal static extern PWRet pw_runH(IntPtr wafHandle, PWArgs parameters, ulong timeLeftInUs);

        [DllImport(DllName)]
        internal static extern void pw_freeReturn(PWRet output);

        [DllImport(DllName)]
        internal static extern IntPtr pw_initAdditiveH(IntPtr powerwafHandle);

        [DllImport(DllName)]
        internal static extern PWRet pw_runAdditive(IntPtr context, PWArgs newArgs, ulong timeLeftInUs);

        [DllImport(DllName)]
        internal static extern void pw_clearAdditive(IntPtr context);

        [DllImport(DllName)]
        internal static extern PWArgs pw_getInvalid();

        [DllImport(DllName)]
        internal static extern PWArgs pw_createStringWithLength(string s, ulong length);

        [DllImport(DllName)]
        internal static extern PWArgs pw_createString(string s);

        [DllImport(DllName)]
        internal static extern PWArgs pw_createInt(long value);

        [DllImport(DllName)]
        internal static extern PWArgs pw_createUint(ulong value);

        [DllImport(DllName)]
        internal static extern PWArgs pw_createArray();

        [DllImport(DllName)]
        internal static extern PWArgs pw_createMap();

        [DllImport(DllName)]
        internal static extern bool pw_addArray(ref PWArgs array, PWArgs entry);

        // Setting entryNameLength to 0 will result in the entryName length being re-computed with strlen
        [DllImport(DllName)]
        internal static extern bool pw_addMap(ref PWArgs map, string entryName, ulong entryNameLength, PWArgs entry);

        [DllImport(DllName)]
        internal static extern void pw_freeArg(ref PWArgs input);

#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
