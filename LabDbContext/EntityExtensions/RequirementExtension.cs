﻿using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public partial class Requirement
    {
        #region Properties

        public string VariantName => MethodVariant?.Name;

        #endregion Properties
    }
}