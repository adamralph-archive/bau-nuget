﻿// <copyright file="NuGetCliCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class NuGetCliCommandRequestBase
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s");

        protected NuGetCliCommandRequestBase()
        {
            this.Verbosity = null;
            this.NonInteractive = true;
            this.ConfigFile = null;
        }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; private set; }

        public string ConfigFile { get; set; }

        public virtual List<string> CreateCommandLineArguments()
        {
            var arguments = new List<string>();

            if (!string.IsNullOrWhiteSpace(this.Verbosity))
            {
                // NOTE: Verbose is a valid flag but it is deprecated in favor of Verbosity and should not be used
                arguments.Add("-Verbosity " + this.Verbosity);
            }

            if (this.NonInteractive)
            {
                arguments.Add("-NonInteractive");
            }

            if (!string.IsNullOrWhiteSpace(this.ConfigFile))
            {
                arguments.Add("-ConfigFile " + this.QuoteWrapCliValue(this.ConfigFile));
            }

            return arguments;
        }

        protected virtual string QuoteWrapCliValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\""; // supply something
            }

            if (WhiteSpaceRegex.IsMatch(value))
            {
                // NOTE: single-quotes does not seem to work even though that is a suggested method over double-quotes
                var quotedResult = "\"" + value + "\"";

                // NOTE: there are better ways to fix this: http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp/12364234
                if (quotedResult.EndsWith("\\\""))
                {
                    quotedResult = quotedResult.Substring(0, quotedResult.Length - 2) + "/\""; // HACK: just flip the slash and hope for the best
                }

                return quotedResult;
            }

            return value;
        }
    }
}
