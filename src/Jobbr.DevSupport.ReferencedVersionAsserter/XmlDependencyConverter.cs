﻿using System;
using System.Xml;

namespace Jobbr.DevSupport.ReferencedVersionAsserter
{
    public static class XmlDependencyConverter
    {
        public static NuspecDependency Convert(XmlNode dependencyNode)
        {
            if (dependencyNode == null)
            {
                throw new ArgumentNullException(nameof(dependencyNode));
            }

            if (dependencyNode.Attributes == null)
            {
                return null;
            }

            var id = dependencyNode.Attributes["id"];
            var versionString = dependencyNode.Attributes["version"];

            if (id == null || versionString == null)
            {
                return null;
            }

            var versionStringValue = versionString.Value;
            var nuspecDependency = new NuspecDependency { Name = id.Value, Version = versionStringValue };

            return NuspecVersionParser.Parse(nuspecDependency, versionStringValue);
        }
    }
}