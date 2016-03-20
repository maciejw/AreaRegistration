using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace App
{
    public class ResourcePathReplacer
    {
        private const string Backslash = "\\";
        private const string Slash = "/";
        private const string Dot = ".";

        private readonly Regex regexReplacer;

        public ResourcePathReplacer()
        {
            var escapedCharacters = Regex.Escape($"{Backslash}{Slash}");

            this.regexReplacer = new Regex($"[{escapedCharacters}]", RegexOptions.Compiled);
        }

        [ExcludeFromCodeCoverage]
        private string ToDotEvaluator(Match match)
        {
            switch (match.Value)
            {
                case Backslash:
                case Slash: return Dot;
            }

            throw new NotSupportedException($"Unexpected match {match.Value}");
        }

        public string ToResourcePath(string path)
        {
            Contract.Requires(path != null);

            return regexReplacer.Replace(path, ToDotEvaluator);
        }
    }

}
