﻿using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IInstrumentServiceProvider
    {
        CalibrationReport RegisterNewCalibration(Instrument target);
        Instrument RegisterNewInstrument();
    }
}
