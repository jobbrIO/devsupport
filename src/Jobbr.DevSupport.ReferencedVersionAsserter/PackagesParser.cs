﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Jobbr.DevSupport.ReferencedVersionAsserter
{
    public class PackagesParser
    {
        public List<NuspecDependency> Dependencies { get; set; } = new List<NuspecDependency>();

        public PackagesParser(string packageConfigFile = null)
        {
            if (!File.Exists(packageConfigFile) || packageConfigFile == null)
            {
                throw new ArgumentException($"File '{packageConfigFile}' does not exist!", nameof(packageConfigFile));
            }

            Parse(packageConfigFile);
        }

        private void Parse(string packageConfigFile)
        {
            // Parse dependencies
            var doc = new XmlDocument();

            doc.Load(packageConfigFile);

            var packageNodes = doc.SelectNodes("packages/package");
            if (packageNodes == null)
            {
                return;
            }

            foreach (XmlNode dependencyNode in packageNodes)
            {
                var nuspecDependency = XmlDependencyConverter.Convert(dependencyNode);

                this.Dependencies.Add(nuspecDependency);
            }
        }
    }
}
