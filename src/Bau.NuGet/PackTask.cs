﻿// <copyright file="PackTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PackTask : CommandTask
    {
        private readonly List<string> nuspecsOrProjects = new List<string>();
        private readonly HashSet<string> exclusions = new HashSet<string>();
        private readonly Dictionary<string, string> properties =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public ICollection<string> NuSpecsOrProjects
        {
            get { return this.nuspecsOrProjects; }
        }

        public string OutputDirectory { get; set; }

        public string NuSpecBasePath { get; set; }

        public string VersionValue { get; set; }

        public ICollection<string> Exclusions
        {
            get { return this.exclusions; }
        }

        public bool Symbols { get; set; }

        public bool Tool { get; set; }

        public bool Build { get; set; }

        public bool NoDefaultExcludes { get; set; }

        public bool NoPackageAnalysis { get; set; }

        public bool EmptyDirectoriesExcluded { get; set; }

        public bool ReferencedProjectsIncluded { get; set; }

        public IDictionary<string, string> PropertiesCollection
        {
            get { return this.properties; }
        }

        public string MiniClientVersionValue { get; set; }

        protected override string OperationName
        {
            get { return "pack"; }
        }

        public void AddExcludes(params string[] excludes)
        {
            this.exclusions.UnionWith(excludes);
        }

        public void AddExcludes(IEnumerable<string> excludes)
        {
            this.exclusions.UnionWith(excludes);
        }

        public PackTask Files(params string[] nuspecsOrProjects)
        {
            this.nuspecsOrProjects.AddRange(nuspecsOrProjects);
            return this;
        }

        public PackTask Files(IEnumerable<string> nuspecsOrProjects)
        {
            this.nuspecsOrProjects.AddRange(nuspecsOrProjects);
            return this;
        }

        public PackTask Output(string outputDirectory)
        {
            this.OutputDirectory = outputDirectory;
            return this;
        }

        public PackTask NuSpecBase(string basePath)
        {
            this.NuSpecBasePath = basePath;
            return this;
        }

        public PackTask Version(string version)
        {
            this.VersionValue = version;
            return this;
        }

        public PackTask Exclude(params string[] excludes)
        {
            this.AddExcludes(excludes);
            return this;
        }

        public PackTask Exclude(IEnumerable<string> excludes)
        {
            this.AddExcludes(excludes);
            return this;
        }

        public PackTask MakeSymbols(bool enabled = true)
        {
            this.Symbols = enabled;
            return this;
        }

        public PackTask AsTool(bool enabled = true)
        {
            this.Tool = enabled;
            return this;
        }

        public PackTask PerformBuild(bool enabled = true)
        {
            this.Build = enabled;
            return this;
        }

        public PackTask DisableDefaultExcludes(bool enabled = true)
        {
            this.NoDefaultExcludes = enabled;
            return this;
        }

        public PackTask DisablePackageAnalysis(bool enabled = true)
        {
            this.NoPackageAnalysis = enabled;
            return this;
        }

        public PackTask ExcludeEmptyDirectories(bool enabled = true)
        {
            this.EmptyDirectoriesExcluded = enabled;
            return this;
        }

        public PackTask IncludeReferencedProjects(bool enabled = true)
        {
            this.ReferencedProjectsIncluded = enabled;
            return this;
        }

        public PackTask Property(string key, string value)
        {
            this.properties[key] = value;
            return this;
        }

        public PackTask Properties(IDictionary<string, string> pairs)
        {
            foreach (var pair in pairs)
            {
                this.properties[pair.Key] = pair.Value;
            }

            return this;
        }

        public PackTask MiniClientVersion(string version)
        {
            this.MiniClientVersionValue = version;
            return this;
        }

        protected override IEnumerable<string> CreateCustomCommandLineOptions()
        {
            if (this.OutputDirectory != null)
            {
                yield return "-OutputDirectory " + CommandTask.EncodeArgumentValue(this.OutputDirectory);
            }

            if (this.NuSpecBasePath != null)
            {
                yield return "-BasePath " + CommandTask.EncodeArgumentValue(this.NuSpecBasePath);
            }

            if (this.VersionValue != null)
            {
                yield return "-Version " + CommandTask.EncodeArgumentValue(this.VersionValue);
            }

            foreach (var exclusion in this.Exclusions)
            {
                yield return "-Exclude " + CommandTask.EncodeArgumentValue(exclusion);
            }

            if (this.Symbols)
            {
                yield return "-Symbols";
            }

            if (this.Tool)
            {
                yield return "-Tool";
            }

            if (this.Build)
            {
                yield return "-Build";
            }

            if (this.NoDefaultExcludes)
            {
                yield return "-NoDefaultExcludes";
            }

            if (this.NoPackageAnalysis)
            {
                yield return "-NoPackageAnalysis";
            }

            if (this.EmptyDirectoriesExcluded)
            {
                yield return "-ExcludeEmptyDirectories";
            }

            if (this.ReferencedProjectsIncluded)
            {
                yield return "-IncludeReferencedProjects";
            }

            if (this.properties.Any())
            {
                var value = string.Join(
                    ";", this.PropertiesCollection.Select(property => string.Concat(property.Key, "=", property.Value)));

                yield return "-Properties " + CommandTask.EncodeArgumentValue(value);
            }

            if (this.MiniClientVersionValue != null)
            {
                yield return "-MinClientVersion " + CommandTask.EncodeArgumentValue(this.MiniClientVersionValue);
            }
        }

        protected override IEnumerable<string> GetTargetFiles()
        {
            return this.NuSpecsOrProjects;
        }
    }
}
