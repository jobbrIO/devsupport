﻿using System;

namespace Jobbr.DevSupport.ReferencedVersionAsserter
{
    public static class NuspecVersionParser
    {
        public static NuspecDependency Parse(NuspecDependency nuspecDependency, string versionStringValue)
        {
            // Parse Version
            if (versionStringValue.Contains(","))
            {
                if(!((versionStringValue.StartsWith("[") || versionStringValue.StartsWith("(")) && (versionStringValue.EndsWith("]") || versionStringValue.EndsWith(")"))))
                {
                    throw new ArgumentException();
                }
      
                var rangeParts = versionStringValue.Split(new[] {","}, StringSplitOptions.None);

                // Left Part
                if (!string.IsNullOrWhiteSpace(rangeParts[0]) && rangeParts[0].Length > 1)
                {
                    var leftVersion = CreateVersion(rangeParts[0].Substring(1));
                    leftVersion.Inclusive = rangeParts[0].StartsWith("[");

                    nuspecDependency.MinVersion = leftVersion;
                }

                if (!string.IsNullOrWhiteSpace(rangeParts[1]) && rangeParts[1].Length > 1)
                {
                    // Right Part
                    var rightVersion = CreateVersion(rangeParts[1].Substring(0, rangeParts[1].Length - 1));
                    rightVersion.Inclusive = rangeParts[1].EndsWith("]");

                    nuspecDependency.MaxVersion = rightVersion;
                }
            }
            else
            {
                // [1.0]	x == 1.0	Exact version match
                if (versionStringValue.StartsWith("[") && versionStringValue.EndsWith("]"))
                {
                    var exactVersion = CreateVersion(versionStringValue.Substring(1, versionStringValue.Length - 2));

                    exactVersion.Inclusive = true;

                    nuspecDependency.MinVersion = exactVersion;
                    nuspecDependency.MaxVersion = exactVersion;
                }
                else
                {
                    var minVersion = CreateVersion(versionStringValue);
                    minVersion.Inclusive = true;

                    nuspecDependency.MinVersion = minVersion;
                    nuspecDependency.MaxVersion = null;
                }
            }

            return nuspecDependency;
        }

        private static NuspecVersion CreateVersion(string versionStringValue)
        {
            // Filter PreTags
            var tagSplit = versionStringValue.Split(new[] {"-"}, StringSplitOptions.RemoveEmptyEntries);

            var versionParts = tagSplit[0].Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var major = versionParts.Length >= 1 ? Int32.Parse(versionParts[0]) : 0;
                var minor = versionParts.Length >= 2 ? Int32.Parse(versionParts[1]) : 0;
                var bugfix = versionParts.Length == 3 ? Int32.Parse(versionParts[2]) : 0;

                var exactVersion = new NuspecVersion() { Major = major, Minor = minor, Bugfix = bugfix };

                if (tagSplit.Length == 2)
                {
                    exactVersion.Tag = tagSplit[1];
                }

                return exactVersion;
            }
            catch (Exception)
            {
                throw new Exception($"Unable to cast version-string '{versionStringValue}'.");
            }
        }
    }
}