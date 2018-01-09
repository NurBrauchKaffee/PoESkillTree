﻿using System.Collections.Generic;
using PoESkillTree.Computation.Parsing.ModifierBuilding;

namespace PoESkillTree.Computation.Parsing
{
    /// <summary>
    /// The parsing result type used as output of <see cref="MatcherDataParser"/>.
    /// </summary>
    public class MatcherDataParseResult
    {
        public MatcherDataParseResult(IIntermediateModifier modifier, IReadOnlyDictionary<string, string> regexGroups)
        {
            Modifier = modifier;
            RegexGroups = regexGroups;
        }

        /// <summary>
        /// <see cref="Data.MatcherData.Modifier"/> of the matched <see cref="Data.MatcherData"/>
        /// </summary>
        public IIntermediateModifier Modifier { get; }

        /// <summary>
        /// Regex group names of the matched <see cref="Data.MatcherData"/>'s <see cref="Data.MatcherData.Regex"/>
        /// and their captured substrings.
        /// </summary>
        public IReadOnlyDictionary<string, string> RegexGroups { get; }
    }
}