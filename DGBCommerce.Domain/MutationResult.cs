﻿using System.Text.RegularExpressions;

namespace DGBCommerce.Domain
{
    public partial class MutationResult
    {
        public Guid Identifier { get; set; }
        public int ErrorCode { get; set; }
        public string? Message { get; set; }

        public string Constraint
            => ErrorCode switch
            {
                2601 => RegexUniqueConstraintViolation().Matches(Message!)[1].Value.Trim('\''),
                2627 => RegexUniqueConstraintViolation().Matches(Message!)[0].Value.Trim('\''),
                _ => !string.IsNullOrWhiteSpace(Message) ? Message : string.Empty,
            };

        public bool Success
            => this.ErrorCode == 0;

        [GeneratedRegex("\'.*?\'")]
        private static partial Regex RegexUniqueConstraintViolation();
    }
}
