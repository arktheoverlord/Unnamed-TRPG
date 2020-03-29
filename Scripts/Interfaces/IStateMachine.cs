using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG.Interfaces {
    public interface IStateMachine {
        void StartMachine();

        void UpdateMachine();

        bool HasEnded();
    }
}
