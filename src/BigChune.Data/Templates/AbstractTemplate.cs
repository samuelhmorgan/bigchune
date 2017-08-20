using System.Collections.Generic; 

namespace BigChune.Data.Templates
{
    public abstract class AbstractTemplate
    {
        public Dictionary<string, object> Parameters { get; set; }

        protected AbstractTemplate()
        {
            Parameters = new Dictionary<string, object> ();
        }

        public abstract string Query { get; }
    }
}
