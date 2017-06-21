﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class BatchExtension
    {
        public static Material GetMaterial(this Batch entry)
        {
            // Returns loaded material for batch entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.ExternalConstruction)
                                        .Include(mat => mat.Recipe.Colour)
                                        .FirstOrDefault(mat => mat.ID == entry.MaterialID);
            }
        }
    }
}
