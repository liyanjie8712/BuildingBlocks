using System;

namespace Liyanjie.TemplateMatching
{
    public class InlineConstraint
    {
        /// <summary>
        /// Creates a new <see cref="InlineConstraint"/>.
        /// </summary>
        /// <param name="constraint">The constraint text.</param>
        public InlineConstraint(string constraint)
        {
            Constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }

        /// <summary>
        /// Gets the constraint text.
        /// </summary>
        public string Constraint { get; }
    }
}
