using System;
using System.Collections.Generic;

namespace Liyanjie.TemplateMatching
{
    public class RouteTemplate
    {
        const string Separator = "/";

        public RouteTemplate(string template, List<TemplateSegment> segments)
        {
            TemplateText = template;

            Segments = segments ?? throw new ArgumentNullException(nameof(segments));

            Parameters = new List<TemplatePart>();
            for (var i = 0; i < segments.Count; i++)
            {
                var segment = Segments[i];
                for (var j = 0; j < segment.Parts.Count; j++)
                {
                    var part = segment.Parts[j];
                    if (part.IsParameter)
                    {
                        Parameters.Add(part);
                    }
                }
            }
        }

        public string TemplateText { get; }

        public IList<TemplatePart> Parameters { get; }

        public IList<TemplateSegment> Segments { get; }

        public TemplateSegment GetSegment(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return index >= Segments.Count ? null : Segments[index];
        }

        /// <summary>
        /// Gets the parameter matching the given name.
        /// </summary>
        /// <param name="name">The name of the parameter to match.</param>
        /// <returns>The matching parameter or <c>null</c> if no parameter matches the given name.</returns>
        public TemplatePart GetParameter(string name)
        {
            for (var i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                if (string.Equals(parameter.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return parameter;
                }
            }

            return null;
        }
    }
}
