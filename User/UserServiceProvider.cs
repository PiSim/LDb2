﻿using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    public class UserServiceProvider : IUserServiceProvider
    {
        private DBEntities _entities;
        private IUnityContainer _container;

        public UserServiceProvider(DBEntities entities,
                                    IUnityContainer container)
        {
            _container = container;
            _entities = entities;
        }
        
    }
}