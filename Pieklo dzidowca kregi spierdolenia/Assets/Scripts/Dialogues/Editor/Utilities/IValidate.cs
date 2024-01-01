using System.Collections.Generic;

namespace jbzd.Dialogues.Editor.Utilities
{
    public interface IValidate
    {
        public abstract List<string> WarningInfos { get; set; }

        public abstract bool IsRuleViolated();
    }
}
