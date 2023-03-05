using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine
{
    public abstract class ScriptBehaviour
    {
        protected ScriptBehaviour()
        {
            Kudo.AddScriptBehaviour(this);
        }

        ~ScriptBehaviour()
        {
            Kudo.RemoveScriptBehaviour(this);
        }

        public virtual void Draw() { }
        public virtual void Update() { }
    }
}
