using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.TemplateMatching
{
    public class TemplateSegment
    {
        public bool IsSimple => Parts.Count == 1;

        public List<TemplatePart> Parts { get; } = new List<TemplatePart>();

        internal string DebuggerToString()
        {
            return string.Join(string.Empty, Parts.Select(p => p.DebuggerToString()));
        }
    }
}
