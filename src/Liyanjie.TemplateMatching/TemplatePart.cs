using System;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.TemplateMatching
{
    public class TemplatePart
    {
        public static TemplatePart CreateLiteral(string text)
        {
            return new TemplatePart()
            {
                IsLiteral = true,
                Text = text,
            };
        }

        public static TemplatePart CreateParameter(string name,
            bool isCatchAll,
            bool isOptional,
            object defaultValue,
            IEnumerable<InlineConstraint> inlineConstraints)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new TemplatePart()
            {
                IsParameter = true,
                Name = name,
                IsCatchAll = isCatchAll,
                IsOptional = isOptional,
                DefaultValue = defaultValue,
                InlineConstraints = inlineConstraints ?? Enumerable.Empty<InlineConstraint>(),
            };
        }

        public bool IsCatchAll { get; private set; }
        public bool IsLiteral { get; private set; }
        public bool IsParameter { get; private set; }
        public bool IsOptional { get; private set; }
        public bool IsOptionalSeparator { get; set; }
        public string Name { get; private set; }
        public string Text { get; private set; }
        public object DefaultValue { get; private set; }
        public IEnumerable<InlineConstraint> InlineConstraints { get; private set; }

        internal string DebuggerToString()
        {
            if (IsParameter)
            {
                return "{" + (IsCatchAll ? "*" : string.Empty) + Name + (IsOptional ? "?" : string.Empty) + "}";
            }
            else
            {
                return Text;
            }
        }
    }
}
